using DG.Tweening;
namespace KidLetters.Tracing
{
    using UnityEngine;

    public class WalkingStar : MonoBehaviour
    {
        private void Start()
        {
            transform.localScale = new Vector3();
            transform.DOScale(1, .3f).SetEase(Ease.OutBack);
        }
    }
}