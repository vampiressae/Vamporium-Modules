using System;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;
using DG.Tweening;
using UnityEngine.Events;

namespace VamporiumStats
{
    [Serializable]
    public class StatDisplay
    {
        public enum DisplayType
        {
            None = 0, Value = 10, ValueAndMax = 20, Percentage = 30, ProgressBar = 50, TintGraphic = 100,
            Position = 200, Rotation = 210, Scale = 220, Activate = 300, Enable = 400, 
            EvaluateFloat = 500, EvaluateVector2 = 501, EvaluateVector3 = 502, EvaluateColor = 510
        }
        public enum TintMode { None, Stat, Custom, Gradient }
        public enum TransformAxis { X, Y, Z }
        public enum ActivateWhen { Never = 0, Empty = 10, NotEmpty = 11, Full = 20, NotFull = 21, LessThan = 30, MoreThan = 31, Always = 100 }

        [LabelWidth(50), HorizontalGroup("type"), GUIColor("@StatsExtensions.GUIColor")] public DisplayType Type;
        [HideLabel, HorizontalGroup("type", .15f), ShowIf(nameof(HasDuration))] public float Duration;
        [HideLabel, HorizontalGroup("type", 15), GUIColor("$ActiveGUIColor")] public bool Active = true;
        // text
        [VerticalGroup("all", VisibleIf = nameof(Active))]
        [LabelWidth(50), HorizontalGroup("all/text1"), ShowIf(nameof(IsText))] public TMP_Text Text;
        [HideLabel, HorizontalGroup("all/text1", .15f), ShowIf(nameof(IsFloat))] public string FloatFormat = "f0";
        [LabelWidth(50), VerticalGroup("all"), ShowIf(nameof(IsText)), LabelText("Format")] public string StringFormat = "{0}";
        // image fill
        [LabelWidth(50), VerticalGroup("all"), ShowIf(nameof(IsImage))] public Image Image;
        // tint graphic
        [LabelWidth(50), HorizontalGroup("all/graphic1"), ShowIf(nameof(IsTintGraphic))] public Graphic Graphic;
        [HideLabel, HorizontalGroup("all/graphic1", .3f), ShowIf(nameof(IsTint))] public TintMode Tint;
        [LabelWidth(50), VerticalGroup("all"), ShowIf(nameof(IsColor))] public Color Color;
        [LabelWidth(50), VerticalGroup("all"), ShowIf("IsGradient")] public Gradient Gradient;

        [LabelWidth(50), VerticalGroup("all"), ShowIf("IsGradientError"), LabelText("Gradient")]
        [ShowInInspector, VerticalGroup("all")] public string GradientError => "Incompatible with stats without a Max value!";
        // transform
        [LabelWidth(50), HorizontalGroup("all/t1", VisibleIf = nameof(IsTransform)), LabelText("Target")] public Transform Transform;
        [HideLabel, HorizontalGroup("all/t1", .15f)] public TransformAxis Axis;
        [LabelWidth(50), HorizontalGroup("all/t2", VisibleIf = nameof(IsTransform)), LabelText("$FirstValueLabel")] public float FirstValue;
        [LabelWidth(30), HorizontalGroup("all/t2", .45f), LabelText("$LastValueLabel")] public float LastValue;
        // gameobject / mono
        [LabelWidth(50), VerticalGroup("all"), ShowIf(nameof(IsGameObject))] public GameObject GameObject;
        [LabelWidth(50), VerticalGroup("all"), ShowIf(nameof(IsMono))] public Component Component;
        [LabelWidth(50), HorizontalGroup("all/go"), ShowIf(nameof(IsActivator)), LabelText("When")] public ActivateWhen Activator;
        [HideLabel, HorizontalGroup("all/go", .15f), ShowIf(nameof(IsActivatorInt))] public int ActivatorValue;
        // animation curve
        [LabelWidth(50), HorizontalGroup("all/curve"), ShowIf(nameof(IsEvaluate))] public AnimationCurve Curve = AnimationCurve.Linear(0, 0, 1, 1);
        [LabelWidth(10), HorizontalGroup("all/curve", .15f), ShowIf(nameof(IsEvaluateMultiplier)), LabelText("*")] public float CurveMultiplier = 1;
        [HideLabel, HorizontalGroup("all/curve", .15f), ShowIf(nameof(IsEvaluateAxis))] public TransformAxis CurveAxis;
        [LabelWidth(50), VerticalGroup("all"), ShowIf(nameof(Type), DisplayType.EvaluateColor), LabelText("Gradient")] public Gradient CurveGradient;
        [HideLabel, VerticalGroup("all"), ShowIf(nameof(Type), DisplayType.EvaluateFloat)] public UnityEvent<float> CurveEventFloat;
        [HideLabel, VerticalGroup("all"), ShowIf(nameof(Type), DisplayType.EvaluateVector2)] public UnityEvent<Vector2> CurveEventVector2;
        [HideLabel, VerticalGroup("all"), ShowIf(nameof(Type), DisplayType.EvaluateVector3)] public UnityEvent<Vector3> CurveEventVector3;
        [HideLabel, VerticalGroup("all"), ShowIf(nameof(Type), DisplayType.EvaluateColor)] public UnityEvent<Color> CurveEventColor;

        public Vector3 TransformOrigin { get; set; }
        private bool _inited;

        public void BeforeRefresh()
        {
            if (!_inited)
            {
                switch (Type)
                {
                    case DisplayType.Position: TransformOrigin = Transform ? Transform.localPosition : default; break;
                    case DisplayType.Rotation: TransformOrigin = Transform ? Transform.localEulerAngles : default; break;
                    case DisplayType.Scale: TransformOrigin = Transform ? Transform.localScale : default; break;
                }
                _inited = true;
            }
        }

        public void Kill()
        {
            switch (Type)
            {
                case DisplayType.ProgressBar: DOTween.Kill(Image); break;
                case DisplayType.TintGraphic: DOTween.Kill(Graphic); break;

                case DisplayType.Position:
                case DisplayType.Rotation:
                case DisplayType.Scale:
                    DOTween.Kill(Transform);
                    break;
            }
        }

        public bool IsFloat => Type == DisplayType.Percentage;
        private bool IsText => Type >= DisplayType.Value && Type <= DisplayType.Percentage;
        private bool HasDuration => Type == DisplayType.ProgressBar || Type == DisplayType.TintGraphic || IsTransform;
        private bool IsImage => Type == DisplayType.ProgressBar;
        private bool IsTintGraphic => Type == DisplayType.TintGraphic;
        private bool IsTransform => Type >= DisplayType.Position && Type <= DisplayType.Scale;
        private bool IsTint => Type == DisplayType.TintGraphic;
        private bool IsColor => IsTint && Tint == TintMode.Custom;
        private bool IsMono => Type == DisplayType.Enable;
        private bool IsGameObject => Type == DisplayType.Activate;
        private bool IsActivator => IsGameObject || IsMono;
        private bool IsActivatorInt => IsActivator && (Activator == ActivateWhen.LessThan || Activator == ActivateWhen.MoreThan);
        private bool IsEvaluate => Type >= DisplayType.EvaluateFloat && Type <= DisplayType.EvaluateColor;
        private bool IsEvaluateAxis => Type == DisplayType.EvaluateVector2 || Type == DisplayType.EvaluateVector3;
        private bool IsEvaluateMultiplier => Type >= DisplayType.EvaluateFloat && Type < DisplayType.EvaluateColor;

#if UNITY_EDITOR
        private bool IsGradient => IsTint && Tint == TintMode.Gradient && StatInEditor.Max > 0;
        private bool IsGradientError => IsTint && Tint == TintMode.Gradient && StatInEditor.Max < 1;
        private string FirstValueLabel => StatInEditor.Max > 0 ? "Empty" : "Start";
        private string LastValueLabel => StatInEditor.Max > 0 ? "Full" : "Step";

        private Stat StatInEditor { get; set; }
        public void OnValidate(Stat stat) => StatInEditor = stat;
        private Color ActiveGUIColor => Active ? Color.green : Color.red;
#endif
    }
}