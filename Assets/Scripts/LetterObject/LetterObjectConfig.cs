using UnityEngine;

namespace KidLetters
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "LetterObjectConfig", menuName = "KidLetters/LetterObjectConfig", order = 0)]
    public class LetterObjectConfig : Config<LetterObjectConfig>
    {
        public float width = .5f;
        public GameObject standardLetterFillerPrefab;
        public GameObject blankLetterFillerPrefab;
    }
}