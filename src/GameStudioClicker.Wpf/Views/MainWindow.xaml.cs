using GameStudioClicker.Core.Models;
using GameStudioClicker.Core.Persistence;
using GameStudioClicker.Wpf.ViewModels;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace GameStudioClicker.Wpf.Views;

public partial class MainWindow : Window
{
    private readonly GameState _gameState;
    private readonly JsonGameSaveService _jsonGameSaveService;

    private static readonly string SaveDirectoryPath =
    Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "GameStudioClicker");

    private static readonly string SaveFilePath =
        Path.Combine(SaveDirectoryPath, "game_save.json");

    public MainWindow()
    {


        InitializeComponent();

        _jsonGameSaveService = new JsonGameSaveService();
        _gameState = new GameState();

        Directory.CreateDirectory(SaveDirectoryPath);

        var result = _jsonGameSaveService.LoadFromFile(SaveFilePath);
        if (result != null)
        {
            _gameState.RestoreFromSaveData(result);
        }

        var viewModel = new MainViewModel(_gameState);

        Closing += MainWindowClosing;
        DataContext = viewModel;
    }

    private void MainWindowClosing(object? sender, CancelEventArgs e)
    {
        var saveData = _gameState.CreateSaveData();
        _jsonGameSaveService.SaveToFile(saveData, SaveFilePath);
    }
}
