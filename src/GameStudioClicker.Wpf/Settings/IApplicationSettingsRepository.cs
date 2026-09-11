namespace GameStudioClicker.Wpf.Settings
{
    public interface IApplicationSettingsRepository
    {
        ApplicationSettings Load();
        void Save(ApplicationSettings saveSettings);
    }
}
