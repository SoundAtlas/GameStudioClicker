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
using System.Windows.Media.Effects;
using System.Windows.Threading;

namespace GameStudioClicker.Wpf.Views;

public partial class MainWindow : Window
{
    // Saves live outside the build directory so every build configuration shares progress.
    private static readonly string SaveDirectoryPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "GameStudioClicker");

    private static readonly string SaveFilePath = Path.Combine(
        SaveDirectoryPath,
        "game_save.json");

    private readonly GameState _gameState;
    private readonly MainViewModel _mainViewModel;
    private readonly JsonGameSaveService _jsonGameSaveService;
    private readonly DispatcherTimer _autosaveTimer;

    private bool _nextFeedbackMovesRight = true;

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
        GameSaveData? saveData = _jsonGameSaveService.LoadFromFile(SaveFilePath);
        if (saveData != null)
        {
            _gameState.RestoreFromSaveData(saveData);

            if (saveData.SavedAtUtc != default)
            {
                TimeSpan elapsedTime = DateTime.UtcNow - saveData.SavedAtUtc;
                offlineLinesEarned = _gameState.ApplyOfflineProgress(elapsedTime);
            }
        }

        _mainViewModel = new MainViewModel(_gameState, offlineLinesEarned);
        _mainViewModel.SaveRequested += SaveRequested;

        _autosaveTimer.Start();
        Closing += MainWindowClosing;
        DataContext = _mainViewModel;
    }

    private void SaveGame()
    {
        GameSaveData saveData = _gameState.CreateSaveData();
        saveData.SavedAtUtc = DateTime.UtcNow;
        _jsonGameSaveService.SaveToFile(saveData, SaveFilePath);

        // Save confirmation can be re-enabled after its frequency is reconsidered.
        // _mainViewModel.ShowSaveConfirmation();
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
        _autosaveTimer.Stop();
        SaveGame();
    }

    // Each click creates its own feedback element so rapid clicks can finish independently.
    private void WriteCodeButton_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MainViewModel viewModel)
        {
            return;
        }

        var feedbackText = new TextBlock
        {
            Text = $"+{CompactNumberFormatter.Format(viewModel.LinesPerClick)}",
            Foreground = (Brush)FindResource("SuccessBrush"),
            FontSize = 18,
            FontWeight = FontWeights.Bold,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            IsHitTestVisible = false,
            Effect = new DropShadowEffect
            {
                Color = ((SolidColorBrush)FindResource("GameBackgroundBrush")).Color,
                BlurRadius = 3,
                ShadowDepth = 0,
                Opacity = 1
            }
        };


        Panel.SetZIndex(feedbackText, 1);

        var movement = new TranslateTransform();
        feedbackText.RenderTransform = movement;
        ClickFeedbackLayer.Children.Add(feedbackText);

        double horizontalDistance = _nextFeedbackMovesRight ? 50 : -50;
        _nextFeedbackMovesRight = !_nextFeedbackMovesRight;

        var movementDuration = new Duration(TimeSpan.FromSeconds(0.85));

        var fadeAnimation = new DoubleAnimation
            (1, 0, new Duration(TimeSpan.FromSeconds(0.45)))
        {
            BeginTime = TimeSpan.FromSeconds(0.4)
        };

        var horizontalAnimation =
            new DoubleAnimation(0, horizontalDistance, movementDuration);
        var verticalAnimation =
            new DoubleAnimation(0, -40, movementDuration)
            {
                EasingFunction = new CubicEase
                {
                    EasingMode = EasingMode.EaseOut
                }
            };

        fadeAnimation.Completed += (_, _) =>
            ClickFeedbackLayer.Children.Remove(feedbackText);

        feedbackText.BeginAnimation(UIElement.OpacityProperty, fadeAnimation);
        movement.BeginAnimation(TranslateTransform.XProperty, horizontalAnimation);
        movement.BeginAnimation(TranslateTransform.YProperty, verticalAnimation);
    }
}
