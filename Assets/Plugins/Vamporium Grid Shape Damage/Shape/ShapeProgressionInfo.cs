using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace ShapedNumbers
{
    [CreateAssetMenu(menuName = "Progression Info")]
    public class ShapeProgressionInfo : ScriptableObject
    {
        [Serializable]
        public class Module
        {
            [HideLabel, HorizontalGroup(40)] public int Max;
            [HideLabel, HorizontalGroup] public Color Color;
        }

        public Module[] Modules;

        public bool GetModuleAndTier(int value, out Module module, out int tier)
        {
            module = null;
            tier = -1;

            if (value >= 0)
                for (int i = 0; i < Modules.Length; i++)
                {
                    if (value > Modules[i].Max) continue;
                    module = Modules[i];
                    tier = value - (i == 0 ? 0 : Modules[i - 1].Max);
                    return true;
                }

            return false;
        }
    }
}
