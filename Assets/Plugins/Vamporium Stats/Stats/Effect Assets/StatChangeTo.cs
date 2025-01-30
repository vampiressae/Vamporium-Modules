using UnityEngine;

namespace VamporiumStats
{
    [CreateAssetMenu(menuName = "Vamporium/Stats/Effects/Stat Change To")]
    public class StatChangeTo : StatEffectAsset
    {
        private enum ChangeTo { Empty, Full }

        [SerializeField] private ChangeTo _changeTo;

        public override NeededFields Needed => NeededFields.Holder | NeededFields.StatAsset;

        public override void Trigger(StatEffect effect, Stat stat)
        {
            if (effect.Holder.GetStat(effect.StatAsset, out var target))
                switch (_changeTo)
                {
                    case ChangeTo.Empty: target.Set(0); break;
                    case ChangeTo.Full: target.Set(target.Max); break;
                }
        }
    }
}
