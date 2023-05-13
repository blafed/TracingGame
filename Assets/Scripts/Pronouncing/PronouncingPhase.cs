using KidLetters.Home;
using System.Collections;
using UnityEngine;

namespace KidLetters
{
    public class PronouncingPhase : Phase<PronouncingPhase>
    {
        public bool skip = false;
        public int letterIndexInWord => wordInfo.indexOfLetter(letterId);
        public int letterId { get; private set; }
        // public LetterRaw letter { get; private set; }
        public WordInfo wordInfo { get; private set; }
        public bool isAfterTracing { get; private set; }

        public LetterFiller letter { get; private set; }


        public void playLetterAudio(int letterId)
        {
            GeneralAudioPlayer.o.play(LetterList.o.getAudioClip(letterId));
        }
        public void playWordAudio()
        {
            if (wordInfo.clip)
            {
                GeneralAudioPlayer.o.play(wordInfo.clip);
            }
        }


        public void setArgs(int letterId)
        {
            this.isAfterTracing = false;
            this.letterId = letterId;
            wordInfo = WordList.o.getWordByStartingLetter(letterId);
        }
        public void setArgsAfterTracing(int letterId, WordInfo wordInfo)
        {
            this.isAfterTracing = true;
            this.letterId = letterId;
            this.wordInfo = wordInfo;
        }


        protected override void onEnter()
        {
            letter = LetterFiller.createStandardFiller(LetterContainer.o.getLetter(letterId));
            StartCoroutine(cycle());
        }
        protected override void onExit()
        {
            StopAllCoroutines();
            Destroy(letter.gameObject);
        }
        IEnumerator cycle()
        {
            Home.LetterContainer.o.setActiveLetters(false, x => x == PronouncingPhase.o.letter);
            if (skip && !isAfterTracing)
            {
                TracingPhase.o.setArgs(letterId, wordInfo);
                Phase.change(TracingPhase.o);
                yield break;
            }

            if (!isAfterTracing)
                yield return Pronouncing.FocusOnLetter.o.play();
            yield return Pronouncing.WordView.o.play();
            yield return Pronouncing.WordPicturePlayer.o.play();

            if (isAfterTracing)
            {
                yield return Pronouncing.LeadPronouncingToHome.o.play();
                HomePhase.o.setArgs(letterId);
                Phase.change(HomePhase.o);
            }
            else
            {
                yield return Pronouncing.TerminatePronouncing.o.play();
                TracingPhase.o.setArgs(letterId, wordInfo);
                Phase.change(TracingPhase.o);
            }
        }

    }
}