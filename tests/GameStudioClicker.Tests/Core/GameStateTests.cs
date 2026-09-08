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
    public void NewGameState_ActiveUpgradeIds_AreUnique()
    {
        var gameState = new GameState();

        int uniqueIdCount = gameState.ActiveUpgrades
            .Select(upgrade => upgrade.Id)
            .Distinct()
            .Count();

        Assert.AreEqual(gameState.ActiveUpgrades.Count, uniqueIdCount);
    }

    [TestMethod]
    public void NewGameState_ActiveUpgrades_AreInExpectedProgressionOrder()
    {
        var gameState = new GameState();
        string[] expectedIds =
        [
            "mouse_pad",
            "gaming_mouse",
            "mechanical_keyboard",
            "noise_cancelling_headset",
            "onboarding_handbook",
            "graphics_card",
            "second_monitor",
            "ergonomic_desk_setup",
            "developer_laptop",
            "code_review_checklist",
            "professional_ide_license",
            "high_end_workstation",
            "intern_mentorship_program",
            "build_server",
            "automated_testing_suite",
            "architecture_workshop",
            "automated_development_pipeline",
            "capture_studio",
            "pair_programming_sessions",
            "studio_server_rack",
            "proprietary_game_engine",
            "technical_leadership_training",
            "render_farm",
            "global_cloud_infrastructure",
            "ai_workstations",
            "developer_toolkit",
            "neural_motion_capture_system",
            "autonomous_qa_swarm",
            "self_organizing_dev_teams",
            "quantum_build_server",
            "predictive_game_engine",
            "adaptive_learning_program"
        ];
        string[] actualIds = gameState.ActiveUpgrades
            .Select(upgrade => upgrade.Id)
            .ToArray();

        CollectionAssert.AreEqual(expectedIds, actualIds);
    }

    [TestMethod]
    public void TryPurchaseActiveUpgrade_WhenFirstUpgradePurchaseSucceeds_UpdatesState()
    {
        var gameState = new GameState();
        gameState.RestoreFromSaveData(new GameSaveData
        {
            LinesOfCode = 100
        });
        ActiveUpgrade mousePad = GetActiveUpgrade(gameState, "mouse_pad");
        ActiveUpgrade gamingMouse = GetActiveUpgrade(gameState, "gaming_mouse");

        bool result = gameState.TryPurchaseActiveUpgrade(mousePad);

        Assert.IsTrue(result);
        Assert.AreEqual(0L, gameState.LinesOfCode);
        Assert.AreEqual(2L, gameState.LinesPerClick);
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
        Assert.AreEqual(2L, gameState.LifetimeLinesOfCode);
        Assert.AreEqual(2L, gameState.LinesGeneratedByWorkers);
        Assert.AreEqual(2L, gameState.LinesGeneratedWhileOnline);
        Assert.AreEqual(0L, gameState.LinesGeneratedManually);
        Assert.AreEqual(0L, gameState.LinesGeneratedWhileOffline);
    }

    [TestMethod]
    public void WriteCode_UpdatesManualAndOnlineStatistics()
    {
        var gameState = new GameState();
        long linesPerClick = gameState.LinesPerClick;

        gameState.WriteCode();

        Assert.AreEqual(linesPerClick, gameState.LinesOfCode);
        Assert.AreEqual(linesPerClick, gameState.LifetimeLinesOfCode);
        Assert.AreEqual(1L, gameState.LifetimeManualClicks);
        Assert.AreEqual(linesPerClick, gameState.LinesGeneratedManually);
        Assert.AreEqual(linesPerClick, gameState.LinesGeneratedWhileOnline);
        Assert.AreEqual(0L, gameState.LinesGeneratedByWorkers);
        Assert.AreEqual(0L, gameState.LinesGeneratedWhileOffline);
    }

    [TestMethod]
    public void CanPurchaseTargetedUpgrade_WithoutTargetWorker_ReturnsFalse()
    {
        var gameState = new GameState();
        gameState.RestoreFromSaveData(new GameSaveData
        {
            LinesOfCode = 1_200,
            PurchasedActiveUpgradeIds = ["noise_cancelling_headset"]
        });
        ActiveUpgrade onboardingHandbook =
            GetActiveUpgrade(gameState, "onboarding_handbook");

        bool result = gameState.CanPurchaseActiveUpgrade(onboardingHandbook);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void TryPurchaseTargetedUpgrade_WithTargetWorker_OnlyMultipliesThatWorker()
    {
        var gameState = new GameState();
        gameState.RestoreFromSaveData(new GameSaveData
        {
            LinesOfCode = 1_200,
            PurchasedActiveUpgradeIds = ["noise_cancelling_headset"],
            WorkerUpgradeCounts = new Dictionary<string, int>
            {
                ["intern"] = 1,
                ["junior_developer"] = 1
            }
        });
        ActiveUpgrade onboardingHandbook =
            GetActiveUpgrade(gameState, "onboarding_handbook");
        WorkerUpgrade intern = GetWorkerUpgrade(gameState, "intern");
        WorkerUpgrade juniorDeveloper = GetWorkerUpgrade(gameState, "junior_developer");

        bool result = gameState.TryPurchaseActiveUpgrade(onboardingHandbook);

        Assert.IsTrue(result);
        Assert.AreEqual(4L, gameState.GetWorkerLinesPerSecondPerEmployee(intern));
        Assert.AreEqual(20L, gameState.GetWorkerLinesPerSecondPerEmployee(juniorDeveloper));
        Assert.AreEqual(24L, gameState.LinesPerSecond);
    }

    [TestMethod]
    public void CreateSaveData_CopiesPersistentState()
    {
        var gameState = new GameState();
        gameState.RestoreFromSaveData(new GameSaveData
        {
            LinesOfCode = 500,
            LifetimeLinesOfCode = 1_000,
            LifetimeManualClicks = 50,
            LifetimeEmployeesHired = 7,
            LifetimeActiveUpgradesPurchased = 2,
            LinesGeneratedManually = 400,
            LinesGeneratedByWorkers = 600,
            LinesGeneratedWhileOnline = 700,
            LinesGeneratedWhileOffline = 300,
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
        Assert.AreEqual(1_000L, saveData.LifetimeLinesOfCode);
        Assert.AreEqual(50L, saveData.LifetimeManualClicks);
        Assert.AreEqual(7L, saveData.LifetimeEmployeesHired);
        Assert.AreEqual(2L, saveData.LifetimeActiveUpgradesPurchased);
        Assert.AreEqual(400L, saveData.LinesGeneratedManually);
        Assert.AreEqual(600L, saveData.LinesGeneratedByWorkers);
        Assert.AreEqual(700L, saveData.LinesGeneratedWhileOnline);
        Assert.AreEqual(300L, saveData.LinesGeneratedWhileOffline);
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
    public void RestoreFromSaveData_WithMissingCollections_UsesEmptyCollections()
    {
        var gameState = new GameState();
        var saveData = new GameSaveData
        {
            LinesOfCode = 25,
            PurchasedActiveUpgradeIds = null!,
            WorkerUpgradeCounts = null!
        };

        gameState.RestoreFromSaveData(saveData);

        Assert.AreEqual(25L, gameState.LinesOfCode);
        Assert.AreEqual(1L, gameState.LinesPerClick);
        Assert.AreEqual(0L, gameState.LinesPerSecond);
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
        Assert.AreEqual(220L, gameState.LifetimeLinesOfCode);
        Assert.AreEqual(220L, gameState.LinesGeneratedByWorkers);
        Assert.AreEqual(220L, gameState.LinesGeneratedWhileOffline);
        Assert.AreEqual(0L, gameState.LinesGeneratedManually);
        Assert.AreEqual(0L, gameState.LinesGeneratedWhileOnline);
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
