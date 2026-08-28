using GameStudioClicker.Core.Models;
using GameStudioClicker.Wpf.ViewModels;
using System.Windows;

namespace GameStudioClicker.Wpf.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);
        DataContext = viewModel;
    }
}
