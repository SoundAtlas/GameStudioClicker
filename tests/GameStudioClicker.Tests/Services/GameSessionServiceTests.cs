using GameStudioClicker.Core.Models;
using GameStudioClicker.Core.Persistence;
using GameStudioClicker.Wpf.Services;

namespace GameStudioClicker.Tests;

[TestClass]
public class GameSessionServiceTests
{
    [TestMethod]
    public void Start_WithExistingSave_RestoresStateAndAppliesOfflineProgress()
    {
        string saveDirectoryPath = CreateTemporarySaveDirectoryPath();
        string saveFilePath = Path.Combine(saveDirectoryPath, "game_save.json");
        Directory.CreateDirectory(saveDirectoryPath);

        var saveService = new JsonGameSaveService();
        saveService.SaveToFile(
            new GameSaveData
            {
                LinesOfCode = 100,
                LifetimeLinesOfCode = 100,
                WorkerUpgradeCounts = new Dictionary<string, int>
                {
                    ["intern"] = 1
                },
                SavedAtUtc = DateTime.UtcNow - TimeSpan.FromHours(30)
            },
            saveFilePath);

        var session = new GameSessionService(saveDirectoryPath);

        try
        {
            session.Start();

            WorkerUpgrade intern = session.GameState.WorkerUpgrades
                .Single(worker => worker.Id == "intern");
            Assert.AreEqual(1, intern.WorkerCount);
            Assert.AreEqual(172_800L, session.OfflineLinesEarned);
            Assert.AreEqual(172_900L, session.GameState.LinesOfCode);
        }
        finally
        {
            session.Dispose();
            DeleteTemporarySaveDirectory(saveDirectoryPath);
        }
    }

    [TestMethod]
    public void Save_WritesCurrentGameStateAndTimestamp()
    {
        string saveDirectoryPath = CreateTemporarySaveDirectoryPath();
        string saveFilePath = Path.Combine(saveDirectoryPath, "game_save.json");
        var session = new GameSessionService(saveDirectoryPath);

        try
        {
            session.GameState.RestoreFromSaveData(new GameSaveData
            {
                LinesOfCode = 456,
                LifetimeLinesOfCode = 789,
                PurchasedActiveUpgradeIds = ["mouse_pad"],
                WorkerUpgradeCounts = new Dictionary<string, int>
                {
                    ["intern"] = 3
                }
            });

            session.Save();

            var saveService = new JsonGameSaveService();
            GameSaveData? savedData = saveService.LoadFromFile(saveFilePath);
            Assert.IsNotNull(savedData);
            Assert.AreEqual(456L, savedData.LinesOfCode);
            Assert.AreEqual(789L, savedData.LifetimeLinesOfCode);
            CollectionAssert.Contains(savedData.PurchasedActiveUpgradeIds, "mouse_pad");
            Assert.AreEqual(3, savedData.WorkerUpgradeCounts["intern"]);
            Assert.AreNotEqual(default, savedData.SavedAtUtc);
        }
        finally
        {
            session.Dispose();
            DeleteTemporarySaveDirectory(saveDirectoryPath);
        }
    }

    [TestMethod]
    public void Dispose_WhenCalledTwice_RemainsSafeAndSavesFinalState()
    {
        string saveDirectoryPath = CreateTemporarySaveDirectoryPath();
        string saveFilePath = Path.Combine(saveDirectoryPath, "game_save.json");
        var session = new GameSessionService(saveDirectoryPath);

        try
        {
            session.GameState.RestoreFromSaveData(new GameSaveData
            {
                LinesOfCode = 321
            });

            session.Dispose();
            session.Dispose();

            var saveService = new JsonGameSaveService();
            GameSaveData? savedData = saveService.LoadFromFile(saveFilePath);
            Assert.IsNotNull(savedData);
            Assert.AreEqual(321L, savedData.LinesOfCode);
        }
        finally
        {
            session.Dispose();
            DeleteTemporarySaveDirectory(saveDirectoryPath);
        }
    }

    private static string CreateTemporarySaveDirectoryPath()
    {
        return Path.Combine(
            Path.GetTempPath(),
            $"GameStudioClicker-{Guid.NewGuid():N}");
    }

    private static void DeleteTemporarySaveDirectory(string saveDirectoryPath)
    {
        if (Directory.Exists(saveDirectoryPath))
        {
            Directory.Delete(saveDirectoryPath, recursive: true);
        }
    }
}
