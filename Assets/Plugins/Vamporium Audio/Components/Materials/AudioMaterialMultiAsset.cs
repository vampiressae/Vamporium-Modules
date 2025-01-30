using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VamporiumAudio
{
    [CreateAssetMenu(menuName = "Vamporium/Audio/Material Multi")]
    public class AudioMaterialMultiAsset : ScriptableObject
    {
        [System.Serializable]
        public class Module
        {
            [HideLabel] public string Key;
            public AudioClip[] Clips;
        }

        [InlineProperty, HideLabel] public AudioMaterialSettings Settings = new();
        [Space]
        [SerializeField] protected Module[] _modules;

        public AudioMaterial AudioMaterial => new(Settings);
        public AudioClip[] GetByRandom() => _modules.Length == 0 ? null : _modules[Random.Range(0, _modules.Length)].Clips;
        public AudioClip[] GetByIndex(int index) => index < 0 || index >= _modules.Length ? null : _modules[index].Clips;

        protected virtual string[] ForceModuleKeys { get; } = null;
        protected virtual bool ForceExclusively { get; } = false;

        public AudioClip[] GetByKey(string key)
        {
            foreach (var module in _modules)
                if (module.Key == key)
                    return module.Clips;
            return null;
        }

#if UNITY_EDITOR
        public Module[] ModulesInEditor { get => _modules; set => _modules = value; }

        private void Reset() => ForceModules();
        protected virtual void OnValidate() => ForceModules();

        private void ForceModules()
        { 
            if (ForceModuleKeys != null)
            {
                var exclusive = ForceExclusively;
                var list = new List<Module>();
                var needs = new List<string>(ForceModuleKeys);

                if (_modules != null)
                    foreach (var module in _modules)
                        if (!exclusive || needs.Contains(module.Key))
                        {
                            list.Add(module);
                            needs.Remove(module.Key);
                        }
                foreach (var need in needs)
                    list.Add(new() { Key = need });

                _modules = list.ToArray();
            }
        }

        [HorizontalGroup("search", 110), SerializeField, PropertyOrder(10), ToggleLeft, LabelWidth(100)] private bool _customSearch;
        [HorizontalGroup("search"), SerializeField, PropertyOrder(10), HideLabel, ShowIf(nameof(_customSearch))] private string _customString;
        [Button, PropertySpace]
        protected virtual void AutoGetAudioClips()
        {
            if (_modules == null) return;
            foreach (var module in _modules)
                if (AudioEditorExtensions.GetAudioClipsInEditor($"{name} {module.Key}", GetInstanceID(), out module.Clips))
                    UnityEditor.EditorUtility.SetDirty(this);

            if (!_customSearch) return;
            foreach (var one in _customString.Split(new string[] { "," }, System.StringSplitOptions.None))
                foreach (var module in _modules)
                    if (module.Clips == null || module.Clips.Length == 0 || one.Trim().StartsWith("!"))
                        if (AudioEditorExtensions.GetAudioClipsInEditor($"{one.Trim(' ', '!')} {module.Key}", GetInstanceID(), out var customClips))
                        {
                            if (customClips.Length != 0) module.Clips = customClips;
                            UnityEditor.EditorUtility.SetDirty(this);
                        }
        }
#endif
    }
}
