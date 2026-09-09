using System.Text.Json;

namespace GameStudioClicker.Core.Persistence;

public class JsonGameSaveRepository : IGameSaveRepository
{
    // Converts a save snapshot to readable JSON.
    public string SerializeGameSaveData(GameSaveData saveData)
    {
        return JsonSerializer.Serialize(
            saveData,
            new JsonSerializerOptions { WriteIndented = true });
    }

    private readonly string _filePath;

    public JsonGameSaveRepository(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("A save file path is required", nameof(filePath));
        }

        _filePath = filePath;
    }
    // Writes a complete snapshot, replacing the previous save file.
    public void Save(GameSaveData saveData)
    {
        string json = SerializeGameSaveData(saveData);

        // Ensure the directory exists before writing the file.
        string? directoryPath = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        File.WriteAllText(_filePath, json);
    }

    // A missing file represents a new game and is not treated as an error.
    public GameSaveData? Load()
    {
        if (!File.Exists(_filePath))
        {
            return null;
        }

        try
        {
            string json = File.ReadAllText(_filePath);
            GameSaveData? saveData = JsonSerializer.Deserialize<GameSaveData>(json);

            return saveData is null
                ? throw new JsonException("Save data was null")
                : saveData;
        }
        catch (JsonException)
        {
            string timestamp = DateTime.UtcNow.ToString("yyyyMMdd-HHmmssfff");
            string corruptFilePath = $"{_filePath}.corrupt-{timestamp}";

            File.Move(_filePath, corruptFilePath);
            return null;
        }
    }
}
