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
        public long MechanicalKeyboardCost { get; private set; } = 100;
        public bool IsMechanicalKeyboardPurchased { get; private set; }
        public bool IsMechanicalKeyboardAvailable => !IsMechanicalKeyboardPurchased;
        public bool CanPurchaseMechanicalKeyboard =>
            LinesOfCode >= MechanicalKeyboardCost &&
            IsMechanicalKeyboardAvailable;


        // Ultrawide monitor upgrade (active production)
        public long UltrawideMonitorCost { get; private set; } = 1000;
        public bool IsUltrawideMonitorPurchased { get; private set; }
        public bool IsUltrawideMonitorUnlocked => IsMechanicalKeyboardPurchased;
        public bool IsUltrawideMonitorAvailable =>
            IsUltrawideMonitorUnlocked &&
            !IsUltrawideMonitorPurchased;
        public bool CanPurchaseUltrawideMonitor =>
            LinesOfCode >= UltrawideMonitorCost &&
            IsUltrawideMonitorAvailable;

        // Intern upgrade (passive production)
        public long InternCost { get; private set; } = 50;
        public int InternCount { get; private set; } = 0;
        public bool CanPurchaseIntern => LinesOfCode >= InternCost;

        // Junior developer upgrade (passive production)
        public long JuniorDeveloperCost { get; private set; } = 2000;
        public int JuniorDeveloperCount { get; private set; } = 0;
        public bool IsJuniorDeveloperUnlocked => InternCount >= 5;
        public bool CanPurchaseJuniorDeveloper => LinesOfCode >= JuniorDeveloperCost && IsJuniorDeveloperUnlocked;

        // Persistence

        // Creates an independent snapshot containing only values that need to be saved.
        public GameSaveData CreateSaveData()
        {
            return new GameSaveData
            {
                LinesOfCode = this.LinesOfCode,
                IsMechanicalKeyboardPurchased = this.IsMechanicalKeyboardPurchased,
                IsUltrawideMonitorPurchased = this.IsUltrawideMonitorPurchased,
                InternCount = this.InternCount,
                JuniorDeveloperCount = this.JuniorDeveloperCount,
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
            InternCount = Math.Max(0, saveData.InternCount);
            JuniorDeveloperCount = Math.Max(0, saveData.JuniorDeveloperCount);
            IsMechanicalKeyboardPurchased = saveData.IsMechanicalKeyboardPurchased;
            IsUltrawideMonitorPurchased = saveData.IsUltrawideMonitorPurchased;

            // Recalculate values that are derived from the number of upgrades owned.
            LinesPerClick = 1;
            if (IsMechanicalKeyboardPurchased)
            {
                LinesPerClick *= 2;
            }
            if (IsUltrawideMonitorPurchased)
            {
                LinesPerClick *= 2;
            }

            LinesPerSecond = 2 * InternCount + 20 * JuniorDeveloperCount;

            InternCost = 50;
            for (int i = 0; i < InternCount; i++)
            {
                InternCost *= 2;
            }

            JuniorDeveloperCost = 2000;
            for (int i = 0; i < JuniorDeveloperCount; i++)
            {
                JuniorDeveloperCost *= 2;
            }
        }

        // Upgrade purchases
        public bool TryPurchaseMechanicalKeyboard()
        {
            if (CanPurchaseMechanicalKeyboard)
            {
                LinesOfCode -= MechanicalKeyboardCost;
                LinesPerClick *= 2;
                IsMechanicalKeyboardPurchased = true;
                return true;
            }
            return false;
        }

        public bool TryPurchaseUltrawideMonitor()
        {
            if (CanPurchaseUltrawideMonitor)
            {
                LinesOfCode -= UltrawideMonitorCost;
                LinesPerClick *= 2;
                IsUltrawideMonitorPurchased = true;
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

        public bool TryPurchaseJuniorDeveloper()
        {
            if (CanPurchaseJuniorDeveloper)
            {
                LinesOfCode -= JuniorDeveloperCost;
                JuniorDeveloperCount += 1;
                LinesPerSecond += 20;
                JuniorDeveloperCost *= 2;
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
