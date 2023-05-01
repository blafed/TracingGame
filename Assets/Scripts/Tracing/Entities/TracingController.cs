using System.Collections;
using UnityEngine;
using DG.Tweening;
namespace KidLetters.Tracing
{
    public class TracingController : Singleton<TracingController>
    {
        static TracingPhase phase => TracingPhase.o;
        static Letter letter => phase.letter;


        IEnumerator spawnEdgePoints()
        {
            EdgePointDealer.o.spawnEdgePoints(TracingConfig.o.edgePointPrefab, letter);
            yield return new WaitForSeconds(EdgePointDealer.o.estimatedWaitTime);

        }
        public IEnumerator play()
        {

            yield return spawnEdgePoints();
            yield return new WaitForSeconds(.5f);

            // yield return letter.doColor(Backgrounds.o.getBackgroundColor(), .5f).WaitForCompletion();
            // letter.setTextEnabled(false);
            phase.playStage(0, null);
            // Backgrounds.o.changeRandomly(BackgroundsList.forTracing);

            for (int i = 0; i < phase.tracingStages.Length; i++)
            {
                var stageInfo = phase.tracingStages[i];
                if (stageInfo.showThinLetter)
                {
                    phase.letter.setNormalWidth();
                    phase.letter.setTextEnabled(true);
                    phase.letter.setColor(Color.white);
                    yield return letter.doWidth(.2f, .5f).WaitForCompletion();
                }
                else
                {
                    phase.letter.setTextEnabled(false);
                }
                if (i != 0)
                {
                    EdgePointDealer.o.clearEdgePointsTween();
                    yield return new WaitForSeconds(.5f);
                    yield return spawnEdgePoints();
                    yield return new WaitForSeconds(.5f);
                }
                phase.playStage(i, null);
                //waiting for Start() function to be called on TracingStage script, by waiting for the next frame
                yield return new WaitForFixedUpdate();
                yield return stageCycle();
            }
            EdgePointDealer.o.clearEdgePointsTween();
            yield return new WaitForSeconds(1f);
            letter.setNormalWidth();
        }
        IEnumerator stageCycle()
        {
            var stage = phase.currentStage;


            for (int i = 0; i < stage.segmentCount; i++)
            {
                var segmentIndex = i;
                yield return new WaitForSeconds(.5f);
                EdgePointDealer.o.onStartSegment(segmentIndex);
                yield return new WaitUntil(() => stage.segmentIndex != i);
                EdgePointDealer.o.onEndSegment(segmentIndex);
            }
            EdgePointDealer.o.onEndSegment(stage.segmentCount - 1);

            yield return new WaitForSeconds(1f);
        }
    }
}