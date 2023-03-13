using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace KidLetters.Pronouncing
{
    public class TerminatePronouncing : Singleton<TerminatePronouncing>
    {
        public float duration = .8f;


        private void Start()
        {
            PronouncingPhase.o.onExitEvent += clean;
        }
        void clean()
        {
            StopAllCoroutines();
        }

        public IEnumerator play()
        {
            var letter = PronouncingPhase.o.letter;
            int i = 0;
            StartCoroutine(WordView.o.fadeOutLetters(duration, x =>
            {
                var j = i;
                i++;
                return PronouncingPhase.o.letterIndexInWord == j;
            }));
            yield return new WaitForSeconds(duration);
            letter.gameObject.SetActive(true);
        }
    }
}