using GameStudioClicker.Core.Models;
using GameStudioClicker.Wpf.ViewModels;
using System.ComponentModel;

namespace GameStudioClicker.Tests;

[TestClass]
public class MainViewModelTests
{
    private string? _changedPropertyName;

    private void HandlePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        _changedPropertyName = e.PropertyName;
    }

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
    public void NewMainViewModel_ExposesMechanicalKeyboardCost()
    {
        // Arrange
        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);

        // Act: No action needed, we are testing the initial state

        // Assert
        Assert.AreEqual(100L, viewModel.MechanicalKeyboardCost);
    }

    [TestMethod]
    public void NewMainViewModel_ReportsMechanicalKeyboardAsAvailable()
    {
        // Arrange
        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);

        // Act: No action needed, we are testing the initial state

        // Assert
        Assert.IsTrue(viewModel.IsMechanicalKeyboardAvailable);
    }

    [TestMethod]
    public void PurchaseMechanicalKeyboardCommand_NewGame_CannotExecute()
    {
        // Arrange
        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);

        // Act
        bool canExecute =
            viewModel.PurchaseMechanicalKeyboardCommand.CanExecute(null);

        // Assert
        Assert.IsFalse(canExecute);
    }

    [TestMethod]
    public void PurchaseMechanicalKeyboardCommand_WithEnoughLines_CanExecute()
    {
        // Arrange
        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);
        for (int i = 0; i < 100; i++)
        {
            viewModel.WriteCodeCommand.Execute(null);
        }

        // Act
        bool canExecute =
            viewModel.PurchaseMechanicalKeyboardCommand.CanExecute(null);

        // Assert
        Assert.IsTrue(canExecute);
    }

    [TestMethod]
    public void PurchaseMechanicalKeyboardCommand_WhenExecuted_UpdatesExposedGameState()
    {
        // Arrange
        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);
        for (int i = 0; i < 105; i++)
        {
            viewModel.WriteCodeCommand.Execute(null);
        }

        // Act
        viewModel.PurchaseMechanicalKeyboardCommand.Execute(null);

        // Assert
        Assert.AreEqual(5L, viewModel.LinesOfCode);
        Assert.AreEqual(2L, viewModel.LinesPerClick);
        Assert.IsFalse(viewModel.IsMechanicalKeyboardAvailable);
        Assert.IsTrue(viewModel.IsUltrawideMonitorAvailable);
    }

    [TestMethod]
    public void PurchaseMechanicalKeyboardCommand_AfterPurchaseWithoutEnoughLines_CannotExecute()
    {
        // Arrange
        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);
        for (int i = 0; i < 100; i++)
        {
            viewModel.WriteCodeCommand.Execute(null);
        }

        // Act
        viewModel.PurchaseMechanicalKeyboardCommand.Execute(null);

        // Assert
        Assert.IsFalse(viewModel.PurchaseMechanicalKeyboardCommand.CanExecute(null));
    }

    [TestMethod]
    public void PurchaseMechanicalKeyboardCommand_WhenExecuted_KeepsFixedKeyboardCost()
    {
        // Arrange
        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);
        for (int i = 0; i < 100; i++)
        {
            viewModel.WriteCodeCommand.Execute(null);
        }

        // Act
        viewModel.PurchaseMechanicalKeyboardCommand.Execute(null);

        // Assert
        Assert.AreEqual(100L, viewModel.MechanicalKeyboardCost);
    }

    [TestMethod]
    public void PurchaseMechanicalKeyboardCommand_AfterFirstPurchaseWithEnoughLines_CannotExecute()
    {
        // Arrange
        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);
        for (int i = 0; i < 200; i++)
        {
            viewModel.WriteCodeCommand.Execute(null);
        }

        // Act
        viewModel.PurchaseMechanicalKeyboardCommand.Execute(null);

        // Assert
        Assert.IsFalse(viewModel.PurchaseMechanicalKeyboardCommand.CanExecute(null));
    }

    [TestMethod]
    public void PurchaseMechanicalKeyboardCommand_WhenExecuted_RaisesPropertyChangedForMonitorAvailability()
    {
        // Arrange
        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);

        for (int i = 0; i < 100; i++)
        {
            viewModel.WriteCodeCommand.Execute(null);
        }

        _changedPropertyName = null;
        viewModel.PropertyChanged += HandlePropertyChanged;

        // Act
        viewModel.PurchaseMechanicalKeyboardCommand.Execute(null);

        // Assert
        Assert.AreEqual(nameof(MainViewModel.IsUltrawideMonitorAvailable), _changedPropertyName);
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

        _changedPropertyName = null;
        viewModel.PropertyChanged += HandlePropertyChanged;

        // Act
        viewModel.WriteCodeCommand.Execute(null);

        // Assert
        Assert.AreEqual(nameof(MainViewModel.LinesOfCode), _changedPropertyName);
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
}
