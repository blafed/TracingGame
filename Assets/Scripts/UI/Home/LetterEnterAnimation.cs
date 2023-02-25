using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
public class LetterEnterAnimation : MonoBehaviour
{

    public static LetterEnterAnimation o { get; private set; }



    [System.Serializable]
    class InitialStage
    {
        public float duration = .5f;
        public float audioRepeatDelay = .5f;
        public float zoom = 5;
        public Ease zoomEase = Ease.InBack;
        public Ease moveEase = Ease.OutQuad;
    }
    [System.Serializable]
    class LettersStage
    {
        public float zoom = 8;
        public float delay = .5f;
        public float fadeDuration = .5f;
        public float delayBetweenLetters = .5f;

        public Ease zoomEase = Ease.OutBack;
    }

    [System.Serializable]
    class FullWordStage
    {
        public float zoom = 10;
        public float delay = .7f;
        public float picturePunch = .2f;
        public float audioRepeatDelay = .5f;

        public Ease pictureScaleEase = Ease.OutBack;
    }


    [System.Serializable]
    class TerminateStage
    {
        public float delay = 1;
        public float duration = 1;
        public float zoom = 5;
    }



    [SerializeField] InitialStage initialFocus;
    [SerializeField] LettersStage otherLetters;

    [SerializeField] FullWordStage fullWord;
    [SerializeField] TerminateStage terminate;







    [SerializeField]
    Color highlightColor = Color.white, unhighlightColor = Color.grey;
    [Space]
    [SerializeField] TextMeshPro wordText;
    [SerializeField] SpriteRenderer picture;
    [SerializeField] AudioSource audioSource;

    Transform pictureContainer => picture.transform.parent;

    public event System.Action onFinish;



    List<Tween> tweens = new List<Tween>();

    private void Awake()
    {
        o = this;

        pictureContainer.gameObject.SetActive(false);
        wordText.gameObject.SetActive(false);
    }


    IEnumerator playAudioClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();

        yield return new WaitForSeconds(clip.length);
    }

    public void play(Letter l, WordInfo word)
    {
        StartCoroutine(playCycle(l, word));
    }
    IEnumerator playCycle(Letter l, WordInfo word)
    {
        LetterContainer.o.hideAll(x => x == l);
        transform.position = l.transform.position;
        var cam = CameraControl.o;
        cam.zoom(initialFocus.zoom, initialFocus.duration).SetEase(initialFocus.zoomEase).add(tweens);
        cam.move(l.transform.position, initialFocus.duration).SetEase(initialFocus.moveEase).add(tweens);

        yield return new WaitForSeconds(initialFocus.duration);
        yield return playAudioClip(LetterList.o.getAudioClip(l.letterId));
        yield return new WaitForSeconds(initialFocus.audioRepeatDelay);
        yield return playAudioClip(LetterList.o.getAudioClip(l.letterId));
        cam.zoom(otherLetters.zoom, otherLetters.delay).SetEase(otherLetters.zoomEase).add(tweens);
        cam.move(wordText.transform.position, otherLetters.delay).SetEase(Ease.OutQuad).add(tweens);



        yield return new WaitForSeconds(otherLetters.delay);
        wordText.gameObject.SetActive(true);
        wordText.alpha = 0;
        wordText.text = word.word;
        wordText.color = unhighlightColor;
        wordText.alpha = 0;
        l.text.DOFade(0, otherLetters.fadeDuration).add(tweens);

        yield return wordText.DOFade(1, otherLetters.fadeDuration).add(tweens).WaitForCompletion();
        wordText.color = Color.white;
        for (int i = 0; i < word.letterCount; i++)
        {
            wordText.text = TextHelper.makeColoredString(word.word, i, 1, unhighlightColor, highlightColor);
            var letterId = word.getLetterId(i);
            var clip = LetterList.o.getAudioClip(letterId);

            yield return playAudioClip(clip);
            yield return new WaitForSeconds(otherLetters.delayBetweenLetters);
        }
        wordText.text = word.word;
        wordText.color = Color.white;
        // wordText.DOColor(highlightColor, .5f);
        picture.sprite = word.picture;
        pictureContainer.gameObject.SetActive(true);
        pictureContainer.localScale = default;
        pictureContainer.DOScale(1f.vector(), fullWord.delay).SetEase(fullWord.pictureScaleEase).add(tweens);
        cam.zoom(fullWord.zoom, fullWord.delay).add(tweens);
        yield return new WaitForSeconds(fullWord.delay);
        if (word.clip)
        {
            yield return playAudioClip(word.clip);
            yield return new WaitForSeconds(fullWord.audioRepeatDelay);
            yield return playAudioClip(word.clip);
        }

        // yield return pictureContainer.DOPunchScale(.2f.vector(), fullWord.picturePunch);
        yield return new WaitForSeconds(terminate.delay);
        pictureContainer.DOScale(0, terminate.duration).add(tweens);
        wordText.DOFade(0, terminate.duration).add(tweens);
        cam.zoom(terminate.zoom, terminate.duration).add(tweens);
        yield return new WaitForSeconds(terminate.duration);
        l.text.DOFade(1, terminate.duration).add(tweens);
        cam.move(l.transform.position, terminate.duration).add(tweens);
        yield return new WaitForSeconds(terminate.duration);
        onFinish?.Invoke();

    }

    public void stop()
    {
        foreach (var x in tweens)
        {
            x.Kill();
        }
        StopAllCoroutines();
    }
    public void complete()
    {
        stop();
        onFinish?.Invoke();
    }

}