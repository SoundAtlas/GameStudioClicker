using System.Text.Json;

namespace GameStudioClicker.Core.Persistence
{
    public class JsonGameSaveService
    {
        // Converts the save snapshot to readable JSON.
        public string SerializeGameSaveData(GameSaveData saveData)
        {
            return JsonSerializer.Serialize(saveData, new JsonSerializerOptions { WriteIndented = true });
        }

        // Writes a complete snapshot, replacing the previous save file.
        public void SaveToFile(GameSaveData saveData, string filePath)
        {
            string json = SerializeGameSaveData(saveData);

            File.WriteAllText(filePath, json);
        }

        // A missing file represents a new game and is not treated as an error.
        public GameSaveData? LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            try
            {
                string json = File.ReadAllText(filePath);

                GameSaveData? saveData = JsonSerializer.Deserialize<GameSaveData>(json);

                return saveData == null ? throw new JsonException("Save data was null") : saveData;
            }
            catch (JsonException)
            {
                string timeStamp = DateTime.UtcNow.ToString("yyyyMMdd-HHmmssfff");

                string corruptFilePath = $"{filePath}.corrupt-{timeStamp}";

                File.Move(filePath, corruptFilePath);

                return null;
            }
        }
    }
}
