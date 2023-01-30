using System.Collections.Generic;
using UnityEngine;

namespace MergeAndFight.Merge
{
    public class CellBuyHandler : MonoBehaviour
    {
        [SerializeField] private List<CellToBuy> _cellsToBuy;
        [SerializeField] private CellsField _cellsField;

        private int _currentCellToBuyIndex;
        private CellToBuy _currentCellToBuy;

        private void OnDisable()
        {
            if (_currentCellToBuy != null)
                _currentCellToBuy.CellBought -= OnCellBought;
        }

        public void LoadCells()
        {
            _currentCellToBuyIndex = CustomPlayerPrefs.GetInt(Prefs.CurrencyPrefs.CellsBought, 0);

            TryUpdateCurrentCell();

            if (_currentCellToBuyIndex > 0)
            {
                for (int i = 0; i < _currentCellToBuyIndex; i++)
                {
                    AddCellToField(i);
                }
            }

            if (_currentCellToBuyIndex + 1 < _cellsToBuy.Count)
            {
                for (int i = _currentCellToBuyIndex + 1; i < _cellsToBuy.Count; i++)
                {
                    _cellsToBuy[i].DisableView();
                    _cellsToBuy[i].EnableClosedCellView();
                }
            }
        }

        private void OnCellBought()
        {
            AddCellToField(_currentCellToBuyIndex);

            _currentCellToBuy.CellBought -= OnCellBought;
            _currentCellToBuyIndex += 1;
            CustomPlayerPrefs.SetInt(Prefs.CurrencyPrefs.CellsBought, _currentCellToBuyIndex);

            TryUpdateCurrentCell();
        }

        private void TryUpdateCurrentCell()
        {
            _currentCellToBuy = _currentCellToBuyIndex < _cellsToBuy.Count ? _cellsToBuy[_currentCellToBuyIndex] : null;

            if (_currentCellToBuy != null)
            {
                _currentCellToBuy.EnableView();
                _currentCellToBuy.CellBought += OnCellBought;
            }
        }

        private void AddCellToField(int cellIndex)
        {
            _cellsField.AddCell(_cellsToBuy[cellIndex].Cell);
            _cellsToBuy[cellIndex].DisableView();
        }
    }
}
