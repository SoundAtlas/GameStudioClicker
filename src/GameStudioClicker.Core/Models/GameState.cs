using GameStudioClicker.Core.Persistence;

namespace GameStudioClicker.Core.Models
{
    public class GameState
    {
        // Core production state
        public long LinesOfCode { get; private set; }
        public long LinesPerClick { get; private set; } = 1;
        public long LinesPerSecond { get; private set; } = 0;

        // Mechanical keyboard upgrade (active production)
        public long MechanicalKeyboardCost { get; private set; } = 25;
        public int MechanicalKeyboardCount { get; private set; } = 0;
        public bool CanPurchaseMechanicalKeyboard
        {
            get
            {
                return LinesOfCode >= MechanicalKeyboardCost;
            }
        }

        // Intern upgrade (passive production)
        public long InternCost { get; private set; } = 50;
        public int InternCount { get; private set; } = 0;
        public bool CanPurchaseIntern
        {
            get
            {
                return LinesOfCode >= InternCost;
            }
        }

        // Persistence

        // Creates an independent snapshot containing only values that need to be saved.
        public GameSaveData CreateSaveData()
        {
            return new GameSaveData
            {
                LinesOfCode = this.LinesOfCode,
                MechanicalKeyboardCount = this.MechanicalKeyboardCount,
                InternCount = this.InternCount
            };
        }

        // Restores saved values and rebuilds production rates and future upgrade costs.
        public void RestoreFromSaveData(GameSaveData saveData)
        {
            // Reject null save data before reading any values from it.
            if (saveData == null)
            {
                throw new ArgumentNullException(nameof(saveData), "Save data cannot be null.");
            }

            // Clamp persisted values in case the save file was edited or corrupted.
            LinesOfCode = Math.Max(0L, saveData.LinesOfCode);
            MechanicalKeyboardCount = Math.Max(0, saveData.MechanicalKeyboardCount);
            InternCount = Math.Max(0, saveData.InternCount);

            // Recalculate values that are derived from the number of upgrades owned.
            LinesPerClick = 1 + MechanicalKeyboardCount;
            LinesPerSecond = 2 * InternCount;
            MechanicalKeyboardCost = 25;

            for (int i = 0; i < MechanicalKeyboardCount; i++)
            {
                MechanicalKeyboardCost *= 2;
            }

            InternCost = 50;
            for (int i = 0; i < InternCount; i++)
            {
                InternCost *= 2;
            }
        }

        // Upgrade purchases

        public bool TryPurchaseMechanicalKeyboard()
        {
            if (CanPurchaseMechanicalKeyboard)
            {
                LinesOfCode -= MechanicalKeyboardCost;
                MechanicalKeyboardCount += 1;
                LinesPerClick += 1;
                MechanicalKeyboardCost *= 2;
                return true;
            }
            return false;
        }

        public bool TryPurchaseIntern()
        {
            if (CanPurchaseIntern)
            {
                LinesOfCode -= InternCost;
                InternCount += 1;
                LinesPerSecond += 2;
                InternCost *= 2;
                return true;
            }
            return false;
        }

        // Code generation
        public void WriteCode()
        {
            LinesOfCode += LinesPerClick;
        }

        public void GeneratePassiveLines()
        {
            LinesOfCode += LinesPerSecond;
        }

        public long ApplyOfflineProgress(TimeSpan elapsed)
        {
            if (elapsed.TotalSeconds <= 0)
            {
                return 0;
            }

            // Limit offline earnings to 24 hours of passive production.
            var maxElapsed = TimeSpan.FromHours(24);
            if (elapsed > maxElapsed)
            {
                elapsed = maxElapsed;
            }

            // Fractional seconds do not produce partial lines of code.
            long wholeSeconds = (long)elapsed.TotalSeconds;

            long offlineLines = wholeSeconds * LinesPerSecond;

            LinesOfCode += offlineLines;

            return offlineLines;
        }
    }
}
