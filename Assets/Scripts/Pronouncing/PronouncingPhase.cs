using System.Collections;
using UnityEngine;

namespace KidLetters
{
    public class PronouncingPhase : Phase<PronouncingPhase>
    {
        public bool skip = false;
        public int letterIndexInWord => wordInfo.indexOfLetter(letterId);
        public int letterId => this.letter.letterId;
        public Letter letter { get; private set; }
        public WordInfo wordInfo { get; private set; }
        public bool isAfterTracing { get; private set; }


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


        public void setArgs(Letter letter)
        {
            this.isAfterTracing = false;
            this.letter = letter;
            wordInfo = WordList.o.getWordByStartingLetter(letter.letterId);
        }
        public void setArgsAfterTracing(Letter letter, WordInfo wordInfo)
        {
            this.isAfterTracing = true;
            this.letter = letter;
            this.wordInfo = wordInfo;
        }


        protected override void onEnter()
        {
            StartCoroutine(cycle());
        }
        protected override void onExit()
        {
            StopAllCoroutines();
            letter.gameObject.SetActive(true);
            letter.setColor(Color.white);
            letter.setTextEnabled(true);
        }
        IEnumerator cycle()
        {
            Home.LetterContainer.o.setActiveLetters(false, x => x == PronouncingPhase.o.letter);
            if (skip && !isAfterTracing)
            {
                TracingPhase.o.setArgs(letter, wordInfo);
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
                HomePhase.o.setArgs(letter);
                Phase.change(HomePhase.o);
            }
            else
            {
                yield return Pronouncing.TerminatePronouncing.o.play();
                TracingPhase.o.setArgs(letter, wordInfo);
                Phase.change(TracingPhase.o);
            }
        }

    }
}