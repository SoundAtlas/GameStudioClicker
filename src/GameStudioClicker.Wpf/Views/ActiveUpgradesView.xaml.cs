using GameStudioClicker.Wpf.Services;
using System.Windows;
using System.Windows.Controls;

namespace GameStudioClicker.Wpf.Views
{
    /// <summary>
    /// Interaction logic for ActiveUpgradesView.xaml
    /// </summary>
    public partial class ActiveUpgradesView : UserControl
    {
        private readonly AudioService _audioService;

        public ActiveUpgradesView()
        {
            _audioService =
                ((GameStudioClicker.Wpf.App)Application.Current).AudioService;

            InitializeComponent();
        }

        private void ActiveUpgradeButton_Click(object sender, RoutedEventArgs e)
        {
            _audioService.PlayActiveUpgradeSound();
        }
    }
}
