using System.IO;
using System.Text.Json;

namespace GameStudioClicker.Wpf.Settings
{
    public class JsonApplicationSettingsRepository : IApplicationSettingsRepository
    {
        private readonly string _filePath;

        public JsonApplicationSettingsRepository(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("A settings file path is required", nameof(filePath));
            }

            _filePath = filePath;
        }
        public ApplicationSettings Load()
        {
            if (!File.Exists(_filePath))
            {
                return new ApplicationSettings();
            }

            try
            {
                string json = File.ReadAllText(_filePath);

                ApplicationSettings? settings =
                    JsonSerializer.Deserialize<ApplicationSettings>(json);

                if (settings is null)
                {
                    return new ApplicationSettings();
                }

                settings.MusicVolumePercent =
                    Math.Clamp(settings.MusicVolumePercent, 0, 100);

                settings.SfxVolumePercent =
                    Math.Clamp(settings.SfxVolumePercent, 0, 100);

                return settings;
            }
            catch (JsonException)
            {
                string timestamp = DateTime.UtcNow.ToString("yyyyMMdd-HHmmssfff");
                string corruptFilePath = $"{_filePath}.corrupt-{timestamp}";

                File.Move(_filePath, corruptFilePath);
                return new ApplicationSettings();
            }
        }

        public void Save(ApplicationSettings saveSettings)
        {
            string json = JsonSerializer.Serialize(
                saveSettings,
                new JsonSerializerOptions { WriteIndented = true });

            string? directoryPath = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            File.WriteAllText(_filePath, json);
        }
    }
}
