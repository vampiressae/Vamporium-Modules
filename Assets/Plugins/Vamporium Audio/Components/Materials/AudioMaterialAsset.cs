using UnityEngine;
using Sirenix.OdinInspector;

namespace VamporiumAudio
{
    [CreateAssetMenu(menuName = "Vamporium/Audio/Material Asset")]
    public class AudioMaterialAsset : ScriptableObject
    {
        [InlineProperty, HideLabel] public AudioMaterial Material = new();

#if UNITY_EDITOR
        private void OnValidate()
        {
            var key = name.Contains("UI") ? "UI" : "Effects";
            if (Material.Channel == null) AudioEditorExtensions.GetChannel(key, out Material.Settings.Channel);
        }

        [Button, PropertySpace]
        protected virtual void AutoGetAudioClips()
        {
            if (Material.GetAudioClipsInEditor(name, GetInstanceID()))
                UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
