using UnityEngine;

namespace VamporiumAudio
{
    public class AudioTrigger3D : AudioTrigger
    {
        private void OnTriggerEnter(Collider collider)
        {
            var rb = collider.attachedRigidbody;
            Play(collider.transform.position, rb ? rb.velocity.magnitude : -1, 0);
        }

        private void OnTriggerExit(Collider collider)
        {
            var rb = collider.attachedRigidbody;
            Play(collider.transform.position, rb ? rb.velocity.magnitude : -1, 0);
        }
    }
}
