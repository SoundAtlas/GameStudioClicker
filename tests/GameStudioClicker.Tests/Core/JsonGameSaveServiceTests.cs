using GameStudioClicker.Core.Persistence;
using System.Text.Json;

namespace GameStudioClicker.Tests;

[TestClass]
public class JsonGameSaveRepositoryTests
{
    [TestMethod]
    public void SerializeGameSaveData_ReturnsReadableJsonWithSavedValues()
    {
        // Arrange
        string filePath = Path.Combine(
            Path.GetTempPath(),
            $"GameStudioClicker-{Guid.NewGuid():N}.json");
        var repository = new JsonGameSaveRepository(filePath);
        var saveData = new GameSaveData
        {
            LinesOfCode = 123,
            WorkerUpgradeCounts = new Dictionary<string, int>
            {
                ["intern"] = 3
            },
            PurchasedActiveUpgradeIds = new List<string>
            {
                "mechanical_keyboard",
                "ultrawide_monitor"
            }
        };

        // Act
        string json = repository.SerializeGameSaveData(saveData);

        // Assert
        StringAssert.Contains(json, Environment.NewLine);
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement root = document.RootElement;
        Assert.AreEqual(123L, root.GetProperty(nameof(GameSaveData.LinesOfCode)).GetInt64());
        JsonElement workerCounts =
            root.GetProperty(nameof(GameSaveData.WorkerUpgradeCounts));
        Assert.AreEqual(3, workerCounts.GetProperty("intern").GetInt32());
        JsonElement purchasedUpgradeIds =
            root.GetProperty(nameof(GameSaveData.PurchasedActiveUpgradeIds));
        Assert.AreEqual(2, purchasedUpgradeIds.GetArrayLength());
        Assert.AreEqual("mechanical_keyboard", purchasedUpgradeIds[0].GetString());
        Assert.AreEqual("ultrawide_monitor", purchasedUpgradeIds[1].GetString());
    }

    [TestMethod]
    public void SaveAndLoad_RoundTripsSaveData()
    {
        // Arrange
        DateTime savedAtUtc = new(2026, 9, 5, 18, 30, 0, DateTimeKind.Utc);
        var saveData = new GameSaveData
        {
            LinesOfCode = 456,
            LifetimeLinesOfCode = 1_000,
            LinesGeneratedManually = 400,
            LinesGeneratedByWorkers = 600,
            LinesGeneratedWhileOnline = 700,
            LinesGeneratedWhileOffline = 300,
            SavedAtUtc = savedAtUtc,
            WorkerUpgradeCounts = new Dictionary<string, int>
            {
                ["intern"] = 5,
                ["junior_developer"] = 2
            },
            PurchasedActiveUpgradeIds = new List<string>
            {
                "mechanical_keyboard",
                "ultrawide_monitor"
            }
        };
        string filePath = Path.Combine(
            Path.GetTempPath(),
            $"GameStudioClicker-{Guid.NewGuid():N}.json");
        var repository = new JsonGameSaveRepository(filePath);

        try
        {
            // Act
            repository.Save(saveData);
            GameSaveData? loadedSaveData = repository.Load();

            // Assert
            Assert.IsNotNull(loadedSaveData);
            Assert.AreEqual(456L, loadedSaveData.LinesOfCode);
            Assert.AreEqual(1_000L, loadedSaveData.LifetimeLinesOfCode);
            Assert.AreEqual(400L, loadedSaveData.LinesGeneratedManually);
            Assert.AreEqual(600L, loadedSaveData.LinesGeneratedByWorkers);
            Assert.AreEqual(700L, loadedSaveData.LinesGeneratedWhileOnline);
            Assert.AreEqual(300L, loadedSaveData.LinesGeneratedWhileOffline);
            Assert.AreEqual(savedAtUtc, loadedSaveData.SavedAtUtc);
            Assert.AreEqual(5, loadedSaveData.WorkerUpgradeCounts["intern"]);
            Assert.AreEqual(2, loadedSaveData.WorkerUpgradeCounts["junior_developer"]);
            CollectionAssert.AreEqual(
                new List<string> { "mechanical_keyboard", "ultrawide_monitor" },
                loadedSaveData.PurchasedActiveUpgradeIds);
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    [TestMethod]
    public void Load_WithObsoleteWorkerCountProperties_IgnoresThem()
    {
        string filePath = Path.Combine(
            Path.GetTempPath(),
            $"GameStudioClicker-{Guid.NewGuid():N}.json");
        var repository = new JsonGameSaveRepository(filePath);
        const string legacyJson = """
                                  {
                                    "LinesOfCode": 456,
                                    "InternCount": 5,
                                    "JuniorDeveloperCount": 2
                                  }
                                  """;

        try
        {
            File.WriteAllText(filePath, legacyJson);

            GameSaveData? loadedSaveData = repository.Load();

            Assert.IsNotNull(loadedSaveData);
            Assert.AreEqual(456L, loadedSaveData.LinesOfCode);
            Assert.IsEmpty(loadedSaveData.WorkerUpgradeCounts);
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    [TestMethod]
    public void Load_WhenFileDoesNotExist_ReturnsNull()
    {
        // Arrange
        string filePath = Path.Combine(
            Path.GetTempPath(),
            $"GameStudioClicker-{Guid.NewGuid():N}.json");
        var repository = new JsonGameSaveRepository(filePath);

        // Act
        GameSaveData? result = repository.Load();

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void Load_WhenJsonIsMalformed_PreservesCorruptFileAndReturnsNull()
    {
        string filePath = Path.Combine(
            Path.GetTempPath(),
            $"GameStudioClicker-{Guid.NewGuid():N}.json");
        var repository = new JsonGameSaveRepository(filePath);
        string corruptPattern = $"{Path.GetFileName(filePath)}.corrupt-*";
        const string malformedJson = "{ not valid json";

        try
        {
            File.WriteAllText(filePath, malformedJson);

            GameSaveData? result = repository.Load();
            string[] corruptFiles = Directory.GetFiles(
                Path.GetDirectoryName(filePath)!,
                corruptPattern);

            Assert.IsNull(result);
            Assert.IsFalse(File.Exists(filePath));
            Assert.HasCount(1, corruptFiles);
            Assert.AreEqual(malformedJson, File.ReadAllText(corruptFiles[0]));
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            foreach (string corruptFile in Directory.GetFiles(
                         Path.GetDirectoryName(filePath)!,
                         corruptPattern))
            {
                File.Delete(corruptFile);
            }
        }
    }
}
