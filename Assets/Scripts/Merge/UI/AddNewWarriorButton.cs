using UnityEngine;

namespace MergeAndFight.Merge
{
    public class AddNewWarriorButton : MergeUIButton
    {
        private const string TextPrefix = "";

        [SerializeField] private WarriorsMergeSpawner _warriorsSpawner;

        protected override void OnEnable()
        {
            base.OnEnable();
            _warriorsSpawner.CellsUpdated += OnCellsUpdated;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _warriorsSpawner.CellsUpdated -= OnCellsUpdated;
        }

        private void Start()
        {
            var buttonState = _warriorsSpawner.IsAbleToSpawnNewWarrior
                && CurrencyHandler.Instance.IsAbleToDecreaseCurrencyAmount(CurrencyHandler.Instance.CurrentAddUnitCost) ? true : false;

            SetButtonInteractableState(buttonState);
            Text.text = TextPrefix + " " + CurrencyHandler.Instance.CurrentAddUnitCost;
        }

        protected override void OnMergeUIButtonClick()
        {
            base.OnMergeUIButtonClick();
            TryAddNewWarrior();
        }

        private void TryAddNewWarrior()
        {
            if (_warriorsSpawner.IsAbleToSpawnNewWarrior && CurrencyHandler.Instance.TryBuyAddUnitUpgrade())
            {
                _warriorsSpawner.SpawnNewWarrior();
                Text.text = TextPrefix + " " + CurrencyHandler.Instance.CurrentAddUnitCost;

                if (_warriorsSpawner.IsAbleToSpawnNewWarrior == false || CurrencyHandler.Instance.IsAbleToDecreaseCurrencyAmount(CurrencyHandler.Instance.CurrentAddUnitCost) == false)
                {
                    SetButtonInteractableState(false);
                }
            }
        }

        private void OnCellsUpdated(bool doesHaveEmpty)
        {
            doesHaveEmpty = doesHaveEmpty && CurrencyHandler.Instance.IsAbleToDecreaseCurrencyAmount(CurrencyHandler.Instance.CurrentAddUnitCost);

            SetButtonInteractableState(doesHaveEmpty);
        }
    }
}
