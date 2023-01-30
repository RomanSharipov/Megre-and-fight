using UnityEngine;
using UnityEngine.UI;

namespace SettingsView
{
    [RequireComponent(typeof(Button))]
    public class SettingsExitButton : MonoBehaviour
    {
        [SerializeField] private ViewPanel _settingsView;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            _settingsView.DisableView();
        }
    }
}
