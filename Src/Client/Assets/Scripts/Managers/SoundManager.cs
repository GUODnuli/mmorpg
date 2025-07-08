using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Managers
{
    public class SoundManager : MonoSingleton<SoundManager>
    {
        public AudioMixer audioMixer;
        public AudioSource musicAudioSource;
        public AudioSource soundAudioSource;

        const string MusicPath = "Music/";
        const string SoundPath = "Sound/";

        private void Start()
        {
            this.musicVolume = Config.MusicVolume;
            this.soundVolume = Config.SoundVolume;
            this.musicOn = Config.MusicOn;
            this.soundOn = Config.SoundOn;
        }

        private bool _musicOn;
        public bool musicOn
        {
            get { return _musicOn; }
            set
            {
                _musicOn = value;
                this.MusicMute(!_musicOn);
            }
        }

        private bool _soundOn;
        public bool soundOn
        {
            get { return _soundOn; }
            set
            {
                _soundOn = value;
                this.SoundMute(!_soundOn);
            }
        }

        private int _musicVolume;
        public int musicVolume
        {
            get { return _musicVolume; }
            set
            {
                _musicVolume = value;
                this.SetVolume("MusicVolume", _musicVolume);
            }
        }

        private int _soundVolume;
        public int soundVolume
        {
            get { return _soundVolume; }
            set
            {
                _soundVolume = value;
                this.SetVolume("SoundVolume", _soundVolume);
            }
        }

        public void MusicMute(bool mute)
        {
            this.SetVolume("MusicVolume", mute ? 0 : musicVolume);
        }

        public void SoundMute(bool mute)
        {
            this.SetVolume("SoundVolume", mute ? 0 : soundVolume);
        }

        private void SetVolume(string name, int value)
        {
            float volume = value * 0.5f - 50f;
            this.audioMixer.SetFloat(name, volume);
        }

        public void PlayMusic(string name)
        {
            AudioClip clip = Resloader.Load<AudioClip>(MusicPath + name);
            if (clip == null)
            {
                Debug.LogWarningFormat("PlayMusic : {0} not existed.", name);
                return;
            }
            if (musicAudioSource.isPlaying)
            {
                musicAudioSource.Stop();
            }

            musicAudioSource.clip = clip;
            musicAudioSource.Play();
        }

        public void PlaySound(string name)
        {
            AudioClip clip = Resloader.Load<AudioClip>(SoundPath + name);
            if (clip == null)
            {
                Debug.LogWarningFormat("PlaySound : {0} not existed.", name);
                return;
            }
            soundAudioSource.PlayOneShot(clip);
        }
    }
}