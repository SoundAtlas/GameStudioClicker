namespace GameStudioClicker.Core.Models
{
    public class ActiveUpgrade
    {
        public string Id { get; }
        public string DisplayName { get; }
        public string Description { get; }
        public long Cost { get; }
        public int ClickMultiplier { get; }
        public int WorkerProductionMultiplier { get; }
        public bool IsPurchased { get; private set; }
        public bool IsUnlocked => Prerequisite == null || Prerequisite.IsPurchased;
        public bool IsAvailable => IsUnlocked && !IsPurchased;

        public ActiveUpgrade? Prerequisite { get; }
        public string? TargetWorkerId { get; }

        public ActiveUpgrade(string id, string displayName, string description, long cost, int clickMultiplier, int workerProductionMultiplier = 1, string? targetWorkerId = null, ActiveUpgrade? prerequisite = null)
        {
            Id = id;
            DisplayName = displayName;
            Description = description;
            Cost = cost;
            ClickMultiplier = clickMultiplier;
            WorkerProductionMultiplier = workerProductionMultiplier;
            TargetWorkerId = targetWorkerId;
            IsPurchased = false;
            Prerequisite = prerequisite;
        }

        public void MarkAsPurchased()
        {
            IsPurchased = true;
        }

        public void RestorePurchaseState(bool isPurchased)
        {
            IsPurchased = isPurchased;
        }
    }
}
