using GameStudioClicker.Core.Models;
using GameStudioClicker.Core.Persistence;

namespace GameStudioClicker.Tests;

[TestClass]
public class GameStateTests
{
    [TestMethod]
    public void NewGameState_HasExpectedInitialState()
    {
        var gameState = new GameState();

        Assert.AreEqual(0L, gameState.LinesOfCode);
        Assert.AreEqual(1L, gameState.LinesPerClick);
        Assert.AreEqual(0L, gameState.LinesPerSecond);
        Assert.AreEqual(100L, gameState.MechanicalKeyboardCost);
        Assert.IsFalse(gameState.IsMechanicalKeyboardPurchased);
        Assert.IsTrue(gameState.IsMechanicalKeyboardAvailable);
        Assert.IsFalse(gameState.CanPurchaseMechanicalKeyboard);
    }

    [TestMethod]
    public void TryPurchaseMechanicalKeyboard_WhenSuccessful_UpdatesOneTimeUpgradeState()
    {
        var gameState = new GameState();
        for (int i = 0; i < 105; i++)
        {
            gameState.WriteCode();
        }

        bool result = gameState.TryPurchaseMechanicalKeyboard();

        Assert.IsTrue(result);
        Assert.AreEqual(5L, gameState.LinesOfCode);
        Assert.AreEqual(2L, gameState.LinesPerClick);
        Assert.IsTrue(gameState.IsMechanicalKeyboardPurchased);
        Assert.IsFalse(gameState.IsMechanicalKeyboardAvailable);
        Assert.IsTrue(gameState.IsUltrawideMonitorAvailable);
        Assert.AreEqual(100L, gameState.MechanicalKeyboardCost);
    }

    [TestMethod]
    public void TryPurchaseMechanicalKeyboard_AfterPurchase_ReturnsFalse()
    {
        var gameState = new GameState();
        gameState.RestoreFromSaveData(new GameSaveData
        {
            LinesOfCode = 1_000,
            IsMechanicalKeyboardPurchased = true
        });

        bool result = gameState.TryPurchaseMechanicalKeyboard();

        Assert.IsFalse(result);
        Assert.AreEqual(1_000L, gameState.LinesOfCode);
        Assert.AreEqual(2L, gameState.LinesPerClick);
    }

    [TestMethod]
    public void TryPurchaseUltrawideMonitor_WhenSuccessful_UpdatesOneTimeUpgradeState()
    {
        var gameState = new GameState();
        gameState.RestoreFromSaveData(new GameSaveData
        {
            LinesOfCode = 1_000,
            IsMechanicalKeyboardPurchased = true
        });

        bool result = gameState.TryPurchaseUltrawideMonitor();

        Assert.IsTrue(result);
        Assert.AreEqual(0L, gameState.LinesOfCode);
        Assert.AreEqual(4L, gameState.LinesPerClick);
        Assert.IsTrue(gameState.IsUltrawideMonitorPurchased);
        Assert.IsFalse(gameState.IsUltrawideMonitorAvailable);
    }

    [TestMethod]
    public void TryPurchaseIntern_WithEnoughLines_UpdatesPassiveProduction()
    {
        var gameState = new GameState();
        for (int i = 0; i < 50; i++)
        {
            gameState.WriteCode();
        }

        bool result = gameState.TryPurchaseIntern();

        Assert.IsTrue(result);
        Assert.AreEqual(0L, gameState.LinesOfCode);
        Assert.AreEqual(1, gameState.InternCount);
        Assert.AreEqual(2L, gameState.LinesPerSecond);
        Assert.AreEqual(100L, gameState.InternCost);
    }

    [TestMethod]
    public void GeneratePassiveLines_AfterPurchasingIntern_AddsProduction()
    {
        var gameState = new GameState();
        gameState.RestoreFromSaveData(new GameSaveData { InternCount = 1 });

        gameState.GeneratePassiveLines();

        Assert.AreEqual(2L, gameState.LinesOfCode);
    }

    [TestMethod]
    public void CreateSaveData_CopiesPersistentState()
    {
        var gameState = new GameState();
        gameState.RestoreFromSaveData(new GameSaveData
        {
            LinesOfCode = 500,
            IsMechanicalKeyboardPurchased = true,
            IsUltrawideMonitorPurchased = true,
            InternCount = 5,
            JuniorDeveloperCount = 2
        });

        GameSaveData saveData = gameState.CreateSaveData();

        Assert.AreEqual(500L, saveData.LinesOfCode);
        Assert.IsTrue(saveData.IsMechanicalKeyboardPurchased);
        Assert.IsTrue(saveData.IsUltrawideMonitorPurchased);
        Assert.AreEqual(5, saveData.InternCount);
        Assert.AreEqual(2, saveData.JuniorDeveloperCount);
    }

    [TestMethod]
    public void RestoreFromSaveData_RestoresStateAndRecalculatesDerivedValues()
    {
        var gameState = new GameState();
        var saveData = new GameSaveData
        {
            LinesOfCode = 250,
            IsMechanicalKeyboardPurchased = true,
            IsUltrawideMonitorPurchased = true,
            InternCount = 5,
            JuniorDeveloperCount = 2
        };

        gameState.RestoreFromSaveData(saveData);

        Assert.AreEqual(250L, gameState.LinesOfCode);
        Assert.IsTrue(gameState.IsMechanicalKeyboardPurchased);
        Assert.IsTrue(gameState.IsUltrawideMonitorPurchased);
        Assert.AreEqual(4L, gameState.LinesPerClick);
        Assert.AreEqual(5, gameState.InternCount);
        Assert.AreEqual(2, gameState.JuniorDeveloperCount);
        Assert.AreEqual(50L, gameState.LinesPerSecond);
        Assert.AreEqual(1_600L, gameState.InternCost);
        Assert.AreEqual(8_000L, gameState.JuniorDeveloperCost);
    }

    [TestMethod]
    public void RestoreFromSaveData_WithNegativeWorkerValues_ClampsValuesToZero()
    {
        var gameState = new GameState();
        var saveData = new GameSaveData
        {
            LinesOfCode = -1,
            InternCount = -1,
            JuniorDeveloperCount = -1
        };

        gameState.RestoreFromSaveData(saveData);

        Assert.AreEqual(0L, gameState.LinesOfCode);
        Assert.AreEqual(0, gameState.InternCount);
        Assert.AreEqual(0, gameState.JuniorDeveloperCount);
        Assert.AreEqual(1L, gameState.LinesPerClick);
        Assert.AreEqual(0L, gameState.LinesPerSecond);
        Assert.AreEqual(50L, gameState.InternCost);
        Assert.AreEqual(2_000L, gameState.JuniorDeveloperCost);
    }

    [TestMethod]
    public void RestoreFromSaveData_WithNull_ThrowsArgumentNullException()
    {
        var gameState = new GameState();

        Assert.ThrowsExactly<ArgumentNullException>(
            () => gameState.RestoreFromSaveData(null!));
    }

    [TestMethod]
    public void ApplyOfflineProgress_WithWorkers_AddsWholeSecondProduction()
    {
        var gameState = new GameState();
        gameState.RestoreFromSaveData(new GameSaveData
        {
            InternCount = 1,
            JuniorDeveloperCount = 1
        });

        long earnedLines = gameState.ApplyOfflineProgress(TimeSpan.FromSeconds(10.8));

        Assert.AreEqual(220L, earnedLines);
        Assert.AreEqual(220L, gameState.LinesOfCode);
    }

    [TestMethod]
    public void ApplyOfflineProgress_WithNegativeElapsedTime_DoesNotAddLines()
    {
        var gameState = new GameState();
        gameState.RestoreFromSaveData(new GameSaveData
        {
            LinesOfCode = 100,
            InternCount = 1
        });

        long earnedLines = gameState.ApplyOfflineProgress(TimeSpan.FromMinutes(-5));

        Assert.AreEqual(0L, earnedLines);
        Assert.AreEqual(100L, gameState.LinesOfCode);
    }

    [TestMethod]
    public void ApplyOfflineProgress_OverTwentyFourHours_CapsProductionAtTwentyFourHours()
    {
        var gameState = new GameState();
        gameState.RestoreFromSaveData(new GameSaveData { InternCount = 1 });

        long earnedLines = gameState.ApplyOfflineProgress(TimeSpan.FromHours(30));

        Assert.AreEqual(172_800L, earnedLines);
        Assert.AreEqual(172_800L, gameState.LinesOfCode);
    }
}
