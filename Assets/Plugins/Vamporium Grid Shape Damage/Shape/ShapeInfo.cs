using UnityEngine;
using Sirenix.OdinInspector;

namespace ShapedNumbers
{
    [CreateAssetMenu(menuName = "Shape Info")]
    public class ShapeInfo : ScriptableObject
    {
        [SerializeField, HideLabel, InlineProperty] private Shape _shape;
    }
}
 