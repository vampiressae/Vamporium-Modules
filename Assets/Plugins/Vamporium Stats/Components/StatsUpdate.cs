using Sirenix.OdinInspector;
using UnityEngine;

namespace VamporiumStats
{
    public class StatsUpdate : StatsComponentBase
    {
        [System.Serializable]
        private class Module
        {
            [HorizontalGroup, HideLabel, GUIColor("@StatsExtensions.GUIColor")] public StatAsset StatAsset;
            [HorizontalGroup(85), LabelWidth(50)] public float Intervals;
            [HorizontalGroup(45), LabelWidth(10), LabelText("+")] public int Add;
            [HideLabel, HorizontalGroup(15), GUIColor("$ActiveGUIColor")] public bool Active = true;

            private Stat _stat;
            private float _lastTime;

            public void Init(StatsHolder holder) => holder.GetStat(StatAsset, out _stat);

            public void Update(float time)
            {
                if (_stat == null || !Active) return;
                if (time - _lastTime < Intervals) return;
                _stat.Add(Add);
                _lastTime = time;
            }

#if UNITY_EDITOR
            private Color ActiveGUIColor => Active ? Color.green : Color.red;
#endif
        }

        [SerializeField] private Module[] _modules;

        private void Start()
        {
            foreach (var module in _modules)
                module.Init(Holder);
        }

        private void Update()
        {
            foreach (var module in _modules)
                module.Update(Time.time);
        }
    }
}
