namespace GameStudioClicker.Core.Models;

public class WorkerUpgrade
{
    public WorkerUpgrade(
        string id,
        string displayName,
        long baseCost,
        long baseLinesPerSecond,
        WorkerUpgrade? prerequisite = null,
        int requiredPrerequisiteCount = 0)
    {
        Id = id;
        DisplayName = displayName;
        BaseCost = baseCost;
        BaseLinesPerSecond = baseLinesPerSecond;
        CurrentCost = baseCost;
        WorkerCount = 0;
        Prerequisite = prerequisite;
        RequiredPrerequisiteCount = requiredPrerequisiteCount;
    }

    // Identity and base economy values
    public string Id { get; }
    public string DisplayName { get; }
    public long BaseCost { get; }
    public long BaseLinesPerSecond { get; }

    // Current ownership and cost
    public long CurrentCost { get; private set; }
    public int WorkerCount { get; private set; }
    public long TotalLinesPerSecond => WorkerCount * BaseLinesPerSecond;

    // Unlock progression
    public WorkerUpgrade? Prerequisite { get; }
    public int RequiredPrerequisiteCount { get; }
    public bool IsUnlocked =>
        Prerequisite == null ||
        Prerequisite.WorkerCount >= RequiredPrerequisiteCount;
    public bool IsVisible => IsUnlocked || Prerequisite?.IsUnlocked == true;
    public bool IsMystery => !IsUnlocked && IsVisible;

    public void AddWorker()
    {
        WorkerCount++;
        CurrentCost *= 2;
    }

    public void RestoreWorkerCount(int workerCount)
    {
        WorkerCount = 0;
        CurrentCost = BaseCost;

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
