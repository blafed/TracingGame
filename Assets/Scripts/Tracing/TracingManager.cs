using System.Collections.Generic;
using UnityEngine;

namespace KidLetters
{
    public class TracingManager : Singleton<TracingManager>
    {

        Letter optLetter;
        LetterFiller filler;

        void setOptainingLetter(Letter letter)
        {
            if (letter != optLetter)
            {
                optLetter = letter;
                filler = letter.createBasedFiller(LetterObjectConfig.o.blankLetterFillerPrefab);
            }
        }
        public List<Transform> spawnEdgePoints(GameObject prefab, LetterSegmentFiller segment)
        {
            var result = new List<Transform>(2);

            var obj = Instantiate(prefab);
            obj.transform.position = segment.startPoint;
            obj.transform.parent = segment.transform;
            result.Add(obj.transform);

            obj = Instantiate(prefab);
            obj.transform.position = segment.endPoint;
            obj.transform.parent = segment.transform;
            result.Add(obj.transform);



            return result;
        }
        public List<Transform> spawnEdgePoints(GameObject edgePointPrefab, Letter letter)
        {
            List<Transform> result = new List<Transform>(2 * letter.segmentCount);

            setOptainingLetter(letter);

            for (int i = 0; i < filler.segmentCount; i++)
            {
                var s = filler[i];
                var res = spawnEdgePoints(edgePointPrefab, s);
                result.AddRange(res);
            }
            return result;
        }

    }

}