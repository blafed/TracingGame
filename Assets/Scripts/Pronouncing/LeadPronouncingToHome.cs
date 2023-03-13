using System.Collections;
using UnityEngine;

namespace KidLetters.Pronouncing
{
    public class LeadPronouncingToHome : Singleton<LeadPronouncingToHome>
    {
        public float duration = 1;


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
            yield return new WaitForSeconds(duration);
        }
    }
}