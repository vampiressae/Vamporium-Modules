using System.Collections.Generic;
using UnityEngine;

namespace VamporiumStats
{
    public class StatsEffects : StatsComponentBase
    {
        [SerializeField] private List<StatsEffectsModule> _modules = new();

        protected override void OnChange(Stat stat)
        {
            base.OnChange(stat);
            foreach (var module in _modules)
                if (module.Asset == stat.Asset)
                    module.Refresh(this, stat);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            _modules.ForEach(module => module.OnValidate(this));
        }
#endif
    }
}