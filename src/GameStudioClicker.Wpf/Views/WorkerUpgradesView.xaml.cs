using GameStudioClicker.Wpf.Services;
using System.Windows;
using System.Windows.Controls;

namespace GameStudioClicker.Wpf.Views
{
    /// <summary>
    /// Interaction logic for WorkerUpgradesView.xaml
    /// </summary>
    public partial class WorkerUpgradesView : UserControl
    {
        private readonly AudioService _audioService;
        public WorkerUpgradesView()
        {
            InitializeComponent();
            _audioService = new AudioService();
        }

        private void HireEmployee_Click(object sender, RoutedEventArgs e)
        {
            _audioService.PlayHireEmployeeSound();
        }
    }
}
