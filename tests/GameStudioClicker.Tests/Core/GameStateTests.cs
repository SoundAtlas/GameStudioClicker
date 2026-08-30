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
        Assert.AreEqual(1_000L, gameState.LinesPerClick);
        Assert.AreEqual(0L, gameState.LinesPerSecond);

        ActiveUpgrade mousePad = GetActiveUpgrade(gameState, "mouse_pad");
        ActiveUpgrade gamingMouse = GetActiveUpgrade(gameState, "gaming_mouse");
        WorkerUpgrade intern = GetWorkerUpgrade(gameState, "intern");

        Assert.AreEqual(100L, mousePad.Cost);
        Assert.IsFalse(mousePad.IsPurchased);
        Assert.IsTrue(mousePad.IsAvailable);
        Assert.IsFalse(gameState.CanPurchaseActiveUpgrade(mousePad));
        Assert.IsFalse(gamingMouse.IsAvailable);
        Assert.AreEqual(50L, intern.CurrentCost);
        Assert.AreEqual(0, intern.WorkerCount);
    }

    [TestMethod]
    public void TryPurchaseActiveUpgrade_WhenFirstUpgradePurchaseSucceeds_UpdatesState()
    {
        var gameState = new GameState();
        ActiveUpgrade mousePad = GetActiveUpgrade(gameState, "mouse_pad");
        ActiveUpgrade gamingMouse = GetActiveUpgrade(gameState, "gaming_mouse");

        gameState.WriteCode();

        bool result = gameState.TryPurchaseActiveUpgrade(mousePad);

        Assert.IsTrue(result);
        Assert.AreEqual(900L, gameState.LinesOfCode);
        Assert.AreEqual(2_000L, gameState.LinesPerClick);
        Assert.IsTrue(mousePad.IsPurchased);
        Assert.IsFalse(mousePad.IsAvailable);
        Assert.IsTrue(gamingMouse.IsAvailable);
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
                "mouse_pad"
            }
        });

        ActiveUpgrade mousePad = GetActiveUpgrade(gameState, "mouse_pad");

        bool result = gameState.TryPurchaseActiveUpgrade(mousePad);

        Assert.IsFalse(result);
        Assert.AreEqual(1_000L, gameState.LinesOfCode);
        Assert.AreEqual(2L, gameState.LinesPerClick);
    }

    [TestMethod]
    public void TryPurchaseActiveUpgrade_WhenPrerequisiteWasPurchased_UpdatesState()
    {
        var gameState = new GameState();
        gameState.RestoreFromSaveData(new GameSaveData
        {
            LinesOfCode = 400,
            PurchasedActiveUpgradeIds = new List<string>
            {
                "mouse_pad"
            }
        });

        ActiveUpgrade gamingMouse = GetActiveUpgrade(gameState, "gaming_mouse");

        bool result = gameState.TryPurchaseActiveUpgrade(gamingMouse);

        Assert.IsTrue(result);
        Assert.AreEqual(0L, gameState.LinesOfCode);
        Assert.AreEqual(4L, gameState.LinesPerClick);
        Assert.IsTrue(gamingMouse.IsPurchased);
        Assert.IsFalse(gamingMouse.IsAvailable);
    }

    [TestMethod]
    public void TryPurchaseWorkerUpgrade_WithEnoughLines_UpdatesPassiveProduction()
    {
        var gameState = new GameState();
        gameState.RestoreFromSaveData(new GameSaveData
        {
            LinesOfCode = 50
        });
        WorkerUpgrade intern = GetWorkerUpgrade(gameState, "intern");

        bool result = gameState.TryPurchaseWorkerUpgrade(intern);

        Assert.IsTrue(result);
        Assert.AreEqual(0L, gameState.LinesOfCode);
        Assert.AreEqual(1, intern.WorkerCount);
        Assert.AreEqual(2L, gameState.LinesPerSecond);
        Assert.AreEqual(100L, intern.CurrentCost);
    }

    [TestMethod]
    public void GeneratePassiveLines_AfterPurchasingIntern_AddsProduction()
    {
        var gameState = new GameState();
        gameState.RestoreFromSaveData(new GameSaveData
        {
            WorkerUpgradeCounts = new Dictionary<string, int>
            {
                ["intern"] = 1
            }
        });

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
            WorkerUpgradeCounts = new Dictionary<string, int>
            {
                ["intern"] = 5,
                ["junior_developer"] = 2
            },
            PurchasedActiveUpgradeIds = new List<string>
            {
                "mouse_pad",
                "gaming_mouse"
            }
        });

        GameSaveData saveData = gameState.CreateSaveData();

        Assert.AreEqual(500L, saveData.LinesOfCode);
        CollectionAssert.AreEqual(
            new List<string> { "mouse_pad", "gaming_mouse" },
            saveData.PurchasedActiveUpgradeIds);
        Assert.AreEqual(5, saveData.WorkerUpgradeCounts["intern"]);
        Assert.AreEqual(2, saveData.WorkerUpgradeCounts["junior_developer"]);
    }

    [TestMethod]
    public void RestoreFromSaveData_RestoresStateAndRecalculatesDerivedValues()
    {
        var gameState = new GameState();
        var saveData = new GameSaveData
        {
            LinesOfCode = 250,
            WorkerUpgradeCounts = new Dictionary<string, int>
            {
                ["intern"] = 5,
                ["junior_developer"] = 2
            },
            PurchasedActiveUpgradeIds = new List<string>
            {
                "mouse_pad",
                "gaming_mouse"
            }
        };

        gameState.RestoreFromSaveData(saveData);

        ActiveUpgrade mousePad = GetActiveUpgrade(gameState, "mouse_pad");
        ActiveUpgrade gamingMouse = GetActiveUpgrade(gameState, "gaming_mouse");
        WorkerUpgrade intern = GetWorkerUpgrade(gameState, "intern");
        WorkerUpgrade juniorDeveloper = GetWorkerUpgrade(gameState, "junior_developer");

        Assert.AreEqual(250L, gameState.LinesOfCode);
        Assert.IsTrue(mousePad.IsPurchased);
        Assert.IsTrue(gamingMouse.IsPurchased);
        Assert.AreEqual(4L, gameState.LinesPerClick);
        Assert.AreEqual(5, intern.WorkerCount);
        Assert.AreEqual(2, juniorDeveloper.WorkerCount);
        Assert.AreEqual(50L, gameState.LinesPerSecond);
        Assert.AreEqual(1_600L, intern.CurrentCost);
        Assert.AreEqual(8_000L, juniorDeveloper.CurrentCost);
    }

    [TestMethod]
    public void RestoreFromSaveData_WithNegativeWorkerValues_ClampsValuesToZero()
    {
        var gameState = new GameState();
        var saveData = new GameSaveData
        {
            LinesOfCode = -1,
            WorkerUpgradeCounts = new Dictionary<string, int>
            {
                ["intern"] = -1,
                ["junior_developer"] = -1
            }
        };

        gameState.RestoreFromSaveData(saveData);

        WorkerUpgrade intern = GetWorkerUpgrade(gameState, "intern");
        WorkerUpgrade juniorDeveloper = GetWorkerUpgrade(gameState, "junior_developer");

        Assert.AreEqual(0L, gameState.LinesOfCode);
        Assert.AreEqual(0, intern.WorkerCount);
        Assert.AreEqual(0, juniorDeveloper.WorkerCount);
        Assert.AreEqual(1L, gameState.LinesPerClick);
        Assert.AreEqual(0L, gameState.LinesPerSecond);
        Assert.AreEqual(50L, intern.CurrentCost);
        Assert.AreEqual(2_000L, juniorDeveloper.CurrentCost);
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
            WorkerUpgradeCounts = new Dictionary<string, int>
            {
                ["intern"] = 1,
                ["junior_developer"] = 1
            }
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
            WorkerUpgradeCounts = new Dictionary<string, int>
            {
                ["intern"] = 1
            }
        });

        long earnedLines = gameState.ApplyOfflineProgress(TimeSpan.FromMinutes(-5));

        Assert.AreEqual(0L, earnedLines);
        Assert.AreEqual(100L, gameState.LinesOfCode);
    }

    [TestMethod]
    public void ApplyOfflineProgress_OverTwentyFourHours_CapsProductionAtTwentyFourHours()
    {
        var gameState = new GameState();
        gameState.RestoreFromSaveData(new GameSaveData
        {
            WorkerUpgradeCounts = new Dictionary<string, int>
            {
                ["intern"] = 1
            }
        });

        long earnedLines = gameState.ApplyOfflineProgress(TimeSpan.FromHours(30));

        Assert.AreEqual(172_800L, earnedLines);
        Assert.AreEqual(172_800L, gameState.LinesOfCode);
    }

    private static ActiveUpgrade GetActiveUpgrade(GameState gameState, string id)
    {
        return gameState.ActiveUpgrades.Single(upgrade => upgrade.Id == id);
    }

    private static WorkerUpgrade GetWorkerUpgrade(GameState gameState, string id)
    {
        return gameState.WorkerUpgrades.Single(upgrade => upgrade.Id == id);
    }
}
