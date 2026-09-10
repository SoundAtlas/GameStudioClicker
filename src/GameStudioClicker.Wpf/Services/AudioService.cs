using System.IO;
using System.Windows.Media;

namespace GameStudioClicker.Wpf.Services
{
    public sealed class AudioService
    {
        private readonly MediaPlayer _soundtrackPlayer = new();
        private readonly string[] _soundtrackPaths;
        private int _currentSoundtrackIndex;
        private bool _isSoundtrackOpen;

        private readonly MediaPlayer _writeCodePressPlayer = new();
        private readonly string _writeCodePressSoundPath;
        private bool _isWriteCodePressSoundOpen;

        private readonly MediaPlayer _writeCodeReleasePlayer = new();
        private readonly string _writeCodeReleaseSoundPath;
        private bool _isWriteCodeReleaseSoundOpen;

        private readonly MediaPlayer _menuClickPlayer = new();
        private readonly string _menuClickSoundPath;
        private bool _isMenuClickSoundOpen;

        private readonly MediaPlayer _achievementEarnedPlayer = new();
        private readonly string _achievementEarnedSoundPath;
        private bool _isAchievementEarnedSoundOpen;

        private readonly MediaPlayer _activeUpgradePlayer = new();
        private readonly string _activeUpgradeSoundPath;
        private bool _isActiveUpgradeSoundOpen;

        private readonly MediaPlayer _hireEmployeePlayer = new();
        private readonly string _hireEmployeeSoundPath;
        private bool _isHireEmployeeSoundOpen;


        public AudioService()
        {
            _soundtrackPaths =
                [
                Path.Combine(AppContext.BaseDirectory, "Assets", "Audio", "Music", "soundtrack1.wav"),
                Path.Combine(AppContext.BaseDirectory, "Assets", "Audio", "Music", "soundtrack2.wav")
                ];

            _soundtrackPlayer.Volume = 0.2;
            _soundtrackPlayer.MediaEnded += SoundtrackPlayer_MediaEnded;

            _writeCodePressSoundPath = Path.Combine(
                AppContext.BaseDirectory, "Assets", "Audio", "SFX", "menu_click_sound2.wav");
            _writeCodePressPlayer.Volume = 0.2;
            _writeCodeReleaseSoundPath = Path.Combine(
                AppContext.BaseDirectory, "Assets", "Audio", "SFX", "menu_click_sound3.wav");
            _writeCodeReleasePlayer.Volume = 0.2;

            _menuClickSoundPath = Path.Combine(
                AppContext.BaseDirectory, "Assets", "Audio", "SFX", "menu_click_sound1.wav");
            _menuClickPlayer.Volume = 0.2;

            _achievementEarnedSoundPath = Path.Combine(
                AppContext.BaseDirectory, "Assets", "Audio", "SFX", "achievement_unlock.wav");
            _achievementEarnedPlayer.Volume = 0.2;

            _activeUpgradeSoundPath = Path.Combine(
                AppContext.BaseDirectory, "Assets", "Audio", "SFX", "blip3.wav");
            _activeUpgradePlayer.Volume = 0.2;

            _hireEmployeeSoundPath = Path.Combine(
                AppContext.BaseDirectory, "Assets", "Audio", "SFX", "click.wav");
            _hireEmployeePlayer.Volume = 0.2;
        }

        private void SoundtrackPlayer_MediaEnded(object? sender, EventArgs e)
        {
            _currentSoundtrackIndex++;

            if (_currentSoundtrackIndex >= _soundtrackPaths.Length)
            {
                _currentSoundtrackIndex = 0;
            }

            PlayCurrentSoundtrack();
        }

        public void StartSoundtrack()
        {
            if (_isSoundtrackOpen)
            {
                return;
            }

            _isSoundtrackOpen = true;
            PlayCurrentSoundtrack();
        }

        private void PlayCurrentSoundtrack()
        {
            string currentPath =
                _soundtrackPaths[_currentSoundtrackIndex];

            _soundtrackPlayer.Open(new Uri(currentPath, UriKind.Absolute));

            _soundtrackPlayer.Play();
        }

        public void SetMasterVolume(double masterVolume)
        {
            _soundtrackPlayer.Volume = 0.2 * masterVolume;
            _writeCodePressPlayer.Volume = 0.2 * masterVolume;
            _writeCodeReleasePlayer.Volume = 0.2 * masterVolume;
            _menuClickPlayer.Volume = 0.2 * masterVolume;
            _achievementEarnedPlayer.Volume = 0.2 * masterVolume;
            _activeUpgradePlayer.Volume = 0.2 * masterVolume;
            _hireEmployeePlayer.Volume = 0.2 * masterVolume;
        }

        public void PlayWriteCodePressSound()
        {
            // Avoids audio click when opening the game
            if (!_isWriteCodePressSoundOpen)
            {
                _writeCodePressPlayer.Open(
                    new Uri(_writeCodePressSoundPath, UriKind.Absolute));

                _isWriteCodePressSoundOpen = true;
            }
            // Resets file to play from the beginning, so that the sound can be played multiple times in a row
            _writeCodePressPlayer.Position = TimeSpan.Zero;
            _writeCodePressPlayer.Play();
        }

        public void PlayWriteCodeReleaseSound()
        {
            if (!_isWriteCodeReleaseSoundOpen)
            {
                _writeCodeReleasePlayer.Open(
                    new Uri(_writeCodeReleaseSoundPath, UriKind.Absolute));
                _isWriteCodeReleaseSoundOpen = true;
            }
            _writeCodeReleasePlayer.Position = TimeSpan.Zero;
            _writeCodeReleasePlayer.Play();
        }

        public void PlayMenuClickSound()
        {
            if (!_isMenuClickSoundOpen)
            {
                _menuClickPlayer.Open(
                    new Uri(_menuClickSoundPath, UriKind.Absolute));

                _isMenuClickSoundOpen = true;
            }
            _menuClickPlayer.Position = TimeSpan.Zero;
            _menuClickPlayer.Play();
        }

        public void PlayAchievementEarnedSound()
        {
            if (!_isAchievementEarnedSoundOpen)
            {
                _achievementEarnedPlayer.Open(
                    new Uri(_achievementEarnedSoundPath, UriKind.Absolute));
                _isAchievementEarnedSoundOpen = true;
            }
            _achievementEarnedPlayer.Position = TimeSpan.Zero;
            _achievementEarnedPlayer.Play();
        }

        public void PlayActiveUpgradeSound()
        {
            if (!_isActiveUpgradeSoundOpen)
            {
                _activeUpgradePlayer.Open(
                    new Uri(_activeUpgradeSoundPath, UriKind.Absolute));
                _isActiveUpgradeSoundOpen = true;
            }
            _activeUpgradePlayer.Position = TimeSpan.Zero;
            _activeUpgradePlayer.Play();
        }

        public void PlayHireEmployeeSound()
        {
            if (!_isHireEmployeeSoundOpen)
            {
                _hireEmployeePlayer.Open(
                    new Uri(_hireEmployeeSoundPath, UriKind.Absolute));
                _isHireEmployeeSoundOpen = true;
            }
            _hireEmployeePlayer.Position = TimeSpan.Zero;
            _hireEmployeePlayer.Play();
        }

        public void Close()
        {
            _soundtrackPlayer.MediaEnded -= SoundtrackPlayer_MediaEnded;
            _soundtrackPlayer.Close();
            _currentSoundtrackIndex = 0;

            _writeCodePressPlayer.Close();
            _writeCodeReleasePlayer.Close();
            _menuClickPlayer.Close();
            _achievementEarnedPlayer.Close();
            _activeUpgradePlayer.Close();
            _hireEmployeePlayer.Close();

            _isSoundtrackOpen = false;
            _isWriteCodePressSoundOpen = false;
            _isWriteCodeReleaseSoundOpen = false;
            _isMenuClickSoundOpen = false;
            _isAchievementEarnedSoundOpen = false;
            _isActiveUpgradeSoundOpen = false;
            _isHireEmployeeSoundOpen = false;
        }
    }
}
