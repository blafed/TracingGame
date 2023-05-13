
using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace KidLetters.Tracing
{
    public class FocusOnLetter : Singleton<FocusOnLetter>
    {
        public Ease ease = Ease.Linear;
        public float duration = 1;
        public float cameraZoom = 5;


        public IEnumerator play()
        {
            var letter = TracingPhase.o.letter;
            letter.setColor(Color.white);
            letter.setAlpha(1);
            CameraControl.o.zoom(cameraZoom, duration, ease);
            CameraControl.o.move(TracingPhase.o.letter.transform.position, duration, ease);
            yield return new WaitForSeconds(duration);
            Backgrounds.o.changeRandomly(BackgroundsList.forTracing);
        }

    }
}