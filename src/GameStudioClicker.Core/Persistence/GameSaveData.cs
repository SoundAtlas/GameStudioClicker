namespace GameStudioClicker.Core.Persistence
{
    public class GameSaveData
    {
        // Only persistent values belong here; production rates and costs are recalculated when loading.
        public long LinesOfCode { get; set; }
        public long LifetimeLinesOfCode { get; set; }
        public long LifetimeManualClicks { get; set; }
        public long LifetimeEmployeesHired { get; set; }
        public long LifetimeActiveUpgradesPurchased { get; set; }
        public long LinesGeneratedManually { get; set; }
        public long LinesGeneratedByWorkers { get; set; }
        public long LinesGeneratedWhileOnline { get; set; }
        public long LinesGeneratedWhileOffline { get; set; }
        public List<string> PurchasedActiveUpgradeIds { get; set; } = new List<string>();
        public Dictionary<string, int> WorkerUpgradeCounts { get; set; } = new Dictionary<string, int>();
        public List<string> EarnedAchievementIds { get; set; } = new List<string>();

        // UTC avoids timezone and daylight-saving changes in offline progress calculations.
        public DateTime SavedAtUtc { get; set; }
    }
}
