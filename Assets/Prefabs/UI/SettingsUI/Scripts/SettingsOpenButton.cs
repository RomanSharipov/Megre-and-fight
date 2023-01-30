using UnityEngine;
using UnityEngine.UI;

namespace SettingsView
{
    [RequireComponent(typeof(Button))]
    public class SettingsOpenButton : MonoBehaviour
    {
        [SerializeField] private ViewPanel _settingParent;

        private Button _settingsButton;

        private void Awake()
        {
            _settingsButton = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _settingsButton.onClick.AddListener(OnSettingsButtonClick);
        }

        private void OnDisable()
        {
            _settingsButton.onClick.RemoveListener(OnSettingsButtonClick);
        }

        private void Start()
        {
            var settingInstallable = _settingParent.GetComponentsInChildren<ISaveSettingInstallable>();

            foreach (var saveSettingInstallable in settingInstallable)
            {
                saveSettingInstallable.Init();
            }

            _settingParent.DisableView();
        }

        private void OnSettingsButtonClick()
        {
            if (_settingParent.gameObject.activeSelf == false)
                _settingParent.EnableView();
        }
    }
}
