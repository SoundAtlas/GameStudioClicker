using GameStudioClicker.Core.Models;
using GameStudioClicker.Wpf.Commands;

namespace GameStudioClicker.Wpf.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly GameState _gameState;

        public long LinesOfCode
        {
            get { return _gameState.LinesOfCode; }
        }

        public long LinesPerClick
        {
            get { return _gameState.LinesPerClick; }
        }

        public long MechanicalKeyboardCost
        {
            get { return _gameState.MechanicalKeyboardCost; }
        }

        public bool IsMechanicalKeyboardOwned
        {
            get { return _gameState.IsMechanicalKeyboardOwned; }
        }

        public RelayCommand WriteCodeCommand { get; }
        public RelayCommand PurchaseMechanicalKeyboardCommand { get; }

        public MainViewModel(GameState gameState)
        {
            _gameState = gameState ?? throw new ArgumentNullException(nameof(gameState));

            WriteCodeCommand = new RelayCommand(ExecuteWriteCode);
            PurchaseMechanicalKeyboardCommand =
                new RelayCommand(
                    ExecutePurchaseMechanicalKeyboard,
                    CanExecutePurchaseMechanicalKeyboard);
        }

        private void ExecuteWriteCode(object? parameter)
        {
            _gameState.WriteCode();
            OnPropertyChanged(nameof(LinesOfCode));

            PurchaseMechanicalKeyboardCommand.RaiseCanExecuteChanged();
        }

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
            OnPropertyChanged(nameof(IsMechanicalKeyboardOwned));

            // Makes WPF check if the command can be executed again, which will update the button's enabled state
            PurchaseMechanicalKeyboardCommand.RaiseCanExecuteChanged();
        }

        private bool CanExecutePurchaseMechanicalKeyboard(object? parameter)
        {
            return _gameState.CanPurchaseMechanicalKeyboard;
        }
    }
}
