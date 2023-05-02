namespace KidLetters.Home
{
    using UnityEngine;

    public class LetterButton : MonoBehaviour
    {
        LetterRaw letterRaw;
        private void Awake()
        {
            var collider = gameObject.AddComponent<BoxCollider2D>();
            letterRaw = GetComponent<LetterRaw>();
            collider.size = letterRaw.size;
            collider.isTrigger = true;

        }



        private void OnMouseDown()
        {
            HomePhase.o.selectLetter(letterRaw);
        }
    }
}