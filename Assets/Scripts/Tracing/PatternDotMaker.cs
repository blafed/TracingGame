using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
namespace KidLetters.Tracing
{

    [System.Obsolete]
    public class PatternDotMaker : Singleton<PatternDotMaker>
    {
        public Material dotMaterial;
        [SerializeField] GameObject maskPrefab;







        public GameObject applyOn(Pattern pattern)
        {
            var radius = pattern.dotRadius;
            if (pattern is SplinePattern splinePattern)
            {
                var renderer = splinePattern.GetComponent<SpriteShapeRenderer>();
                for (int i = 0; i < renderer.materials.Length; i++)
                {
                    renderer.materials[i] = dotMaterial;
                }

                renderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            }

            else if (pattern is ObjectPattern objectPattern)
            {
            }
            var m = Instantiate(maskPrefab, pattern.transform, false);




            return m;
        }
    }
}