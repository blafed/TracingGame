using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace KidLetters.WordPictureAnimations
{
    public class Delay : MonoBehaviour
    {
        public float time = 1;
        public CallbackObjects onFinish;


        public bool isDelayFinished { get; private set; }


        IEnumerator Start()
        {
            yield return new WaitForSeconds(time);
            isDelayFinished = true;
            onFinish?.Invoke();
        }
    }
}