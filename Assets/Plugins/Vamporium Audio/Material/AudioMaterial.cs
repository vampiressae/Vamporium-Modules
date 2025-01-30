using UnityEngine;
using UnityEngine.Audio;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace VamporiumAudio
{
    [System.Serializable]
    public class AudioMaterial
    {
        public enum ClipPickingMode { FullRandom, SelectiveRandom, Sequential }

        [HideLabel, HorizontalGroup] public ClipPickingMode Mode;
        public AudioClip[] Clips;
        [InlineProperty, HideLabel, HideIf("HideSettingsInEditor")] public AudioMaterialSettings Settings = new();

        [ShowInInspector, ShowIf("ShowIndexInEditor"), ReadOnly]
        [HorizontalGroup(100), LabelWidth(16), LabelText("#")]
        private int _index;

        public AudioMixerGroup Channel => Settings.Channel;
        public Vector2 Volume => Settings.Volume;
        public Vector2 Pitch => Settings.Pitch;
        public float SpatialBlend => Settings.SpatialBlend;
        public bool Loop => Settings.Loop;

        public AudioMaterial() { }
        public AudioMaterial(AudioMaterialSettings settings) => Settings = settings;
        public AudioMaterial(AudioClip clip) => Clips = new AudioClip[] { clip };
        public AudioMaterial(AudioClip[] clips) => Clips = clips;
        public AudioMaterial(AudioMaterial material, bool deep)
        {
            Settings = deep ? new(material.Settings) : material.Settings;
            Clips = deep ? new List<AudioClip>(material.Clips).ToArray() : material.Clips;
        }

        public AudioClip GetClip() => GetClip(Mode);
        public AudioClip GetClip(ClipPickingMode mode)
        {
            var index = GetIndex(mode);
            return index > -1 ? Clips[index] : null;
        }

        public int GetIndex(ClipPickingMode mode)
        {
            if (Clips == null || Clips.Length == 0) return -1;
            if (Clips.Length == 1) return 0;
            switch (mode)
            {
                case ClipPickingMode.FullRandom:
                    return Random.Range(0, Clips.Length);

                case ClipPickingMode.SelectiveRandom:
                    int index;
                    var failsafe = 100;
                    do index = Random.Range(0, Clips.Length);
                    while (failsafe-- > 0 && _index == index);
                    _index = index;
                    return _index;

                case ClipPickingMode.Sequential:
                    _index = (_index + 1) % Clips.Length;
                    return _index;
            }
            return -1;
        }

#if UNITY_EDITOR
        public bool GetAudioClipsInEditor(string name, int instanceID)
            => AudioEditorExtensions.GetAudioClipsInEditor(name, instanceID, out Clips);

        [HideInInspector] public bool HideSettingsInEditor { get; set; }

        private bool ShowIndexInEditor => Application.isPlaying && Mode == ClipPickingMode.Sequential;
#endif
    }
}
