using UnityEngine;
using UnityEngine.Audio;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace VamporiumAudio
{
    public class AmbientPlayer : MonoBehaviour, IAudioPlayerWithEvents
    {
        public event System.Action<AudioPlayer> OnPlay, OnStop, OnFinish;

        [SerializeField] private bool _playOnEnable = false;
        [SerializeField] private AudioMixerGroup _channel;
        [SerializeField] private AudioClip _clip;
        [SerializeField, Range(0, 1)] private float _volume = 1;
        [SerializeField] private float _fade = 2;
        [SerializeField, Range(0, 1)] private float _spacialBlend = 1;

        private AudioPlayer _player;

        public AudioPlayer Player => _player;

        private void Awake()
        {
            var material = new AudioMaterial();
            material.Settings.Channel = _channel;
            material.Settings.SpatialBlend = _spacialBlend;
            material.Settings.Loop = true;

            _player = AudioManager.GetNewPlayer(":: Ambient Player").SetMaterial(material).SetAutoDestroy(false).SetParent(transform);
        }

        private void OnDestroy()
        {
            if (_player == null) return;
            _player.transform.parent = null;
            _player.Source.Play();
            _player.FadeOut(_fade, true);
        }

        private void OnEnable() { if (_playOnEnable) Play(); }

        public void Play(AudioClip clip)
        {
            _player.Play(clip).SetVolume(0).Source.DOFade(_volume, _fade);
            OnPlay?.Invoke(_player);
        }

        [Button, HorizontalGroup, HideInEditorMode] public void Play() => Play(_clip);
        [Button, HorizontalGroup, HideInEditorMode] public void Stop() => _player.Source.DOFade(0, _fade).onComplete += FullStop;

        private void FullStop()
        {
            _player.Stop();
            OnStop?.Invoke(_player);
            OnFinish?.Invoke(_player);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_channel == null) AudioEditorExtensions.GetChannel("Ambient", out _channel);
        }
#endif
    }
}
