using GameStudioClicker.Wpf.Services;
using System.Windows;

namespace GameStudioClicker.Wpf;

public partial class App : Application
{
    public AudioService AudioService { get; } = new();

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        AudioService.StartSoundtrack();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        AudioService.Close();
        base.OnExit(e);
    }
}
