using System;
using gishadev.tools.Core;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace gishadev.tools.Audio
{
    public class MusicPlayer : AudioPlayer<MusicData>
    {
        private readonly AudioManager _audioManager;
        private MusicData _currentMusic;

        private UnityEvent<MusicData> MusicInitiated = new();

        public MusicPlayer(AudioManager audioManager)
        {
            _audioManager = audioManager;
            MusicInitiated.AddListener(HandleAutoSequencing);
        }

        public override void Play(MusicData data)
        {
            // Randomize clip.
            if (data.AudioClips.Length > 1)
                data.AudioSource.clip = data.AudioClips[Random.Range(0, data.AudioClips.Length)];

            InitPlay(data);
        }

        // If we have auto-sequencing - start next audio clip when delay is over. 
        private void HandleAutoSequencing(MusicData data)
        {
            if (_audioManager.MasterData.MusicAutoSequencing)
                _audioManager.DelayFunc(() =>
                {
                    var oldIndex = Array.FindIndex(data.AudioClips, x => x == data.AudioSource.clip);
                    var nextValue = data.AudioClips.GetNextValue(oldIndex);
                    data.AudioSource.clip = nextValue;
                    InitPlay(data);
                }, data.AudioSource.clip.length / data.AudioSource.pitch);
        }

        public override void Pause(MusicData data)
        {
            data.AudioSource.Pause();
        }

        public override void Stop(MusicData data)
        {
            data.AudioSource.Stop();
            _audioManager.StopCoroutine(nameof(_audioManager.DelayFunc));
        }

        private void InitPlay(MusicData newMusic)
        {
            if (_currentMusic != null)
            {
                if (_currentMusic.IsFade)
                    _audioManager.FadeOut(_currentMusic);
                else
                    _currentMusic.Stop();
            }

            if (newMusic.IsFade)
                _audioManager.FadeIn(newMusic);
            newMusic.AudioSource.Play();
            _currentMusic = newMusic;
            
            MusicInitiated?.Invoke(_currentMusic);
        }
    }
}