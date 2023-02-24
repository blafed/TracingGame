using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LetterContainer : MonoBehaviour
{
    public static LetterContainer o;

    [SerializeField] float cameraSize = 15;

    new Camera camera => Extensions2.mainCamera;


    [System.Serializable]
    class LeavingPosition
    {
        public float distance = 20;
        public float duration = 1;
        public Ease ease = Ease.InQuad;
    }

    [SerializeField]
    LeavingPosition leavingPosition = new LeavingPosition();


    // [SerializeField] float columnHeight = 7;
    // [SerializeField] int columnCount = 5;

    // [SerializeField] Letter[] allLetters;
    // public FlowList<LetterItem> letters;


    [System.Serializable]
    class LetterData
    {
        public Letter letter;
        public Vector2 leavingPosition;
        public Vector2 startPosition;
    }

    List<LetterData> letters = new();



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
                startPosition = x.transform.localPosition,
                leavingPosition = x.transform.localPosition.normalized * leavingPosition.distance
            });
        }



        // System.Array.Sort(allLetters, (a, b) => a.letterId.CompareTo(b.letterId));

        // int i = 0;
        // Vector2 p = new Vector2();
        // var rowCount = Mathf.Ceil(allLetters.Length / (float)columnCount);


        // foreach (var x in allLetters)
        // {
        //     var leftCount = allLetters.Length - i;
        //     var letter = Instantiate(x.gameObject).GetComponent<Letter>();
        //     var collider = letter.gameObject.AddComponent<BoxCollider>();
        //     letter.onClick += () => onLetterClick(letter);
        //     letter.transform.parent = transform;


        //     var inThisRow = i % columnCount;
        //     var rowSize = Mathf.Min(leftCount, columnCount);

        //     var f = inThisRow / (float)rowSize + .5f;

        //     i++;
        // }
    }

    Letter selectedLetter;


    public void clean()
    {
        selectedLetter = null;
    }

    private void onLetterClick(Letter letter)
    {
        if (selectedLetter)
            return;
        letter.transform.DOPunchScale(.2f.vector(), .2f);
        var word = WordList.o.getRandomContains(letter.letterId);
        if (word == null)
        {
            Debug.LogError("No Word for this letter  " + LetterUtility.letterToString(letter.letterId));
            return;
        }
        selectedLetter = letter;
        LetterEnterAnimation.o.play(letter, word);
    }


    public void removeAllLetters(System.Predicate<Letter> exclude = null)
    {
        foreach (var x in letters)
        {
            if (exclude != null && exclude(x.letter))
                continue;
            x.letter.transform.localPosition = x.startPosition;
            x.letter.transform.DOLocalMove(x.leavingPosition, leavingPosition.duration)
            .SetEase(leavingPosition.ease);
        }
    }
    public void showAllLetters()
    {
        foreach (var x in letters)
        {
            x.letter.transform.localPosition = x.leavingPosition;
            x.letter.transform.DOLocalMove(x.startPosition, leavingPosition.duration)
            .SetEase(leavingPosition.ease);
        }
    }





}