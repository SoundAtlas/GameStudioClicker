using GameStudioClicker.Core.Models;
using GameStudioClicker.Wpf.Commands;
using GameStudioClicker.Wpf.Formatting;
using System.Windows.Threading;

namespace GameStudioClicker.Wpf.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly GameState _gameState;
    private readonly DispatcherTimer _passiveTimer;
    private readonly DispatcherTimer _offlineMessageTimer;
    private readonly DispatcherTimer _saveConfirmationMessageTimer;

    private bool _isStatisticsViewVisible;

    private bool _showOfflineEarnings;
    private bool _showSaveConfirmation;

    public MainViewModel(GameState gameState, long offlineLinesEarned = 0)
    {
        _gameState = gameState ?? throw new ArgumentNullException(nameof(gameState));

        var activeUpgradeViewModels = new List<ActiveUpgradeViewModel>();
        foreach (ActiveUpgrade upgrade in _gameState.ActiveUpgrades)
        {
            activeUpgradeViewModels.Add(new ActiveUpgradeViewModel(_gameState, upgrade));
        }

        ActiveUpgrades = activeUpgradeViewModels;
        RefreshActiveUpgradeVisibility();

        var workerUpgradeViewModels = new List<WorkerUpgradeViewModel>();
        foreach (WorkerUpgrade upgrade in _gameState.WorkerUpgrades)
        {
            workerUpgradeViewModels.Add(new WorkerUpgradeViewModel(_gameState, upgrade));
        }

        WorkerUpgrades = workerUpgradeViewModels;

        OfflineLinesEarned = Math.Max(0, offlineLinesEarned);
        _showOfflineEarnings = OfflineLinesEarned > 0;

        _offlineMessageTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(7)
        };
        _offlineMessageTimer.Tick += OfflineMessageTimer_Tick;

        _passiveTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _passiveTimer.Tick += PassiveTimer_Tick;

        _saveConfirmationMessageTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1.5)
        };
        _saveConfirmationMessageTimer.Tick += SaveConfirmationMessage_Tick;

        WriteCodeCommand = new RelayCommand(ExecuteWriteCode);
        PurchaseActiveUpgradeCommand = new RelayCommand(
            ExecutePurchaseActiveUpgrade,
            CanExecutePurchaseActiveUpgrade);
        PurchaseWorkerUpgradeCommand = new RelayCommand(
            ExecutePurchaseWorkerUpgrade,
            CanExecutePurchaseWorkerUpgrade);
        ToggleStatisticsCommand = new RelayCommand(ExecuteToggleStatistics);

        if (_showOfflineEarnings)
        {
            _offlineMessageTimer.Start();
        }

        _passiveTimer.Start();
    }

    // Production displayed by the main coding panel.
    public long LinesOfCode => _gameState.LinesOfCode;
    public long LinesPerClick => _gameState.LinesPerClick;
    public long LinesPerSecond => _gameState.LinesPerSecond;
    public long LifetimeLinesOfCode => _gameState.LifetimeLinesOfCode;

    // Navigation and view state
    public bool IsStatisticsViewVisible
    {
        get
        {
            return _isStatisticsViewVisible;
        }
        private set
        {
            if (_isStatisticsViewVisible == value)
            {
                return;
            }
            _isStatisticsViewVisible = value;
            OnPropertyChanged();
        }
    }

    // One-time notifications.
    public long OfflineLinesEarned { get; }
    public bool HasOfflineEarnings => _showOfflineEarnings;
    public string OfflineEarningsMessage =>
        $"Your Employees wrote {CompactNumberFormatter.Format(OfflineLinesEarned)} lines of code while you were away!";
    public bool HasSaveConfirmation => _showSaveConfirmation;

    // Collections and commands consumed by the view.
    public IReadOnlyList<ActiveUpgradeViewModel> ActiveUpgrades { get; }
    public IReadOnlyList<WorkerUpgradeViewModel> WorkerUpgrades { get; }
    public RelayCommand WriteCodeCommand { get; }
    public RelayCommand PurchaseActiveUpgradeCommand { get; }
    public RelayCommand PurchaseWorkerUpgradeCommand { get; }
    public RelayCommand ToggleStatisticsCommand { get; }

    public event EventHandler? SaveRequested;

    public void ShowSaveConfirmation()
    {
        _showSaveConfirmation = true;
        OnPropertyChanged(nameof(HasSaveConfirmation));

        // Restarting the timer keeps the message visible after closely spaced saves.
        _saveConfirmationMessageTimer.Stop();
        _saveConfirmationMessageTimer.Start();
    }

    private void ExecuteWriteCode(object? parameter)
    {
        _gameState.WriteCode();
        OnPropertyChanged(nameof(LinesOfCode));
        OnPropertyChanged(nameof(LifetimeLinesOfCode));

        RefreshPurchaseCommands();
        RefreshActiveUpgradeStates();
    }

    private bool CanExecutePurchaseActiveUpgrade(object? parameter)
    {
        if (parameter is ActiveUpgradeViewModel upgradeViewModel)
        {
            return _gameState.CanPurchaseActiveUpgrade(upgradeViewModel.Upgrade);
        }

        return false;
    }

    private void ExecutePurchaseActiveUpgrade(object? parameter)
    {
        if (parameter is ActiveUpgradeViewModel upgradeViewModel &&
            _gameState.TryPurchaseActiveUpgrade(upgradeViewModel.Upgrade))
        {
            OnPropertyChanged(nameof(LinesOfCode));
            OnPropertyChanged(nameof(LinesPerClick));
            OnPropertyChanged(nameof(LinesPerSecond));

            SaveRequested?.Invoke(this, new EventArgs());

            RefreshWorkerUpgradeStates();
            RefreshActiveUpgradeStates();
            RefreshPurchaseCommands();
        }
    }

    private bool CanExecutePurchaseWorkerUpgrade(object? parameter)
    {
        if (parameter is WorkerUpgradeViewModel upgradeViewModel)
        {
            return _gameState.CanPurchaseWorkerUpgrade(upgradeViewModel.Upgrade);
        }

        return false;
    }

    private void ExecutePurchaseWorkerUpgrade(object? parameter)
    {
        if (parameter is WorkerUpgradeViewModel upgradeViewModel &&
            _gameState.TryPurchaseWorkerUpgrade(upgradeViewModel.Upgrade))
        {
            OnPropertyChanged(nameof(LinesOfCode));
            OnPropertyChanged(nameof(LinesPerSecond));

            SaveRequested?.Invoke(this, new EventArgs());

            RefreshWorkerUpgradeStates();
            RefreshActiveUpgradeStates();
            RefreshPurchaseCommands();
        }
    }

    private void ExecuteToggleStatistics(object? parameter)
    {
        IsStatisticsViewVisible = !IsStatisticsViewVisible;
    }


    private void PassiveTimer_Tick(object? sender, EventArgs e)
    {
        _gameState.GeneratePassiveLines();
        OnPropertyChanged(nameof(LinesOfCode));
        OnPropertyChanged(nameof(LifetimeLinesOfCode));

        RefreshPurchaseCommands();
        RefreshActiveUpgradeStates();
    }

    private void OfflineMessageTimer_Tick(object? sender, EventArgs e)
    {
        _offlineMessageTimer.Stop();
        _showOfflineEarnings = false;
        OnPropertyChanged(nameof(HasOfflineEarnings));
    }

    private void SaveConfirmationMessage_Tick(object? sender, EventArgs e)
    {
        _saveConfirmationMessageTimer.Stop();
        _showSaveConfirmation = false;
        OnPropertyChanged(nameof(HasSaveConfirmation));
    }

    private void RefreshPurchaseCommands()
    {
        PurchaseActiveUpgradeCommand.RaiseCanExecuteChanged();
        PurchaseWorkerUpgradeCommand.RaiseCanExecuteChanged();
    }

    private void RefreshActiveUpgradeStates()
    {
        foreach (ActiveUpgradeViewModel upgradeViewModel in ActiveUpgrades)
        {
            upgradeViewModel.RefreshState();
        }

        RefreshActiveUpgradeVisibility();
    }

    private void RefreshWorkerUpgradeStates()
    {
        foreach (WorkerUpgradeViewModel upgradeViewModel in WorkerUpgrades)
        {
            upgradeViewModel.RefreshState();
        }
    }

    private void RefreshActiveUpgradeVisibility()
    {
        int visibleUpgradeCount = 0;

        foreach (ActiveUpgradeViewModel upgradeViewModel in ActiveUpgrades)
        {
            bool shouldBeVisible =
                !upgradeViewModel.IsPurchased &&
                visibleUpgradeCount < 3;

            upgradeViewModel.UpdateVisibility(shouldBeVisible);

            if (shouldBeVisible)
            {
                visibleUpgradeCount++;
            }
        }
    }
}
