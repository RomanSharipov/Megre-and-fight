using System;
using UnityEngine;
using UnityEngine.UI;

namespace MergeAndFight.Merge
{
    public class CellToBuy : MonoBehaviour
    {
        [SerializeField] private Cell _cell;
        [Header("Buy UI")]
        [SerializeField] private Button _buyCellButton;
        [SerializeField] private TMPro.TMP_Text _text;
        [SerializeField] private Image _closedCellImage;
        [SerializeField] private LayerMask _cellsToBuyLayerMask;

        public Cell Cell => _cell;

        public event Action CellBought;

        private void OnDisable()
        {
            _buyCellButton.onClick.RemoveListener(OnBuyCellButtonClick);
        }

        public void EnableClosedCellView() => _closedCellImage.gameObject.SetActive(true);

        public void EnableView()
        {
            _text.gameObject.SetActive(true);
            _buyCellButton.gameObject.SetActive(true);
            _closedCellImage.gameObject.SetActive(true);

            _buyCellButton.onClick.AddListener(OnBuyCellButtonClick);

            _text.text = CurrencyHandler.Instance.CurrentCellCost.ToString();
        }

        public void DisableView()
        {
            _text.gameObject.SetActive(false);
            _buyCellButton.gameObject.SetActive(false);
            _closedCellImage.gameObject.SetActive(false);

            _buyCellButton.onClick.RemoveListener(OnBuyCellButtonClick);
        }

        private void OnBuyCellButtonClick()
        {
            if (CurrencyHandler.Instance.TryBuyCell())
                CellBought?.Invoke();
        }
    }
}
