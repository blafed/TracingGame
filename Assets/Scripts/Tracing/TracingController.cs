using System.Collections;
using UnityEngine;
using DG.Tweening;
namespace KidLetters.Tracing
{
    public class TracingController : Singleton<TracingController>
    {
        static TracingPhase phase => TracingPhase.o;
        static LetterFiller letter => phase.letter;


        [System.Serializable]
        class BeginStage
        {
            public float delay;
            public AudioSource audio;
        }

        [System.Serializable]
        class ThinLetter
        {
            public AudioSource audio;
        }
        [System.Serializable]
        class FadeLetter
        {
            public AudioSource audio;
        }


        [SerializeField] BeginStage beginStage = new BeginStage();
        [SerializeField] ThinLetter thinLetter = new ThinLetter();
        [SerializeField] FadeLetter fadeLetter = new FadeLetter();

        IEnumerator spawnEdgePoints()
        {

            EdgePointDealer.o.spawnEdgePoints(TracingConfig.o.edgePointPrefab, letter);
            yield return new WaitForSeconds(EdgePointDealer.o.estimatedWaitTime);

        }
        public IEnumerator play()
        {



            // yield return letter.doColor(Backgrounds.o.getBackgroundColor(), .5f).WaitForCompletion();
            // letter.setTextEnabled(false);
            // Backgrounds.o.changeRandomly(BackgroundsList.forTracing);

            for (int i = 0; i < phase.stageInfos.Count; i++)
            {
                if (beginStage.audio)
                    beginStage.audio.Play();
                yield return new WaitForSeconds(beginStage.delay);
                var stageInfo = phase.stageInfos[i];


                if (stageInfo.showThinLetter)
                {
                    if (thinLetter.audio)
                        thinLetter.audio.Play();
                    phase.letter.setNormalWidth();
                    phase.letter.setEnabled(true);
                    phase.letter.setColor(Color.white);
                    yield return letter.doWidth(.2f, .5f).WaitForCompletion();
                }
                else
                {
                    if (fadeLetter.audio)
                        fadeLetter.audio.Play();
                    yield return letter.doColor(Backgrounds.o.getBackgroundColor(), .5f).WaitForCompletion();
                    // phase.letter.setEnabled(false);
                }


                if (!stageInfo.disableEdgePoints)
                {
                    yield return spawnEdgePoints();
                    yield return new WaitForSeconds(.5f);
                }
                yield return stageCycle(i);
                letter.setNormalWidth();
                letter.setColor(Color.white);
                if (!stageInfo.disableEdgePoints)
                {
                    EdgePointDealer.o.clearEdgePointsTween();
                    yield return new WaitForSeconds(.5f);
                }

            }
            EdgePointDealer.o.clearEdgePointsTween();
            yield return new WaitForSeconds(1f);
            letter.setNormalWidth();
            letter.setColor(Color.white);
        }
        IEnumerator stageCycle(int stageIndex)
        {
            phase.playStage(stageIndex, null);
            var stage = phase.currentStage;

            stage.onStartSegment += onStartSegment;
            stage.onEndSegment += onEndSegment;
            stage.onWrongTracing += onWrongTracing;

            EdgePointDealer.o.onStartSegment(0);
            yield return stage.play();
            yield return new WaitForSeconds(1f);

            Destroy(stage.gameObject);

        }

        void onStartSegment()
        {
            var stage = phase.currentStage;
            if (stage.state == TracingState.tracing && !stage.info.autoTracing && !stage.info.disableIndicating)
            {
                if (stage.currentSegment.isDot)
                    IndicatingDot.o.showOnPattern(stage.currentSegment);
                else
                    IndicatingArrow.o.showOnPattern(stage.currentSegment);


            }
            else
            {
                IndicatingArrow.o.hide();
                IndicatingDot.o.hide();
            }
        }

        void onEndSegment()
        {
            IndicatingArrow.o.hide();
            IndicatingDot.o.hide();
        }
        void onWrongTracing()
        {
            EdgePointDealer.o.onWrongSegment(phase.currentStage.segmentIndex);
        }
    }

}