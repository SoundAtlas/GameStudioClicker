using GameStudioClicker.Core.Models;
using GameStudioClicker.Wpf.Formatting;

namespace GameStudioClicker.Wpf.ViewModels
{
    public class AchievementViewModel : ViewModelBase
    {
        private readonly GameState _gameState;
        private readonly Achievement _achievement;

        public AchievementViewModel(GameState gameState, Achievement achievement)
        {
            _gameState = gameState ?? throw new ArgumentNullException(nameof(gameState));
            _achievement = achievement ?? throw new ArgumentNullException(nameof(achievement));
        }

        public string Id => _achievement.Id;
        public string DisplayName => _achievement.DisplayName;
        public string Description => _achievement.Description;
        public long RequirementValue => _achievement.RequirementValue;
        public bool IsEarned => _achievement.IsEarned;
        public bool IsSecret => _achievement.IsSecret;
        public long CurrentProgress =>
            Math.Min(_gameState.GetAchievementProgress(_achievement), RequirementValue);
        public string ProgressText =>
            $"{CompactNumberFormatter.Format(CurrentProgress)} / " +
            $"{CompactNumberFormatter.Format(RequirementValue)}";

        public void RefreshState()
        {
            OnPropertyChanged(nameof(IsEarned));
            OnPropertyChanged(nameof(CurrentProgress));
            OnPropertyChanged(nameof(ProgressText));
        }
    }
}
