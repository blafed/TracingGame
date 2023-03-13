using DG.Tweening;
using System.Collections.Generic;
using System.Collections;

using UnityEngine;
using System;

namespace KidLetters.Pronouncing
{
    public class FocusOnLetter : Singleton<FocusOnLetter>
    {

        public bool isDone { get; private set; }

        [SerializeField] float cameraZoom = 10;
        [SerializeField] float duration = 1;


        [SerializeField] Ease zoomEase = Ease.InBack;
        [SerializeField] Ease moveEase = Ease.InOutQuad;


        private void Start()
        {
            PronouncingPhase.o.onExitEvent += onPhaseExit;

        }

        private void onPhaseExit()
        {
            StopAllCoroutines();
            CameraControl.o.stop();
        }


        public IEnumerator play()
        {
            Home.LetterContainer.o.setActiveLetters(false, x => x == PronouncingPhase.o.letter);
            CameraControl.o.zoom(cameraZoom, duration, zoomEase);
            CameraControl.o.move(PronouncingPhase.o.letter.transform.position, duration, moveEase);
            PronouncingPhase.o.playLetterAudio(PronouncingPhase.o.letter.letterId);
            PronouncingPhase.o.letter.transform.DOPunchScale(.2f.vector(), .2f);
            yield return new WaitForSeconds(duration);
        }


    }
}