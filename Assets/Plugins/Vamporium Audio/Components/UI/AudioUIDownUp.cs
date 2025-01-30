using UnityEngine;
using UnityEngine.EventSystems;

namespace VamporiumAudio
{
    public class AudioUIDownUp : AudioComponent, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private AudioMaterialMultiAsset _material;

        protected override AudioMaterial Material => new(new AudioMaterialSettings(_material.Settings));

        private float _time;

        public void OnPointerDown(PointerEventData eventData)
        {
            _player.Play(_material.GetByIndex(0));
            _time = Time.time;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (Time.time - _time < 0.2f) return;
            _player.Play(_material.GetByIndex(1));
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (_material == null) AudioEditorExtensions.GetAsset("default ui down-up", out _material);
        }
#endif
    }
}
