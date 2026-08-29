using GameStudioClicker.Core.Models;
using GameStudioClicker.Core.Persistence;

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
    public void GameState_StartsWithOneLinePerClick()
    {
        // Arrange
        var gameState = new GameState();

        // Act: No action needed, we are testing the initial state

        // Assert
        Assert.AreEqual(1L, gameState.LinesPerClick);
    }

    [TestMethod]
    public void GameState_MechanicalKeyboardCost_IsTwentyFive()
    {
        // Arrange
        var gameState = new GameState();

        // Act: No action needed, we are testing the initial state

        // Assert
        Assert.AreEqual(25L, gameState.MechanicalKeyboardCost);
    }

    [TestMethod]
    public void GameState_WithTwentyFourLines_CannotPurchaseMechanicalKeyboard()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        for (int i = 0; i < 24; i++)
        {
            gameState.WriteCode();
        }

        // Assert
        Assert.IsFalse(gameState.CanPurchaseMechanicalKeyboard);
    }

    [TestMethod]
    public void GameState_WithTwentyFiveLines_CanPurchaseMechanicalKeyboard()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        for (int i = 0; i < 25; i++)
        {
            gameState.WriteCode();
        }

        // Assert
        Assert.IsTrue(gameState.CanPurchaseMechanicalKeyboard);
    }

    [TestMethod]
    public void NewGameState_StartsWithZeroMechanicalKeyboards()
    {
        // Arrange
        var gameState = new GameState();

        // Act: No action needed, we are testing the initial state

        // Assert
        Assert.AreEqual(0, gameState.MechanicalKeyboardCount);
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
    public void NewGameState_StartsWithZeroLinesPerSecond()
    {
        // Arrange
        var gameState = new GameState();

        // Act: No action needed, we are testing the initial state

        // Assert
        Assert.AreEqual(0L, gameState.LinesPerSecond);
    }

    [TestMethod]
    public void NewGameState_StartsWithZeroInterns()
    {
        // Arrange
        var gameState = new GameState();

        // Act: No action needed, we are testing the initial state

        // Assert
        Assert.AreEqual(0, gameState.InternCount);
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
        Assert.AreEqual(0, gameState.MechanicalKeyboardCount);
        Assert.AreEqual(0L, gameState.LinesOfCode);
        Assert.AreEqual(1L, gameState.LinesPerClick);
    }

    [TestMethod]
    public void TryPurchaseMechanicalKeyboard_WithEnoughLines_ReturnsTrue()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        for (int i = 0; i < 25; i++)
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
        for (int i = 0; i < 30; i++)
        {
            gameState.WriteCode();
        }
        gameState.TryPurchaseMechanicalKeyboard();

        // Assert
        Assert.AreEqual(5L, gameState.LinesOfCode);
    }

    [TestMethod]
    public void TryPurchaseMechanicalKeyboard_WithEnoughLines_IncreasesLinesPerClick()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        for (int i = 0; i < 25; i++)
        {
            gameState.WriteCode();
        }
        gameState.TryPurchaseMechanicalKeyboard();

        // Assert
        Assert.AreEqual(2L, gameState.LinesPerClick);
    }

    [TestMethod]
    public void TryPurchaseMechanicalKeyboard_WhenSuccessful_IncrementsKeyboardCount()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        for (int i = 0; i < 25; i++)
        {
            gameState.WriteCode();
        }
        gameState.TryPurchaseMechanicalKeyboard();


        // Assert
        Assert.AreEqual(1, gameState.MechanicalKeyboardCount);
    }

    [TestMethod]
    public void TryPurchaseMechanicalKeyboard_WhenSuccessful_DoublesNextKeyboardCost()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        for (int i = 0; i < 25; i++)
        {
            gameState.WriteCode();
        }
        gameState.TryPurchaseMechanicalKeyboard();


        // Assert   
        Assert.AreEqual(50L, gameState.MechanicalKeyboardCost);
    }

    [TestMethod]
    public void TryPurchaseMechanicalKeyboard_AfterFirstPurchase_CanPurchaseAnotherKeyboard()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        for (int i = 0; i < 75; i++)
        {
            gameState.WriteCode();
        }
        gameState.TryPurchaseMechanicalKeyboard();
        var canPurchaseSecondKeyboard = gameState.CanPurchaseMechanicalKeyboard;

        // Assert   
        Assert.IsTrue(canPurchaseSecondKeyboard);
    }

    [TestMethod]
    public void TryPurchaseMechanicalKeyboard_Twice_IncreasesLinesPerClickToThree()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        for (int i = 0; i < 75; i++)
        {
            gameState.WriteCode();
        }
        gameState.TryPurchaseMechanicalKeyboard();
        gameState.TryPurchaseMechanicalKeyboard();


        // Assert   
        Assert.AreEqual(3L, gameState.LinesPerClick);
    }

    [TestMethod]
    public void TryPurchaseMechanicalKeyboard_Twice_IncreasesKeyboardCountToTwo()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        for (int i = 0; i < 75; i++)
        {
            gameState.WriteCode();
        }
        gameState.TryPurchaseMechanicalKeyboard();
        gameState.TryPurchaseMechanicalKeyboard();


        // Assert   
        Assert.AreEqual(2, gameState.MechanicalKeyboardCount);
    }

    [TestMethod]
    public void WriteCode_AfterMechanicalKeyboardPurchase_AddsTwoLines()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        for (int i = 0; i < 25; i++)
        {
            gameState.WriteCode();
        }
        gameState.TryPurchaseMechanicalKeyboard();
        gameState.WriteCode();

        // Assert
        Assert.AreEqual(2L, gameState.LinesOfCode);
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
    public void GeneratePassiveLines_ZeroProduction_DoesNotAddLines()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        gameState.GeneratePassiveLines();
        gameState.GeneratePassiveLines();
        gameState.GeneratePassiveLines();

        // Assert
        Assert.AreEqual(0L, gameState.LinesOfCode);
    }

    [TestMethod]
    public void GeneratePassiveLines_AfterPurchasingIntern_AddsTwoLines()
    {
        // Arrange
        var gameState = new GameState();
        for (int i = 0; i < 50; i++)
        {
            gameState.WriteCode();
        }

        // Act
        gameState.TryPurchaseIntern();
        gameState.GeneratePassiveLines();

        // Assert       
        Assert.AreEqual(2L, gameState.LinesOfCode);
    }

    [TestMethod]
    public void TryPurchaseIntern_WithEnoughLines_IncreasesLinesPerSecond()
    {
        // Arrange
        var gameState = new GameState();
        for (int i = 0; i < 50; i++)
        {
            gameState.WriteCode();
        }

        // Act
        gameState.TryPurchaseIntern();

        // Assert       
        Assert.AreEqual(2L, gameState.LinesPerSecond);
    }

    [TestMethod]
    public void CreateSaveData_CopiesCurrentPersistentState()
    {
        // Arrange
        var gameState = new GameState();
        for (int i = 0; i < 75; i++)
        {
            gameState.WriteCode();
        }
        gameState.TryPurchaseMechanicalKeyboard();
        gameState.TryPurchaseIntern();

        // Act
        GameSaveData saveData = gameState.CreateSaveData();

        // Assert
        Assert.AreEqual(0L, saveData.LinesOfCode);
        Assert.AreEqual(1, saveData.MechanicalKeyboardCount);
        Assert.AreEqual(1, saveData.InternCount);
    }

    [TestMethod]
    public void RestoreFromSaveData_RestoresStateAndRecalculatesDerivedValues()
    {
        // Arrange
        var gameState = new GameState();
        var saveData = new GameSaveData
        {
            LinesOfCode = 250,
            MechanicalKeyboardCount = 2,
            InternCount = 3
        };

        // Act
        gameState.RestoreFromSaveData(saveData);

        // Assert
        Assert.AreEqual(250L, gameState.LinesOfCode);
        Assert.AreEqual(2, gameState.MechanicalKeyboardCount);
        Assert.AreEqual(3L, gameState.LinesPerClick);
        Assert.AreEqual(100L, gameState.MechanicalKeyboardCost);
        Assert.AreEqual(3, gameState.InternCount);
        Assert.AreEqual(6L, gameState.LinesPerSecond);
        Assert.AreEqual(400L, gameState.InternCost);
    }

    [TestMethod]
    public void RestoreFromSaveData_WithNegativeValues_ClampsValuesToZero()
    {
        // Arrange
        var gameState = new GameState();
        var saveData = new GameSaveData
        {
            LinesOfCode = -1,
            MechanicalKeyboardCount = -1,
            InternCount = -1
        };

        // Act
        gameState.RestoreFromSaveData(saveData);

        // Assert
        Assert.AreEqual(0L, gameState.LinesOfCode);
        Assert.AreEqual(0, gameState.MechanicalKeyboardCount);
        Assert.AreEqual(0, gameState.InternCount);
        Assert.AreEqual(1L, gameState.LinesPerClick);
        Assert.AreEqual(0L, gameState.LinesPerSecond);
        Assert.AreEqual(25L, gameState.MechanicalKeyboardCost);
        Assert.AreEqual(50L, gameState.InternCost);
    }

    [TestMethod]
    public void RestoreFromSaveData_WithNull_ThrowsArgumentNullException()
    {
        // Arrange
        var gameState = new GameState();

        // Act and assert
        Assert.ThrowsExactly<ArgumentNullException>(() => gameState.RestoreFromSaveData(null!));
    }

}
