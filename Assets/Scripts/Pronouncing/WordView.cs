using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace KidLetters.Pronouncing
{
    using Home;
    public class WordView : Singleton<WordView>
    {
        [System.Serializable]
        class CameraOptions
        {
            public float zoom = 10;
            public Vector2 offset = new Vector2(0, 6);
            public float duration = .5f;
            public float moveUnderDuration = .3f;
        }
        [System.Serializable]
        class GlowOptions
        {
            public Color targetColor = Color.white;
            public float targetScale = 1.3f;
            [Space]
            public Color normalColor = Color.grey;
            public float normalScale = 1;
            public float duration = .5f;
            public bool punchScale = true;
        }
        [SerializeField] float spacing = .3f;
        [SerializeField] float letterDelay = .5f;
        [SerializeField] float paddingTimeStart = 1f;
        [SerializeField] float paddingTimeEnd = 1.5f;
        [SerializeField]
        GlowOptions glowOptions = new GlowOptions();
        [SerializeField]
        CameraOptions cameraOptions = new CameraOptions();


        PronouncingPhase phase => PronouncingPhase.o;



        List<LetterFiller> letters = new();
        float width;

        private void Start()
        {
            PronouncingPhase.o.onExitEvent += clean;
        }

        /// <summary>
        /// create the letters of word
        /// </summary>
        /// <returns>the width of word</returns>
        float createLetters()
        {
            WordInfo wordInfo = phase.wordInfo;
            var indexOfTargetLetter = wordInfo.indexOfLetter(phase.letterId);
            Vector2 nextPosition = new Vector2();


            for (int i = 0; i < wordInfo.letterCount; i++)
            {
                // var dif = i - indexOfTargetLetter;
                var letterId = wordInfo.getLetterId(i);
                var c = LetterFiller.createStandardFiller(LetterContainer.o.getLetter(letterId));
                nextPosition.x += c.relativeViewRect.size.x / 2;

                this.letters.Add(c);
                c.transform.parent = transform;
                c.transform.localPosition = nextPosition;
                // c.transform.localPosition += c.relativeViewRect.center.toVector3();
                nextPosition.x += c.relativeViewRect.size.x / 2 + spacing;


                try
                {
                    c.setColor(glowOptions.normalColor);
                }
                catch { }
                c.transform.localScale = glowOptions.normalScale.vector();
                c.gameObject.SetActive(indexOfTargetLetter == i);


            }
            return width = nextPosition.x - spacing;
        }


        public IEnumerator play()
        {
            createLetters();

            WordInfo wordInfo = phase.wordInfo;
            Vector2 letterPos = phase.letter.transform.position;
            var letterIndex = wordInfo.indexOfLetter(phase.letterId);
            var relLetterPos = (Vector2)letters[letterIndex].transform.localPosition;
            transform.position = letterPos - relLetterPos;

            // phase.letter.text.DOColor(glowOptions.normalColor, glowOptions.duration);
            yield return new WaitForSeconds(paddingTimeStart);
            phase.letter.doFade(0, cameraOptions.duration);
            var focusPos = -relLetterPos + letterPos + Vector2.right * width / 2f;
            CameraControl.o.move(focusPos, cameraOptions.duration, Ease.InOutQuad);
            CameraControl.o.zoom(cameraOptions.zoom, cameraOptions.duration);
            yield return new WaitForSeconds(cameraOptions.duration);

            // letters[letterIndex].setColor(glowOptions.targetColor);
            // letters[letterIndex].doColor(glowOptions.normalColor, glowOptions.duration);
            phase.letter.setAlpha(1);
            phase.letter.gameObject.SetActive(false);





            yield return new WaitForSeconds(paddingTimeStart);


            var actualI = 0;
            for (int i = 0; i < this.letters.Count; i++)
            {
                var x = this.letters[i];
                if (i != 0)
                    yield return new WaitForSeconds(letterDelay);

                var letterId = wordInfo.getLetterId(i);

                var playAudio = wordInfo.spellingClips[actualI];
                if (wordInfo.isDigraph(i))
                {
                    StartCoroutine(playLetter(x, null, playAudio.length));
                    yield return playLetter(this.letters[i + 1], playAudio);
                    i++;
                }
                else
                    yield return playLetter(x, playAudio);

                actualI++;
            }
            yield return new WaitForSeconds(paddingTimeEnd);
            yield return CameraControl.o.move(cameraOptions.offset + focusPos, cameraOptions.moveUnderDuration, Ease.InOutQuad).WaitForCompletion();

        }

        IEnumerator playLetter(LetterFiller letter, AudioClip playAudio, float customWait = 0)
        {
            letter.gameObject.SetActive(true);
            letter.setColor(glowOptions.normalColor);
            letter.doColor(glowOptions.targetColor, glowOptions.duration);
            letter.transform.DOScale(glowOptions.targetScale, glowOptions.duration);
            if (glowOptions.punchScale)
                letter.transform.DOPunchScale(.2f.vector(), .2f);
            // yield return GeneralAudioPlayer.o.playWaitFinish(LetterList.o.getAudioClip(letter.letterId));
            var wordInfo = PronouncingPhase.o.wordInfo;
            if (playAudio)
                yield return GeneralAudioPlayer.o.playWaitFinish(playAudio);
            if (customWait > 0)
                yield return new WaitForSeconds(customWait);
            letter.doColor(glowOptions.normalColor, glowOptions.duration);

        }


        public IEnumerator fadeOutLetters(float duration, System.Predicate<LetterFiller> except = null)
        {
            foreach (var x in letters)
            {
                if (except != null && except(x))
                    continue;
                x.doColor(Backgrounds.o.getBackgroundColor(), duration);
                // x.doFade(0, duration);
            }
            yield return new WaitForSeconds(duration);
        }

        public IEnumerator setHighlightAll(bool highlighted, float duration = -1)
        {
            if (duration < 0)
                duration = glowOptions.duration;
            var fromColor = highlighted ? glowOptions.normalColor : glowOptions.targetColor;
            var targetColor = highlighted ? glowOptions.targetColor : glowOptions.normalColor;
            foreach (var x in letters)
            {
                x.setColor(fromColor);
                x.doColor(targetColor, duration).SetEase(Ease.InOutQuad);
                x.transform.DOPunchScale(.2f.vector(), .2f);
            }
            yield return new WaitForSeconds(duration);
        }


        void clean()
        {
            foreach (var x in letters)
            {
                Destroy(x.gameObject);
            }
            letters.Clear();

            StopAllCoroutines();

            phase.letter.gameObject.SetActive(true);
            phase.letter.setColor(Color.white);
            phase.letter.setAlpha(1);
            phase.letter.doKill();
            phase.letter.DOKill();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(width, 1));
        }
    }
}