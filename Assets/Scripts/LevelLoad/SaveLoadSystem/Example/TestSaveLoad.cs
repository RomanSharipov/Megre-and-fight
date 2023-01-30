using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SaveLoadSystem.Example
{
    public class TestSaveLoad : Singleton<TestSaveLoad>
    {
        private const int DefaultPrice = 0;
        
        [Header("File name")]
        [SerializeField] private string _fileName = "TestData";
        [Header("UI")]
        [SerializeField] private TMP_InputField _priceInputField;
        [SerializeField] private Toggle _isBoughtToggle;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _loadButton;

        private SaveLoadSystem<TestData> _saveLoadSystem;

        protected override void OnAwake()
        {
            base.OnAwake();
            _saveLoadSystem = new SaveLoadSystem<TestData>(_fileName);
        }

        private void OnEnable()
        {
            _saveButton.onClick.AddListener(SaveData);
            _loadButton.onClick.AddListener(LoadData);
        }

        private void OnDisable()
        {
            _saveButton.onClick.RemoveListener(SaveData);
            _loadButton.onClick.RemoveListener(LoadData);
        }

        private void Start()
        {
            LoadData();
        }

        private void SaveData()
        {
            if (int.TryParse(_priceInputField.text, out int price) == false)
                throw new ArgumentOutOfRangeException($"Price must be a number!");
            
            var data = new TestData(price, _isBoughtToggle.isOn);

            _saveLoadSystem.SaveData(data);
        }

        private void LoadData()
        {
            var testData = _saveLoadSystem.GetLoadedData();

            _priceInputField.text = (testData.Price > DefaultPrice ? testData.Price : DefaultPrice).ToString();
            _isBoughtToggle.isOn = testData.IsBought;
        }
    }
}