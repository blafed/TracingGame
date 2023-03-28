using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KidLetters
{
    using WordPictureAnimations;
    public class WordPictureAnimation : MonoBehaviour
    {
        public bool overrideMeta = false;
        [SerializeField] bool autoTerminate = false;
        [SerializeField] float autoTerminateDuration = 2;
        [System.Serializable]
        public class Meta
        {
            [System.Obsolete]
            public float duration = 1;
            public float audioPlayDelay = .5f;
        }
        public Meta meta;
        public CallbackObjects onAwake = new CallbackObjects();
        public bool isTerminated { get; private set; }

        void Awake()
        {
            onAwake?.Invoke();
        }

        IEnumerator Start()
        {
            if (autoTerminate)
            {
                yield return new WaitForSeconds(autoTerminateDuration);
                isTerminated = true;
            }
        }

        public void terminate()
        {
            isTerminated = true;
        }
    }
}