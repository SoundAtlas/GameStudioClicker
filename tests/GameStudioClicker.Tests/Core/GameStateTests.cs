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

    [TestMethod]
    public void TryPurchaseMechanicalKeyboard_WithoutEnoughLines_ReturnsFalse()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        var result = gameState.TryPurchaseMechanicalKeyboard();

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void TryPurchaseMechanicalKeyboard_WithoutEnoughLines_DoesNotChangeGameState()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        gameState.TryPurchaseMechanicalKeyboard();

        // Assert
        Assert.IsFalse(gameState.IsMechanicalKeyboardOwned);
        Assert.AreEqual(0L, gameState.LinesOfCode);
        Assert.AreEqual(1L, gameState.LinesPerClick);
    }

    [TestMethod]
    public void TryPurchaseMechanicalKeyboard_WithEnoughLines_ReturnsTrue()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        for (int i = 0; i < 10; i++)
        {
            gameState.WriteCode();
        }
        var result = gameState.TryPurchaseMechanicalKeyboard();

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void TryPurchaseMechanicalKeyboard_WhenSuccessful_DeductsKeyboardCost()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        for (int i = 0; i < 15; i++)
        {
            gameState.WriteCode();
        }
        gameState.TryPurchaseMechanicalKeyboard();

        // Assert   
        Assert.AreEqual(5L, gameState.LinesOfCode);
    }

    [TestMethod]
    public void TryPurchaseMechanicalKeyboard_WithEnoughLines_MarksKeyboardAsOwned()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        for (int i = 0; i < 10; i++)
        {
            gameState.WriteCode();
        }
        gameState.TryPurchaseMechanicalKeyboard();

        // Assert   
        Assert.IsTrue(gameState.IsMechanicalKeyboardOwned);
    }

    [TestMethod]
    public void TryPurchaseMechanicalKeyboard_WithEnoughLines_IncreasesLinesPerClick()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        for (int i = 0; i < 10; i++)
        {
            gameState.WriteCode();
        }
        gameState.TryPurchaseMechanicalKeyboard();

        // Assert   
        Assert.AreEqual(2L, gameState.LinesPerClick);
    }

    [TestMethod]
    public void TryPurchaseMechanicalKeyboard_WhenAlreadyOwned_ReturnsFalse()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        for (int i = 0; i < 20; i++)
        {
            gameState.WriteCode();
        }
        gameState.TryPurchaseMechanicalKeyboard();
        var secondResult = gameState.TryPurchaseMechanicalKeyboard();

        // Assert   
        Assert.IsFalse(secondResult);
    }

    [TestMethod]
    public void WriteCode_AfterMechanicalKeyboardPurchase_AddsTwoLines()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        for (int i = 0; i < 10; i++)
        {
            gameState.WriteCode();
        }
        gameState.TryPurchaseMechanicalKeyboard();
        gameState.WriteCode();

        // Assert   
        Assert.AreEqual(2L, gameState.LinesOfCode);
    }
}
