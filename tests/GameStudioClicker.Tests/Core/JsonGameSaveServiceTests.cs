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
            IsMechanicalKeyboardPurchased = true,
            IsUltrawideMonitorPurchased = true,
            InternCount = 3
        };

        // Act
        string json = service.SerializeGameSaveData(saveData);

        // Assert
        StringAssert.Contains(json, Environment.NewLine);
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement root = document.RootElement;
        Assert.AreEqual(123L, root.GetProperty(nameof(GameSaveData.LinesOfCode)).GetInt64());
        Assert.IsTrue(root.GetProperty(nameof(GameSaveData.IsMechanicalKeyboardPurchased)).GetBoolean());
        Assert.IsTrue(root.GetProperty(nameof(GameSaveData.IsUltrawideMonitorPurchased)).GetBoolean());
        Assert.AreEqual(3, root.GetProperty(nameof(GameSaveData.InternCount)).GetInt32());
    }

    [TestMethod]
    public void SaveToFileAndLoadFromFile_RoundTripsSaveData()
    {
        // Arrange
        var service = new JsonGameSaveService();
        var saveData = new GameSaveData
        {
            LinesOfCode = 456,
            IsMechanicalKeyboardPurchased = true,
            IsUltrawideMonitorPurchased = true,
            InternCount = 5
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
            Assert.IsTrue(loadedSaveData.IsMechanicalKeyboardPurchased);
            Assert.IsTrue(loadedSaveData.IsUltrawideMonitorPurchased);
            Assert.AreEqual(5, loadedSaveData.InternCount);
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
