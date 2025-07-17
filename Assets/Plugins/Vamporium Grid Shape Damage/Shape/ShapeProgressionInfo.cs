using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Progression Info")]
public class ShapeProgressionInfo : ScriptableObject
{
    [Serializable]
    public class Module
    {
        public int Max;
        public Color Color;
    }

    public Module[] Modules;

}
