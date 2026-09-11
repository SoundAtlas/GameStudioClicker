using NAudio.Wave;
using System.IO;
using System.Windows.Media;

namespace GameStudioClicker.Wpf.Services
{
    public sealed class AudioService
    {
        private SmoothedVolumeSampleProvider? _musicVolumeProvider;
        private double _musicVolume = 1.0;
        private double _sfxVolume = 1.0;

        private AudioFileReader? _soundtrackReader;
        private WaveOutEvent? _soundtrackOutput;
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
                Path.Combine(AppContext.BaseDirectory, "Assets", "Audio", "Music", "soundtrack1_first_commit.mp3"),
                Path.Combine(AppContext.BaseDirectory, "Assets", "Audio", "Music", "soundtrack2_after_hours.mp3"),
                Path.Combine(AppContext.BaseDirectory, "Assets", "Audio", "Music", "soundtrack3_build_pipeline.mp3"),
                Path.Combine(AppContext.BaseDirectory, "Assets", "Audio", "Music", "soundtrack4_going_gold.mp3"),
                Path.Combine(AppContext.BaseDirectory, "Assets", "Audio", "Music", "soundtrack5_studio_empire.mp3")
                ];

            _writeCodePressSoundPath = Path.Combine(
                AppContext.BaseDirectory, "Assets", "Audio", "SFX", "menu_click_sound2.wav");
            _writeCodeReleaseSoundPath = Path.Combine(
                AppContext.BaseDirectory, "Assets", "Audio", "SFX", "menu_click_sound3.wav");


            _menuClickSoundPath = Path.Combine(
                AppContext.BaseDirectory, "Assets", "Audio", "SFX", "menu_click_sound1.wav");

            _achievementEarnedSoundPath = Path.Combine(
                AppContext.BaseDirectory, "Assets", "Audio", "SFX", "achievement_unlock.wav");

            _activeUpgradeSoundPath = Path.Combine(
                AppContext.BaseDirectory, "Assets", "Audio", "SFX", "blip3.wav");

            _hireEmployeeSoundPath = Path.Combine(
                AppContext.BaseDirectory, "Assets", "Audio", "SFX", "click.wav");
        }

        public void StartSoundtrack()
        {
            if (_isSoundtrackOpen)
            {
                return;
            }

            _isSoundtrackOpen = true;
            PlayCurrentSoundTrack();
        }

        private void PlayCurrentSoundTrack()
        {
            _soundtrackReader = new AudioFileReader(
                _soundtrackPaths[_currentSoundtrackIndex]);

            _musicVolumeProvider = new SmoothedVolumeSampleProvider(
                _soundtrackReader, (float)(0.5 * _musicVolume));

            _soundtrackOutput = new WaveOutEvent();

            _soundtrackOutput.PlaybackStopped += SoundtrackOutput_PlaybackStopped;

            _soundtrackOutput.Init(_musicVolumeProvider);

            _soundtrackOutput.Play();
        }

        private void SoundtrackOutput_PlaybackStopped(
            object? sender,
            StoppedEventArgs e)
        {
            if (!_isSoundtrackOpen || e.Exception is not null)
            {
                return;
            }

            if (_soundtrackOutput is not null)
            {
                _soundtrackOutput.PlaybackStopped -=
                    SoundtrackOutput_PlaybackStopped;

                _soundtrackOutput.Dispose();
            }

            _soundtrackReader?.Dispose();

            _soundtrackOutput = null;
            _soundtrackReader = null;
            _musicVolumeProvider = null;

            // Move to the next soundtrack in the list, wrapping around if necessary
            _currentSoundtrackIndex =
                (_currentSoundtrackIndex + 1) % _soundtrackPaths.Length;

            PlayCurrentSoundTrack();
        }

        public void SetMusicVolume(double musicVolume)
        {
            _musicVolume = Math.Clamp(musicVolume, 0, 1);
            if (_musicVolumeProvider is not null)
            {
                _musicVolumeProvider.Volume =
                    (float)(0.5 * _musicVolume);
            }
        }

        public void SetSfxVolume(double sfxVolume)
        {
            _sfxVolume = Math.Clamp(sfxVolume, 0, 1);
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

            _writeCodePressPlayer.Volume = 0.4 * _sfxVolume;

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

            _writeCodeReleasePlayer.Volume = 0.4 * _sfxVolume;

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

            _menuClickPlayer.Volume = _sfxVolume;

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

            _achievementEarnedPlayer.Volume = _sfxVolume; // Apply the SFX volume to the achievement sound

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

            _activeUpgradePlayer.Volume = _sfxVolume;

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

            _hireEmployeePlayer.Volume = _sfxVolume;

            _hireEmployeePlayer.Position = TimeSpan.Zero;
            _hireEmployeePlayer.Play();
        }

        public void Close()
        {
            _isSoundtrackOpen = false;

            if (_soundtrackOutput is not null)
            {
                _soundtrackOutput.PlaybackStopped -=
                    SoundtrackOutput_PlaybackStopped;

                _soundtrackOutput.Stop();
                _soundtrackOutput.Dispose();
            }

            _soundtrackReader?.Dispose();

            _soundtrackOutput = null;
            _soundtrackReader = null;
            _musicVolumeProvider = null;

            _writeCodePressPlayer.Close();
            _writeCodeReleasePlayer.Close();
            _menuClickPlayer.Close();
            _achievementEarnedPlayer.Close();
            _activeUpgradePlayer.Close();
            _hireEmployeePlayer.Close();

            _isWriteCodePressSoundOpen = false;
            _isWriteCodeReleaseSoundOpen = false;
            _isMenuClickSoundOpen = false;
            _isAchievementEarnedSoundOpen = false;
            _isActiveUpgradeSoundOpen = false;
            _isHireEmployeeSoundOpen = false;
        }
    }
}
