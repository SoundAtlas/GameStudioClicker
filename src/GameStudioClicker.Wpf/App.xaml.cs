using GameStudioClicker.Wpf.Services;
using GameStudioClicker.Wpf.Settings;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;



namespace GameStudioClicker.Wpf;

public partial class App : Application
{
    public AudioService AudioService { get; } = new();
    private IApplicationSettingsRepository? _applicationSettingsRepository;
    public ApplicationSettings ApplicationSettings { get; private set; } = new();

    protected override void OnStartup(StartupEventArgs e)
    {
        EventManager.RegisterClassHandler(
            typeof(Button),
            Button.MouseEnterEvent,
            new MouseEventHandler(Button_MouseEnter));

        base.OnStartup(e);

        string directoryPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "GameStudioClicker");
        string settingsFilePath = Path.Combine(
            directoryPath,
            "settings.json");

        _applicationSettingsRepository =
            new JsonApplicationSettingsRepository(settingsFilePath);
        ApplicationSettings = _applicationSettingsRepository.Load();

        AudioService.SetMusicVolume(ApplicationSettings.MusicVolumePercent);
        AudioService.SetSfxVolume(ApplicationSettings.SfxVolumePercent);
        AudioService.StartSoundtrack();
    }

    private void Button_MouseEnter(object? sender, MouseEventArgs e)
    {
        if (sender is Button { IsEnabled: true })
        {
            AudioService.PlayButtonHoverSound();
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        SaveSettings();
        AudioService.Close();
        base.OnExit(e);
    }

    public void SaveSettings()
    {
        _applicationSettingsRepository?.Save(ApplicationSettings);
    }
}
