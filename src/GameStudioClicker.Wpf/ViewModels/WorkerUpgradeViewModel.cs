using GameStudioClicker.Core.Models;
using GameStudioClicker.Wpf.Formatting;

namespace GameStudioClicker.Wpf.ViewModels;

public class WorkerUpgradeViewModel : ViewModelBase
{
    private readonly GameState _gameState;
    private readonly WorkerUpgrade _workerUpgrade;

    public WorkerUpgradeViewModel(GameState gameState, WorkerUpgrade workerUpgrade)
    {
        _gameState = gameState ?? throw new ArgumentNullException(nameof(gameState));
        _workerUpgrade = workerUpgrade ?? throw new ArgumentNullException(nameof(workerUpgrade));
    }

    // Display and production values exposed to the worker-card template.
    public string Id => _workerUpgrade.Id;
    public string DisplayName => _workerUpgrade.DisplayName;
    public string Description =>
        $"Produces {CompactNumberFormatter.Format(LinesPerSecondPerEmployee)} lines of code / second";
    public string? PortraitPath => Id switch
    {
        "intern" => "/Assets/Workers/worker_intern.png",
        "junior_developer" => "/Assets/Workers/worker_junior_developer.png",
        "senior_developer" => "/Assets/Workers/worker_senior_developer.png",
        "lead_developer" => "/Assets/Workers/worker_lead_developer.png",
        "engineering_manager" => "/Assets/Workers/worker_engineering_manager.png",
        "studio_director" => "/Assets/Workers/worker_studio_director.png",
        _ => null
    };
    public long CurrentCost => _workerUpgrade.CurrentCost;
    public int WorkerCount => _workerUpgrade.WorkerCount;
    public long LinesPerSecondPerEmployee =>
        _gameState.GetWorkerLinesPerSecondPerEmployee(_workerUpgrade);
    public long CurrentLinesPerSecond =>
        _gameState.GetWorkerLinesPerSecond(_workerUpgrade);
    public long TotalLinesPerSecond => _workerUpgrade.TotalLinesPerSecond;

    // Unlock state for regular and mystery worker cards.
    public bool IsUnlocked => _workerUpgrade.IsUnlocked;
    public bool IsVisible => _workerUpgrade.IsVisible;
    public bool IsMystery => _workerUpgrade.IsMystery;
    public string UnlockRequirementText
    {
        get
        {
            WorkerUpgrade? prerequisite = _workerUpgrade.Prerequisite;

            if (prerequisite is null)
            {
                return string.Empty;
            }

            if (_workerUpgrade.IsUnlocked)
            {
                return "Hire to reveal";
            }

            return $"Requires {_workerUpgrade.RequiredPrerequisiteCount} × " +
                   $"{prerequisite.DisplayName} " +
                   $"({prerequisite.WorkerCount}/" +
                   $"{_workerUpgrade.RequiredPrerequisiteCount})";
        }
    }

    internal WorkerUpgrade Upgrade => _workerUpgrade;

    public void RefreshState()
    {
        OnPropertyChanged(nameof(CurrentCost));
        OnPropertyChanged(nameof(WorkerCount));
        OnPropertyChanged(nameof(TotalLinesPerSecond));
        OnPropertyChanged(nameof(IsUnlocked));
        OnPropertyChanged(nameof(IsVisible));
        OnPropertyChanged(nameof(IsMystery));
        OnPropertyChanged(nameof(UnlockRequirementText));
        OnPropertyChanged(nameof(CurrentLinesPerSecond));
        OnPropertyChanged(nameof(LinesPerSecondPerEmployee));
        OnPropertyChanged(nameof(Description));
    }
}
