using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace VamporiumAudio
{
    [System.Serializable]
    public class AudioMaterialSettings
    {
        [LabelWidth(100)] public AudioMixerGroup Channel;
        [LabelWidth(100), MinMaxSlider(0, 1, true)] public Vector2 Volume = Vector2.one;
        [LabelWidth(100), MinMaxSlider(0, 2, true)] public Vector2 Pitch = Vector2.one;
        [LabelWidth(100), Range(0, 1)] public float SpatialBlend;
        [LabelWidth(100), HorizontalGroup("lastrow")] public bool Loop = false;

        public AudioMaterialSettings() { }

        public AudioMaterialSettings(AudioMixerGroup channel, Vector2 volume, Vector2 pitch, float spatialBlend, bool loop)
        {
            Channel = channel;
            Volume = volume;
            Pitch = pitch;
            SpatialBlend = spatialBlend;
            Loop = loop;
        }

        public AudioMaterialSettings(AudioMixerGroup channel, float volume, float pitch, float spatialBlend, bool loop)
        {
            Channel = channel;
            Volume = new(volume, volume);
            Pitch = new(pitch, pitch);
            SpatialBlend = spatialBlend;
            Loop = loop;
        }

        public AudioMaterialSettings(AudioMaterialSettings copyFrom)
        {
            Channel = copyFrom.Channel;
            Volume = copyFrom.Volume;
            Pitch = copyFrom.Pitch;
            SpatialBlend = copyFrom.SpatialBlend;
            Loop = copyFrom.Loop;
        }

        public AudioMaterialSettings SetChannel(AudioMixerGroup channel)
        {
            Channel = channel;
            return this;
        }

        public AudioMaterialSettings SetVolume(Vector2 volume)
        {
            Volume = volume;
            return this;
        }

        public AudioMaterialSettings SetPitch(Vector2 pitch)
        {
            Pitch = pitch;
            return this;
        }

        public AudioMaterialSettings SetSpatialBlend(bool threeD)
        {
            SpatialBlend = threeD ? 1 : 0;
            return this;
        }

        public AudioMaterialSettings SetSpatialBlend(float blend)
        {
            SpatialBlend = blend;
            return this;
        }

        public AudioMaterialSettings SetLoop(bool loop)
        {
            Loop = loop;
            return this;
        }

#if UNITY_EDITOR
        [ValueDropdown(nameof(PresetsDropdownInEditor)), OnValueChanged(nameof(PresetChangedInEditor))]
        [HorizontalGroup("lastrow", 97), LabelText("Presets"), LabelWidth(45), ShowInInspector] private string _presetInEditor = "";
        private IEnumerable PresetsDropdownInEditor() => this.MaterialSettingsPresetNames();
        private void PresetChangedInEditor() => _presetInEditor = this.MaterialSettingsPresetChangedInEditor(_presetInEditor);
#endif
    }
}
