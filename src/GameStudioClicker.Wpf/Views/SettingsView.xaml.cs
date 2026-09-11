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

        public SettingsView()
        {
            _audioService =
                ((GameStudioClicker.Wpf.App)Application.Current).AudioService;

            InitializeComponent();
        }

        private void MusicVolumeSlider_ValueChanged(
            object sender,
            RoutedPropertyChangedEventArgs<double> e)
        {
            _audioService.SetMusicVolume(e.NewValue);
        }
        private void SFXVolumeSlider_ValueChanged(
            object sender,
            RoutedPropertyChangedEventArgs<double> e)
        {
            _audioService.SetSfxVolume(e.NewValue);
        }
    }
}
