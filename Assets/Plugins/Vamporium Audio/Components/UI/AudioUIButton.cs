using UnityEngine;
using UnityEngine.UI;

namespace VamporiumAudio
{
    public class AudioUIButton : AudioComponentWithMaterialAsset
    {
        [SerializeField] private Button _button;

        protected override void Awake()
        {
            base.Awake();
            _button.onClick.AddListener(PlayOnButton);
        }

        private void PlayOnButton() => Play();

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (_material == null) AudioEditorExtensions.GetAsset("default ui button", out _material);
            if (_button == null) _button = GetComponent<Button>();
        }
#endif
    }
}
