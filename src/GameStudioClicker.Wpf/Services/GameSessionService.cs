using GameStudioClicker.Core.Models;
using GameStudioClicker.Core.Persistence;
using System.IO;
using System.Windows.Threading;

namespace GameStudioClicker.Wpf.Services
{
    public sealed class GameSessionService : IDisposable
    {
        private readonly string _saveDirectoryPath;
        private readonly string _saveFilePath;
        private readonly JsonGameSaveService _jsonGameSaveService;

        private readonly DispatcherTimer _autosaveTimer;
        private bool _isDisposed;

        public GameState GameState { get; }
        public long OfflineLinesEarned { get; private set; }

        public GameSessionService(string? saveDirectoryPath = null)
        {
            _saveDirectoryPath = saveDirectoryPath ??
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "GameStudioClicker");

            _saveFilePath = Path.Combine(_saveDirectoryPath, "game_save.json");

            _jsonGameSaveService = new JsonGameSaveService();
            GameState = new GameState();

            _autosaveTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(30)
            };
            _autosaveTimer.Tick += AutoSaveTimer_Tick;
        }

        private void AutoSaveTimer_Tick(object? sender, EventArgs e)
        {
            Save();
        }

        private void Load()
        {
            // Reset in case there is no usable save file. 
            OfflineLinesEarned = 0;
            // Ensure the save directory exists
            Directory.CreateDirectory(_saveDirectoryPath);

            GameSaveData? saveData = _jsonGameSaveService.LoadFromFile(_saveFilePath);
            // If Save Data is null, return immediately - start a new game
            if (saveData is null)
            {
                return;
            }

            GameState.RestoreFromSaveData(saveData);

            // No valid timestamp so return without calculating offline earnings. 
            if (saveData.SavedAtUtc == default)
            {
                return;
            }

            // Calculate offline earnings
            TimeSpan elapsedTime = DateTime.UtcNow - saveData.SavedAtUtc;

            OfflineLinesEarned = GameState.ApplyOfflineProgress(elapsedTime);
        }

        public void Save()
        {
            Directory.CreateDirectory(_saveDirectoryPath);

            GameSaveData saveData = GameState.CreateSaveData();
            saveData.SavedAtUtc = DateTime.UtcNow;
            _jsonGameSaveService.SaveToFile(saveData, _saveFilePath);
        }

        public void Start()
        {
            Load();
            _autosaveTimer.Start();
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;

            _autosaveTimer.Stop();
            _autosaveTimer.Tick -= AutoSaveTimer_Tick;

            Save();
        }
    }
}
