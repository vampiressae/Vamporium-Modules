using UnityEngine;

namespace VamporiumAudio
{
    public static class AudioExtensions
    {
        public static bool IsValid(this AudioMaterial material)
            => material != null && material.Clips != null && material.Clips.Length > 0;

        public static float GetRandom(this Vector2 value) => Random.Range(value.x, value.y);
    }
}
