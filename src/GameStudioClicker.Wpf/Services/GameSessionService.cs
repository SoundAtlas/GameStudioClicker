using GameStudioClicker.Core.Models;
using GameStudioClicker.Core.Persistence;
using System.Windows.Threading;

namespace GameStudioClicker.Wpf.Services
{
    public sealed class GameSessionService : IDisposable
    {
        private readonly IGameSaveRepository _gameSaveRepository;

        private readonly DispatcherTimer _autosaveTimer;
        private bool _isDisposed;

        public GameState GameState { get; }
        public long OfflineLinesEarned { get; private set; }

        public GameSessionService(IGameSaveRepository gameSaveRepository)
        {
            _gameSaveRepository = gameSaveRepository ??
                throw new ArgumentNullException(nameof(gameSaveRepository));

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
            // Reset in case there is no usable saved data. 
            OfflineLinesEarned = 0;

            GameSaveData? saveData = _gameSaveRepository.Load();
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
            GameSaveData saveData = GameState.CreateSaveData();
            saveData.SavedAtUtc = DateTime.UtcNow;
            _gameSaveRepository.Save(saveData);
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
