using System.Collections.Generic;
using UnityEngine;

namespace VamporiumStats
{
    public class StatsDisplay : StatsComponentBase
    {
        [SerializeField] private List<StatsDisplayModule> _modules = new();

        protected override void OnChange(Stat stat)
        {
            base.OnChange(stat);
            foreach (var module in _modules)
                if (module.Asset == stat.Asset)
                    module.Refresh(stat);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _modules.ForEach(module => module.Kill());
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            _modules.ForEach(module=> module.OnValidate(this));

            if (!Application.isPlaying) return;
            Holder.Stats.ForEach(stat => OnChange(stat));
        }
#endif
    }
}