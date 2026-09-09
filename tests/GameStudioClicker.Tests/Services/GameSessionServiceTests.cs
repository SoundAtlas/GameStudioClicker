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
        var repository = new InMemoryGameSaveRepository(
            new GameSaveData
            {
                LinesOfCode = 100,
                LifetimeLinesOfCode = 100,
                WorkerUpgradeCounts = new Dictionary<string, int>
                {
                    ["intern"] = 1
                },
                SavedAtUtc = DateTime.UtcNow - TimeSpan.FromHours(30)
            });

        var session = new GameSessionService(repository);

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
        }
    }

    [TestMethod]
    public void Save_WritesCurrentGameStateAndTimestamp()
    {
        var repository = new InMemoryGameSaveRepository();
        var session = new GameSessionService(repository);

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

            GameSaveData? savedData = repository.SavedData;
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
        }
    }

    [TestMethod]
    public void Dispose_WhenCalledTwice_RemainsSafeAndSavesFinalState()
    {
        var repository = new InMemoryGameSaveRepository();
        var session = new GameSessionService(repository);

        try
        {
            session.GameState.RestoreFromSaveData(new GameSaveData
            {
                LinesOfCode = 321
            });

            session.Dispose();
            session.Dispose();

            GameSaveData? savedData = repository.SavedData;
            Assert.IsNotNull(savedData);
            Assert.AreEqual(321L, savedData.LinesOfCode);
        }
        finally
        {
            session.Dispose();
        }
    }

    private sealed class InMemoryGameSaveRepository : IGameSaveRepository
    {
        public InMemoryGameSaveRepository(GameSaveData? savedData = null)
        {
            SavedData = savedData;
        }

        public GameSaveData? SavedData { get; private set; }

        public GameSaveData? Load()
        {
            return SavedData;
        }

        public void Save(GameSaveData saveData)
        {
            SavedData = saveData;
        }
    }
}
