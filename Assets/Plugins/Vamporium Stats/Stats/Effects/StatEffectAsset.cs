using System;
using UnityEngine;

namespace VamporiumStats
{
    public abstract class StatEffectAsset : ScriptableObject
    {
        [Flags] public enum NeededFields 
        { 
            None = 0, 
            GameObject = 1, Prefab = 2, StatAsset = 4, 
            String = 8, Int = 16, Float = 32, Bool = 64,
            Holder = 128, Event = 256,
        }

        public abstract NeededFields Needed { get; }
        public virtual string GetNeededName(NeededFields field) => string.Empty;

        public abstract void Trigger(StatEffect effect, Stat stat);
    }
}
