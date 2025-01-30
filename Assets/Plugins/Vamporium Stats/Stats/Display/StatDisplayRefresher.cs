using System;
using UnityEngine;
using DG.Tweening;

namespace VamporiumStats
{
    public class StatDisplayRefresher
    {
        private Ease _ease { get; set; }
        private float _delay = 0;

        public void Refresh(Stat stat, StatDisplay display, Ease ease)
        {
            if (!display.Active) return;

            _ease = ease;
            var duration = display.Duration;
            display.BeforeRefresh();

            switch (display.Type)
            {
                case StatDisplay.DisplayType.Value:
                    if (display.Text == null) break;
                    display.Text.text = string.Format(display.StringFormat, stat.Current.ToString());
                    break;

                case StatDisplay.DisplayType.ValueAndMax:
                    if (display.Text == null) break;
                    display.Text.text = string.Format(display.StringFormat, $"{stat.Current}/{stat.Max}");
                    break;

                case StatDisplay.DisplayType.Percentage:
                    if (display.Text == null) break;
                    var percent = Percent(stat).ToString(display.FloatFormat);
                    display.Text.text = string.Format(display.StringFormat, $"{percent}%");
                    break;

                case StatDisplay.DisplayType.ProgressBar:
                    if (display.Image == null) break;
                    DOTween.Kill(display.Image);
                    var fill = display.Image.fillAmount;
                    Tween(() => DOTween.To(() => fill, x => display.Image.fillAmount = x, Progress(stat), duration));
                    break;

                case StatDisplay.DisplayType.TintGraphic:
                    if (display.Graphic == null) break;
                    DOTween.Kill(display.Graphic);
                    switch (display.Tint)
                    {
                        case StatDisplay.TintMode.Stat: Tween(() => display.Graphic.DOColor(stat.Asset.Color, duration)); break;
                        case StatDisplay.TintMode.Custom: Tween(() => display.Graphic.DOColor(display.Color, duration)); break;
                        case StatDisplay.TintMode.Gradient: Tween(() => display.Graphic.DOColor(Gradient(stat, display), duration)); break;
                    }
                    break;

                case StatDisplay.DisplayType.Position:
                case StatDisplay.DisplayType.Rotation:
                case StatDisplay.DisplayType.Scale:
                    if (display.Transform == null) break;
                    DOTween.Kill(display.Transform);
                    var vector = display.TransformOrigin;
                    var value = stat.Max > 0 ? ValueLerp(stat, display) : ValueAdd(stat, display);
                    switch (display.Axis)
                    {
                        case StatDisplay.TransformAxis.X: vector.x = value; break;
                        case StatDisplay.TransformAxis.Y: vector.y = value; break;
                        case StatDisplay.TransformAxis.Z: vector.z = value; break;
                    }
                    switch (display.Type)
                    {
                        case StatDisplay.DisplayType.Position: Tween(() => display.Transform.DOLocalMove(vector, duration)); break;
                        case StatDisplay.DisplayType.Rotation: Tween(() => display.Transform.DOLocalRotate(vector, duration)); break;
                        case StatDisplay.DisplayType.Scale: Tween(() => display.Transform.DOScale(vector, duration)); break;
                    }
                    break;

                case StatDisplay.DisplayType.Activate:
                    if (display.GameObject == null) break;
                    display.GameObject.SetActive(Activator(stat, display));
                    break;

                case StatDisplay.DisplayType.Enable:
                    if (display.Component == null) break;
                    if (display.Component is MonoBehaviour mono) mono.enabled = Activator(stat, display);
                    else if (display.Component is Renderer renderer) renderer.enabled = Activator(stat, display);
                    break;

                case StatDisplay.DisplayType.EvaluateFloat:
                    display.CurveEventFloat.Invoke(display.Curve.Evaluate(Progress(stat)) * display.CurveMultiplier);
                    break;

                case StatDisplay.DisplayType.EvaluateVector2:
                case StatDisplay.DisplayType.EvaluateVector3:
                    Vector3 v = default;
                    var evaluation = display.Curve.Evaluate(Progress(stat)) * display.CurveMultiplier;
                    switch (display.CurveAxis)
                    {
                        case StatDisplay.TransformAxis.X: v.x = evaluation; break;
                        case StatDisplay.TransformAxis.Y: v.y = evaluation; break;
                        case StatDisplay.TransformAxis.Z: v.z = evaluation; break;
                    }
                    switch (display.Type)
                    {
                        case StatDisplay.DisplayType.EvaluateVector2: display.CurveEventVector2.Invoke(v); break;
                        case StatDisplay.DisplayType.EvaluateVector3: display.CurveEventVector3.Invoke(v); break;
                    }
                    break;

                case StatDisplay.DisplayType.EvaluateColor:
                    display.CurveEventColor.Invoke(display.CurveGradient.Evaluate(display.Curve.Evaluate(Progress(stat))));
                    break;
            }
        }

        private Tween Tween(Func<Tween> func) => func.Invoke().SetEase(_ease).SetDelay(_delay);
        private float Progress(Stat stat) => stat.Max > 0 ? stat.Current / (float)stat.Max : 0;
        private float Percent(Stat stat) => Progress(stat) * 100;
        private Color Gradient(Stat stat, StatDisplay display) => display.Gradient.Evaluate(Progress(stat));
        private float ValueLerp(Stat stat, StatDisplay display) => Mathf.Lerp(display.FirstValue, display.LastValue, Progress(stat));
        private float ValueAdd(Stat stat, StatDisplay display) => display.FirstValue + display.LastValue * stat.Current;
        private bool Activator(Stat stat, StatDisplay display) => display.Activator switch
        {
            StatDisplay.ActivateWhen.Never => false,
            StatDisplay.ActivateWhen.Always => true,
            StatDisplay.ActivateWhen.Empty => stat.IsEmpty,
            StatDisplay.ActivateWhen.NotEmpty => !stat.IsEmpty,
            StatDisplay.ActivateWhen.Full => stat.IsFull,
            StatDisplay.ActivateWhen.NotFull => !stat.IsFull,
            StatDisplay.ActivateWhen.LessThan => stat.Current < display.ActivatorValue,
            StatDisplay.ActivateWhen.MoreThan => stat.Current > display.ActivatorValue,
            _ => false,
        };
    }
}
