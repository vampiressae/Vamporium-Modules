using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class ShapeManagerTEST : MonoBehaviour
{
    [SerializeField] private Vector2Int _testArea;
    [SerializeField] private float _shapeSize;
    [SerializeField] private ShapeBehaviourTest _shapePrefab;
    [SerializeField] private RectTransform _shapeParent;
    [SerializeField] private float _maxReach;

    private readonly List<ShapeBehaviourTest> _shapes = new();

    [ShowInInspector, HideInEditorMode]
    private ShapeBehaviourTest _current;

    private void Start()
    {
        _maxReach =  _shapeSize * 0.75f;
        _shapeParent.sizeDelta = (Vector2)_testArea * _shapeSize;
        for (int y = 0; y < _testArea.y; y++)
            for (int x = 0; x < _testArea.x; x++)
            {
                var shape = Instantiate(_shapePrefab, _shapeParent);
                shape.Init(new(x, y), _shapeSize, _testArea);
                _shapes.Add(shape);
            }
    }

    private void Update()
    {
        ShapeBehaviourTest shape = null;
        float reach = _maxReach;
        var mouse = Input.mousePosition;

        for (int i = 0; i < _shapes.Count; i++)
        {
            _shapes[i].SetActive(false);

            var distance = Vector2.Distance(_shapes[i].RT.position, mouse);
            if (distance > reach) continue;

            reach = distance;
            shape = _shapes[i];
        }

        _current = shape;

        if (_current == null) return;
        _current.SetActive(true);
    }
}
