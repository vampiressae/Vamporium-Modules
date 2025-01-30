using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VamporiumStats
{
    using static StatEffect;

    [System.Serializable]
    public class StatsEffectsModule
    {
        [HideLabel] public StatAsset Asset;
        [SerializeField] private StatEffect[] _effects;

        private readonly StatEffectTriggerer _triggerer = new();

        public void Refresh(StatsEffects _, Stat stat)
        {
            foreach (StatEffect effect in _effects)
                _triggerer.TryTrigger(effect, stat);
        }

        public void Trigger(string key)
        {
            key = key.ToLower();
            foreach (StatEffect effect in _effects.Where(fx => fx.Trigger == EffectTrigger.Manual))
                if (effect.Key.ToLower() == key)
                    _triggerer.Trigger(effect);
        }

#if UNITY_EDITOR
        public void OnValidate(StatsEffects stats)
        {
            foreach (var effect in _effects)
                effect.OnValidate(stats, this);
        }
#endif
    }
}