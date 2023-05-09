using UnityEngine.UI;
using TMPro;
namespace KidLetters.Previewer
{
    using UnityEngine;

    public class SettingItem : MonoBehaviour
    {
        public SettingInfo info;
        [SerializeField] TextMeshProUGUI label;
        [SerializeField] TMP_Dropdown dropdown;
        [SerializeField] Toggle toggle;
        [SerializeField] Button button;

        PreviewerManager previewerManager;

        //these value will not update when calling value-change-event
        //they will be updated after calling value-chagnged-event
        //so we can use them in value-changed-event to get the old value
        public int dropdownValue { get; set; }
        public bool toggleValue { get; set; }


        private void Awake()
        {
            previewerManager = GetComponentInParent<PreviewerManager>();

            toggle.onValueChanged.AddListener(onToggleValueChanged);
            dropdown.onValueChanged.AddListener(onDropdownValueChanged);
            button.onClick.AddListener(() => previewerManager.onSettingAction(info.code));
        }


        private void Start()
        {
            label.text = info.label;

            button.gameObject.SetActive(info.useActionButton);
            dropdown.gameObject.SetActive(info.type == SettingType.dropdown);
            toggle.gameObject.SetActive(info.type == SettingType.toggle);


            if (info.type == SettingType.dropdown)
            {
                dropdown.ClearOptions();
                dropdown.AddOptions(info.dropdownOptions);
                dropdown.value = info.defaultDropdownValue;
                onDropdownValueChanged(info.defaultDropdownValue);
            }
            if (info.type == SettingType.toggle)
            {
                toggle.isOn = info.defaultBoolValue;
                onToggleValueChanged(info.defaultBoolValue);
            }
        }
        private void onToggleValueChanged(bool value)
        {

            previewerManager.onSettingChanged(info.code, value);

            toggleValue = value;

        }

        private void onDropdownValueChanged(int value)
        {
            previewerManager.onSettingChanged(info.code, value);
            dropdownValue = value;
        }


        public void setDropdownValue(int value)
        {
            dropdown.value = value;
        }
    }




}