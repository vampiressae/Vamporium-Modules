using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace VamporiumAudio
{
    public class AudioAgent : MonoBehaviour
    {
        [InfoBox("If let null, the Channel of the AudioMaterial will not be changed.", VisibleIf = "InfoBox")]
        [SerializeField] private AudioMixerGroup _channel;

        public void Play(AudioMaterialAsset material) => AudioManager.Play(material.Material);
        public void PlayWithChannel(AudioClip clip) => AudioManager.Play(clip).SetChannel(_channel);
        public void PlayWithChannel(AudioMaterialAsset material) => AudioManager.Play(material.Material).SetChannel(_channel);

#if UNITY_EDITOR
        private void Reset()
        {
            if (_channel == null) AudioEditorExtensions.GetChannel(GetComponentInParent<Canvas>() ? "UI" : "Effects", out _channel);
        }

        private bool InfoBox => _channel == null;
#endif
    }
}
