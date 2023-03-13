using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KidLetters
{
    public class WordPictureAnimation : MonoBehaviour
    {
        public bool overrideMeta = false;
        [System.Serializable]
        public class Meta
        {
            public float duration = 1;
            public float audioPlayDelay = .5f;
        }
        public Meta meta;
    }
}