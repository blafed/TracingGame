using System.Collections.Generic;
using System.Collections;
using UnityEngine;


namespace KidLetters.Tracing
{
    public class TracingRunner : MonoBehaviour
    {

        public IEnumerator play()
        {
            startPlaying();

            while (true)
            {
                yield return Extensions2.WaitForFixedUpdate;
                whilePlaying(Time.fixedDeltaTime);
            }
        }


        void startPlaying()
        {

        }
        void whilePlaying(float dt)
        {

        }
    }
}