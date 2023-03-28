using TMPro;
using UnityEngine;

namespace KidLetters.Pronouncing.Developing
{
    public class WordPicturePreviewer : MonoBehaviour
    {

        [SerializeField] TextMeshProUGUI wordText;
        GameObject _createdWordPictureGameObject;
        Transform container => transform;
        WordInfo wordInfo;


        WordPictureAnimation createWordPictureAnimation(WordInfo wordInfo)
        {
            if (!wordInfo.prefab)
                return null;
            var p = Instantiate(wordInfo.prefab);
            p.transform.parent = container;
            p.transform.localScale = Vector3.one;
            p.transform.localPosition = new Vector3();
            _createdWordPictureGameObject = p;
            var a = p.GetComponent<WordPictureAnimation>();
            return a;
        }
        WordPictureAnimation createWordPictureAnimation()
        {
            var wordInfo = this.wordInfo;
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

        public void preview(WordInfo word)
        {
            if (_createdWordPictureGameObject)
                Destroy(_createdWordPictureGameObject);
            this.wordInfo = word;
            createWordPictureAnimation();
            wordText.text = word.word;
        }


        private void OnGUI()
        {
            foreach (var x in WordList.o.wordList)
            {
                if (GUILayout.Button(x.word))
                {
                    preview(x);
                }
            }
        }
    }
}