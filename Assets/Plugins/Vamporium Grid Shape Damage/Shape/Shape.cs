using System;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
public class Shape
{
    private int[,] _cells;

#if UNITY_EDITOR
    private int max = 5;

    [OnInspectorGUI]
    private void OnInspectorGUI()
    {
        _cells ??= new int[1, 1];
        var cols = _cells.GetLength(0);
        var rows = _cells.GetLength(1);

        for (int y = 0; y < rows; y++)
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < cols; x++)
            {
                if (GUILayout.Button(_cells[x, y].ToString(), GUILayout.Width(20)))
                    _cells[x, y] = (_cells[x, y] + 1) % (max + 1);
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.BeginVertical();
        if (GUILayout.Button("NOW"))
            _cells = new int[5, 5];
        GUILayout.EndVertical();
    }
#endif

}
