using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace KidLetters.Pronouncing
{
    public class WordPicturePlayer : Singleton<WordPicturePlayer>
    {
        [SerializeField] Transform container;
        [SerializeField] float containerScale = 2;
        // [SerializeField] float scaleDuration = .5f;

        [SerializeField]
        WordPictureAnimation.Meta defaultPlayingMeta = new WordPictureAnimation.Meta
        {
            duration = 1,
            audioPlayDelay = .4f
        };
        [SerializeField] float endPaddingTime = 1;
        [SerializeField] float startScaleDuration = .5f;
        [SerializeField] float audioPlayDelay = .5f;
        [SerializeField] float enterDuration = 1;
        [SerializeField] float exitDuration = 1;
        [SerializeField] float stayDuration = 1;
        [SerializeField] float exitTime = 1;
        // [SerializeField] Vector2 offset = new Vector2(0, 6);


        GameObject wordPicture;

        private void Start()
        {
            PronouncingPhase.o.onExitEvent += clean;

        }

        private void clean()
        {
            Destroy(wordPicture);
            StopAllCoroutines();
        }

        GameObject _createdWordPictureGameObject;
        WordArtAnimation createWordPictureAnimation(WordInfo wordInfo)
        {
            if (!wordInfo.prefab)
                return null;
            var p = wordPicture = Instantiate(wordInfo.prefab);
            p.transform.parent = container;
            p.transform.localScale = Vector3.one;
            p.transform.localPosition = new Vector3();
            _createdWordPictureGameObject = p;
            var a = p.GetComponent<WordArtAnimation>();

            a.enterDuration = enterDuration;
            a.exitDuration = exitDuration;
            return a;
        }
        WordArtAnimation createWordPictureAnimation()
        {
            var wordInfo = PronouncingPhase.o.wordInfo;
            if (!wordInfo.prefab && wordInfo.picture)
            {
                var catWord = WordList.o.findWord("cat");

                var catAnimation = createWordPictureAnimation(catWord);
                var spriteRenderer = _createdWordPictureGameObject.GetComponentInChildren<SpriteRenderer>();
                spriteRenderer.sprite = wordInfo.picture;
                return catAnimation;
            }
            return createWordPictureAnimation(wordInfo);
        }
        public IEnumerator play()
        {


            var wordInfo = PronouncingPhase.o.wordInfo;


            transform.position = CameraControl.o.transform.position;


            var animation = createWordPictureAnimation();
            animation.onEnter();
            //wait for enter animation
            yield return new WaitForSeconds(enterDuration);

            // var m = animation && animation.overrideMeta ? animation.meta : defaultPlayingMeta;
            StartCoroutine(WordView.o.setHighlightAll(true));
            animation.playAnimation();


            //wait a delay before playing the audio
            yield return new WaitForSeconds(audioPlayDelay);

            GeneralAudioPlayer.o.play(wordInfo.clip);
            //wait for the animation to end
            yield return new WaitForSeconds(animation.duration - audioPlayDelay);
            //stay with the art for a while
            yield return new WaitForSeconds(stayDuration);
            animation.onExit();
            yield return new WaitForSeconds(exitDuration);

            // yield return container.transform.DOScale(Vector3.one, scaleDuration).WaitForCompletion();

        }
    }
}