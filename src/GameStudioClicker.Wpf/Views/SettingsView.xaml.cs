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
            _audioService.SetMusicVolume
                    (ConvertSliderValueToVolume(e.NewValue));
        }
        private void SFXVolumeSlider_ValueChanged(
            object sender,
            RoutedPropertyChangedEventArgs<double> e)
        {
            _audioService.SetSfxVolume
                    (ConvertSliderValueToVolume(e.NewValue));
        }


        private static double ConvertSliderValueToVolume(double sliderValue)
        {
            double normalizedValue = sliderValue / 100.0; // Convert slider value (0-100) to a normalized value (0.0-1.0)

            if (normalizedValue <= 0)
            {
                return 0;
            }

            const double minDecibles = -30;
            // Convert the normalized value to a volume level in decibels
            double decibles =
                minDecibles + (normalizedValue * -minDecibles);

            // Convert decibels to a linear volume scale (0.0-1.0)
            return Math.Pow(10, decibles / 20);
        }
    }
}
