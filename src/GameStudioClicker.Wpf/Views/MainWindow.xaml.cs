using GameStudioClicker.Core.Models;
using GameStudioClicker.Core.Persistence;
using GameStudioClicker.Wpf.Formatting;
using GameStudioClicker.Wpf.ViewModels;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace GameStudioClicker.Wpf.Views;

public partial class MainWindow : Window
{
    // Long-lived objects shared by the window lifecycle and its view model.
    private readonly GameState _gameState;

    private readonly MainViewModel _mainViewModel;
    private readonly JsonGameSaveService _jsonGameSaveService;
    private readonly DispatcherTimer _autosaveTimer;

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
        _autosaveTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(30)
        };
        _autosaveTimer.Tick += AutoSaveTimer_Tick;

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

        _mainViewModel = new MainViewModel(_gameState, offlineLinesEarned);

        _mainViewModel.SaveRequested += SaveRequested;

        _autosaveTimer.Start();

        // Saving is tied to the window lifecycle rather than a manual UI command.
        Closing += MainWindowClosing;

        DataContext = _mainViewModel;
    }

    private void SaveRequested(object? sender, EventArgs e)
    {
        SaveGame();
    }

    private void AutoSaveTimer_Tick(object? sender, EventArgs e)
    {
        SaveGame();
    }

    private void MainWindowClosing(object? sender, CancelEventArgs e)
    {
        // Stop the autosave
        _autosaveTimer.Stop();
        // Perform a final save before the window closes.
        SaveGame();
    }

    private void SaveGame()
    {
        var saveData = _gameState.CreateSaveData();
        saveData.SavedAtUtc = DateTime.UtcNow;
        _jsonGameSaveService.SaveToFile(saveData, SaveFilePath);


        // Show save confirmation message (can be enabled later if desired)
        // _mainViewModel.ShowSaveConfirmation();
    }

    private bool _nextFeedbackMovesRight = true;
    // Animation for the "Write Code" button click feedback.
    private void WriteCodeButton_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MainViewModel viewModel)
        {
            return;
        }


        TextBlock feedbackText = new TextBlock
        {
            Text = $"+{CompactNumberFormatter.Format(viewModel.LinesPerClick)}",
            Foreground = Brushes.PowderBlue,
            FontSize = 16,
            FontWeight = FontWeights.SemiBold,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            IsHitTestVisible = false
        };

        Panel.SetZIndex(feedbackText, 1);

        var movement = new TranslateTransform();
        feedbackText.RenderTransform = movement;

        ClickFeedbackLayer.Children.Add(feedbackText);

        double horizontalDistance = _nextFeedbackMovesRight ? 50 : -50;
        _nextFeedbackMovesRight = !_nextFeedbackMovesRight;

        var duration = new Duration(TimeSpan.FromSeconds(0.65));

        var fadeAnimation = new DoubleAnimation(1, 0, duration);
        var hortizontalAnimation = new DoubleAnimation(0, horizontalDistance, duration);
        var verticalAnimation = new DoubleAnimation(0, -50, duration)
        {
            EasingFunction = new CubicEase
            {
                EasingMode = EasingMode.EaseOut
            }
        };

        fadeAnimation.Completed += (_, _) =>
            ClickFeedbackLayer.Children.Remove(feedbackText);

        feedbackText.BeginAnimation(
            UIElement.OpacityProperty,
            fadeAnimation);

        movement.BeginAnimation(
            TranslateTransform.XProperty,
            hortizontalAnimation);

        movement.BeginAnimation(
            TranslateTransform.YProperty,
            verticalAnimation);

    }
}
