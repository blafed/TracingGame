using DG.Tweening;
using UnityEngine;

public class EnterGamePhase : Phase<EnterGamePhase>
{
    [SerializeField] float wordDuration = 3;
    public float targetCameraSize = 5;
    public float focusTime = 1f;
    public float initialGlowDelay = 1f;
    public float cameraRepeatlyDeltaSize = .5f;
    public float glowTimeRate = .5f;


    public int letterId { get; set; }
    public LetterItem letterItem { get; set; }
    WordInfo selectedWordInfo;

    new Camera camera;

    public void enter(int letterId, LetterItem letterItem)
    {
        this.letterItem = letterItem;
        change(o);
    }


    protected override void onEnter()
    {
        var originCameraZ = 0;
        camera
         = Camera.main;
        var seq = DOTween.Sequence();
        seq.Append(camera.DOOrthoSize(targetCameraSize, focusTime).SetEase(Ease.InBack))
        .Join(
        camera.transform.DOMove(letterItem.transform.position.setZ(originCameraZ), focusTime).SetEase(Ease.InCubic))
        .OnComplete(() =>
        {
            camera.DOOrthoSize(targetCameraSize - cameraRepeatlyDeltaSize, glowTimeRate)
            .SetEase(Ease.OutBack)
            .SetLoops(3, LoopType.Incremental).SetDelay(initialGlowDelay)
            .OnComplete(() => selectRandomWord());
        });
    }

    void selectRandomWord()
    {
        WordInfo word = WordList.o.getRandomContains(letterId);
        if (word == null)
        {
            Debug.LogError("Word not found for letter " + LetterUtility.letterToString(letterId));
            return;
        }
        selectedWordInfo = word;
        letterItem.setWord(word.word);
        Invoke("transit", wordDuration);
    }


    void transit()
    {
        GamePhase.o.letterId = this.letterId;
        GamePhase.o.origin = this.letterItem.transform.position;
        change(GamePhase.o);
    }


    protected override void onExit()
    {
    }
}