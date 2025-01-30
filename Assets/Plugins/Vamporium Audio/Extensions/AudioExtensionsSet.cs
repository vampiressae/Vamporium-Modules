using UnityEngine;
using UnityEngine.Audio;

namespace VamporiumAudio
{
    public static class AudioExtensionsSet
    {
        public static AudioPlayer SetDefaults(this AudioPlayer player)
        {
            if (player)
            {
                player.Source.dopplerLevel = 0;
                player.Source.rolloffMode = AudioRolloffMode.Linear;
                player.Source.maxDistance = AudioManager.MaxDistance;
            }
            return player;
        }

        public static AudioPlayer SetMaterial(this AudioPlayer player, AudioMaterial material)
        {
            if (player) player.Material = material;
            return player;
        }

        public static AudioPlayer SetVolume(this AudioPlayer player, float value)
            => player.SetVolume(new Vector2(value, value));

        public static AudioPlayer SetVolume(this AudioPlayer player, Vector2 value)
        {
            if (player) player.Source.volume = value.GetRandom();
            return player;
        }

        public static AudioPlayer SetPitch(this AudioPlayer player, float value)
            => player.SetPitch(new Vector2(value, value));

        public static AudioPlayer SetPitch(this AudioPlayer player, Vector2 value)
        {
            if (player) player.Source.pitch = value.GetRandom();
            return player;
        }

        public static AudioPlayer SetSpatialBlend(this AudioPlayer player, float value)
        {
            if (player) player.Source.spatialBlend = value;
            return player;
        }

        public static AudioPlayer SetLoop(this AudioPlayer player, bool value)
        {
            if (player)
            {
                player.Source.loop = value;
                player.ScheduleFinish();
            }
            return player;
        }

        public static AudioPlayer SetChannel(this AudioPlayer player, AudioMixerGroup value)
        {
            if (player) player.Source.outputAudioMixerGroup = value;
            AudioManager.Settings.SetVolume(value);
            return player;
        }

        public static AudioPlayer SetTime(this AudioPlayer player, float time)
        {
            if (player)
            {
                player.Source.time = time;
                player.ScheduleFinish();
            }
            return player;
        }

        public static AudioPlayer SetParent(this AudioPlayer player, Transform parent)
        {
            if (player)
            {
                player.transform.SetParent(parent ? parent : AudioManager.Parent);
                player.transform.localPosition = default;
            }
            return player;
        }

        public static AudioPlayer SetPosition(this AudioPlayer player, Vector3 position)
        {
            if (player)
            {
                player.transform.SetParent(AudioManager.Parent);
                player.transform.position = position;
            }
            return player;
        }

    }
}
