using UnityEngine;
using UnityEngine.UI;
using VamporiumAudio;

public class AmbientStartStop : MonoBehaviour
{
    [SerializeField] private AmbientPlayer _ambient;
    [SerializeField] private Toggle _toggle;

    private void Start() => _toggle.onValueChanged.AddListener(Toggle);

    private void Toggle(bool value)
    {
        if (value) _ambient.Play(); 
        else _ambient.Stop();
    }
}
