using UnityEngine;

public class SinMoveTransform : MonoBehaviour
{
    [SerializeField] private float _distance = 15;
    [SerializeField] private float _speed = 1;
    [SerializeField, Range(0, 1)] private float _offset;

    private float _y, _z;
    private void Awake()
    {
        _y = transform.position.y;
        _z = transform.position.z;
    }

    private void Update()
    {
        var x = Mathf.Sin(Time.time * _speed + Mathf.PI * _offset) * _distance;
        transform.position = new(x, _y, _z);
    }
}
