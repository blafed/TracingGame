using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using DG.Tweening;
namespace KidLetters.WordPictureAnimations
{
    using UnityEngine;

    public class Grow : MonoBehaviour
    {
        public bool invert;
        public float duration = .5f;
        public Ease ease = Ease.OutBack;
        public CallbackObjects onFinish;

        private IEnumerator Start()
        {
            var s = transform.lossyScale;
            transform.localScale = !invert ? Vector3.zero : s;
            transform.DOScale(!invert ? s : Vector3.zero, duration).SetEase(ease);
            yield return new WaitForSeconds(duration);
            onFinish?.Invoke();
        }
    }
}