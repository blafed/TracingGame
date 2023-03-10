using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LetterContainer : MonoBehaviour
{
    public static LetterContainer o;

    [System.Serializable]
    class LeavingPosition
    {
        public float distance = 20;
        public float duration = 1;
        public Ease ease = Ease.InQuad;
    }
    [System.Serializable]
    class LetterData
    {
        public Letter letter;
        public Vector2 leavingPosition;
        public Vector2 startPosition;
    }

    [SerializeField]
    LeavingPosition leavingPosition = new LeavingPosition();
    [SerializeField] float cameraSize = 15;


    List<LetterData> letters = new();


    new Camera camera => Extensions2.mainCamera;

    private void Awake()
    {
        o = this;
        var letters = GetComponentsInChildren<Letter>();
        this.letters.Capacity = letters.Length;


        foreach (var x in letters)
        {
            x.onClick += () => onLetterClick(x);
            var collider = x.gameObject.AddComponent<BoxCollider>();
            collider.size = x.size.toVector3(1);
            collider.isTrigger = true;

            this.letters.Add(new LetterData
            {
                letter = x,
                startPosition = x.transform.position,
                leavingPosition = x.transform.position.normalized * leavingPosition.distance
            });
        }
    }

    Letter selectedLetter;


    public void clean()
    {
        selectedLetter = null;
    }

    private void onLetterClick(Letter letter)
    {
        if (LetterContainerPhase.o != Phase.current)
            return;
        if (!LetterContainerPhase.o.onSelectLetter(letter))
        {
            return;
        }
        letter.transform.DOPunchScale(.2f.vector(), .2f);

        return;
        var word = WordList.o.getRandomContains(letter.letterId);
        if (word == null)
        {
            Debug.LogError("No Word for this letter  " + LetterUtility.letterToString(letter.letterId));
            return;
        }
        selectedLetter = letter;
        LetterEnterAnimation.o.play(letter, word);
    }


    public void hideAllNoTween(System.Predicate<Letter> exclude = null)
    {
        foreach (var x in letters)
        {
            if (exclude != null && exclude(x.letter))
                continue;
            x.letter.gameObject.SetActive(false);
            // x.letter.transform.position = x.leavingPosition;
        }
    }
    public void showAllNoTween(System.Predicate<Letter> exclude = null)
    {
        foreach (var x in letters)
        {
            if (exclude != null && exclude(x.letter))
                continue;
            x.letter.gameObject.SetActive(true);
            // x.letter.transform.position = x.startPosition;
        }
    }
    public Tween hideAll(System.Predicate<Letter> exclude = null)
    {
        foreach (var x in letters)
        {
            if (exclude != null && exclude(x.letter))
                continue;
            x.letter.gameObject.SetActive(false);
            x.letter.transform.position = x.startPosition;
            x.letter.transform.DOMove(x.leavingPosition, leavingPosition.duration)
            .SetEase(leavingPosition.ease);
        }

        return DOTween.Sequence().PrependInterval(leavingPosition.duration);
    }
    public Tween showAll(System.Predicate<Letter> exclude = null)
    {
        foreach (var x in letters)
        {
            if (exclude != null && exclude(x.letter))
                continue;
            x.letter.gameObject.SetActive(true);
            x.letter.transform.position = x.leavingPosition;
            x.letter.transform.DOMoveCurvy(x.startPosition, leavingPosition.duration, 2f)
            .SetEase(leavingPosition.ease);
        }
        return DOTween.Sequence().PrependInterval(leavingPosition.duration);
    }

    public Tween adjustCamera()
    {
        return DOTween.Sequence().Join(
        CameraControl.o.move(transform.position)).Join(CameraControl.o.zoom(cameraSize));
    }



    public Letter findLetter(int letterId)
    {
        return letters.Find(x => x.letter.letterId == letterId).letter;
    }



}