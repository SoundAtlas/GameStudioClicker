using GameStudioClicker.Core.Models;
using GameStudioClicker.Wpf.Commands;
using System.Windows.Threading;

namespace GameStudioClicker.Wpf.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

        // Game state and timers
        private readonly GameState _gameState;
        private readonly DispatcherTimer _passiveTimer;
        private readonly DispatcherTimer _offlineMessageTimer;
        private bool _showOfflineEarnings;

        // Production displayed by the main coding panel
        public long LinesOfCode => _gameState.LinesOfCode;
        public long LinesPerClick => _gameState.LinesPerClick;
        public long LinesPerSecond => _gameState.LinesPerSecond;

        // Offline earnings notification
        public bool HasOfflineEarnings => _showOfflineEarnings;
        public long OfflineLinesEarned { get; }
        public string OfflineEarningsMessage =>
            $"Your interns & Employees wrote {OfflineLinesEarned} lines of code while you were away!";

        public IReadOnlyList<ActiveUpgradeViewModel> ActiveUpgrades { get; }
        public IReadOnlyList<WorkerUpgradeViewModel> WorkerUpgrades { get; }

        // Commands exposed to the view
        public RelayCommand WriteCodeCommand { get; }
        public RelayCommand PurchaseActiveUpgradeCommand { get; }
        public RelayCommand PurchaseWorkerUpgradeCommand { get; }

        // Construction and setup
        public MainViewModel(GameState gameState, long offlineLinesEarned = 0)
        {
            _gameState = gameState ?? throw new ArgumentNullException(nameof(gameState));

            // Create view models for each active upgrade to expose to the view
            List<ActiveUpgradeViewModel> activeUpgradeViewModels = new List<ActiveUpgradeViewModel>();
            foreach (var upgrade in _gameState.ActiveUpgrades)
            {
                activeUpgradeViewModels.Add(new ActiveUpgradeViewModel(upgrade));
            }
            ActiveUpgrades = activeUpgradeViewModels;

            // Create view models for each worker upgrade to expose to the view
            List<WorkerUpgradeViewModel> workerUpgradeViewModels = new List<WorkerUpgradeViewModel>();
            foreach (var upgrade in _gameState.WorkerUpgrades)
            {
                workerUpgradeViewModels.Add(new WorkerUpgradeViewModel(upgrade));
            }
            WorkerUpgrades = workerUpgradeViewModels;

            // Offline earnings are only shown if the player earned more than 0 lines of code while away.
            OfflineLinesEarned = Math.Max(0, offlineLinesEarned);
            _showOfflineEarnings = OfflineLinesEarned > 0;

            // This timer hides the one-time offline earnings notification.
            _offlineMessageTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(7)
            };
            _offlineMessageTimer.Tick += OfflineMessageTimer_Tick;

            // This timer advances passive production while the game is running.
            _passiveTimer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1),
            };
            _passiveTimer.Tick += PassiveTimer_Tick;

            WriteCodeCommand = new RelayCommand(ExecuteWriteCode);
            PurchaseActiveUpgradeCommand =
                new RelayCommand(
                    ExecutePurchaseActiveUpgrade,
                    CanExecutePurchaseActiveUpgrade);
            PurchaseWorkerUpgradeCommand =
                new RelayCommand(
                    ExecutePurchaseWorkerUpgrade,
                    CanExecutePurchaseWorkerUpgrade);

            if (_showOfflineEarnings)
            {
                _offlineMessageTimer.Start();
            }

            _passiveTimer.Start();
        }

        private bool CanExecutePurchaseWorkerUpgrade(object? parameter)
        {
            if (parameter is WorkerUpgradeViewModel upgradeViewModel)
            {
                return _gameState.CanPurchaseWorkerUpgrade(upgradeViewModel.Upgrade);
            }
            return false;
        }

        private void ExecutePurchaseWorkerUpgrade(object? parameter)
        {
            if (parameter is WorkerUpgradeViewModel upgradeViewModel &&
                _gameState.TryPurchaseWorkerUpgrade(upgradeViewModel.Upgrade))
            {

                OnPropertyChanged(nameof(LinesOfCode));
                OnPropertyChanged(nameof(LinesPerSecond));

                RefreshWorkerUpgradeStates();
                RefreshPurchaseCommands();

            }
        }

        private bool CanExecutePurchaseActiveUpgrade(object? parameter)
        {
            if (parameter is ActiveUpgradeViewModel upgradeViewModel)
            {
                return _gameState.CanPurchaseActiveUpgrade(upgradeViewModel.Upgrade);
            }
            return false;
        }

        private void ExecutePurchaseActiveUpgrade(object? parameter)
        {
            if (parameter is ActiveUpgradeViewModel upgradeViewModel &&
                _gameState.TryPurchaseActiveUpgrade(upgradeViewModel.Upgrade))
            {
                OnPropertyChanged(nameof(LinesOfCode));
                OnPropertyChanged(nameof(LinesPerClick));

                RefreshActiveUpgradeStates();
                RefreshPurchaseCommands();
            }
        }

        // Timer handlers
        private void OfflineMessageTimer_Tick(object? sender, EventArgs e)
        {
            _offlineMessageTimer.Stop();

            _showOfflineEarnings = false;
            OnPropertyChanged(nameof(HasOfflineEarnings));
        }

        private void PassiveTimer_Tick(object? sender, EventArgs e)
        {
            _gameState.GeneratePassiveLines();
            OnPropertyChanged(nameof(LinesOfCode));
            RefreshPurchaseCommands();
        }

        // Manual production command handler
        private void ExecuteWriteCode(object? parameter)
        {
            _gameState.WriteCode();
            OnPropertyChanged(nameof(LinesOfCode));

            RefreshPurchaseCommands();
        }

        // Command state helpers
        private void RefreshPurchaseCommands()
        {
            PurchaseActiveUpgradeCommand.RaiseCanExecuteChanged();
            PurchaseWorkerUpgradeCommand.RaiseCanExecuteChanged();
        }

        private void RefreshActiveUpgradeStates()
        {
            foreach (var upgradeViewModel in ActiveUpgrades)
            {
                upgradeViewModel.RefreshState();
            }
        }
        private void RefreshWorkerUpgradeStates()
        {
            foreach (var upgradeViewModel in WorkerUpgrades)
            {
                upgradeViewModel.RefreshState();
            }
        }
    }
}
