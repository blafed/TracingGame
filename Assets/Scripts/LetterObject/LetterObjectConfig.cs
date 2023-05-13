using UnityEngine;

namespace KidLetters
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "LetterObjectConfig", menuName = "KidLetters/LetterObjectConfig", order = 0)]
    public class LetterObjectConfig : Config<LetterObjectConfig>
    {
        [Tooltip("The width of the font of letter")]
        public float width = .5f;
        [Tooltip("For letters with a 'dot' like 'i' and 'j', this is the radius of the dot")]
        public float dotRadius = .4f;

        [Space]
        [Tooltip("The of letter font that is used to generat procedural letter font")]
        public GameObject standardLetterFillerPrefab;
        public GameObject blankLetterFillerPrefab;
    }
}