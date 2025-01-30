using UnityEngine;

namespace VamporiumStats
{
    public abstract class StatsComponentBase : MonoBehaviour
    {
        public StatsHolder Holder;

        protected virtual void Awake() => Holder.Stats.ForEach(Subscribe);
        protected virtual void OnDestroy() => Holder.Stats.ForEach(Unsubscribe);

        protected void Subscribe(Stat stat)
        {
            if (stat == null) return;

            stat.OnInit += OnInit;
            stat.OnUninit += OnUninit;
            stat.OnChange += OnChange;
            stat.OnFull += OnFull;
            stat.OnEmpty += OnEmpty;
        }

        protected void Unsubscribe(Stat stat)
        {
            if (stat == null) return;

            stat.OnInit -= OnInit;
            stat.OnUninit -= OnUninit;
            stat.OnChange -= OnChange;
            stat.OnFull -= OnFull;
            stat.OnEmpty -= OnEmpty;
        }

        protected virtual void OnEmpty(Stat stat) { }
        protected virtual void OnFull(Stat stat) { }
        protected virtual void OnChange(Stat stat) { }
        protected virtual void OnUninit(Stat stat) { }
        protected virtual void OnInit(Stat stat) { }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (Holder == null) Holder = GetComponent<StatsHolder>(); 
        }
#endif
    }
}