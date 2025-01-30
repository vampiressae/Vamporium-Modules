using UnityEngine;

namespace VamporiumStats
{
    [CreateAssetMenu(menuName = "Vamporium/Stats/Stat")]
    public class StatAsset : ScriptableObject
    {
        public string Name;
        public Sprite Icon;
        public Color Color;
    }
}
