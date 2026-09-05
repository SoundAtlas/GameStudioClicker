using GameStudioClicker.Core.Models;

namespace GameStudioClicker.Wpf.ViewModels
{
    public class ActiveUpgradeViewModel : ViewModelBase
    {
        private readonly GameState _gameState;
        private readonly ActiveUpgrade _activeUpgrade;
        public string Id => _activeUpgrade.Id;
        public string DisplayName => _activeUpgrade.DisplayName;
        public string Description => _activeUpgrade.Description;
        public long Cost => _activeUpgrade.Cost;
        public bool IsPurchased => _activeUpgrade.IsPurchased;
        public bool IsAvailable => _activeUpgrade.IsAvailable;
        public bool IsVisible { get; private set; }
        public string RequirementText
        {
            get
            {
                var requirements = new List<string>();

                // Check if the upgrade has a prerequisite that is not purchased
                if (_activeUpgrade.Prerequisite is { IsPurchased: false } prerequistite)
                {
                    requirements.Add($"Requires {prerequistite.DisplayName}.");
                }

                // Check if the upgrade targets a specific worker and that worker has no workers
                if (_activeUpgrade.TargetWorkerId != null)
                {
                    foreach (var workerUpgrade in _gameState.WorkerUpgrades)
                    {
                        if (workerUpgrade.Id == _activeUpgrade.TargetWorkerId &&
                            workerUpgrade.WorkerCount == 0)
                        {
                            requirements.Add($"Requires at least 1 {workerUpgrade.DisplayName}.");
                            break;
                        }
                    }
                }
                // Check if the upgrade targets all workers and that there are no workers at all
                if (_activeUpgrade.TargetAllWorkers)
                {
                    bool hasAnyWorkers = false;
                    foreach (var workerUpgrade in _gameState.WorkerUpgrades)
                    {
                        if (workerUpgrade.WorkerCount > 0)
                        {
                            hasAnyWorkers = true;
                            break;
                        }
                    }
                    if (!hasAnyWorkers)
                    {
                        requirements.Add("Requires at least 1 worker.");
                    }
                }

                return string.Join(Environment.NewLine, requirements);
            }
        }

        public bool HasUnmetRequirement => !string.IsNullOrEmpty(RequirementText);



        // Internal property to access the underlying ActiveUpgrade model
        internal ActiveUpgrade Upgrade => _activeUpgrade;

        public ActiveUpgradeViewModel(GameState gameState, ActiveUpgrade activeUpgrade)
        {
            _gameState = gameState ?? throw new ArgumentNullException(nameof(gameState));
            _activeUpgrade = activeUpgrade ?? throw new ArgumentNullException(nameof(activeUpgrade));
        }

        public void RefreshState()
        {
            OnPropertyChanged(nameof(IsPurchased));
            OnPropertyChanged(nameof(IsAvailable));
            OnPropertyChanged(nameof(HasUnmetRequirement));
            OnPropertyChanged(nameof(RequirementText));
        }

        public void UpdateVisibility(bool isVisible)
        {
            IsVisible = isVisible;
            OnPropertyChanged(nameof(IsVisible));
        }
    }
}
