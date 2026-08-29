namespace GameStudioClicker.Core.Persistence
{
    public class GameSaveData
    {
        // Only persistent values belong here; production rates and costs are recalculated when loading.
        public long LinesOfCode { get; set; }
        public int MechanicalKeyboardCount { get; set; }
        public int InternCount { get; set; }

        // UTC avoids timezone and daylight-saving changes in offline progress calculations.
        public DateTime SavedAtUtc { get; set; }
    }
}
