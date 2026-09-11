using GameStudioClicker.Wpf.Services;
using System.Windows;
using System.Windows.Controls;

namespace GameStudioClicker.Wpf.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        private readonly AudioService _audioService;

        private readonly App _app;
        private bool isInitializing = true;

        public SettingsView()
        {
            _app = (App)Application.Current;
            _audioService = _app.AudioService;

            InitializeComponent();

            MusicVolumeSlider.Value = _app.ApplicationSettings.MusicVolumePercent;
            SFXVolumeSlider.Value = _app.ApplicationSettings.SfxVolumePercent;

            isInitializing = false;
        }

        private void MusicVolumeSlider_ValueChanged(
            object sender,
            RoutedPropertyChangedEventArgs<double> e)
        {
            if (isInitializing)
            {
                return;
            }

            _audioService.SetMusicVolume(e.NewValue);

            _app.ApplicationSettings.MusicVolumePercent =
                Convert.ToInt32(e.NewValue);

            _app.SaveSettings();
        }
        private void SFXVolumeSlider_ValueChanged(
            object sender,
            RoutedPropertyChangedEventArgs<double> e)
        {
            if (isInitializing)
            {
                return;
            }

            _audioService.SetSfxVolume(e.NewValue);

            _app.ApplicationSettings.SfxVolumePercent =
                Convert.ToInt32(e.NewValue);

            _app.SaveSettings();
        }
    }
}
