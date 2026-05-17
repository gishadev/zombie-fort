using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace gishadev.tools.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public const string AUDIO_MASTER_ASSET = "AudioMasterSO";

        public static AudioManager I
        {
            get
            {
                if (_current)
                    return _current;

                _current = new GameObject("[AudioManager]").AddComponent<AudioManager>();
                DontDestroyOnLoad(_current.gameObject);

                return _current;
            }
        }

        private static AudioManager _current;

        public delegate void DelayedDelegate();

        public event Action<AudioData> AudioStarted;
        public AudioMasterSO MasterData => _masterData;


        private AudioMasterSO _masterData;
        private bool _isInitialized;

        private float _musicVolumePercentage = 1f;
        private float _sfxVolumePercentage = 1f;


        private void Awake()
        {
            TryInit();
        }

        public void SetSFXVolume(float volumePercent)
        {
            var sfxData = GetAudioCollection<SFXData>();

            foreach (var sfx in sfxData)
                sfx.AudioSource.volume = volumePercent * sfx.InitialVolume;

            _sfxVolumePercentage = volumePercent;
        }

        public void SetMusicVolume(float volumePercent)
        {
            var musicData = GetAudioCollection<MusicData>();

            foreach (var music in musicData)
                music.AudioSource.volume = volumePercent * music.InitialVolume;

            _musicVolumePercentage = volumePercent;
        }

        public void PlayAudio<T>(int index) where T : AudioData, new()
        {
            TryInit();

            var audioCollection = GetAudioCollection<T>();

            if (index < 0 || index > audioCollection.Length - 1)
            {
                Debug.LogError("There is no sfx with index " + index);
                return;
            }

            var data = audioCollection.ToArray()[index];

            data.Play();

            AudioStarted?.Invoke(data);

            Debug.Log($"I'm playing: {data.Name} of type {typeof(T)}");
        }

        public void PlayAudio(MusicAudioEnum enumEntry) => PlayAudio<MusicData>((int) enumEntry);
        public void PlayAudio(SFXAudioEnum enumEntry) => PlayAudio<SFXData>((int) enumEntry);

        #region Initialization

        private void Init()
        {
            _masterData = Resources.Load<AudioMasterSO>(AUDIO_MASTER_ASSET);
            InitCollection(MasterData.SFXCollection);
            InitCollection(MasterData.MusicCollection);

            _isInitialized = true;
        }

        private void TryInit()
        {
            if (!_isInitialized)
                Init();
        }

        private void InitCollection<T>(IEnumerable<T> collection) where T : AudioData, new()
        {
            // Init audio player.
            BaseAudioPlayer audioPlayer =
                typeof(T) == typeof(MusicData) ? new MusicPlayer(this) : new SFXPlayer(this);

            foreach (var audio in collection)
            {
                var child = new GameObject(audio.Name);
                child.transform.SetParent(transform);

                var audioSource = child.AddComponent<AudioSource>();
                audio.InitAudioSource(audioSource);
                audio.InitAudioPlayer(audioPlayer);
            }
        }

        #endregion


        public void FadeIn(AudioData audioData)
        {
            StartCoroutine(FadeInRoutine(audioData));
        }

        public void FadeOut(AudioData audioData)
        {
            StartCoroutine(FadeOutRoutine(audioData));
        }

        public void DelayFunc(DelayedDelegate delayedDelegate, float delay)
        {
            StopCoroutine(nameof(DelayFuncRoutine));
            StartCoroutine(DelayFuncRoutine(delayedDelegate, delay));
        }

        private IEnumerator FadeInRoutine(AudioData audioData)
        {
            audioData.AudioSource.volume = 0f;
            var volume = audioData.AudioSource.volume;

            while (audioData.AudioSource.volume * _musicVolumePercentage < audioData.InitialVolume * _musicVolumePercentage)
            {
                volume += Time.deltaTime / MasterData.FadeTransitionTime * _musicVolumePercentage;
                audioData.AudioSource.volume = volume;
                yield return null;
            }
        }

        private IEnumerator FadeOutRoutine(AudioData audioData)
        {
            var volume = audioData.AudioSource.volume;

            while (audioData.AudioSource.volume * _musicVolumePercentage > 0)
            {
                volume -= Time.deltaTime / MasterData.FadeTransitionTime * _musicVolumePercentage;
                audioData.AudioSource.volume = volume;
                yield return null;
            }

            if (audioData.AudioSource.volume == 0)
            {
                audioData.AudioSource.Stop();
                audioData.AudioSource.volume = audioData.InitialVolume * _musicVolumePercentage;
            }
        }

        private IEnumerator DelayFuncRoutine(DelayedDelegate delayedDelegate, float delay)
        {
            yield return new WaitForSeconds(delay);
            delayedDelegate();
        }


        private T[] GetAudioCollection<T>() where T : AudioData, new()
        {
            return typeof(T) == typeof(MusicData)
                ? MasterData.MusicCollection.Cast<T>().ToArray()
                : MasterData.SFXCollection.Cast<T>().ToArray();
        }
    }
}