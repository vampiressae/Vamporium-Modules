#if UNITY_EDITOR
using UnityEngine;
using Sirenix.OdinInspector;

namespace VamporiumStats
{
    public partial class StatEffect
    {
        private GUIStyle _style;
        private bool ShowTextForDummies { get; set; }

        [Button("i", 22), HorizontalGroup("all/asset", 15), HideIf(nameof(Trigger), EffectTrigger.None)]
        private void DoShowTextForDummies() => ShowTextForDummies = !ShowTextForDummies;

        public string GetAsStringForDummies()
        {
            if (Trigger == EffectTrigger.None) return "<color=red>DO NOTHING</color>";
            var text = $"When <b>{ObjectNameForDummies(StatAssetInEditor, "ASSET")}</b> {TriggerTextForDummies} ";
            //text += $"{ActionTextForDummies}";
            return text;
        }

        private string TriggerTextForDummies => Trigger switch
        {
            EffectTrigger.None => "<color=red>does nothing</color>...",
            EffectTrigger.Manual => $"is triggered manually with the {(string.IsNullOrEmpty(Key) ? "<color=red>EMPTY KEY</color>" : Key)} key,",
            EffectTrigger.Change => $"is changing in <b>{Direction.ToString().ToLower()}</b> direction,",
            _ => $"is {Trigger.ToString().ToLower()}, ",
        };

        //private string ActionTextForDummies => Action switch
        //{
        //    EffectAction.Nothing => "do nothig...",
        //    EffectAction.UnityEvent => "trigger the <b>Unity Event</b>.",
        //    EffectAction.SpawnAttached => $"spawn <b>{ObjectNameForDummies(Prefab, "PREFAB")}</b> as child of {ObjectNameForDummies(GO, "PARENT")} ",
        //    EffectAction.SpawnDetached => $"spawn <b>{ObjectNameForDummies(Prefab, "PREFAB")}</b> at the position of {ObjectNameForDummies(GO, "PARENT")}",
        //    EffectAction.ChangeStat => $"{ChangeForDummies}, on <b>{ObjectNameForDummies(Holder, "HOLDER")}</b>.",
        //    EffectAction.TriggerAsset => $"trigger the <b>{ObjectNameForDummies(EffectAsset, "ASSET")}</b> asset.",
        //    EffectAction.Destroy => $"destroy <b>{ObjectNameForDummies(GO, "TARGET")}</b>.",
        //    EffectAction.Log => $"log \"{String}\".",
        //    _ => "<color=red>UNEXPECTED ACTION!</color>",
        //};

        private string ObjectNameForDummies(Object obj, string error)
            => obj == null ? $"<color=red>NO {error} GIVEN</color>" : obj.name;

        //private string ChangeStatForDummies => ChangeMode switch
        //{
        //    StatChangeMode.Nothing => "<color=red>NOTHING</color>.",
        //    StatChangeMode.Number => (ChangeByValue >= 0 ? "+" : "") + ChangeByValue.ToString(),
        //    StatChangeMode.Stat => $"the value of {ObjectNameForDummies(ChangeByStat, "STAT")}",
        //    _ => "<color=red>UNEXPECTED CHANGE MODE!</color>",
        //};

        //private string ChangeForDummies => Change switch
        //{
        //    StatChangeType.Nothing => $"<color=red>change nothing</color> on <b>{ObjectNameForDummies(StatAsset, "STAT")}</b>",
        //    StatChangeType.Empty => $"change <b>{ObjectNameForDummies(StatAsset, "STAT")}</b> to 0",
        //    StatChangeType.Fill => $"change <b>{ObjectNameForDummies(StatAsset, "STAT")}</b> to its max",
        //    StatChangeType.SetTo => $"set <b>{ObjectNameForDummies(StatAsset, "STAT")}</b> to {ChangeStatForDummies}",
        //    StatChangeType.AddTo => $"change the value of <b>{ObjectNameForDummies(StatAsset, "STAT")}</b> by {ChangeStatForDummies}",
        //    _ => "<color=red>UNEXPECTED CHANGE TYPE!</color>",
        //};
    }
}
#endif
