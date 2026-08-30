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

        ActiveUpgrade keyboard = GetActiveUpgrade(gameState, "mechanical_keyboard");
        ActiveUpgrade monitor = GetActiveUpgrade(gameState, "ultrawide_monitor");

        Assert.AreEqual(100L, keyboard.Cost);
        Assert.IsFalse(keyboard.IsPurchased);
        Assert.IsTrue(keyboard.IsAvailable);
        Assert.IsFalse(gameState.CanPurchaseActiveUpgrade(keyboard));
        Assert.IsFalse(monitor.IsAvailable);
    }

    [TestMethod]
    public void TryPurchaseActiveUpgrade_WhenKeyboardPurchaseSucceeds_UpdatesOneTimeUpgradeState()
    {
        var gameState = new GameState();
        ActiveUpgrade keyboard = GetActiveUpgrade(gameState, "mechanical_keyboard");
        ActiveUpgrade monitor = GetActiveUpgrade(gameState, "ultrawide_monitor");

        for (int i = 0; i < 105; i++)
        {
            gameState.WriteCode();
        }

        bool result = gameState.TryPurchaseActiveUpgrade(keyboard);

        Assert.IsTrue(result);
        Assert.AreEqual(5L, gameState.LinesOfCode);
        Assert.AreEqual(2L, gameState.LinesPerClick);
        Assert.IsTrue(keyboard.IsPurchased);
        Assert.IsFalse(keyboard.IsAvailable);
        Assert.IsTrue(monitor.IsAvailable);
        Assert.AreEqual(100L, keyboard.Cost);
    }

    [TestMethod]
    public void TryPurchaseActiveUpgrade_AfterPurchase_ReturnsFalse()
    {
        var gameState = new GameState();
        gameState.RestoreFromSaveData(new GameSaveData
        {
            LinesOfCode = 1_000,
            PurchasedActiveUpgradeIds = new List<string>
            {
                "mechanical_keyboard"
            }
        });

        ActiveUpgrade keyboard = GetActiveUpgrade(gameState, "mechanical_keyboard");

        bool result = gameState.TryPurchaseActiveUpgrade(keyboard);

        Assert.IsFalse(result);
        Assert.AreEqual(1_000L, gameState.LinesOfCode);
        Assert.AreEqual(2L, gameState.LinesPerClick);
    }

    [TestMethod]
    public void TryPurchaseActiveUpgrade_WhenMonitorPurchaseSucceeds_UpdatesOneTimeUpgradeState()
    {
        var gameState = new GameState();
        gameState.RestoreFromSaveData(new GameSaveData
        {
            LinesOfCode = 1_000,
            PurchasedActiveUpgradeIds = new List<string>
            {
                "mechanical_keyboard"
            }
        });

        ActiveUpgrade monitor = GetActiveUpgrade(gameState, "ultrawide_monitor");

        bool result = gameState.TryPurchaseActiveUpgrade(monitor);

        Assert.IsTrue(result);
        Assert.AreEqual(0L, gameState.LinesOfCode);
        Assert.AreEqual(4L, gameState.LinesPerClick);
        Assert.IsTrue(monitor.IsPurchased);
        Assert.IsFalse(monitor.IsAvailable);
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
            InternCount = 5,
            JuniorDeveloperCount = 2,
            PurchasedActiveUpgradeIds = new List<string>
            {
                "mechanical_keyboard",
                "ultrawide_monitor"
            }
        });

        GameSaveData saveData = gameState.CreateSaveData();

        Assert.AreEqual(500L, saveData.LinesOfCode);
        CollectionAssert.AreEqual(
            new List<string> { "mechanical_keyboard", "ultrawide_monitor" },
            saveData.PurchasedActiveUpgradeIds);
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
            InternCount = 5,
            JuniorDeveloperCount = 2,
            PurchasedActiveUpgradeIds = new List<string>
            {
                "mechanical_keyboard",
                "ultrawide_monitor"
            }
        };

        gameState.RestoreFromSaveData(saveData);

        ActiveUpgrade keyboard = GetActiveUpgrade(gameState, "mechanical_keyboard");
        ActiveUpgrade monitor = GetActiveUpgrade(gameState, "ultrawide_monitor");

        Assert.AreEqual(250L, gameState.LinesOfCode);
        Assert.IsTrue(keyboard.IsPurchased);
        Assert.IsTrue(monitor.IsPurchased);
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

    private static ActiveUpgrade GetActiveUpgrade(GameState gameState, string id)
    {
        return gameState.ActiveUpgrades.Single(upgrade => upgrade.Id == id);
    }
}
