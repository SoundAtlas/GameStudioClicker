using GameStudioClicker.Core.Persistence;
using GameStudioClicker.Wpf.Formatting;
using GameStudioClicker.Wpf.Services;
using GameStudioClicker.Wpf.ViewModels;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace GameStudioClicker.Wpf.Views;

public partial class MainWindow : Window
{
    private readonly MainViewModel _mainViewModel;
    private readonly GameSessionService _gameSessionService;
    private readonly AudioService _audioService;

    public MainWindow()
    {
        InitializeComponent();

        _audioService = new AudioService();

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
        _mainViewModel.SaveRequested += SaveRequested;
        _mainViewModel.AchievementNotificationShown += AchievementNotificationShow;


        Closing += MainWindowClosing;
        DataContext = _mainViewModel;
    }

    private void AchievementNotificationShow(object? sender, EventArgs e)
    {
        _audioService.PlayAchievementEarnedSound();
    }

    private void SaveRequested(object? sender, EventArgs e)
    {
        _gameSessionService.Save();
    }

    private void MainWindowClosing(object? sender, CancelEventArgs e)
    {
        _mainViewModel.SaveRequested -= SaveRequested;
        _mainViewModel.AchievementNotificationShown -= AchievementNotificationShow;
        _mainViewModel.Dispose();
        _gameSessionService.Dispose();
        _audioService.Close();
    }

    // WriteCodeButton pressed sound
    private void WriteCodeButton_PreviewMouseLeftButtonDown(
        object sender, MouseButtonEventArgs e)
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

}
