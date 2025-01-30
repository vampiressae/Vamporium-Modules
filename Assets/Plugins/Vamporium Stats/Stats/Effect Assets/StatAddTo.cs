using UnityEngine;

namespace VamporiumStats
{
    [CreateAssetMenu(menuName = "Vamporium/Stats/Effects/Stat Add To")]
    public class StatAddTo : StatEffectAsset
    {
        public override NeededFields Needed => NeededFields.Holder | NeededFields.StatAsset | NeededFields.Int;
        public override string GetNeededName(NeededFields field) => field switch
        {
            NeededFields.Int => "Add",
            _ => string.Empty,
        };

        public override void Trigger(StatEffect effect, Stat stat)
        {
            if (effect.Holder.GetStat(effect.StatAsset, out var target))
                target.Add(effect.Int);
        }
    }

}
