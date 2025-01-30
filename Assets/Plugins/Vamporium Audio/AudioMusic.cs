using UnityEngine;
using DG.Tweening;
using System;

namespace VamporiumAudio
{
    public class AudioMusic
    {
        public event Action OnPlay, OnStop, OnFinish;

        public float Volume { get; private set; } = 0.5f;
        public float CrossFade { get; private set; } = 1;
        public float FadeInOut { get; private set; } = 0;

        private AudioPlayer _playerA, _playerB;
        public AudioPlayer Current { get; private set; }
        private AudioPlayer NonCurrentPlayer => Current == _playerA ? _playerB : _playerA;

        public AudioMusic()
        {
            var material = new AudioMaterial(new AudioMaterialSettings() { Loop = true });
            _playerA = SetupPlayer("A").SetMaterial(material);
            _playerB = SetupPlayer("B").SetMaterial(material);
            Current = _playerA;
        }

        private AudioPlayer SetupPlayer(string key)
        {
            var player = AudioManager.GetNewPlayer(":: Music Player " + key);
            player.OnStop += OnPlayerStop;
            player.OnFinish += OnPlayerFinish;
            return player;
        }

        private void OnPlayerStop(AudioPlayer _) => OnStop?.Invoke();
        private void OnPlayerFinish(AudioPlayer _) => OnFinish?.Invoke();

        public AudioMusic Play(AudioClip[] clips, float crossFade)
            => Play(AudioManager.GetAClip(clips), crossFade);

        public AudioMusic Play(AudioClip clip, float crossFade)
        {
            var fade = CrossFade;
            SetCrossFade(crossFade);
            Play(clip);
            SetCrossFade(fade);
            return this;
        }

        public AudioMusic Play(AudioClip[] clips)
            => Play(AudioManager.GetAClip(clips));

        public AudioMusic Play(AudioClip clip)
        {
            if (Current.Source.clip == clip) return this;

            var playing = Current.IsPlaying;
            var player = playing ? NonCurrentPlayer : Current;

            if (playing) Current.Source.DOFade(0, clip ? CrossFade : FadeInOut).onComplete += StopNonCurrent;
            player.Play(clip).SetVolume(0).Source.DOFade(Volume, playing ? CrossFade : FadeInOut);

            Current = player;

            OnPlay?.Invoke();
            return this;
        }

        private void StopNonCurrent() => NonCurrentPlayer.Stop();

        public AudioMusic Stop()
        {
            Current.Stop();
            return this;
        }

        public AudioMusic SetVolume(float volume)
        {
            Volume = volume;
            _playerA.SetVolume(volume);
            _playerB.SetVolume(volume);
            return this;
        }

        public AudioMusic SetCrossFade(float fade)
        {
            CrossFade = fade;
            return this;
        }

        public AudioMusic SetFadeInOut(float fade)
        {
            FadeInOut = fade;
            return this;
        }
    }
}
