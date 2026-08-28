namespace GameStudioClicker.Core.Models
{
    public class GameState
    {
        public long LinesOfCode { get; private set; }
        public long LinesPerClick { get; private set; } = 1;
        public long MechanicalKeyboardCost { get; private set; } = 10;
        public int MechanicalKeyboardCount { get; private set; } = 0;
        public bool CanPurchaseMechanicalKeyboard
        {
            get
            {
                return LinesOfCode >= MechanicalKeyboardCost;
            }
        }

        public void WriteCode()
        {
            LinesOfCode += LinesPerClick;
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
    }
}
