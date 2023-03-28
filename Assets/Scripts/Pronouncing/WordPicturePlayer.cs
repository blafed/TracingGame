using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace KidLetters.Pronouncing
{
    public class WordPicturePlayer : Singleton<WordPicturePlayer>
    {
        [SerializeField] Transform container;
        // [SerializeField] float scaleDuration = .5f;

        [SerializeField]
        WordPictureAnimation.Meta defaultPlayingMeta = new WordPictureAnimation.Meta
        {
            duration = 1,
            audioPlayDelay = .4f
        };
        [SerializeField] float endPaddingTime = 1;
        [SerializeField] float startScaleDuration = .5f;
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
        WordPictureAnimation createWordPictureAnimation(WordInfo wordInfo)
        {
            if (!wordInfo.prefab)
                return null;
            var p = wordPicture = Instantiate(wordInfo.prefab);
            p.transform.parent = container;
            p.transform.localScale = Vector3.one;
            p.transform.localPosition = new Vector3();
            _createdWordPictureGameObject = p;
            var a = p.GetComponent<WordPictureAnimation>();
            return a;
        }
        WordPictureAnimation createWordPictureAnimation()
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
            // container.localScale = Vector3.zero;
            // container.DOScale(1, startScaleDuration);
            // yield return new WaitForSeconds(startScaleDuration);

            var m = animation && animation.overrideMeta ? animation.meta : defaultPlayingMeta;
            StartCoroutine(WordView.o.setHighlightAll(true));




            yield return new WaitForSeconds(m.audioPlayDelay);

            GeneralAudioPlayer.o.play(wordInfo.clip);
            yield return new WaitUntil(() => animation.isTerminated);
            yield return new WaitForSeconds(endPaddingTime);
            yield return container.DOScale(0, exitTime).WaitForCompletion();

            // yield return container.transform.DOScale(Vector3.one, scaleDuration).WaitForCompletion();

        }
    }
}