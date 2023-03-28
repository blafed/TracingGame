using UnityEngine;

namespace KidLetters.WordPictureAnimations
{
    public class Terminate : MonoBehaviour
    {

        private void Start()
        {
            WordPictureAnimation animation = GetComponentInParent<WordPictureAnimation>();
            animation.terminate();
        }
    }
}