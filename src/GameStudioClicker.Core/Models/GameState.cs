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

    }
}
