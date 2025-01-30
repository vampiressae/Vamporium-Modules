using Sirenix.OdinInspector;
using UnityEngine;

namespace VamporiumAudio
{
    public class AudioEmitter : AudioComponent
    {
        private enum RadiusType { None, Circle, Sphere}

        protected override AudioMaterial Material => _material.Material;

        [SerializeField] private AudioMaterialAsset _material;
        [SerializeField] private Vector2 _interval;
        [SerializeField] private RadiusType _radiusType;
        [SerializeField, HideIf(nameof(_radiusType), RadiusType.None)] private float _radius;

        private void OnEnable() => Schedule();
        private void OnDisable() => Unschedule();

        private void Schedule()
        {
            Unschedule();
            Invoke(nameof(Emit), _interval.GetRandom());
        }

        private void Unschedule() => CancelInvoke(nameof(Emit));

        public void Emit()
        {
            Play();
            Schedule();

            switch(_radiusType)
            {
                case RadiusType.None:
                    _player.transform.localPosition = default;
                    break;

                case RadiusType.Circle:
                    Vector3 radius2D = Random.insideUnitCircle * _radius;
                    _player.transform.localPosition = radius2D;
                    break;

                case RadiusType.Sphere:
                    Vector3 radius3D = Random.insideUnitSphere * _radius;
                    _player.transform.localPosition = radius3D;
                    break;
            }
        }
    }
}
