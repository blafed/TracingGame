using System.Collections.Generic;
using UnityEngine;

namespace KidLetters.Home
{
    public class LetterContainer : Singleton<LetterContainer>
    {
        [System.Serializable]
        class CameraOptions
        {
            public float duration = 1;
            public float cameraZoom = 25;
            public DG.Tweening.Ease ease;
            public Vector2 cameraFocus = new Vector2();
        }


        List<LetterRaw> letters = new List<LetterRaw>();


        [SerializeField] CameraOptions cameraOptions = new CameraOptions();


        private void Start()
        {
            letters = new List<LetterRaw>(GetComponentsInChildren<LetterRaw>());
            foreach (var x in letters)
            {
                var col = x.gameObject.AddComponent<LetterButton>();
                var filler = LetterFiller.createStandardFiller(x);
                filler.transform.parent = x.transform;

            }
        }


        public void adjustCamera()
        {
            CameraControl.o.move(cameraOptions.cameraFocus, cameraOptions.duration);
            CameraControl.o.zoom(cameraOptions.cameraZoom, cameraOptions.duration);
            setActiveLetters(true);
        }

        // void onPhaseEnter()
        // {

        // }



        public void selectLetter(LetterRaw letter)
        {
            HomePhase.o.selectLetter(letter);
        }

        public void setActiveLetters(bool active, System.Predicate<LetterRaw> except = null)
        {
            foreach (var x in letters)
            {
                if (except != null && except(x))
                    continue;
                x.gameObject.SetActive(active);
            }
        }


        public LetterRaw getLetter(int letterId)
        {
            return letters.Find(x => x.letterId == letterId);
        }


        public LetterRaw instantiateLetter(int letterId)
        {
            return Instantiate(LetterPrefabContainer.o.getLetterPrefab(letterId), getLetter(letterId).transform.position, default).GetComponent<LetterRaw>();
        }

    }
}