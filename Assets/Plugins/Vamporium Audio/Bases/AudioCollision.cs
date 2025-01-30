using UnityEngine;

namespace VamporiumAudio
{
    public abstract class AudioCollision : AudioComponentWithMaterialAsset
    {
        [SerializeField] protected float _loudness = 0.1f;
        [SerializeField, Range(0, 1)] protected float _minVolume = 0.1f;

        protected void Play(Vector3 lastContactPoint, float magnitude)
        {
            var volume = _loudness * magnitude * _material.Material.Volume;

            if (volume.x < _minVolume) volume.x = _minVolume;
            if (volume.y < _minVolume) volume.y = _minVolume;

            Play().SetVolume(volume).transform.position = lastContactPoint;
        }
    }
}