using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;


namespace KidLetters.Tracing
{
    public class MinorStar : MonoBehaviour
    {
        [SerializeField] Image image;
        [SerializeField] Color initialColor;
        [SerializeField] Color doneColor;



        bool wasDone = false;


        public void setDone(bool value)
        {
            if (value)
            {
                image.color = doneColor;
                if (!wasDone)
                    transform.DOPunchScale(.2f.vector(), .2f);
            }
            else
                image.color = initialColor;
            wasDone = value;
        }

        private void Start()
        {
            image.color = initialColor;
        }





    }
}