using System.Text.Json;
using GameStudioClicker.Core.Persistence;

namespace GameStudioClicker.Tests;

[TestClass]
public class JsonGameSaveServiceTests
{
    [TestMethod]
    public void SerializeGameSaveData_ReturnsReadableJsonWithSavedValues()
    {
        // Arrange
        var service = new JsonGameSaveService();
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
        string json = service.SerializeGameSaveData(saveData);

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
    public void SaveToFileAndLoadFromFile_RoundTripsSaveData()
    {
        // Arrange
        var service = new JsonGameSaveService();
        DateTime savedAtUtc = new(2026, 9, 5, 18, 30, 0, DateTimeKind.Utc);
        var saveData = new GameSaveData
        {
            LinesOfCode = 456,
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

        try
        {
            // Act
            service.SaveToFile(saveData, filePath);
            GameSaveData? loadedSaveData = service.LoadFromFile(filePath);

            // Assert
            Assert.IsNotNull(loadedSaveData);
            Assert.AreEqual(456L, loadedSaveData.LinesOfCode);
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
    public void LoadFromFile_WhenFileDoesNotExist_ReturnsNull()
    {
        // Arrange
        var service = new JsonGameSaveService();
        string filePath = Path.Combine(
            Path.GetTempPath(),
            $"GameStudioClicker-{Guid.NewGuid():N}.json");

        // Act
        GameSaveData? result = service.LoadFromFile(filePath);

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void LoadFromFile_WhenJsonIsMalformed_PreservesCorruptFileAndReturnsNull()
    {
        var service = new JsonGameSaveService();
        string filePath = Path.Combine(
            Path.GetTempPath(),
            $"GameStudioClicker-{Guid.NewGuid():N}.json");
        string corruptPattern = $"{Path.GetFileName(filePath)}.corrupt-*";
        const string malformedJson = "{ not valid json";

        try
        {
            File.WriteAllText(filePath, malformedJson);

            GameSaveData? result = service.LoadFromFile(filePath);
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
