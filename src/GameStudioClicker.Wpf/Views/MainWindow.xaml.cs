using GameStudioClicker.Core.Models;
using GameStudioClicker.Core.Persistence;
using GameStudioClicker.Wpf.ViewModels;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace GameStudioClicker.Wpf.Views;

public partial class MainWindow : Window
{
    // Long-lived objects shared by the window lifecycle and its view model.
    private readonly GameState _gameState;
    private readonly JsonGameSaveService _jsonGameSaveService;

    // Save files live outside the build directory so Debug, Release, and published builds share progress.
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

        long offlineLinesEarned = 0;
        // Restore persisted progress before creating the view model.
        var result = _jsonGameSaveService.LoadFromFile(SaveFilePath);
        if (result != null)
        {
            _gameState.RestoreFromSaveData(result);

            // Apply passive production earned since the last valid save time.
            if (result.SavedAtUtc != default)
            {
                var elapsedTime = DateTime.UtcNow - result.SavedAtUtc;
                offlineLinesEarned = _gameState.ApplyOfflineProgress(elapsedTime);
            }
        }

        var viewModel = new MainViewModel(_gameState, offlineLinesEarned);

        // Saving is tied to the window lifecycle rather than a manual UI command.
        Closing += MainWindowClosing;

        DataContext = viewModel;
    }

    private void MainWindowClosing(object? sender, CancelEventArgs e)
    {
        // Capture the timestamp as close as possible to writing the save file.
        var saveData = _gameState.CreateSaveData();
        saveData.SavedAtUtc = DateTime.UtcNow;
        _jsonGameSaveService.SaveToFile(saveData, SaveFilePath);
    }
}
