using UnityEngine;

namespace VamporiumAudio
{
    public class AudioTrigger2D : AudioTrigger
    {
        private void OnTriggerEnter2D(Collider2D collider)
        {
            var rb = collider.attachedRigidbody;
            Play(collider.transform.position, rb ? rb.velocity.magnitude : -1, 0);
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            var rb = collider.attachedRigidbody;
            Play(collider.transform.position, rb ? rb.velocity.magnitude : -1, 0);
        }
    }
}
