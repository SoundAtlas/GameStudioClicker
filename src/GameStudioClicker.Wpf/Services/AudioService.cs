using System.IO;
using System.Windows.Media;

namespace GameStudioClicker.Wpf.Services
{
    public sealed class AudioService
    {
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





        public AudioService()
        {
            _writeCodePressSoundPath = Path.Combine(
                AppContext.BaseDirectory, "Assets", "Audio", "SFX", "menu_click_sound2.wav");
            _writeCodeReleaseSoundPath = Path.Combine(
                AppContext.BaseDirectory, "Assets", "Audio", "SFX", "menu_click_sound3.wav");

            _menuClickSoundPath = Path.Combine(
                AppContext.BaseDirectory, "Assets", "Audio", "SFX", "menu_click_sound1.wav");
            _menuClickPlayer.Volume = 0.2;

            _achievementEarnedSoundPath = Path.Combine(
                AppContext.BaseDirectory, "Assets", "Audio", "SFX", "achievement_unlock.wav");
            _achievementEarnedPlayer.Volume = 0.2;
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

        public void Close()
        {
            _writeCodePressPlayer.Close();
            _writeCodeReleasePlayer.Close();
            _menuClickPlayer.Close();
            _achievementEarnedPlayer.Close();


            _isWriteCodePressSoundOpen = false;
            _isWriteCodeReleaseSoundOpen = false;
            _isMenuClickSoundOpen = false;
            _isAchievementEarnedSoundOpen = false;
        }
    }
}
