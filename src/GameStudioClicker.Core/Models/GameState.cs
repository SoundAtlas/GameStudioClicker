namespace GameStudioClicker.Core.Models
{
    public class GameState
    {
        public long LinesOfCode { get; private set; }
        public long LinesPerClick { get; private set; } = 1;
        public long MechanicalKeyboardCost { get; } = 10;
        public bool IsMechanicalKeyboardOwned { get; private set; }
        public bool CanPurchaseMechanicalKeyboard
        {
            get
            {
                return !IsMechanicalKeyboardOwned &&
                    LinesOfCode >= MechanicalKeyboardCost;
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
                IsMechanicalKeyboardOwned = true;
                LinesPerClick += 1;
                return true;
            }
            return false;
        }
    }
}
