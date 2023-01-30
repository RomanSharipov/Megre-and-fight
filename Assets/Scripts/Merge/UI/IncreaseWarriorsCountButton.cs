using UnityEngine;

namespace MergeAndFight.Merge
{
    public class IncreaseWarriorsCountButton : MergeUIButton
    {
        private const string TextPrefix = "";

        [SerializeField] private CellsField _cellsField;

        protected override void OnEnable()
        {
            base.OnEnable();
            _cellsField.UnitsAmountOnCellsUpdated += OnUnitsAmountOnCellsUpdated;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _cellsField.UnitsAmountOnCellsUpdated -= OnUnitsAmountOnCellsUpdated;
        }

        private void Start()
        {
            SetIncreaseAmountButtonState();
        }

        protected override void OnMergeUIButtonClick()
        {
            base.OnMergeUIButtonClick();
            TryIncreaceWarriorsAmount();
        }

        private void OnUnitsAmountOnCellsUpdated()
        {
            SetIncreaseAmountButtonState();
        }

        private void SetIncreaseAmountButtonState()
        {
            var buttonState = _cellsField.CellsAbleToIncreaseAmount > 0
                        && CurrencyHandler.Instance.IsAbleToDecreaseCurrencyAmount(CurrencyHandler.Instance.CurrentUpgradeUnitAmountCost) ? true : false;

            SetButtonInteractableState(buttonState);
            Text.text = TextPrefix + " " + CurrencyHandler.Instance.CurrentUpgradeUnitAmountCost;
        }

        private void TryIncreaceWarriorsAmount()
        {
            if (_cellsField.CellsAbleToIncreaseAmount > 0 && CurrencyHandler.Instance.TryBuyUnitIncreaceUngrade())
            {
                _cellsField.IncreaseWarriorsAmount();
                Text.text = TextPrefix + " " + CurrencyHandler.Instance.CurrentUpgradeUnitAmountCost;

                if (_cellsField.CellsAbleToIncreaseAmount <= 0 || CurrencyHandler.Instance.IsAbleToDecreaseCurrencyAmount(CurrencyHandler.Instance.CurrentUpgradeUnitAmountCost) == false)
                {
                    SetButtonInteractableState(false);
                }
            }
        }
    }
}
