using System.Collections;
using UnityEngine;

namespace KidLetters.WordPictureAnimations
{
    public class BallBounce : MonoBehaviour
    {
        public float gravity = -10;
        public float bounce = .5f;
        public float groundLevel = -.5f;
        public Vector2 velocity = Vector2.zero;


        public CallbackObjects onBounce;

        bool didReflected = false;

        private void FixedUpdate()
        {
            velocity += Vector2.up * gravity * Time.fixedDeltaTime;
            var p = transform.localPosition;
            if (p.y < groundLevel && !didReflected)
            {
                didReflected = true;
                doBounce();
            }
            else
            {
                didReflected = false;
            }

            transform.localPosition += (Vector3)velocity * Time.fixedDeltaTime;
        }


        public void doBounce()
        {
            velocity = Vector2.Reflect(velocity, Vector2.up) * bounce;
            onBounce?.Invoke();
        }
    }

}