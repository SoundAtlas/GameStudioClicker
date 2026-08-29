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
        Assert.AreEqual(25L, viewModel.MechanicalKeyboardCost);
    }

    [TestMethod]
    public void NewMainViewModel_ReportsMechanicalKeyboardAsNotOwned()
    {
        // Arrange
        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);

        // Act: No action needed, we are testing the initial state

        // Assert
        Assert.AreEqual(0, viewModel.MechanicalKeyboardCount);
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
        for (int i = 0; i < 25; i++)
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
        for (int i = 0; i < 30; i++)
        {
            viewModel.WriteCodeCommand.Execute(null);
        }

        // Act
        viewModel.PurchaseMechanicalKeyboardCommand.Execute(null);

        // Assert
        Assert.AreEqual(5L, viewModel.LinesOfCode);
        Assert.AreEqual(2L, viewModel.LinesPerClick);
        Assert.AreEqual(1, viewModel.MechanicalKeyboardCount);
    }

    [TestMethod]
    public void PurchaseMechanicalKeyboardCommand_AfterPurchaseWithoutEnoughLines_CannotExecute()
    {
        // Arrange
        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);
        for (int i = 0; i < 25; i++)
        {
            viewModel.WriteCodeCommand.Execute(null);
        }

        // Act
        viewModel.PurchaseMechanicalKeyboardCommand.Execute(null);

        // Assert
        Assert.IsFalse(viewModel.PurchaseMechanicalKeyboardCommand.CanExecute(null));
    }

    [TestMethod]
    public void PurchaseMechanicalKeyboardCommand_WhenExecuted_UpdatesNextKeyboardCost()
    {
        // Arrange
        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);
        for (int i = 0; i < 25; i++)
        {
            viewModel.WriteCodeCommand.Execute(null);
        }

        // Act
        viewModel.PurchaseMechanicalKeyboardCommand.Execute(null);

        // Assert
        Assert.AreEqual(50L, viewModel.MechanicalKeyboardCost);
    }

    [TestMethod]
    public void PurchaseMechanicalKeyboardCommand_AfterFirstPurchaseWithEnoughLines_CanExecute()
    {
        // Arrange
        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);
        for (int i = 0; i < 75; i++)
        {
            viewModel.WriteCodeCommand.Execute(null);
        }

        // Act
        viewModel.PurchaseMechanicalKeyboardCommand.Execute(null);

        // Assert
        Assert.IsTrue(viewModel.PurchaseMechanicalKeyboardCommand.CanExecute(null));
    }

    [TestMethod]
    public void PurchaseMechanicalKeyboardCommand_WhenExecuted_RaisesPropertyChangedForMechanicalKeyboardCost()
    {
        // Arrange
        var gameState = new GameState();
        var viewModel = new MainViewModel(gameState);

        for (int i = 0; i < 25; i++)
        {
            viewModel.WriteCodeCommand.Execute(null);
        }

        _changedPropertyName = null;
        viewModel.PropertyChanged += HandlePropertyChanged;

        // Act
        viewModel.PurchaseMechanicalKeyboardCommand.Execute(null);

        // Assert
        Assert.AreEqual(nameof(MainViewModel.MechanicalKeyboardCost), _changedPropertyName);
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
}
