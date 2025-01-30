using UnityEngine;

namespace VamporiumStats
{
    [CreateAssetMenu(menuName = "Vamporium/Stats/Effects/Event")]
    public class Event : StatEffectAsset
    {
        public override NeededFields Needed => NeededFields.Event;

        public override void Trigger(StatEffect effect, Stat stat) 
            => effect.UnityEvent.Invoke();
    }
}
