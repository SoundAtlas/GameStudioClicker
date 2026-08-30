using GameStudioClicker.Core.Models;

namespace GameStudioClicker.Wpf.ViewModels
{
    public class ActiveUpgradeViewModel : ViewModelBase
    {
        private readonly ActiveUpgrade _activeUpgrade;
        public string Id => _activeUpgrade.Id;
        public string DisplayName => _activeUpgrade.DisplayName;
        public string Description => _activeUpgrade.Description;
        public long Cost => _activeUpgrade.Cost;
        public bool IsPurchased => _activeUpgrade.IsPurchased;
        public bool IsAvailable => _activeUpgrade.IsAvailable;
        public bool IsVisible { get; private set; }

        // Internal property to access the underlying ActiveUpgrade model
        internal ActiveUpgrade Upgrade => _activeUpgrade;

        public ActiveUpgradeViewModel(ActiveUpgrade activeUpgrade)
        {
            _activeUpgrade = activeUpgrade ?? throw new ArgumentNullException(nameof(activeUpgrade));
        }

        public void RefreshState()
        {
            OnPropertyChanged(nameof(IsPurchased));
            OnPropertyChanged(nameof(IsAvailable));
        }

        public void UpdateVisibility(bool isVisible)
        {
            IsVisible = isVisible;
            OnPropertyChanged(nameof(IsVisible));
        }
    }
}
