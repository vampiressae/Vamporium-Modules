using UnityEngine;

namespace VamporiumAudio
{
    public abstract class AudioComponentWithMaterialAsset : AudioComponent
    {
        [SerializeField] protected AudioMaterialAsset _material;
        protected override AudioMaterial Material => _material.Material;
    }

    public abstract class AudioComponentWithMaterialMultiAsset : AudioComponent
    {
        [SerializeField] protected AudioMaterialMultiAsset _multiMaterial;
        protected override AudioMaterial Material => _material;

        private AudioMaterial _material;

        protected override void Awake()
        {
            _material = new(new AudioMaterialSettings(_multiMaterial.Settings));
            base.Awake();
        }
    }
}
