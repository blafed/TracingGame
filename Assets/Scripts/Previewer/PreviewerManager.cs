using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

namespace KidLetters
{
    using Previewer;
    public class PreviewerManager : PhaseEntity
    {
        [System.Serializable]
        public class PreviewData
        {
            public bool skipPronouncing = false;
            public int letterId = -1;
            public string word = "";
            public PatternCode patternCode;
        }
        public PreviewData data = new PreviewData();
        public List<SettingInfo> settings = new List<SettingInfo>();
        [SerializeField] FlowList<SettingItem> items = new FlowList<SettingItem>();



        public void onSettingChanged(SettingCode type, bool value)
        {
            switch (type)
            {
                case SettingCode.skipPronouncing:
                    // PronouncingPhase.o.skip = value;
                    data.skipPronouncing = value;
                    break;
                case SettingCode.word:
                    break;
            }
        }

        public void onSettingChanged(SettingCode type, int value)
        {
            switch (type)
            {
                case SettingCode.pattern:
                    data.patternCode = (PatternCode)value;
                    break;
                case SettingCode.word:
                    if (value == 0)
                        data.word = "";
                    else
                    {
                        var wordInfo = WordList.o.listAll()[value - 1];
                        data.word = wordInfo.word;
                        items.iterate(settings.Count, x =>
                        {
                            if (x.component.info.code == SettingCode.letter)
                            {
                                var letters = StartingLetters.getStartingLetters(wordInfo.word);
                                var letter = letters.getRandom();
                                var letterId = LetterUtility.charToLetterId(letter[0]);
                                x.component.setDropdownValue(letterId);
                            }
                        });
                    }
                    break;
                case SettingCode.letter:
                    data.letterId = value;
                    break;

            }
        }
        public void onSettingAction(SettingCode type)
        {

        }

        public void preview()
        {
        }
        protected override void register()
        {
            HomePhase.o.registerEntity(this);
        }

        public override void onPhaseEnter()
        {
        }


        protected override void Start()
        {

            foreach (var x in settings)
            {
                switch (x.code)
                {
                    case SettingCode.letter:
                        x.dropdownOptions = new List<string>(30);
                        foreach (var letterInfo in LetterList.o.getLetterInfos())
                        {
                            x.dropdownOptions.Add(LetterUtility.letterToString(letterInfo.letterId));
                        }
                        break;
                    case SettingCode.pattern:
                        x.dropdownOptions = new List<string>(System.Enum.GetNames(typeof(PatternCode)));
                        for (int i = 0; i < (int)PatternCode.COUNT; i++)
                        {
                            x.dropdownOptions[i] = ((PatternCode)i).ToString();
                        }
                        break;
                    case SettingCode.word:
                        x.dropdownOptions = new List<string>(WordList.o.listAll().Count);
                        x.dropdownOptions.Add("---");
                        foreach (var word in WordList.o.listAll())
                        {
                            x.dropdownOptions.Add(word.word);
                        }
                        break;
                }
            }
            items.iterate(settings.Count, x =>
            {
                x.component.info = settings[x.iterationIndex];
            });

            GetComponentInChildren<Button>().onClick.AddListener(preview);

        }
    }

    [System.Serializable]
    public class SettingInfo
    {
        public SettingCode code;
        public SettingType type;
        public string label;
        public bool defaultBoolValue;
        public List<string> dropdownOptions = new List<string>();
        public int defaultDropdownValue;
        public bool useActionButton;
    }
    public enum SettingType
    {
        toggle,
        dropdown,
    }
    public enum SettingCode
    {
        skipPronouncing,
        word,
        pattern,
        letter,


    }

}