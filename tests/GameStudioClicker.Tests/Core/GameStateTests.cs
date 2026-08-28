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
}
