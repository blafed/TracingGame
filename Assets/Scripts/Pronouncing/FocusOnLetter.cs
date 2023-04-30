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

        AudioSource audioSource;
        protected override void Awake()
        {
            base.Awake();
            audioSource = GetComponentInChildren<AudioSource>();
        }
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
            audioSource.Play();
            PronouncingPhase.o.letter.transform.DOPunchScale(.2f.vector(), .2f);
            CameraControl.o.move(PronouncingPhase.o.letter.transform.position, duration, moveEase);
            yield return CameraControl.o.zoom(cameraZoom, duration, zoomEase).WaitForCompletion();
            yield return new WaitForSeconds(.25f);
            PronouncingPhase.o.letter.transform.DOPunchScale(.2f.vector(), .2f);
            yield return GeneralAudioPlayer.o.playWaitFinish(LetterList.o.getAudioClip(PronouncingPhase.o.letterId));
            yield return new WaitForSeconds(.25f);
        }


    }
}