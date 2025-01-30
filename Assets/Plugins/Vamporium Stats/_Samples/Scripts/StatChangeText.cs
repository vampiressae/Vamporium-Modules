using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using TMPro;

namespace VamporiumStats
{
    public class StatChangeText : StatSpawn
    {
        private enum Tinting { None, StatColor, CustomColor, Dynamic }
        [Title("Settings")]
        [SerializeField, LabelWidth(70)] private TMP_Text _text;
        [SerializeField, LabelWidth(70), LabelText("Add + Sign"), SuffixLabel("Add the + sign if positive")] private bool _sign = true;
        [SerializeField, LabelWidth(70)] private Tinting _tinting;
        [SerializeField, LabelWidth(70), ShowIf(nameof(_tinting), Tinting.CustomColor)] private Color _color;
        [SerializeField, LabelWidth(70), ShowIf(nameof(_tinting), Tinting.Dynamic)] private Color _negative;
        [SerializeField, LabelWidth(70), ShowIf(nameof(_tinting), Tinting.Dynamic)] private Color _positive;
        [Title("Animation")]
        [SerializeField, LabelWidth(70)] private Vector3 _position = Vector3.up * 2;
        [SerializeField, LabelWidth(70)] private float _duration = 2;
        [SerializeField, LabelWidth(70)] private float _delay = 0.3f;
        [SerializeField, LabelWidth(70), PropertyRange(0, "FadeMax")] private float _fade = 1;
        [SerializeField, LabelWidth(70)] private float _punch = 0.3f;

        protected override bool DependsOnStat => true;
        private bool IsPositive => _stat.LastDelta > 0;

        private Tween _fadeTween;

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Kill();
        }

        protected override void Init()
        {
            Kill();

            transform.DOMove(transform.position + _position, _duration).SetDelay(_delay);
            transform.DOPunchScale(Vector3.one * _punch, 0.3f);
            _fadeTween = DOTween.ToAlpha(() => _text.color, a => _text.color = a, 0, _fade).SetDelay(_delay + _duration - _fade);
            _fadeTween.onComplete += Finish;

            _text.text = (IsPositive && _sign ? "+" : "") + _stat.LastDelta.ToString();
            if (_tinting != Tinting.None) _text.color = GetColor();
        }

        private void Finish() => Destroy(gameObject);

        private Color GetColor() => _tinting switch
        {
            Tinting.StatColor => _stat.Asset.Color,
            Tinting.CustomColor => _color,
            Tinting.Dynamic => IsPositive ? _positive : _negative,
            _ => Color.white,
        };

        private void Kill()
        {
            DOTween.Kill(transform);
            DOTween.Kill(_fadeTween);
        }

#if UNITY_EDITOR
        private float FadeMax()
        {
            if (_fade > _duration) _fade = _duration;
            return _duration;
        }

        private void OnValidate()
        {
            if (_text == null) _text = GetComponent<TMP_Text>();
        }
#endif
    }
}
