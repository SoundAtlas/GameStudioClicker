namespace GameStudioClicker.Core.Models
{
    public class GameState
    {
        public long LinesOfCode { get; private set; }
        public void WriteCode()
        {
            LinesOfCode += 1;
        }
    }
}
