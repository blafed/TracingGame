using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KidLetters.Tracing
{
    public class IndicatingDotCircle : MonoBehaviour
    {
        SpriteRenderer[] renderers;
        float alpha;

        private void Awake()
        {
            renderers = GetComponentsInChildren<SpriteRenderer>();
            alpha = 1;

        }


        public Tween setAlphaTween(float targetAlpha, float duration)
        {
            return DOTween.To(() => this.alpha, x => setAlpha(x), targetAlpha, duration);


        }

        public void setAlpha(float a)
        {
            foreach (var x in renderers)
                x.color = x.color.alpha(a);
            alpha = a;
        }
    }

}