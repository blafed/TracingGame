using UnityEngine;
using DG.Tweening;
namespace KidLetters
{
    public class WordArtAnimation : MonoBehaviour
    {
        //will be set by animation player
        public float enterDuration { get; set; }
        //will be set by animation player
        public float exitDuration { get; set; }

        public virtual float duration => 1.5f;

        protected bool animationBegan { get; set; }
        /// <summary>
        /// If true, onTransition will be called instead of the default transition
        /// </summary>
        public virtual bool customTransition => false;

        /// <summary>
        /// Custom begining transition with a callback
        /// </summary>
        /// <param name="onTransitionComplete">callback should be called after the transition ends</param>
        public virtual void onEnter()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, enterDuration).SetEase(Ease.OutBack);
        }
        public virtual void onExit()
        {
            transform.DOScale(Vector3.zero, exitDuration).SetEase(Ease.InBack);
        }
        /// <summary>
        /// Plays the actual animation
        /// </summary>
        public virtual void playAnimation()
        {
            animationBegan = true;

            var tweenManager = GetComponent<DOTweenVisualManager>();
            if (tweenManager != null)
                tweenManager.enabled = true;

            foreach (var x in GetComponentsInChildren<Animation>())
            {
                x.Play();
            }
        }

    }

}