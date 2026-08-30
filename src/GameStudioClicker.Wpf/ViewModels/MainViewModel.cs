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

        // Mechanical keyboard upgrade
        public long MechanicalKeyboardCost => _gameState.MechanicalKeyboardCost;
        public bool IsMechanicalKeyboardAvailable => _gameState.IsMechanicalKeyboardAvailable;

        // Ultrawide monitor upgrade
        public long UltrawideMonitorCost => _gameState.UltrawideMonitorCost;
        public bool IsUltrawideMonitorAvailable => _gameState.IsUltrawideMonitorAvailable;

        // Intern upgrade
        public long InternCost => _gameState.InternCost;
        public int InternCount => _gameState.InternCount;

        // Junior developer upgrade
        public long JuniorDeveloperCost => _gameState.JuniorDeveloperCost;
        public int JuniorDeveloperCount => _gameState.JuniorDeveloperCount;
        public bool IsJuniorDeveloperUnlocked => _gameState.IsJuniorDeveloperUnlocked;

        // Commands exposed to the view
        public RelayCommand WriteCodeCommand { get; }
        public RelayCommand PurchaseMechanicalKeyboardCommand { get; }
        public RelayCommand PurchaseUltrawideMonitorCommand { get; }
        public RelayCommand PurchaseJuniorDeveloperCommand { get; }
        public RelayCommand PurchaseInternCommand { get; }

        // Construction and setup
        public MainViewModel(GameState gameState, long offlineLinesEarned = 0)
        {
            _gameState = gameState ?? throw new ArgumentNullException(nameof(gameState));

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
            PurchaseMechanicalKeyboardCommand =
                new RelayCommand(
                    ExecutePurchaseMechanicalKeyboard,
                    CanExecutePurchaseMechanicalKeyboard);
            PurchaseUltrawideMonitorCommand =
                new RelayCommand(
                    ExecutePurchaseUltrawideMonitor,
                    CanExecutePurchaseUltrawideMonitor);
            PurchaseInternCommand =
                new RelayCommand(
                    ExecutePurchaseIntern,
                    CanExecutePurchaseIntern);
            PurchaseJuniorDeveloperCommand =
                new RelayCommand(
                    ExecutePurchaseJuniorDeveloper,
                    CanExecutePurchaseJuniorDeveloper);

            if (_showOfflineEarnings)
            {
                _offlineMessageTimer.Start();
            }

            _passiveTimer.Start();
        }


        // Mechanical keyboard command handlers

        private void ExecutePurchaseMechanicalKeyboard(object? parameter)
        {
            bool purchaseSuccessful =
                _gameState.TryPurchaseMechanicalKeyboard();

            if (!purchaseSuccessful)
            {
                return;
            }

            OnPropertyChanged(nameof(LinesOfCode));
            OnPropertyChanged(nameof(LinesPerClick));
            OnPropertyChanged(nameof(IsMechanicalKeyboardAvailable));
            OnPropertyChanged(nameof(IsUltrawideMonitorAvailable));

            // Makes WPF check if the command can be executed again, which will update the button's enabled state
            RefreshPurchaseCommands();
        }

        private bool CanExecutePurchaseMechanicalKeyboard(object? parameter)
        {
            return _gameState.CanPurchaseMechanicalKeyboard;
        }


        // Ultrawide monitor command handlers

        private void ExecutePurchaseUltrawideMonitor(object? parameter)
        {
            bool purchaseSuccessful =
                _gameState.TryPurchaseUltrawideMonitor();

            if (!purchaseSuccessful)
            {
                return;
            }

            OnPropertyChanged(nameof(LinesOfCode));
            OnPropertyChanged(nameof(LinesPerClick));
            OnPropertyChanged(nameof(IsUltrawideMonitorAvailable));

            RefreshPurchaseCommands();
        }
        private bool CanExecutePurchaseUltrawideMonitor(object? parameter)
        {
            return _gameState.CanPurchaseUltrawideMonitor;
        }

        // Intern command handlers

        private void ExecutePurchaseIntern(object? parameter)
        {
            bool purchaseSuccessful =
                _gameState.TryPurchaseIntern();

            if (!purchaseSuccessful)
            {
                return;
            }

            OnPropertyChanged(nameof(LinesOfCode));
            OnPropertyChanged(nameof(LinesPerSecond));
            OnPropertyChanged(nameof(InternCost));
            OnPropertyChanged(nameof(InternCount));
            OnPropertyChanged(nameof(IsJuniorDeveloperUnlocked));

            RefreshPurchaseCommands();
        }

        private bool CanExecutePurchaseIntern(object? parameter)
        {
            return _gameState.CanPurchaseIntern;
        }

        // Junior developer command handlers
        private void ExecutePurchaseJuniorDeveloper(object? parameter)
        {
            bool purchaseSuccessful =
                _gameState.TryPurchaseJuniorDeveloper();

            if (!purchaseSuccessful)
            {
                return;
            }

            OnPropertyChanged(nameof(LinesOfCode));
            OnPropertyChanged(nameof(LinesPerSecond));
            OnPropertyChanged(nameof(JuniorDeveloperCost));
            OnPropertyChanged(nameof(JuniorDeveloperCount));

            RefreshPurchaseCommands();
        }
        private bool CanExecutePurchaseJuniorDeveloper(object? parameter)
        {
            return _gameState.CanPurchaseJuniorDeveloper;
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
            PurchaseMechanicalKeyboardCommand.RaiseCanExecuteChanged();
            PurchaseUltrawideMonitorCommand.RaiseCanExecuteChanged();

            PurchaseInternCommand.RaiseCanExecuteChanged();
            PurchaseJuniorDeveloperCommand.RaiseCanExecuteChanged();
        }
    }
}
