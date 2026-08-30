using GameStudioClicker.Core.Models;
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
        Assert.AreEqual(1L, viewModel.LinesPerClick);
    }

    [TestMethod]
    public void NewMainViewModel_ExposesActiveUpgradeDetails()
    {
        // Arrange
        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);

        // Act: No action needed, we are testing the initial state

        // Assert
        ActiveUpgradeViewModel keyboard = GetActiveUpgrade(viewModel, "mechanical_keyboard");
        ActiveUpgradeViewModel monitor = GetActiveUpgrade(viewModel, "ultrawide_monitor");

        Assert.AreEqual(100L, keyboard.Cost);
        Assert.IsTrue(keyboard.IsAvailable);
        Assert.IsFalse(monitor.IsAvailable);
    }

    [TestMethod]
    public void PurchaseActiveUpgradeCommand_NewGame_CannotPurchaseKeyboard()
    {
        // Arrange
        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);

        // Act
        ActiveUpgradeViewModel keyboard = GetActiveUpgrade(viewModel, "mechanical_keyboard");

        bool canExecute = viewModel.PurchaseActiveUpgradeCommand.CanExecute(keyboard);

        // Assert
        Assert.IsFalse(canExecute);
    }

    [TestMethod]
    public void PurchaseActiveUpgradeCommand_WithEnoughLines_CanPurchaseKeyboard()
    {
        // Arrange
        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);
        for (int i = 0; i < 100; i++)
        {
            viewModel.WriteCodeCommand.Execute(null);
        }

        // Act
        ActiveUpgradeViewModel keyboard = GetActiveUpgrade(viewModel, "mechanical_keyboard");

        bool canExecute = viewModel.PurchaseActiveUpgradeCommand.CanExecute(keyboard);

        // Assert
        Assert.IsTrue(canExecute);
    }

    [TestMethod]
    public void PurchaseActiveUpgradeCommand_WhenExecuted_UpdatesExposedGameState()
    {
        // Arrange
        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);
        for (int i = 0; i < 105; i++)
        {
            viewModel.WriteCodeCommand.Execute(null);
        }

        ActiveUpgradeViewModel keyboard = GetActiveUpgrade(viewModel, "mechanical_keyboard");
        ActiveUpgradeViewModel monitor = GetActiveUpgrade(viewModel, "ultrawide_monitor");

        // Act
        viewModel.PurchaseActiveUpgradeCommand.Execute(keyboard);

        // Assert
        Assert.AreEqual(5L, viewModel.LinesOfCode);
        Assert.AreEqual(2L, viewModel.LinesPerClick);
        Assert.IsFalse(keyboard.IsAvailable);
        Assert.IsTrue(monitor.IsAvailable);
    }

    [TestMethod]
    public void PurchaseActiveUpgradeCommand_AfterPurchase_CannotPurchaseSameUpgradeAgain()
    {
        // Arrange
        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);
        for (int i = 0; i < 100; i++)
        {
            viewModel.WriteCodeCommand.Execute(null);
        }

        ActiveUpgradeViewModel keyboard = GetActiveUpgrade(viewModel, "mechanical_keyboard");

        // Act
        viewModel.PurchaseActiveUpgradeCommand.Execute(keyboard);

        // Assert
        Assert.IsFalse(viewModel.PurchaseActiveUpgradeCommand.CanExecute(keyboard));
        Assert.AreEqual(100L, keyboard.Cost);
    }

    [TestMethod]
    public void PurchaseActiveUpgradeCommand_WhenExecuted_NotifiesMonitorAvailability()
    {
        // Arrange
        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);

        for (int i = 0; i < 100; i++)
        {
            viewModel.WriteCodeCommand.Execute(null);
        }

        ActiveUpgradeViewModel keyboard = GetActiveUpgrade(viewModel, "mechanical_keyboard");
        ActiveUpgradeViewModel monitor = GetActiveUpgrade(viewModel, "ultrawide_monitor");
        var changedPropertyNames = new List<string?>();
        monitor.PropertyChanged += (_, eventArgs) =>
            changedPropertyNames.Add(eventArgs.PropertyName);

        // Act
        viewModel.PurchaseActiveUpgradeCommand.Execute(keyboard);

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
        Assert.AreEqual(1L, viewModel.LinesOfCode);
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
    public void NewMainViewModel_ExposesInitialInternState()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        var viewModel = new MainViewModel(gameState);

        // Assert
        Assert.AreEqual(0L, viewModel.LinesPerSecond);
        Assert.AreEqual(50L, viewModel.InternCost);
        Assert.AreEqual(0, viewModel.InternCount);
        Assert.IsFalse(viewModel.PurchaseInternCommand.CanExecute(null));
    }

    [TestMethod]
    public void PurchaseInternCommand_WithEnoughLines_UpdatesExposedGameState()
    {
        // Arrange
        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);
        for (int i = 0; i < 50; i++)
        {
            viewModel.WriteCodeCommand.Execute(null);
        }

        // Act
        viewModel.PurchaseInternCommand.Execute(null);

        // Assert
        Assert.AreEqual(0L, viewModel.LinesOfCode);
        Assert.AreEqual(2L, viewModel.LinesPerSecond);
        Assert.AreEqual(100L, viewModel.InternCost);
        Assert.AreEqual(1, viewModel.InternCount);
        Assert.IsFalse(viewModel.PurchaseInternCommand.CanExecute(null));
    }

    private static ActiveUpgradeViewModel GetActiveUpgrade(
        MainViewModel viewModel,
        string id)
    {
        return viewModel.ActiveUpgrades.Single(upgrade => upgrade.Id == id);
    }
}
