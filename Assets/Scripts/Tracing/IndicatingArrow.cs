using DG.Tweening;
using UnityEngine;

namespace KidLetters.Tracing
{
    public class IndicatingArrow : Singleton<IndicatingArrow>
    {
        [SerializeField] float moveDuration = .5f;
        [SerializeField] Ease moveEase;
        public void move(Vector2 v)
        {
            transform.DOMove(v, moveDuration).SetEase(moveEase);
        }
    }
}