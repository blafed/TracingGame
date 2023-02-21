using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class LetterItem : MonoBehaviour
{
    [SerializeField] float wordIncreaseLetterTime = .5f;
    [SerializeField] TextMeshProUGUI letterText;
    [SerializeField] FlowList<TextMeshProUGUI> wordTexts;
    Button _button;
    public Button button => this.getComponentCached(ref _button);
    LayoutElement _element;
    LayoutElement layoutElement => this.getComponentCached(ref _element);
    public int letterId { get; private set; }

    public void init(int letter)
    {
        layoutElement.ignoreLayout = false;
        this.letterId = letter;
        letterText.text = LetterUtility.letterToString(letter);
        button.onClick.AddListener(() =>
        {
            button.interactable = false;
            EnterGamePhase.o.enter(letter, this);
        });
    }


    public void setWord(string text)
    {
        var index = -1;
        for (int i = 0; i < text.Length; i++)
        {
            if (LetterUtility.charToLetterId(text[i]) == letterId)
            {
                index = i;
                break;
            }
        }
        if (index < 0)
        {
            Debug.LogError("Invalid word " + text + " for letter " + LetterUtility.letterToString(letterId), gameObject);
            return;
        }
        var isFirst = index == 0;
        var isLast = index == text.Length - 1;
        var isMid = !isFirst && !isLast;


        wordTexts.iterate(wordTexts.count, x => x.component.text = "");
        wordTexts.iterate(text.Length, x =>
        {
            if (x.iterationIndex != index)
            {
                var anchoredPosition = x.rectTransform.anchoredPosition;
                x.rectTransform.anchoredPosition = anchoredPosition.setX(0);
                x.rectTransform.DOAnchorPosX((x.iterationIndex - index) * 40, .5f).SetDelay(x.iterationIndex * wordIncreaseLetterTime);
                x.component.text = text[x.iterationIndex].ToString();
            }
        });


    }
}