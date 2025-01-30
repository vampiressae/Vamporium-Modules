using UnityEngine;

namespace VamporiumAudio
{
    public class AudioCollision2D : AudioCollision
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var magnitude = collision.relativeVelocity.magnitude;
            Play(collision.GetContact(collision.contactCount - 1).point, magnitude);
        }
    }
}
