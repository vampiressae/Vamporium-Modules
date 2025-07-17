using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace ShapedNumbers
{
    [Serializable]
    public class Shape
    {
        [InlineProperty, OnValueChanged("SizeChanged")] public XY _size;

        [SerializeField, ReadOnly, HideInEditorMode] private List<XY> _keys = new();
        [SerializeField, ReadOnly, HideInEditorMode] private List<int> _values = new();

        private Dictionary<XY, int> _cells = new();

        public int Get(int x, int y) => _cells != null && _cells.TryGetValue(new(x, y), out var cell) ? cell : 0;
        public int Get(XY xy) => _cells != null && _cells.TryGetValue(xy, out var cell) ? cell : 0;

        private void SetCells()
        {
            _cells = new();
            for (int i = 0; i < _keys.Count; i++)
                _cells.Add(_keys[i], _values[i]);
        }

        private void GetCells()
        {
            _keys = new();
            _values = new();

            foreach (var cell in _cells)
            {
                _keys.Add(cell.Key);
                _values.Add(cell.Value);
            }
        }

#if UNITY_EDITOR

        [OnInspectorGUI]
        public void OnInspectorGUI()
        {
            SetCells();
            _cells ??= new();

            GUILayout.Space(10);
            GUILayout.BeginVertical(UnityEditor.EditorStyles.helpBox);
            for (int y = 0; y < _size.Y; y++)
            {
                GUILayout.BeginHorizontal();
                for (int x = 0; x < _size.X; x++)
                {
                    var xy = new XY(x, y);
                    var value = Get(xy);

                    GUI.color = GetColor(value);
                    if (GUILayout.Button(value.ToString(), GUILayout.Width(25), GUILayout.Height(25)))
                    {
                        _cells[xy] = (value + 1) % (21);
                        GetCells();
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
            GUILayout.Space(10);

            GUI.color = Color.white;
            if (GUILayout.Button("Reset"))
            {
                _keys.Clear();
                _values.Clear();
                SetCells();
            }
        }

        private void SizeChanged()
        {
            var excess = new List<XY>();
            foreach (var kv in _cells)
                if (kv.Key.X >= _size.X || kv.Key.Y >= _size.Y)
                    excess.Add(kv.Key);

            foreach (var cell in excess)
                _cells.Remove(cell);

            GetCells();
        }

        private Color GetColor(int value) => value switch
        {
            0 => Color.gray,
            1 or 2 => Color.white,
            3 or 4 or 5 => new(1, 0.7f, 0.5f),
            6 or 7 or 8 or 9 => new(0.9f, 1, 1),
            10 or 11 or 12 or 13 or 14 => new(1, 1, 0.7f),
            15 or 16 or 17 or 18 or 19 or 20 => new(1, 0.7f, 1),
            _ => Color.red,
        };
#endif
    }
}