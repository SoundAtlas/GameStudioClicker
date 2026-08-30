namespace GameStudioClicker.Core.Models
{
    public class WorkerUpgrade
    {
        public string Id { get; }
        public string DisplayName { get; }
        public string Description { get; }
        public long BaseCost { get; }
        public long BaseLinesPerSecond { get; }
        public long CurrentCost { get; private set; }
        public int WorkerCount { get; private set; }
        public long TotalLinesPerSecond => WorkerCount * BaseLinesPerSecond;
        public bool IsUnlocked => Prerequisite == null || Prerequisite.WorkerCount >= RequiredPrerequisiteCount;
        public bool IsVisible => IsUnlocked || Prerequisite?.IsUnlocked == true;
        public bool IsMystery => !IsUnlocked && IsVisible;
        public WorkerUpgrade? Prerequisite { get; }
        public int RequiredPrerequisiteCount { get; }


        public WorkerUpgrade(string id, string displayName, string description, long baseCost, long baseLinesPerSecond, WorkerUpgrade? prerequisite = null, int requiredPrerequisiteCount = 0)
        {
            Id = id;
            DisplayName = displayName;
            Description = description;
            BaseCost = baseCost;
            BaseLinesPerSecond = baseLinesPerSecond;
            CurrentCost = baseCost;
            WorkerCount = 0;
            Prerequisite = prerequisite;
            RequiredPrerequisiteCount = requiredPrerequisiteCount;
        }

        public void AddWorker()
        {
            WorkerCount++;
            CurrentCost *= 2;
        }

        public void RestoreWorkerCount(int workerCount)
        {
            WorkerCount = 0;
            CurrentCost = BaseCost;

            // clamp workerCount to a minimum of 0
            if (workerCount < 0)
            {
                workerCount = 0;
            }

            for (int i = 0; i < workerCount; i++)
            {
                AddWorker();
            }
        }
    }
}
