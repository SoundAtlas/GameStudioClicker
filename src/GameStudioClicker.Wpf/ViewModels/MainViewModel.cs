using GameStudioClicker.Core.Models;
using GameStudioClicker.Wpf.Commands;
using System.Windows.Threading;

namespace GameStudioClicker.Wpf.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly GameState _gameState;

        private readonly DispatcherTimer _passiveTimer;

        public long LinesOfCode => _gameState.LinesOfCode;

        public long LinesPerClick => _gameState.LinesPerClick;

        public long LinesPerSecond => _gameState.LinesPerSecond;


        // LinesPerClick Upgrades

        // Mechanical Keyboard
        public long MechanicalKeyboardCost => _gameState.MechanicalKeyboardCost;

        public int MechanicalKeyboardCount => _gameState.MechanicalKeyboardCount;

        public RelayCommand PurchaseMechanicalKeyboardCommand { get; }

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
            OnPropertyChanged(nameof(MechanicalKeyboardCount));
            OnPropertyChanged(nameof(MechanicalKeyboardCost));

            // Makes WPF check if the command can be executed again, which will update the button's enabled state
            RefreshPurchaseCommands();
        }

        private bool CanExecutePurchaseMechanicalKeyboard(object? parameter)
        {
            return _gameState.CanPurchaseMechanicalKeyboard;
        }


        // LinesPerSecond Upgrades

        // Intern
        public long InternCost => _gameState.InternCost;
        public int InternCount => _gameState.InternCount;

        public RelayCommand PurchaseInternCommand { get; }

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

            RefreshPurchaseCommands();
        }

        private bool CanExecutePurchaseIntern(object? parameter)
        {
            return _gameState.CanPurchaseIntern;
        }

        public RelayCommand WriteCodeCommand { get; }

        // Constructor
        public MainViewModel(GameState gameState)
        {
            // Ensure that the gameState is not null
            _gameState = gameState ?? throw new ArgumentNullException(nameof(gameState));

            // Set up a timer to generate passive lines of code every second
            _passiveTimer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1),
            };
            _passiveTimer.Tick += PassiveTimer_Tick;

            // Set up commands
            WriteCodeCommand = new RelayCommand(ExecuteWriteCode);
            PurchaseMechanicalKeyboardCommand =
                new RelayCommand(
                    ExecutePurchaseMechanicalKeyboard,
                    CanExecutePurchaseMechanicalKeyboard);
            PurchaseInternCommand =
                new RelayCommand(
                    ExecutePurchaseIntern,
                    CanExecutePurchaseIntern);

            // Start the passive generation timer
            _passiveTimer.Start();
        }

        private void PassiveTimer_Tick(object? sender, EventArgs e)
        {
            _gameState.GeneratePassiveLines();
            OnPropertyChanged(nameof(LinesOfCode));
            RefreshPurchaseCommands();
        }

        private void ExecuteWriteCode(object? parameter)
        {
            _gameState.WriteCode();
            OnPropertyChanged(nameof(LinesOfCode));

            RefreshPurchaseCommands();
        }

        // Helpers
        private void RefreshPurchaseCommands()
        {
            PurchaseMechanicalKeyboardCommand.RaiseCanExecuteChanged();
            PurchaseInternCommand.RaiseCanExecuteChanged();
        }
    }
}
