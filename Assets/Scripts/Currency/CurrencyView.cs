using UnityEngine;

public class CurrencyView : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _currencyText;

    private void OnEnable()
    {
        CurrencyHandler.Instance.CurrencyAmountChanged += OnCurrencyAmountChanged;
    }

    private void OnDisable()
    {
        if (CurrencyHandler.Instance != null)
            CurrencyHandler.Instance.CurrencyAmountChanged -= OnCurrencyAmountChanged;
    }

    private void OnCurrencyAmountChanged(int amount) => _currencyText.text = amount.ToString();
}
