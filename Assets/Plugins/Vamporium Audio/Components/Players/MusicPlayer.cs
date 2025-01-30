using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace VamporiumAudio
{
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] private bool _playOnEnable = true;
        [SerializeField] private AudioMixerGroup _channel;
        [SerializeField] private AudioClip[] _clips;

        private void OnEnable() { if (_playOnEnable) Play(); }

        [Button, HorizontalGroup, HideInEditorMode] public void Play() => Play(_clips);
        [Button, HorizontalGroup, HideInEditorMode] public void Stop() => AudioManager.Music.Stop();

        public void Play(AudioClip[] clips) => AudioManager.Music.Play(clips).Current.SetChannel(_channel);

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_channel == null) AudioEditorExtensions.GetChannel("Music", out _channel);
        }

        [HorizontalGroup("edit"), ShowInInspector, HideInEditorMode, ReadOnly, LabelWidth(50), PropertyOrder(9)]
        string _current => Application.isPlaying ? $"{AudioManager.Music.Current.Source.clip.name} ({AudioManager.Music.Current.name.Split(" ").Last()})" : "";

        [ShowInInspector, HideInEditorMode, OnValueChanged(nameof(SetTo)), PropertyOrder(10)]
        [HorizontalGroup("edit", 120), LabelWidth(30), LabelText(" Set:")] private AudioClip _setToValue;

        private void SetTo() => Play(new AudioClip[] { _setToValue });
#endif
    }
}
