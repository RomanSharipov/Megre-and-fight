using UnityEngine;
using DG.Tweening;
using System;

namespace MergeAndFight.Merge
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private CentralCellPoint _cellPoint;
        [SerializeField] private HighlightHandler _highlightHandler;
        [SerializeField] private MergeObject _mergeObject;
        [SerializeField, Min(1)] private int _amountIncreaceStep = 2;
        [Header("Merge effects")]
        [SerializeField] private ParticleSystem _mergeParticle;
        [SerializeField, Min(0f)] private float _scaleTime = 0.5f;

        protected ParticleSystem MergeParticle => _mergeParticle;

        public bool IsEmpty => _mergeObject == null;
        public Vector3 CentralPoint => _cellPoint.transform.position;
        public MergeObject MergeObject => _mergeObject;

        public virtual void SetMergeObject(MergeObject mergeObject)
        {
            if (IsEmpty == false && IsAbleToMerge(mergeObject))
            {
                MergeWith(mergeObject);
                return;
            }

            _mergeObject = mergeObject;
        }

        public MergeObject GetMergeObject()
        {
            var mergeObject = _mergeObject;
            _mergeObject = null;

            return mergeObject;
        }

        public void EnableHighlight(HighlightCellType highlightCellType) => _highlightHandler.EnableHighlight(highlightCellType);

        public void DisableHighlight(HighlightCellType highlightCellType) => _highlightHandler.DisableHighlight(highlightCellType);

        public bool IsAbleToMerge(MergeObject objectToMerge)
        {
            return _mergeObject != null
                && _mergeObject.Warrior.GetType() == objectToMerge.Warrior.GetType()
                && _mergeObject.NextLevelObject != null
                && _mergeObject.Warrior.CurrentLevel == objectToMerge.Warrior.CurrentLevel;
        }

        public bool IsAbleToIncreaseUnitsAmount()
        {
            return _mergeObject.Amount < _mergeObject.MaxAmount;
        }

        public void IncreaseMergeObjectUnitsAmount()
        {
            if (IsAbleToIncreaseUnitsAmount() == false)
                throw new ArgumentOutOfRangeException($"Can't increase amount, because of maximum! Call {nameof(IsAbleToIncreaseUnitsAmount)} first!");

            var amount = _mergeObject.Amount;
            amount *= _amountIncreaceStep;
            _mergeObject.SetAmount(amount);
        }

        private void MergeWith(MergeObject objectToMerge)
        {
            Instantiate(_mergeParticle, CentralPoint, _mergeParticle.transform.rotation);
            var nextLevelObject = Instantiate(_mergeObject.NextLevelObject, CentralPoint, _mergeObject.NextLevelObject.transform.rotation);
            var amount = _mergeObject.Amount * objectToMerge.Amount;

            Destroy(objectToMerge.gameObject);
            Destroy(_mergeObject.gameObject);
            _mergeObject = nextLevelObject;

            var defaultScale = _mergeObject.transform.localScale;
            _mergeObject.transform.localScale = Vector3.zero;
            _mergeObject.transform.DOScale(defaultScale, _scaleTime);

            var newAmount = amount > _mergeObject.MaxAmount ? _mergeObject.MaxAmount : amount;
            _mergeObject.SetAmount(newAmount);
        }
    }
}
