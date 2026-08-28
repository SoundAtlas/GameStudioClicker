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

        public MainViewModel(GameState gameState)
        {
            _gameState = gameState ?? throw new ArgumentNullException(nameof(gameState));

            WriteCodeCommand = new RelayCommand(ExecuteWriteCode);
        }

        private void ExecuteWriteCode(object? parameter)
        {
            _gameState.WriteCode();
            OnPropertyChanged(nameof(LinesOfCode));
        }
    }
}
