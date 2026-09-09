using GameStudioClicker.Core.Models;

namespace GameStudioClicker.Wpf.ViewModels
{
    public class AchievementViewModel : ViewModelBase
    {
        private readonly Achievement _achievement;

        public AchievementViewModel(Achievement achievement)
        {
            _achievement = achievement ?? throw new ArgumentNullException(nameof(achievement));
        }

        public string Id => _achievement.Id;
        public string DisplayName => _achievement.DisplayName;
        public string Description => _achievement.Description;
        public long RequirementValue => _achievement.RequirementValue;
        public bool IsEarned => _achievement.IsEarned;


        public void RefreshState()
        {
            OnPropertyChanged(nameof(IsEarned));
        }
    }
}
