#if UNITY_EDITOR
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEditor.VersionControl;

namespace VamporiumStats
{
    using static StatEffectAsset;
    public partial class StatEffect
    {
        private bool WillTrigger(NeededFields field) => EffectAsset && EffectAsset.Needed.HasFlag(field);
        private string GetAssetFieldName(NeededFields field, string value)
        {
            var ok = WillTrigger(field);
            var name = ok ? EffectAsset.GetNeededName(field) : string.Empty;
            return !string.IsNullOrEmpty(name) ? name : value;
        }

        private bool HasHolder=> WillTrigger(NeededFields.Holder);
        private bool HasGO => WillTrigger(NeededFields.GameObject);
        private bool HasInt => WillTrigger(NeededFields.Int);
        private bool HasFloat => WillTrigger(NeededFields.Float);
        private bool HasPrefab => WillTrigger(NeededFields.Prefab);
        private bool HasString => WillTrigger(NeededFields.String);
        private bool HasStatAsset => WillTrigger(NeededFields.StatAsset);
        private bool HasEvent => WillTrigger(NeededFields.Event);
        private bool HasBool => WillTrigger(NeededFields.Bool);

        private string LabelGO => GetAssetFieldName(NeededFields.GameObject, nameof(GO));
        private string LabelPrefab => GetAssetFieldName(NeededFields.Prefab, nameof(Prefab));
        private string LabelInt => GetAssetFieldName(NeededFields.Int, nameof(Int));
        private string LabelFloat => GetAssetFieldName(NeededFields.Float, nameof(Float));
        private string LabelString => GetAssetFieldName(NeededFields.String, nameof(String));
        private string LabelAsset => GetAssetFieldName(NeededFields.StatAsset, nameof(StatAsset));
        private string LabelBool=> GetAssetFieldName(NeededFields.Bool, nameof(Bool));

        private StatAsset StatAssetInEditor { get; set; }

        public void OnValidate(StatsEffects effects, StatsEffectsModule module)
        {
            StatAssetInEditor = module.Asset;
            if (GO == null) GO = effects.gameObject;
            if (Holder == null) Holder = effects.Holder;
        }

        [OnInspectorGUI]
        private void OnInspectorGUI()
        {
            if (!Active) return;

            InitStyle();
            if (HasPrefab && Prefab)
            {
                var text = string.Empty;
                if (Prefab.TryGetComponent<StatSpawn>(out var spawn)) text = spawn.GetType().Name;
                else if (Prefab.GetComponentInChildren<TMP_Text>()) text = "TextMeshPro";
                else if (Prefab.GetComponentInChildren<ParticleSystem>()) text = "ParticleSystem";
                if (!string.IsNullOrEmpty(text)) DrawLabel($"[{text}]", new(1, 1, 1, 0.5f));
            }

            if (ShowTextForDummies) GUILayout.Label(GetAsStringForDummies(), _style);
        }

        private void DrawLabel(string text, Color color)
        {
            InitStyle();
            GUI.color = color;
            GUILayout.BeginHorizontal();
            GUILayout.Label(" ", GUILayout.Width(48));
            GUILayout.Label(text, _style);
            GUILayout.EndHorizontal();
            GUI.color = Color.white;
        }

        private void InitStyle() => _style = new GUIStyle(UnityEditor.EditorStyles.label) { richText = true, wordWrap = true };
        private Color ActiveGUIColor => Active ? Color.green : Color.red;
    }
}
#endif
