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

        private void MasterVolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double normalizedVolume = e.NewValue / 100;

            if (normalizedVolume <= 0)
            {
                _audioService.SetMasterVolume(0);
                return;
            }

            const double minDecibles = -30;

            double decibles =
                minDecibles + (normalizedVolume * -minDecibles);

            double volume = Math.Pow(10, decibles / 20);

            _audioService.SetMasterVolume(volume);
        }
    }
}
