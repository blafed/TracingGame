using System.Collections.Generic;
using UnityEngine;

namespace KidLetters
{
    /// <summary>
    /// Contains all the letter prefabs
    /// </summary>
    [CreateAssetMenu(fileName = "LetterPrefabContainer", menuName = "KidLetters/LetterPrefabContainer", order = 0)]
    public class LetterPrefabContainer : Config<LetterPrefabContainer>
    {
        public List<GameObject> letterPrefabs = new List<GameObject>();



        public GameObject getLetterPrefab(int letterId)
        {
            foreach (var x in letterPrefabs)
            {
                var n = x.name;
                if (n.Length == 1)
                {
                    if (n[0] == LetterUtility.letterToChar(letterId))
                    {
                        return x;
                    }
                }
            }
            return null;
        }
    }
}