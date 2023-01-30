using UnityEngine;

namespace MergeAndFight.Merge
{
    [RequireComponent(typeof(Canvas))]
    public class AmountView : MonoBehaviour
    {
        private const string TextPrefix = "x";

        [SerializeField] private MergeObject _mergeObject;
        [Header("UI")]
        [SerializeField] private MaxView _maxView;
        [SerializeField] private ViewPanel _panel;
        [SerializeField] private TMPro.TMP_Text _text;

        private void Awake()
        {
            _panel.DisableView();

            var canvas = GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
        }

        private void OnEnable()
        {
            _mergeObject.AmountUpdated += OnAmountUpdated;
            _mergeObject.SwitchedToBattleState += OnSwitchedToBattleState;
        }

        private void OnDisable()
        {
            _mergeObject.AmountUpdated -= OnAmountUpdated;
            _mergeObject.SwitchedToBattleState -= OnSwitchedToBattleState;
        }

        private void OnAmountUpdated(int amount, bool isMax)
        {
            _panel.EnableView();
            _text.text = TextPrefix + amount;

            if (isMax)
                Instantiate(_maxView, transform.position, _maxView.transform.rotation);
        }

        private void OnSwitchedToBattleState()
        {
            _panel.DisableView();
        }
    }
}
