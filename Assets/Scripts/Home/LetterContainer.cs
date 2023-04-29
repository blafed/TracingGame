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


        List<Letter> letters = new List<Letter>();


        [SerializeField] CameraOptions cameraOptions = new CameraOptions();


        protected override void Awake()
        {
            base.Awake();
            letters = new List<Letter>(GetComponentsInChildren<Letter>());
            foreach (var x in letters)
            {
                var col = x.gameObject.AddComponent<BoxCollider>();
                col.isTrigger = true;
                col.size = x.size;
                x.onClick += () => selectLetter(x);
            }
        }
        private void Start()
        {
            // HomePhase.o.onEnterEvent += onPhaseEnter;
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



        public void selectLetter(Letter letter)
        {
            HomePhase.o.selectLetter(letter);
        }

        public void setActiveLetters(bool active, System.Predicate<Letter> except = null)
        {
            foreach (var x in letters)
            {
                if (except != null && except(x))
                    continue;
                x.gameObject.SetActive(active);
            }
        }


        public Letter getLetter(int letterId)
        {
            return letters.Find(x => x.letterId == letterId);
        }


        public Letter instantiateLetter(int letterId)
        {
            return Instantiate(LetterPrefabContainer.o.getLetterPrefab(letterId), getLetter(letterId).transform.position, default).GetComponent<Letter>();
        }

    }
}