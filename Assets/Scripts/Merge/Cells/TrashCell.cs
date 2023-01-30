using UnityEngine;

namespace MergeAndFight.Merge
{
    public class TrashCell : Cell
    {
        [SerializeField, Min(0)] private int _moneyAddPercent = 75;

        public override void SetMergeObject(MergeObject mergeObject)
        {
            DeleteMergeObject(mergeObject);
        }

        private void DeleteMergeObject(MergeObject mergeObject)
        {
            Instantiate(MergeParticle, CentralPoint, MergeParticle.transform.rotation);
            CurrencyHandler.Instance.IncreaseCurrencyAmount((int)((_moneyAddPercent / 100f) * CurrencyHandler.Instance.CurrentAddUnitCost));
            Destroy(mergeObject.gameObject);
        }
    }
}
