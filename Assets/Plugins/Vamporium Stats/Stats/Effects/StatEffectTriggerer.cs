using UnityEngine;

namespace VamporiumStats
{
    using static StatEffect;

    public class StatEffectTriggerer
    {
        public void TryTrigger(StatEffect effect, Stat stat)
        {
            if (!effect.Active) return;
            if (effect.Trigger == EffectTrigger.None) return;
            if (effect.Trigger == EffectTrigger.Empty && !stat.IsEmpty) return;
            if (effect.Trigger == EffectTrigger.Full && !stat.IsFull) return;
            if (!AppropriateDeltaDirection(stat, effect)) return;
            Trigger(effect, stat);
        }

        public void Trigger(StatEffect effect, Stat stat = null) 
            => effect.EffectAsset.Trigger(effect, stat);

        private bool AppropriateDeltaDirection(Stat stat, StatEffect effect)
        {
            if (effect.Trigger != EffectTrigger.Change) return true;
            if (stat.LastDelta == 0) return false;
            switch (effect.Direction)
            {
                case EffectChangeDirection.Positive: return stat.LastDelta > 0;
                case EffectChangeDirection.Negative: return stat.LastDelta < 0;
                default: return true;
            }
        }

        //private int GetValueByChangeMode(StatEffect effect) => effect.ChangeMode switch
        //{
        //    StatChangeMode.Number => effect.ChangeByValue,
        //    StatChangeMode.Stat => effect.Holder.GetStat(effect.ChangeByStat, out var stat) ? stat.Current : 0,
        //    _ => 0,
        //};
    }
}
