using UnityEngine;

namespace VamporiumStats
{
    [CreateAssetMenu(menuName = "Vamporium/Stats/Effects/Log")]
    public class Log : StatEffectAsset
    {
        public override NeededFields Needed => NeededFields.String;
        public override string GetNeededName(NeededFields field) => field switch
        {
            NeededFields.String => "Text",
            _ => string.Empty,
        };

        public override void Trigger(StatEffect effect, Stat stat) 
            => Debug.Log(effect.String);
    }
}
