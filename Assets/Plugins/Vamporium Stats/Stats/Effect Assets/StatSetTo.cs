using UnityEngine;

namespace VamporiumStats
{
    [CreateAssetMenu(menuName = "Vamporium/Stats/Effects/Stat Set To")]
    public class StatSetTo : StatEffectAsset
    {
        public override NeededFields Needed => NeededFields.Holder | NeededFields.StatAsset | NeededFields.Int;
        public override string GetNeededName(NeededFields field) => field switch
        {
            NeededFields.Int => "Set To",
            _ => string.Empty,
        };

        public override void Trigger(StatEffect effect, Stat stat)
        {
            if (effect.Holder.GetStat(effect.StatAsset, out var target))
                target.Set(effect.Int);
        }
    }

}
