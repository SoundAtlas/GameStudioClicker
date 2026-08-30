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
            InternCount = 3,
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
        Assert.AreEqual(3, root.GetProperty(nameof(GameSaveData.InternCount)).GetInt32());
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
        var saveData = new GameSaveData
        {
            LinesOfCode = 456,
            InternCount = 5,
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
            Assert.AreEqual(5, loadedSaveData.InternCount);
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
}
