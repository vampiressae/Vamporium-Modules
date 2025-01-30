using UnityEngine;
using Sirenix.OdinInspector;

namespace VamporiumAudio
{
    public abstract class AudioTrigger : AudioComponentWithMaterialMultiAsset
    {
        [SerializeField] protected float _loudness = 0.1f;
        [SerializeField] protected float _minVolume = 0.1f;

        protected void Play(Vector3 lastContactPoint, float velocity, int materialIndex)
        {
            var volume = (velocity < 0 ? 1 : _loudness * velocity) * Material.Volume;

            if (volume.x < _minVolume) volume.x = _minVolume;
            if (volume.y < _minVolume) volume.y = _minVolume;

            Material.Clips = _multiMaterial.GetByIndex(materialIndex);
            Play().SetVolume(volume).transform.position = lastContactPoint;
        }

#if UNITY_EDITOR
        private GUIStyle _style;

        [OnInspectorGUI]
        private void OnInspectorGUI()
        {
            if (_multiMaterial == null) return;

            _style ??= new(UnityEditor.EditorStyles.label) { wordWrap = true, richText = true };

            GUILayout.BeginVertical("box");
            if (_multiMaterial.ModulesInEditor.Length < 2)
            {
                var msg = "There should be at least <b>2 clip modules</b> in the material ";
                msg += $"<b>{_multiMaterial.name}</b> in order to properly use it!";
                GUILayout.Label(msg, _style);
            }
            else
            {
                GUILayout.Label("Trigger Enter: " + GetModuleKey(0), _style);
                GUILayout.Label("Trigger Enter: " + GetModuleKey(1), _style);
            }
            GUILayout.EndVertical();
        }

        private string GetModuleKey(int index)
        {
            var key = _multiMaterial.ModulesInEditor[index].Key;
            if (string.IsNullOrEmpty(key)) return "<i>NO KEY</i>";
            return $"<b>{key}</i>";
        }
#endif
    }
}
