using System;
using Sirenix.OdinInspector;

namespace ShapedNumbers
{
    [Serializable]
    public struct XY
    {
        [HorizontalGroup, HideLabel] public int X;
        [HorizontalGroup, LabelWidth(6), LabelText(":")] public int Y;

        public XY(int x, int y) { X = x; Y = y; }
        public override readonly string ToString() => $"{X}:{Y}";
    }
}