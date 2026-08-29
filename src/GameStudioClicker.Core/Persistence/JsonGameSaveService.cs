using System.Text.Json;

namespace GameStudioClicker.Core.Persistence
{
    public class JsonGameSaveService
    {
        public string SerializeGameSaveData(GameSaveData saveData)
        {
            return JsonSerializer.Serialize(saveData, new JsonSerializerOptions { WriteIndented = true });
        }

        public void SaveToFile(GameSaveData saveData, string filePath)
        {
            string json = SerializeGameSaveData(saveData);

            File.WriteAllText(filePath, json);
        }

        public GameSaveData? LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            string json = File.ReadAllText(filePath);

            return JsonSerializer.Deserialize<GameSaveData>(json);
        }
    }
}
