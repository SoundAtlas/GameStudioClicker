using GameStudioClicker.Core.Models;
using GameStudioClicker.Core.Persistence;
using GameStudioClicker.Wpf.ViewModels;

namespace GameStudioClicker.Tests;

[TestClass]
public class MainViewModelTests
{
    [TestMethod]
    public void NewMainViewModel_StartsWithZeroLinesOfCode()
    {
        // Arrange
        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);

        // Act: No action needed, we are testing the initial state

        // Assert
        Assert.AreEqual(0L, viewModel.LinesOfCode);
    }

    [TestMethod]
    public void NewMainViewModel_ExposesInitialLinesPerClick()
    {
        // Arrange
        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);

        // Act: No action needed, we are testing the initial state

        // Assert
        Assert.AreEqual(1_000L, viewModel.LinesPerClick);
    }

    [TestMethod]
    public void NewMainViewModel_ExposesActiveUpgradeDetails()
    {
        // Arrange
        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);

        // Act: No action needed, we are testing the initial state

        // Assert
        ActiveUpgradeViewModel mousePad = GetActiveUpgrade(viewModel, "mouse_pad");
        ActiveUpgradeViewModel gamingMouse = GetActiveUpgrade(viewModel, "gaming_mouse");

        Assert.AreEqual(100L, mousePad.Cost);
        Assert.IsTrue(mousePad.IsAvailable);
        Assert.IsFalse(gamingMouse.IsAvailable);
    }

    [TestMethod]
    public void PurchaseActiveUpgradeCommand_NewGame_CannotPurchaseFirstUpgrade()
    {
        // Arrange
        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);

        // Act
        ActiveUpgradeViewModel mousePad = GetActiveUpgrade(viewModel, "mouse_pad");

        bool canExecute = viewModel.PurchaseActiveUpgradeCommand.CanExecute(mousePad);

        // Assert
        Assert.IsFalse(canExecute);
    }

    [TestMethod]
    public void PurchaseActiveUpgradeCommand_WithEnoughLines_CanPurchaseFirstUpgrade()
    {
        // Arrange
        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);
        viewModel.WriteCodeCommand.Execute(null);

        // Act
        ActiveUpgradeViewModel mousePad = GetActiveUpgrade(viewModel, "mouse_pad");

        bool canExecute = viewModel.PurchaseActiveUpgradeCommand.CanExecute(mousePad);

        // Assert
        Assert.IsTrue(canExecute);
    }

    [TestMethod]
    public void PurchaseActiveUpgradeCommand_WhenExecuted_UpdatesExposedGameState()
    {
        // Arrange
        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);
        viewModel.WriteCodeCommand.Execute(null);

        ActiveUpgradeViewModel mousePad = GetActiveUpgrade(viewModel, "mouse_pad");
        ActiveUpgradeViewModel gamingMouse = GetActiveUpgrade(viewModel, "gaming_mouse");

        // Act
        viewModel.PurchaseActiveUpgradeCommand.Execute(mousePad);

        // Assert
        Assert.AreEqual(900L, viewModel.LinesOfCode);
        Assert.AreEqual(2_000L, viewModel.LinesPerClick);
        Assert.IsFalse(mousePad.IsAvailable);
        Assert.IsTrue(gamingMouse.IsAvailable);
    }

    [TestMethod]
    public void PurchaseActiveUpgradeCommand_AfterPurchase_CannotPurchaseSameUpgradeAgain()
    {
        // Arrange
        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);
        viewModel.WriteCodeCommand.Execute(null);

        ActiveUpgradeViewModel mousePad = GetActiveUpgrade(viewModel, "mouse_pad");

        // Act
        viewModel.PurchaseActiveUpgradeCommand.Execute(mousePad);

        // Assert
        Assert.IsFalse(viewModel.PurchaseActiveUpgradeCommand.CanExecute(mousePad));
        Assert.AreEqual(100L, mousePad.Cost);
    }

    [TestMethod]
    public void PurchaseActiveUpgradeCommand_WhenExecuted_NotifiesNextUpgradeAvailability()
    {
        // Arrange
        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);

        viewModel.WriteCodeCommand.Execute(null);

        ActiveUpgradeViewModel mousePad = GetActiveUpgrade(viewModel, "mouse_pad");
        ActiveUpgradeViewModel gamingMouse = GetActiveUpgrade(viewModel, "gaming_mouse");
        var changedPropertyNames = new List<string?>();
        gamingMouse.PropertyChanged += (_, eventArgs) =>
            changedPropertyNames.Add(eventArgs.PropertyName);

        // Act
        viewModel.PurchaseActiveUpgradeCommand.Execute(mousePad);

        // Assert
        CollectionAssert.Contains(
            changedPropertyNames,
            nameof(ActiveUpgradeViewModel.IsAvailable));
    }

    [TestMethod]
    public void WriteCodeCommand_WhenExecuted_IncreasesLinesOfCode()
    {
        // Arrange
        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);

        // Act
        viewModel.WriteCodeCommand.Execute(null);

        // Assert
        Assert.AreEqual(1_000L, viewModel.LinesOfCode);
    }

    [TestMethod]
    public void WriteCodeCommand_WhenExecuted_RaisesPropertyChangedForLinesOfCode()
    {
        // Arrange
        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);

        var changedPropertyNames = new List<string?>();
        viewModel.PropertyChanged += (_, eventArgs) =>
            changedPropertyNames.Add(eventArgs.PropertyName);

        // Act
        viewModel.WriteCodeCommand.Execute(null);

        // Assert
        CollectionAssert.Contains(
            changedPropertyNames,
            nameof(MainViewModel.LinesOfCode));
    }

    [TestMethod]
    public void NewMainViewModel_ExposesInitialWorkerState()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        var viewModel = new MainViewModel(gameState);
        WorkerUpgradeViewModel intern = GetWorkerUpgrade(viewModel, "intern");

        // Assert
        Assert.AreEqual(0L, viewModel.LinesPerSecond);
        Assert.AreEqual(50L, intern.CurrentCost);
        Assert.AreEqual(0, intern.WorkerCount);
        Assert.IsFalse(viewModel.PurchaseWorkerUpgradeCommand.CanExecute(intern));
    }

    [TestMethod]
    public void PurchaseWorkerUpgradeCommand_WithEnoughLines_UpdatesExposedGameState()
    {
        // Arrange
        var gameState = new GameState();
        gameState.RestoreFromSaveData(new GameSaveData
        {
            LinesOfCode = 50
        });
        var viewModel = new MainViewModel(gameState);
        WorkerUpgradeViewModel intern = GetWorkerUpgrade(viewModel, "intern");

        // Act
        viewModel.PurchaseWorkerUpgradeCommand.Execute(intern);

        // Assert
        Assert.AreEqual(0L, viewModel.LinesOfCode);
        Assert.AreEqual(2L, viewModel.LinesPerSecond);
        Assert.AreEqual(100L, intern.CurrentCost);
        Assert.AreEqual(1, intern.WorkerCount);
        Assert.IsFalse(viewModel.PurchaseWorkerUpgradeCommand.CanExecute(intern));
    }

    private static ActiveUpgradeViewModel GetActiveUpgrade(
        MainViewModel viewModel,
        string id)
    {
        return viewModel.ActiveUpgrades.Single(upgrade => upgrade.Id == id);
    }

    private static WorkerUpgradeViewModel GetWorkerUpgrade(
        MainViewModel viewModel,
        string id)
    {
        return viewModel.WorkerUpgrades.Single(upgrade => upgrade.Id == id);
    }
}
