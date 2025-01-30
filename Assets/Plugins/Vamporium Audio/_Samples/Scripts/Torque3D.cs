using UnityEngine;

public class Torque3D : MonoBehaviour
{
    [SerializeField] private float _torque;

    private Rigidbody _rb;
  
    private void Start() => _rb = GetComponent<Rigidbody>();
    public void Torque() => _rb.AddTorque(new(0, 0, _torque));
}
