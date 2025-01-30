using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace VamporiumStats
{
    [System.Serializable]
    public class StatsDisplayModule
    {
        [HideLabel, HorizontalGroup] public StatAsset Asset;
        [SerializeField, HideLabel, HorizontalGroup(.3f)] private Ease _animationEase;
        [SerializeField] private StatDisplay[] _displays;

        private StatDisplayRefresher _refresher;

        public void Refresh(Stat stat)
        {
            _refresher ??= new();

            foreach (StatDisplay display in _displays)
                _refresher.Refresh(stat, display, _animationEase);
        }

        public void Kill()
        {
            foreach (StatDisplay display in _displays)
                display.Kill();
        }

#if UNITY_EDITOR
        public void OnValidate(StatsDisplay stats)
        {
            if (stats.Holder.GetStat(Asset, out var stat))
                foreach (var display in _displays)
                    display.OnValidate(stat);
        }
#endif
    }
}