using UnityEngine;

namespace VamporiumAudio
{
    public class AudioCollision3D : AudioCollision
    {
        private void OnCollisionEnter(Collision collision)
        {
            var magnitude = collision.relativeVelocity.magnitude;
            //Play(collision.GetContact(collision.contactCount - 1).point, magnitude);
            Play(collision.GetContact(0).point, magnitude);

            Debug.Log("NOW");

            foreach (ContactPoint contact in collision.contacts)
            {
                Debug.DrawRay(contact.point, contact.normal, Color.white);
            }
        }
    }
}
