using DG.Tweening;
namespace KidLetters.Tracing
{
    using UnityEngine;

    public class StageButtonContainer : Singleton<StageButtonContainer>
    {

        //fields
        [SerializeField] float enterDuration = .8f;
        [SerializeField] Vector2 cameraOffset = new Vector2(0, .7f);
        [SerializeField] float cameraZoom = 4;
        [SerializeField] Transform indicatingArrow;
        [SerializeField] FlowList<StageButton> playPatternButtons = new FlowList<StageButton>();


        private void Start()
        {
            gameObject.SetActive(false);
            TracingPhase.o.onStageChanged += onStageChanged;
        }

        void onStageChanged(TracingStage stage)
        {
            stage.onDone += refresh;
        }
        void refresh()
        {
            // playPatternButtons.iterate(playPatternButtons.count, x => x.component.refresh());
        }

        public void show()
        {
            gameObject.SetActive(true);
            transform.localScale = new Vector3();
            transform.DOScale(1, enterDuration).SetEase(Ease.OutBack);
            playPatternButtons.iterate(playPatternButtons.count, x => x.gameObject.SetActive(false));

            playPatternButtons.iterate(TracingPhase.o.tracingStages.Length, x =>
            {
                x.gameObject.SetActive(true);
                x.component.init(x.iterationIndex);
            });
            CameraControl.o.move(getFocusPosition());
            CameraControl.o.zoom(cameraZoom);
            refresh();
            hideIndicating();
        }
        public void hide()
        {
            gameObject.SetActive(false);
        }
        public Vector2 getFocusPosition()
        {
            return TracingPhase.o.letter.transform.position + cameraOffset.toVector3();
        }

        public void setIndicatingPosition(Vector2 position, Vector2? direction = default)
        {
            indicatingArrow.transform.DOMove(position, .5f);
            if (!direction.HasValue)
                direction = Vector2.down;
            indicatingArrow.transform.up = -direction.Value;
        }
        public void showIndicating()
        {
            if (!indicatingArrow.gameObject.activeSelf)
            {
                indicatingArrow.gameObject.SetActive(true);
                indicatingArrow.DOScale(1, .25f);
            }
        }
        public void hideIndicating()
        {
            indicatingArrow.DOScale(0, .25f).OnComplete(() =>
            {
                indicatingArrow.gameObject.SetActive(false);
            });
        }
    }
}