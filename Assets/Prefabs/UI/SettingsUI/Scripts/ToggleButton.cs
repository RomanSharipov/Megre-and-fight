using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace SettingsView
{
    [RequireComponent(typeof(Button))]
    public class ToggleButton : MonoBehaviour, ISaveSettingInstallable
    {
        [Header("Toggle buttons")]
        [SerializeField] private ViewPanel _toggleOffView;
        [SerializeField] private ViewPanel _toggleOnView;
        [Header("Audio settings")]
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private List<NameVolume> _nameVolumes;

        private Button _button;
        private bool _isEnabled = false;

        public event Action ButtonClicked;

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

        public void Init()
        {
            foreach (var volume in _nameVolumes)
            {
                var currentVolume = PlayerPrefs.GetFloat(volume.ToString(), 1);
                _isEnabled = currentVolume != 1 ? false : true;

                ChangeButtonState();
            }
        }

        private void OnButtonClick()
        {
            ButtonClicked?.Invoke();

            _isEnabled = !_isEnabled;
            ChangeButtonState();
        }

        private void ChangeButtonState()
        {
            if (_isEnabled)
            {
                ChangeVolume(1f);
                EnableButton();

                return;
            }

            ChangeVolume(0f);
            DisableButton();
        }

        private void EnableButton()
        {
            _toggleOffView.DisableView();
            _toggleOnView.EnableView();
        }

        private void DisableButton()
        {
            _toggleOnView.DisableView();
            _toggleOffView.EnableView();
        }

        private void ChangeVolume(float value)
        {
            var soundValue = value == 1 ? 0 : -80;

            foreach (var volume in _nameVolumes)
            {
                _audioMixer.SetFloat(volume.ToString(), soundValue);
                PlayerPrefs.SetFloat(volume.ToString(), value);
            }
        }

        private enum NameVolume
        {
            Music,
            Sound,
            AllVolume
        }
    }

    public interface ISaveSettingInstallable
    {
        void Init();
    }
}
