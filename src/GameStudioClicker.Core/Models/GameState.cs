namespace GameStudioClicker.Core.Models
{
    public class GameState
    {
        public long LinesOfCode { get; private set; }
        public long LinesPerClick { get; private set; } = 1;
        public long LinesPerSecond { get; private set; } = 0;

        // LinesPerClick Upgrades

        // Mechanical Keyboard
        public long MechanicalKeyboardCost { get; private set; } = 10;
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
        public long InternCost { get; private set; } = 100;
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
                LinesPerSecond += 1;
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
