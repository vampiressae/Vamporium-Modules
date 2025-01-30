using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

namespace VamporiumAudio
{
    public class AudioSettingsVolume : MonoBehaviour
    {
        [SerializeField] private AudioMixerGroup _channel;
        [SerializeField] private Slider _slider;

        private void Awake() => _slider.onValueChanged.AddListener(Refresh);
        private void OnEnable() => _slider.SetValueWithoutNotify(AudioManager.Settings.GetVolume(_channel.name));
        private void Refresh(float volume)
        {
            AudioManager.Settings.SetVolume(_channel.name, volume);
            _channel.audioMixer.SetFloat(_channel.name, AudioManager.RealVolume(volume));
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_slider == null) _slider = GetComponent<Slider>();
        }
#endif
    }
}
