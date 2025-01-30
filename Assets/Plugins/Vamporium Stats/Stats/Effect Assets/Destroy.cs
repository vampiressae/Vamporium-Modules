using UnityEngine;

namespace VamporiumStats
{
    [CreateAssetMenu(menuName = "Vamporium/Stats/Effects/Destroy")]
    public class Destroy : StatEffectAsset
    {
        public override NeededFields Needed => NeededFields.GameObject | NeededFields.Float;
        public override string GetNeededName(NeededFields field) => field switch
        {
            NeededFields.GameObject => "Target",
            NeededFields.Float => "Delay",
            _ => string.Empty,
        };

        public override void Trigger(StatEffect effect, Stat stat) 
            => Destroy(effect.GO, effect.Float);
    }
}
