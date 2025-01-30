using System.Collections.Generic;
using UnityEngine;

namespace VamporiumStats
{
    [CreateAssetMenu(menuName = "Vamporium/Stats/Effects")]
    public class StatEffectAssets : StatEffectAsset
    {
        [SerializeField] private StatEffectAsset[] _effects;

        public override NeededFields Needed => GetNeededFields();
        public override string GetNeededName(NeededFields field)
        {
            var names = new List<string>();
            foreach (var fx in _effects)
                if (fx.Needed.HasFlag(field))
                {
                    var name = fx.GetNeededName(field);
                    if (names.Contains(name)) continue;
                    names.Add(name);
                }
            return string.Join('/', names);
        }

        private NeededFields GetNeededFields()
        {
            var needed = NeededFields.None;
            foreach (var fx in _effects)
                if (!needed.HasFlag(fx.Needed))
                    needed |= fx.Needed;
            return needed;
        }

        public override void Trigger(StatEffect effect, Stat stat)
        {
            foreach(var fx in _effects)
                fx.Trigger(effect, stat);
        }
    }
}
