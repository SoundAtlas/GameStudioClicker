using GameStudioClicker.Core.Models;

namespace GameStudioClicker.Tests;

[TestClass]
public class GameStateTests
{
    [TestMethod]
    public void GameState_StartsWithZeroLinesOfCode()
    {
        // Arrange
        var gameState = new GameState();

        // Act: No action needed, we are testing the initial state

        // Assert
        Assert.AreEqual(0L, gameState.LinesOfCode);
    }

    [TestMethod]
    public void WriteCode_OneLine_IncrementsLinesOfCode()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        gameState.WriteCode();

        // Assert
        Assert.AreEqual(1L, gameState.LinesOfCode);
    }

    [TestMethod]
    public void WriteCode_MultipleLines_IncrementsLinesOfCode()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        gameState.WriteCode();
        gameState.WriteCode();
        gameState.WriteCode();

        // Assert
        Assert.AreEqual(3L, gameState.LinesOfCode);
    }

    [TestMethod]
    public void GameState_StartsWithOneLinePerClick()
    {
        // Arrange
        var gameState = new GameState();

        // Act: No action needed, we are testing the initial state

        // Assert
        Assert.AreEqual(1L, gameState.LinesPerClick);
    }

    [TestMethod]
    public void GameState_MechanicalKeyboardCost_IsTen()
    {
        // Arrange
        var gameState = new GameState();

        // Act: No action needed, we are testing the initial state

        // Assert
        Assert.AreEqual(10L, gameState.MechanicalKeyboardCost);
    }

    [TestMethod]
    public void NewGameState_DoesNotOwnMechanicalKeyboard()
    {
        // Arrange
        var gameState = new GameState();

        // Act: No action needed, we are testing the initial state

        // Assert
        Assert.IsFalse(gameState.IsMechanicalKeyboardOwned);
    }

    [TestMethod]
    public void NewGameState_CannotPurchaseMechanicalKeyboard()
    {
        // Arrange
        var gameState = new GameState();

        // Act: No action needed, we are testing the initial state

        // Assert
        Assert.IsFalse(gameState.CanPurchaseMechanicalKeyboard);
    }

    [TestMethod]
    public void GameState_WithNineLines_CannotPurchaseMechanicalKeyboard()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        for (int i = 0; i < 9; i++)
        {
            gameState.WriteCode();
        }

        // Assert
        Assert.IsFalse(gameState.CanPurchaseMechanicalKeyboard);
    }

    [TestMethod]
    public void GameState_WithTenLines_CanPurchaseMechanicalKeyboard()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        for (int i = 0; i < 10; i++)
        {
            gameState.WriteCode();
        }

        // Assert
        Assert.IsTrue(gameState.CanPurchaseMechanicalKeyboard);
    }
}
