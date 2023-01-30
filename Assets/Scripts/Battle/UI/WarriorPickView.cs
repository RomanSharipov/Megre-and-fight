using System;
using UnityEngine;
using UnityEngine.UI;

namespace MergeAndFight.Fight
{
    [RequireComponent(typeof(RectTransform))]
    public class WarriorPickView : MonoBehaviour
    {
        private const string PulseAnimation = "CountChanged";

        [SerializeField] private WarriorType _warriorType;
        [SerializeField] private Color _disabledColor;
        [Header("UI")]
        [SerializeField] private Animator _animator;
        [SerializeField] private TMPro.TMP_Text _text;
        [SerializeField] private string _prefix;
        [SerializeField] private Button _button;
        [SerializeField] private Vector2 _pickedButtonOffset;

        private RectTransform _rectTransform;
        private Vector2 _defaultPosition;

        public event Action<WarriorType, WarriorPickView> WarriorTypeChanged;

        private void Awake()
        {
            _rectTransform = _button.GetComponent<RectTransform>();
            _defaultPosition = _rectTransform.anchoredPosition;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnWarriorPickButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnWarriorPickButtonClick);
        }

        public void Init(int warriorsCount)
        {
            UpdateWarriorsCount(warriorsCount);

            if (warriorsCount <= 0)
                gameObject.SetActive(false);
        }

        public void TryEnableView(int warriorsCount)
        {
            _rectTransform.anchoredPosition = _defaultPosition;

            if (warriorsCount > 0)
                _button.interactable = true;
        }

        public void UpdateWarriorsCount(int warriorsCount)
        {
            _text.text = _prefix + " " + warriorsCount;
            _animator.SetTrigger(PulseAnimation);

            if (warriorsCount <= 0)
            {
                var colors = _button.colors;
                colors.disabledColor = _disabledColor;
                _button.colors = colors;

                _button.interactable = false;
            }
        }

        private void OnWarriorPickButtonClick()
        {
            _rectTransform.anchoredPosition += _pickedButtonOffset;
            DisableView();
            WarriorTypeChanged?.Invoke(_warriorType, this);
        }

        private void DisableView() => _button.interactable = false;
    }
}
