using System;
using UnityEngine;
using UnityEngine.UI;

namespace MergeAndFight.Merge
{
    [RequireComponent(typeof(Button))]
    public abstract class MergeUIButton : MonoBehaviour
    {
        private const string EnabledAnimationTrigger = "IsEnabled";

        [SerializeField] private TMPro.TMP_Text _text;
        [SerializeField] private Animator _animator;
        [SerializeField] private Image _icon;
        [SerializeField] private AudioSource _clickSound;

        private Button _button;
        private bool _isEnabled = true;
        private bool _isTutorialEnabled = true;

        protected TMPro.TMP_Text Text => _text;

        public event Action ButtonClickCallback;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _clickSound.playOnAwake = false;
        }

        protected virtual void OnEnable()
        {
            _button.onClick.AddListener(OnMergeUIButtonClick);
        }

        protected virtual void OnDisable()
        {
            _button.onClick.RemoveListener(OnMergeUIButtonClick);
        }

        public void SetButtonTutorialState(bool state)
        {
            _isTutorialEnabled = state;
            SetButtonInteractableState(state);
        }

        protected void SetButtonInteractableState(bool state)
        {
            _isEnabled = state && _isTutorialEnabled;
            _animator.SetBool(EnabledAnimationTrigger, state && _isTutorialEnabled);

            if (_button == null)
                Awake();

            _button.interactable = _isEnabled && _isTutorialEnabled;
            _icon.color = _isEnabled && _isTutorialEnabled ? _button.colors.normalColor : _button.colors.disabledColor;
        }

        protected virtual void OnMergeUIButtonClick()
        {
            _clickSound.Play();
            ButtonClickCallback?.Invoke();
        }
    }
}
