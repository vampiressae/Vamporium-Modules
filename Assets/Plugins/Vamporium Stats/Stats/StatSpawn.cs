using UnityEngine;

namespace VamporiumStats
{
    public abstract class StatSpawn : MonoBehaviour
    {
        protected abstract bool DependsOnStat { get; }

        protected Stat _stat;

        protected virtual void Awake() { }
        protected virtual void Start() { }
        protected virtual void OnDestroy() { }

        public virtual void Init(Stat stat)
        {
            if (DependsOnStat && stat == null)
                Debug.LogError("A stat-dependent <b>StatSpawn</b> was not given a <b>Stat</b>", gameObject);
            else
            {
                _stat = stat;
                Init();
            }
        }

        protected abstract void Init();
    }
}
