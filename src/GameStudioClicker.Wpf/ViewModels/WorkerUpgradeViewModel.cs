using GameStudioClicker.Core.Models;
using GameStudioClicker.Wpf.Formatting;

namespace GameStudioClicker.Wpf.ViewModels
{
    public class WorkerUpgradeViewModel : ViewModelBase
    {
        private readonly GameState _gameState;
        private readonly WorkerUpgrade _workerUpgrade;
        public string Id => _workerUpgrade.Id;
        public string DisplayName => _workerUpgrade.DisplayName;
        public string Description =>
            $"Produces {CompactNumberFormatter.Format(LinesPerSecondPerEmployee)} lines of code / second";
        public long CurrentCost => _workerUpgrade.CurrentCost;
        public long CurrentLinesPerSecond =>
            _gameState.GetWorkerLinesPerSecond(_workerUpgrade);
        public int WorkerCount => _workerUpgrade.WorkerCount;
        public long LinesPerSecondPerEmployee =>
            _gameState.GetWorkerLinesPerSecondPerEmployee(_workerUpgrade);
        public long TotalLinesPerSecond => _workerUpgrade.TotalLinesPerSecond;
        public bool IsUnlocked => _workerUpgrade.IsUnlocked;
        public bool IsVisible => _workerUpgrade.IsVisible;
        public bool IsMystery => _workerUpgrade.IsMystery;
        public string UnlockRequirementText => _workerUpgrade.Prerequisite == null
            ? string.Empty
            : $"Requires {_workerUpgrade.RequiredPrerequisiteCount} × " +
              $"{_workerUpgrade.Prerequisite.DisplayName} " +
              $"({_workerUpgrade.Prerequisite.WorkerCount}/" +
              $"{_workerUpgrade.RequiredPrerequisiteCount})";

        internal WorkerUpgrade Upgrade => _workerUpgrade;

        public WorkerUpgradeViewModel(GameState gameState, WorkerUpgrade workerUpgrade)
        {
            _gameState = gameState ?? throw new ArgumentNullException(nameof(gameState));
            _workerUpgrade = workerUpgrade ?? throw new ArgumentNullException(nameof(workerUpgrade));
        }

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
}
