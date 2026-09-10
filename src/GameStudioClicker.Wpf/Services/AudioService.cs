using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.IO;
using System.Windows.Media;

namespace GameStudioClicker.Wpf.Services
{
    public sealed class AudioService
    {
        private MixingSampleProvider? _mixer; // combine music and sound effects
        private VolumeSampleProvider? _musicVolumeProvider; // controls the volume of the music
        private SmoothedVolumeSampleProvider? _masterVolumeProvider; // smooths out volume changes to avoid static noise

        // MediaPlayer instances for different audio tracks and sound effects
        private AudioFileReader? _soundtrackReader;
        private WaveOutEvent? _soundtrackOutput;
        private readonly string[] _soundtrackPaths;
        private int _currentSoundtrackIndex;
        private bool _isSoundtrackOpen;

        private double _masterVolume = 1.0;

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

            WaveFormat mixerFormat =
                WaveFormat.CreateIeeeFloatWaveFormat(48000, 2); // Stereo, 48kHz

            _mixer = new MixingSampleProvider(mixerFormat)
            {
                ReadFully = true
            };

            _masterVolumeProvider = new SmoothedVolumeSampleProvider(
                _mixer,
                (float)_masterVolume);

            _soundtrackReader =
                new AudioFileReader(
                    _soundtrackPaths[_currentSoundtrackIndex]);

            _musicVolumeProvider =
                new VolumeSampleProvider(_soundtrackReader)
                {
                    Volume = 0.2f
                }; // controls the volume of the music

            _mixer.AddMixerInput(_musicVolumeProvider);

            _soundtrackOutput = new WaveOutEvent();
            _soundtrackOutput.Init(_masterVolumeProvider);

            _isSoundtrackOpen = true;
            _soundtrackOutput.Play();


        }

        public void SetMasterVolume(double masterVolume)
        {
            _masterVolume = Math.Clamp(masterVolume, 0, 1);
            ApplyMasterVolume(_masterVolume);
        }

        private void ApplyMasterVolume(double masterVolume)
        {
            if (_masterVolumeProvider is not null)
            {
                _masterVolumeProvider.Volume =
                    (float)masterVolume;
            }
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
            _soundtrackOutput?.Stop();
            _soundtrackOutput?.Dispose();
            _soundtrackReader?.Dispose();

            _soundtrackOutput = null;
            _soundtrackReader = null;
            _musicVolumeProvider = null;
            _masterVolumeProvider = null;
            _mixer = null;

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
