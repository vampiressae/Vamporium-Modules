using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VamporiumStats
{
    [Serializable]
    public class Stat
    {
        public event Action<Stat> OnInit, OnUninit, OnChange, OnFull, OnEmpty;

        [HorizontalGroup("1"), HideLabel] public StatAsset Asset;
        [HorizontalGroup("1", 80), LabelWidth(30), LabelText("Max")] public int Max;

        [HorizontalGroup("2"), SerializeField, LabelWidth(50)] private StatInitAction _onInit = StatInitAction.SetToMax;
        [HorizontalGroup("2", 48), HideLabel, ShowIf(nameof(_onInit), StatInitAction.SetToValue)] public int _initValue;

        [HorizontalGroup("edit"), ShowInInspector, HideInEditorMode, ReadOnly, LabelWidth(50)] public int Current { get; set; }

        public bool IsFull => Max > 0 && Current == Max;
        public bool IsEmpty => Current == 0;
        public int LastDelta { get; private set; }

        public void Init()
        {
            switch (_onInit)
            {
                case StatInitAction.SetToMax: Current = Max; break;
                case StatInitAction.SetToValue: Current = _initValue; break;
            }
            OnInit?.Invoke(this);
            InvokeEvents();
        }

        public void Uninit() => OnUninit?.Invoke(this);

        public void Add(int amount)
        {
            if (amount == 0) return;
            if (amount > 0 && IsFull || amount < 0 && IsEmpty) return;

            var old = Current;

            Current += amount;
            if (Max > 0) Current = Mathf.Clamp(Current, 0, Max);
            LastDelta = Current - old;

            InvokeEvents();
        }

        public void Set(int amount)
        {
            if (amount == Current) return;

            var old = Current;
            Current = amount;
            if (Max > 0) Current = Mathf.Clamp(Current, 0, Max);
            LastDelta = Current - old;

            InvokeEvents();
        }

        private void InvokeEvents()
        {
            OnChange?.Invoke(this);
            if (IsFull) OnFull?.Invoke(this);
            if (IsEmpty) OnEmpty?.Invoke(this);
        }

#if UNITY_EDITOR
        [ShowInInspector, HideInEditorMode, OnValueChanged(nameof(SetTo))]
        [HorizontalGroup("edit", 80), PropertyOrder(10), LabelWidth(30), LabelText(" Set:")] private int _setToValue;
        private void SetTo() => Set(_setToValue);
#endif
    }
}