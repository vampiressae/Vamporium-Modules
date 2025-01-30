using UnityEditor;
using UnityEngine;

namespace VamporiumAudio
{
    public class AudioEditor<T> : Editor where T : MonoBehaviour
    {
        protected T _target;

        protected virtual void OnEnable() => _target = target as T;
    }
}
