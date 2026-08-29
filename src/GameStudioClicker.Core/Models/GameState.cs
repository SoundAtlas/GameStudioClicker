using GameStudioClicker.Core.Persistence;

namespace GameStudioClicker.Core.Models
{
    public class GameState
    {
        // Create a GameSaveData snapshot for saving the game state
        public GameSaveData CreateSaveData()
        {
            return new GameSaveData
            {
                LinesOfCode = this.LinesOfCode,
                MechanicalKeyboardCount = this.MechanicalKeyboardCount,
                InternCount = this.InternCount
            };
        }

        public void RestoreFromSaveData(GameSaveData saveData)
        {
            // reject null saveData
            if (saveData == null)
            {
                throw new ArgumentNullException(nameof(saveData), "Save data cannot be null.");
            }

            // restore the game state from the provided GameSaveData

            LinesOfCode = Math.Max(0L, saveData.LinesOfCode);
            MechanicalKeyboardCount = Math.Max(0, saveData.MechanicalKeyboardCount);
            InternCount = Math.Max(0, saveData.InternCount);

            // recalulate derived values

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

        public long LinesOfCode { get; private set; }
        public long LinesPerClick { get; private set; } = 1;
        public long LinesPerSecond { get; private set; } = 0;

        // LinesPerClick Upgrades

        // Mechanical Keyboard
        public long MechanicalKeyboardCost { get; private set; } = 25;
        public int MechanicalKeyboardCount { get; private set; } = 0;
        public bool CanPurchaseMechanicalKeyboard
        {
            get
            {
                return LinesOfCode >= MechanicalKeyboardCost;
            }
        }

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

        // LinesPerSecond Upgrades

        // Intern
        public long InternCost { get; private set; } = 50;
        public int InternCount { get; private set; } = 0;
        public bool CanPurchaseIntern
        {
            get
            {
                return LinesOfCode >= InternCost;
            }
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
    }
}
