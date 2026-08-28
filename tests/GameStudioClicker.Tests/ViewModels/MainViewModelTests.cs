using GameStudioClicker.Core.Models;
using GameStudioClicker.Wpf.ViewModels;
using System.ComponentModel;

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

    private string? _changedPropertyName;
    private void HandlePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        _changedPropertyName = e.PropertyName;
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
