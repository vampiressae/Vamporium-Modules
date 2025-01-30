using UnityEngine;
using UnityEngine.UI;

namespace VamporiumAudio
{
    public class AudioUIToggle : AudioComponentWithMaterialAsset
    {
        [SerializeField] private Toggle _toggle;

        protected override void Awake()
        {
            base.Awake();
            _toggle.onValueChanged.AddListener(PlayOnToggle);
        }

        private void PlayOnToggle(bool value) => Play();

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (_material == null) AudioEditorExtensions.GetAsset("default ui toggle", out _material);
            if (_toggle == null) _toggle = GetComponent<Toggle>();
        }
#endif
    }
}
