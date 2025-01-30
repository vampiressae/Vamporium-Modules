using UnityEngine;
using Sirenix.OdinInspector;

namespace VamporiumAudio
{
    [CreateAssetMenu(menuName = "Vamporium/Audio/Material Collection")]
    public class AudioMaterialCollection : ScriptableObject
    {
        [System.Serializable]
        private class Module
        {
            public enum SettingsMode { Default, Custom }

            [HideLabel, HorizontalGroup] public string Key;
            [InlineProperty, HideLabel] public AudioMaterial Material;
            [HideLabel, HorizontalGroup(120), EnumToggleButtons, SerializeField] public SettingsMode Settings;

#if UNITY_EDITOR
            public AudioMaterialCollection Collection { get; set; }
            public void OnValidate(AudioMaterialCollection collection)
            {
                Collection = collection;
                Material ??= new(new AudioMaterialSettings(collection.Settings));
                Material.HideSettingsInEditor = Settings == SettingsMode.Default;
            }
#endif
        }

        [FoldoutGroup("Defaults", true), InlineProperty, HideLabel]
        [SerializeField] private AudioMaterialSettings _settings;
        [Space]
        [SerializeField] private Module[] _modules;

        public AudioMaterialSettings Settings => _settings;

        public AudioMaterial GetByRandom() => _modules.Length > 0 ? GetMaterial(_modules[Random.Range(0, _modules.Length)]) : null;
        public AudioMaterial GetByIndex(int index) => index >= 0 || index < _modules.Length ? GetMaterial(_modules[index]) : null;
        public AudioMaterial GetByKey(string key)
        {
            foreach (var module in _modules)
                if (module.Key == key)
                    return GetMaterial(module);
            return null;
        }

        private AudioMaterial GetMaterial(Module module)
        {
            if (module == null) return null;
            var material = new AudioMaterial(module.Material, true);
            if (module.Settings == Module.SettingsMode.Default) material.Settings = Settings;
            return material;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_modules != null)
                foreach (var module in _modules)
                    module.OnValidate(this);
        }

        [Button, PropertySpace]
        protected virtual void AutoGetAudioClips()
        {
            var dirty = false;
            if (_modules != null)
                foreach (var module in _modules)
                    if (module.Material.GetAudioClipsInEditor(module.Key, GetInstanceID()))
                        dirty = true;
            if (dirty) UnityEditor.EditorUtility.SetDirty(this);
        }

        [Button, FoldoutGroup("Defaults")]
        private void SetAllModulesToDefault()
        {
            if (_modules == null) return;
            foreach (var module in _modules)
            {
                module.Settings = Module.SettingsMode.Default;
                module.OnValidate(this);
            }
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
