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



        List<Letter> letters = new();
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
                var c = Instantiate(LetterContainer.o.getLetter(letterId)).GetComponent<Letter>();
                nextPosition.x += c.relativeViewRect.size.x / 2;

                this.letters.Add(c);
                c.transform.parent = transform;
                c.transform.localPosition = nextPosition;
                // c.transform.localPosition += c.relativeViewRect.center.toVector3();
                nextPosition.x += c.relativeViewRect.size.x / 2 + spacing;


                if (c.text)
                {
                    c.text.color = glowOptions.normalColor;
                }
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
            phase.letter.text.DOFade(0, cameraOptions.duration);
            var focusPos = cameraOptions.offset - relLetterPos + letterPos + Vector2.right * width / 2f; ;
            CameraControl.o.move(focusPos, cameraOptions.duration, Ease.InOutQuad);
            CameraControl.o.zoom(cameraOptions.zoom, cameraOptions.duration);
            yield return new WaitForSeconds(cameraOptions.duration);
            phase.letter.text.alpha = 1;
            phase.letter.gameObject.SetActive(false);


            yield return new WaitForSeconds(paddingTimeStart);
            int i = 0;
            foreach (var x in this.letters)
            {
                if (i != 0)
                    yield return new WaitForSeconds(letterDelay);
                yield return playLetter(x);

                i++;
            }
            yield return new WaitForSeconds(paddingTimeEnd);

        }


        IEnumerator playLetter(Letter letter)
        {
            letter.gameObject.SetActive(true);
            letter.text.color = glowOptions.normalColor;
            letter.text.DOColor(glowOptions.targetColor, glowOptions.duration);
            letter.transform.DOScale(glowOptions.targetScale, glowOptions.duration);
            if (glowOptions.punchScale)
                letter.transform.DOPunchScale(.2f.vector(), .2f);
            yield return GeneralAudioPlayer.o.playWaitFinish(LetterList.o.getAudioClip(letter.letterId));
            letter.text.DOColor(glowOptions.normalColor, glowOptions.duration);

        }


        public IEnumerator fadeOutLetters(float duration, System.Predicate<Letter> except = null)
        {
            foreach (var x in letters)
            {
                if (except != null && except(x))
                    continue;
                x.text.DOFade(0, duration);
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
                x.text.color = fromColor;
                x.text.DOColor(targetColor, duration).SetEase(Ease.InOutQuad);
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
            phase.letter.text.color = Color.white;
            phase.letter.text.alpha = 1;
            phase.letter.text.DOKill();
            phase.letter.DOKill();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(width, 1));
        }
    }
}