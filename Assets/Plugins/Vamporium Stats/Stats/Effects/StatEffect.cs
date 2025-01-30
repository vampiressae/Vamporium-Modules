using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace VamporiumStats
{
    [System.Serializable]
    public partial class StatEffect
    {
        public enum EffectTrigger { None, Change, Full, Empty, Manual }
        public enum EffectChangeDirection { Any, Negative, Positive }

        [LabelWidth(50), HorizontalGroup("1"), GUIColor("@StatsExtensions.GUIColor")] public EffectTrigger Trigger;
        [LabelWidth(25), HorizontalGroup("1", 80), ShowIf(nameof(Trigger), EffectTrigger.Manual)] public string Key;
        [HideLabel, HorizontalGroup("1", 0.3f), ShowIf(nameof(Trigger), EffectTrigger.Change)] public EffectChangeDirection Direction;
        [HideLabel, HorizontalGroup("1", 15), GUIColor("$ActiveGUIColor")] public bool Active = true;

        [ValueDropdown("GetAllStatEffectAssets")]
        [LabelWidth(50), VerticalGroup("all"), HorizontalGroup("all/asset"), LabelText("Effect")] public StatEffectAsset EffectAsset;

        [LabelWidth(50), ShowIf("HasHolder")] public StatsHolder Holder;
        [LabelWidth(50), ShowIf("HasGO"), LabelText("$LabelGO")] public GameObject GO;
        [LabelWidth(50), ShowIf("HasPrefab"), LabelText("$LabelPrefab")] public GameObject Prefab;
        [LabelWidth(50), ShowIf("HasStatAsset"), LabelText("Stat")] public StatAsset StatAsset;
        [LabelWidth(50), ShowIf("HasString"), LabelText("$LabelString")] public string String;
        [LabelWidth(50), ShowIf("HasFloat"), LabelText("$LabelFloat")] public float Float;
        [LabelWidth(50), ShowIf("HasInt"), LabelText("$LabelInt")] public int Int;
        [LabelWidth(50), ShowIf("HasBool"), LabelText("$LabelBool")] public bool Bool;
        [VerticalGroup("all"), ShowIf("HasEvent"), LabelText("Event")] public UnityEvent UnityEvent;

#if UNITY_EDITOR
        private IEnumerable GetAllStatEffectAssets()
        {
            var list = new List<ValueDropdownItem>();
            var guids = UnityEditor.AssetDatabase.FindAssets("t:StatEffectAsset");
            foreach (var guid in guids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<StatEffectAsset>(path);
                var custom = !path.ToLower().Contains("plugins") || path.Contains("_");
                list.Add(new((custom ? "Custom/" : "") + asset.name, asset));
            }
            return list;
        }
#endif
    }
}