using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Shape Info")]
public class ShapeInfo : ScriptableObject
{
    [SerializeField, InlineProperty, HideLabel] private Shape _shape;
}
