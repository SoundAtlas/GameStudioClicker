using GameStudioClicker.Core.Persistence;
using GameStudioClicker.Wpf.Formatting;
using GameStudioClicker.Wpf.Services;
using GameStudioClicker.Wpf.ViewModels;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Threading;

namespace GameStudioClicker.Wpf.Views;

public partial class MainWindow : Window
{
    private readonly MainViewModel _mainViewModel;
    private readonly GameSessionService _gameSessionService;
    private readonly AudioService _audioService;

    // Music visualizer fields
    private readonly DispatcherTimer _dispatcherTimer;
    private float _latestMusicLevel;
    private double _displayedMusicLevel;
    private readonly ConcurrentQueue<float> _musicLevelQueue = new();

    public MainWindow()
    {
        _audioService =
            ((GameStudioClicker.Wpf.App)Application.Current).AudioService;

        InitializeComponent();


        string saveDirectoryPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "GameStudioClicker");
        string saveFilePath = Path.Combine(saveDirectoryPath, "game_save.json");

        IGameSaveRepository saveRepository =
            new JsonGameSaveRepository(saveFilePath);

        _gameSessionService = new GameSessionService(saveRepository);
        _gameSessionService.Start();

        _mainViewModel = new MainViewModel(
            _gameSessionService.GameState,
            _gameSessionService.OfflineLinesEarned);

        _mainViewModel.CurrentSoundtrackTitle =
            _audioService.CurrentSoundtrackTitle;

        _audioService.SoundtrackChanged += AudioService_SoundtrackChanged;
        _audioService.MusicLevelChanged += AudioService_MusicLevelChanged;

        _dispatcherTimer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromMilliseconds(33)
        };
        _dispatcherTimer.Tick += DispatcherTimer_Tick;
        _dispatcherTimer.Start();

        _mainViewModel.SaveRequested += SaveRequested;
        _mainViewModel.AchievementNotificationShown += AchievementNotificationShow;


        Closing += MainWindowClosing;
        DataContext = _mainViewModel;
    }

    private void DispatcherTimer_Tick(object? sender, EventArgs e)
    {
        while (_musicLevelQueue.Count > 4)
        {
            if (_musicLevelQueue.TryDequeue(out var musicLevel))
            {
                _latestMusicLevel = musicLevel;
            }
        }

        double targetLevel =
            Math.Clamp(_latestMusicLevel * 2.0, 0.0, 1.0);

        double smoothingAmount;

        if (targetLevel > _displayedMusicLevel)
        {
            smoothingAmount = 0.45;
        }
        else
        {
            smoothingAmount = 0.12;
        }

        // Moves the displayed music level towards the target level with smoothing.
        _displayedMusicLevel +=
            (targetLevel - _displayedMusicLevel) * smoothingAmount;

        VisualizerBar1Scale.ScaleY = Math.Clamp(_displayedMusicLevel * 0.55, 0.05, 1.00);
        VisualizerBar2Scale.ScaleY = Math.Clamp(_displayedMusicLevel * 0.80, 0.05, 1.00);
        VisualizerBar3Scale.ScaleY = Math.Clamp(_displayedMusicLevel * 1.0, 0.05, 1.00);
        VisualizerBar4Scale.ScaleY = Math.Clamp(_displayedMusicLevel * 0.75, 0.05, 1.00);
        VisualizerBar5Scale.ScaleY = Math.Clamp(_displayedMusicLevel * 0.50, 0.05, 1.00);

    }

    private void AudioService_MusicLevelChanged(float level)
    {
        _musicLevelQueue.Enqueue(Math.Clamp(level, 0, 1));
    }

    private void AudioService_SoundtrackChanged(string title)
    {
        // Dispatcher is used to ensure that the UI update occurs on the main thread.
        // This is necessary because the SoundtrackChanged event may be raised from a different thread.
        Dispatcher.Invoke(() =>
        {
            _mainViewModel.CurrentSoundtrackTitle = title;
        });
    }

    private void AchievementNotificationShow(object? sender, EventArgs e)
    {
        _audioService.PlayAchievementEarnedSound();
    }
    private void SaveRequested(object? sender, EventArgs e)
    {
        _gameSessionService.Save();
    }

    // WriteCodeButton pressed sound
    private void WriteCodeButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        _audioService.PlayWriteCodePressSound();
    }

    private bool _nextFeedbackMovesRight = true;
    // Each click creates its own feedback element so rapid clicks can finish independently.
    private void WriteCodeButton_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MainViewModel viewModel)
        {
            return;
        }

        _audioService.PlayWriteCodeReleaseSound();

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
    private void MenuButton_Click(object sender, RoutedEventArgs e)
    {
        _audioService.PlayMenuClickSound();
    }

    private void MainWindowClosing(object? sender, CancelEventArgs e)
    {
        _mainViewModel.SaveRequested -= SaveRequested;
        _mainViewModel.AchievementNotificationShown -= AchievementNotificationShow;
        _audioService.SoundtrackChanged -= AudioService_SoundtrackChanged;
        _audioService.MusicLevelChanged -= AudioService_MusicLevelChanged;
        _dispatcherTimer.Tick -= DispatcherTimer_Tick;
        _dispatcherTimer.Stop();
        _mainViewModel.Dispose();
        _gameSessionService.Dispose();
    }

}
