using GameStudioClicker.Core.Models;

namespace GameStudioClicker.Wpf.ViewModels;

public class ActiveUpgradeViewModel : ViewModelBase
{
    private readonly GameState _gameState;
    private readonly ActiveUpgrade _activeUpgrade;

    public ActiveUpgradeViewModel(GameState gameState, ActiveUpgrade activeUpgrade)
    {
        _gameState = gameState ?? throw new ArgumentNullException(nameof(gameState));
        _activeUpgrade = activeUpgrade ?? throw new ArgumentNullException(nameof(activeUpgrade));
    }

    // Display and purchase state exposed to the active-upgrade template.
    public string Id => _activeUpgrade.Id;
    public string DisplayName => _activeUpgrade.DisplayName;
    public string Description => _activeUpgrade.Description;
    public bool AffectsClickProduction => _activeUpgrade.ClickMultiplier > 1;
    public string ClickEffectText => _activeUpgrade.ClickMultiplier switch
    {
        2 => "Doubles",
        3 => "Triples",
        _ => $"{_activeUpgrade.ClickMultiplier}x"
    };
    public string? IconPath => Id switch
    {
        "mouse_pad" => "/Assets/Upgrades/Active/upgrade_mouse_pad.png",
        "gaming_mouse" => "/Assets/Upgrades/Active/upgrade_gaming_mouse.png",
        "mechanical_keyboard" => "/Assets/Upgrades/Active/upgrade_mechanical_keyboard.png",
        "noise_cancelling_headset" => "/Assets/Upgrades/Active/upgrade_noise_cancelling_headset.png",
        "onboarding_handbook" => "/Assets/Upgrades/Active/upgrade_onboarding_handbook.png",
        "graphics_card" => "/Assets/Upgrades/Active/upgrade_graphics_card.png",
        "second_monitor" => "/Assets/Upgrades/Active/upgrade_second_monitor.png",
        "ergonomic_desk_setup" => "/Assets/Upgrades/Active/upgrade_ergonomic_desk_setup.png",
        _ => null,
    };
    public long Cost => _activeUpgrade.Cost;
    public bool IsPurchased => _activeUpgrade.IsPurchased;
    public bool IsAvailable => _activeUpgrade.IsAvailable;
    public bool IsVisible { get; private set; }
    public bool CanPurchaseActiveUpgrade =>
        _gameState.CanPurchaseActiveUpgrade(_activeUpgrade);

    public string RequirementText
    {
        get
        {
            var requirements = new List<string>();

            if (_activeUpgrade.Prerequisite is { IsPurchased: false } prerequisite)
            {
                requirements.Add($"Requires {prerequisite.DisplayName}.");
            }

            if (_activeUpgrade.TargetWorkerId != null)
            {
                foreach (WorkerUpgrade workerUpgrade in _gameState.WorkerUpgrades)
                {
                    if (workerUpgrade.Id == _activeUpgrade.TargetWorkerId &&
                        workerUpgrade.WorkerCount == 0)
                    {
                        requirements.Add($"Requires at least 1 {workerUpgrade.DisplayName}.");
                        break;
                    }
                }
            }

            if (_activeUpgrade.TargetAllWorkers)
            {
                bool hasAnyWorkers = false;

                foreach (WorkerUpgrade workerUpgrade in _gameState.WorkerUpgrades)
                {
                    if (workerUpgrade.WorkerCount > 0)
                    {
                        hasAnyWorkers = true;
                        break;
                    }
                }

                if (!hasAnyWorkers)
                {
                    requirements.Add("Requires at least 1 worker.");
                }
            }

            return string.Join(Environment.NewLine, requirements);
        }
    }

    public bool HasUnmetRequirement => !string.IsNullOrEmpty(RequirementText);

    // Commands use the model while the View binds only to ViewModel properties.
    internal ActiveUpgrade Upgrade => _activeUpgrade;

    public void RefreshState()
    {
        OnPropertyChanged(nameof(IsPurchased));
        OnPropertyChanged(nameof(IsAvailable));
        OnPropertyChanged(nameof(HasUnmetRequirement));
        OnPropertyChanged(nameof(RequirementText));
        OnPropertyChanged(nameof(CanPurchaseActiveUpgrade));
    }

    public void UpdateVisibility(bool isVisible)
    {
        IsVisible = isVisible;
        OnPropertyChanged(nameof(IsVisible));
    }
}
