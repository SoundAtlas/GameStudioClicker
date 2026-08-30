using GameStudioClicker.Core.Models;

namespace GameStudioClicker.Wpf.ViewModels
{
    public class WorkerUpgradeViewModel : ViewModelBase
    {
        private readonly WorkerUpgrade _workerUpgrade;
        public string Id => _workerUpgrade.Id;
        public string DisplayName => _workerUpgrade.DisplayName;
        public string Description => _workerUpgrade.Description;
        public long CurrentCost => _workerUpgrade.CurrentCost;
        public int WorkerCount => _workerUpgrade.WorkerCount;
        public long TotalLinesPerSecond => _workerUpgrade.TotalLinesPerSecond;
        public bool IsUnlocked => _workerUpgrade.IsUnlocked;
        public bool IsVisible => _workerUpgrade.IsVisible;
        public bool IsMystery => _workerUpgrade.IsMystery;


        internal WorkerUpgrade Upgrade => _workerUpgrade;

        public WorkerUpgradeViewModel(WorkerUpgrade workerUpgrade)
        {
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
        }
    }
}
