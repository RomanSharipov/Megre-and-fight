using System;
using UnityEngine;

public class CurrencyHandler : Singleton<CurrencyHandler>
{
    [Header("Unit currency")]
    [SerializeField, Min(0)] private int _initialUnitCost = 100;
    [SerializeField, Min(0)] private int _unitCostModifier = 50;
    [Header("Upgrade currency")]
    [SerializeField, Min(0)] private int _initialUpgradeCost = 100;
    [SerializeField, Min(0)] private int _upgradeCostModifier = 50;
    [Header("Upgrade cells")]
    [SerializeField, Min(0)] private int _initialCellCost = 100;
    [SerializeField, Min(0)] private int _upgradeCellModifier = 50;

    private int _currentCurrencyAmount;
    private int _currentAddUnitLevel;
    private int _currentUpgradeUnitAmountLevel;
    private int _currentCellLevel;

    public int CurrentMoneyAmount => _currentCurrencyAmount;
    public int CurrentAddUnitCost => _initialUnitCost + _currentAddUnitLevel * _unitCostModifier;
    public int CurrentUpgradeUnitAmountCost => _initialUpgradeCost + _currentUpgradeUnitAmountLevel * _upgradeCostModifier;
    public int CurrentCellCost => _initialCellCost + _currentCellLevel * _upgradeCellModifier;

    public event Action<int> CurrencyAmountChanged;

    private void Awake()
    {
        _currentCurrencyAmount = CustomPlayerPrefs.GetInt(Prefs.CurrencyPrefs.CurrencyAmount, 0);
        _currentAddUnitLevel = CustomPlayerPrefs.GetInt(Prefs.CurrencyPrefs.AddUnitLevel, 0);
        _currentUpgradeUnitAmountLevel = CustomPlayerPrefs.GetInt(Prefs.CurrencyPrefs.UpgradeUnitAmountLevel, 0);
        _currentCellLevel = CustomPlayerPrefs.GetInt(Prefs.CurrencyPrefs.CellsBought, 0);
    }

    private void Start()
    {
        CurrencyAmountChanged?.Invoke(_currentCurrencyAmount);
    }

    public void IncreaseCurrencyAmount(int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException($"Currency amount can't be less, than 0! Now it equals {amount}.");

        _currentCurrencyAmount += amount;
        CurrencyAmountChanged?.Invoke(_currentCurrencyAmount);
        CustomPlayerPrefs.SetInt(Prefs.CurrencyPrefs.CurrencyAmount, _currentCurrencyAmount);
    }

    public bool IsAbleToDecreaseCurrencyAmount(int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException($"Currency amount can't be less, than 0! Now it equals {amount}.");

        return _currentCurrencyAmount >= amount;
    }

    public bool TryBuyAddUnitUpgrade()
    {
        return TryBuyUpgrade(CurrentAddUnitCost, ref _currentAddUnitLevel, Prefs.CurrencyPrefs.AddUnitLevel);
    }

    public bool TryBuyUnitIncreaceUngrade()
    {
        return TryBuyUpgrade(CurrentUpgradeUnitAmountCost, ref _currentUpgradeUnitAmountLevel, Prefs.CurrencyPrefs.UpgradeUnitAmountLevel);
    }

    public bool TryBuyCell()
    {
        return TryBuyUpgrade(CurrentCellCost, ref _currentCellLevel, Prefs.CurrencyPrefs.CellsBought);
    }

    private bool TryBuyUpgrade(int upgradeCost, ref int upgradeLevel, string upgradePref)
    {
        if (IsAbleToDecreaseCurrencyAmount(upgradeCost))
        {
            DecreaseCurrencyAmount(upgradeCost);
            upgradeLevel += 1;
            CustomPlayerPrefs.SetInt(upgradePref, upgradeLevel);

            return true;
        }

        return false;
    }

    private void DecreaseCurrencyAmount(int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException($"Currency amount can't be less, than 0! Now it equals {amount}.");

        if (IsAbleToDecreaseCurrencyAmount(amount) == false)
            throw new ArgumentOutOfRangeException($"Can't decrease amount because of not enought currency! Call {nameof(IsAbleToDecreaseCurrencyAmount)} first!");

        _currentCurrencyAmount -= amount;
        CurrencyAmountChanged?.Invoke(_currentCurrencyAmount);
        CustomPlayerPrefs.SetInt(Prefs.CurrencyPrefs.CurrencyAmount, _currentCurrencyAmount);
    }
}
