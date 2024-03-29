using UnityEngine;

namespace Code.Scripts
{
    public enum SoundEffect
    {
        MenuAction,
        Fart,
        Hit
    }

    public enum BackgroundMusicState
    {
        None,
        InGame
    }

    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : Singleton<AudioManager>
    {
        private AudioSource _fxAudioSource;
        private AudioSource _musicAudioSource;
        private BackgroundMusicState _backgroundMusicState = BackgroundMusicState.InGame;

        // public static AudioManager Instance { get; private set; }
        public float sfxBaseVolume = 0.4f;
        public float musicBaseVolume = 1.0f;

        public const string MusicVolumeKey = "musicVolume";
        public const string SfxVolumeKey = "sfxVolume";

        [Header("Effects")] public AudioClip menuActionSound;
        public AudioClip fartSound;
        public AudioClip[] meowSounds;
        public AudioClip balloonPopSound;

        [Header("Music")] public AudioClip[] inGameMusic;

        void Start()
        {
            var sources = GetComponents<AudioSource>();
            _fxAudioSource = sources[0];
            _fxAudioSource.volume = PlayerPrefs.GetFloat(MusicVolumeKey, sfxBaseVolume);
            if (_musicAudioSource == null)
            {
                _musicAudioSource = sources[1];
            }

            _musicAudioSource.loop = true;
            _musicAudioSource.volume = PlayerPrefs.GetFloat(MusicVolumeKey, musicBaseVolume);
            PlayBackgroundMusic();
        }

        public void GoToBackgroundMusicState(BackgroundMusicState state)
        {
            if (_backgroundMusicState == state)
                return;

            _backgroundMusicState = state;
            PlayBackgroundMusic();
        }

        public void PlayBackgroundMusic()
        {
            if (_musicAudioSource == null)
                return;

            AudioClip musicClip = null;

            switch (_backgroundMusicState)
            {
                case BackgroundMusicState.InGame:
                    if (inGameMusic.Length > 0)
                        musicClip = inGameMusic[Random.Range(0, inGameMusic.Length - 1)];

                    break;
            }

            _musicAudioSource.Stop();
            _musicAudioSource.clip = musicClip;

            if (musicClip != null)
            {
                _musicAudioSource.Play();
            }
        }

        public void SfxVolumeChange(float newValue, bool userSet)
        {
            if (userSet) PlayerPrefs.SetFloat(SfxVolumeKey, newValue);
            var newVolume = sfxBaseVolume * newValue;
            _fxAudioSource.volume = newVolume;
        }

        public void MusicVolumeChange(float newValue, bool userSet)
        {
            if (userSet) PlayerPrefs.SetFloat(MusicVolumeKey, newValue);
            var newVolume = musicBaseVolume * newValue;
            _musicAudioSource.volume = newVolume;
        }

        public void ChangeMusicPitch(float factor = 0.7f)
        {
            _musicAudioSource.pitch = factor;
        }

        public float GetCurrentMusicVolume()
        {
            return _musicAudioSource.volume;
        }

        private void PlaySoundEffect(AudioClip clip, float pitch)
        {
            if (clip != null)
            {
                _fxAudioSource.pitch = pitch;
                _fxAudioSource.PlayOneShot(clip);
            }
        }

        public void PlaySoundEffect(SoundEffect effect)
        {
            float pitch = 1;
            AudioClip clip;
            switch (effect)
            {
                case SoundEffect.MenuAction:
                    clip = menuActionSound;
                    break;
                case SoundEffect.Fart:
                    pitch = Random.Range(0.5f, 2f);
                    clip = fartSound;
                    break;
                case SoundEffect.Hit:
                    PlaySoundEffect(balloonPopSound, 1);
                    pitch = Random.Range(0.5f, 1.2f);
                    clip = meowSounds[Random.Range(0, meowSounds.Length - 1)];
                    break;
                default:
                    clip = null;
                    break;
            }

            PlaySoundEffect(clip, pitch);
        }
    }
}