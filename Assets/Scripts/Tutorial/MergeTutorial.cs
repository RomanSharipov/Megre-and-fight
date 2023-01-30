using UnityEngine;
using UnityEngine.UI;

namespace MergeAndFight.Merge
{
    public class MergeTutorial : MonoBehaviour
    {
        private const string IncreaseTutorialComplete = "IncreaseTutorialComplete";

        [SerializeField, Min(0)] private int _initialMoneyCount = 1000;
        [SerializeField] private Button _buyCellButton;
        [Header("Add units step")]
        [SerializeField] private MergeUIButton _addUnitButton;
        [SerializeField, Min(0)] private int _addUnitButtonClicksCount = 1;
        [SerializeField] private ViewPanel _addUnitStepViewPanel;
        [SerializeField] private GameObject _increaseAmountButtonView;
        [SerializeField] private GameObject _increaseAmountImageView;
        [Header("Increase amount step")]
        [SerializeField] private MergeUIButton _increaseAmountButton;
        [SerializeField, Min(0)] private int _increaseAmountButtonClicksCount = 1;
        [SerializeField] private ViewPanel _increaseAmountStepViewPanel;
        [Header("Merge step")]
        [SerializeField] private WarriorsMergeSpawner _warriorsMergeSpawner;
        [SerializeField] private CellsField _cellsField;
        [SerializeField] private ViewPanel _mergeStepViewPanel;
        [SerializeField, Min(1)] private int _mergesCount = 1;
        [Header("Start fight scene step")]
        [SerializeField] private Button _startLevelButton;
        [SerializeField] private ViewPanel _startBattleStepViewPanel;

        private int _stepTicksCount = 0;
        private bool _isInTutorial = false;

        private void OnEnable()
        {
            _addUnitStepViewPanel.DisableView();
            _increaseAmountStepViewPanel.DisableView();
            _mergeStepViewPanel.DisableView();
            _startBattleStepViewPanel.DisableView();

            if (CustomPlayerPrefs.HasKey(Prefs.CurrencyPrefs.CurrencyAmount) == false)
            {
                StartTutorial();
                return;
            }

            if (CustomPlayerPrefs.HasKey(IncreaseTutorialComplete) == false)
            {
                InitIncreaseAmountStep();
            }
        }

        private void Update()
        {
            if (_isInTutorial && _buyCellButton.gameObject.activeSelf)
                _buyCellButton.gameObject.SetActive(false);
        }

        private void StartTutorial()
        {
            AddInitialCurrency();
            InitAddUnitsStep();
        }

        private void AddInitialCurrency()
        {
            CustomPlayerPrefs.SetInt(Prefs.CurrencyPrefs.CurrencyAmount, _initialMoneyCount);
        }

        private void InitAddUnitsStep()
        {
            _isInTutorial = true;

            _increaseAmountButtonView.SetActive(false);
            _increaseAmountImageView.SetActive(false);
            _buyCellButton.gameObject.SetActive(false);

            _increaseAmountButton.SetButtonTutorialState(false);
            _startLevelButton.interactable = false;
            _cellsField.SetTutorialState(true);

            _addUnitStepViewPanel.EnableView();
            _addUnitButton.SetButtonTutorialState(true);
            _addUnitButton.ButtonClickCallback += OnAddUnitButtonClick;
            _warriorsMergeSpawner.SetTutorialSpawnChances(-1, -1, -1, 100);
        }

        private void OnAddUnitButtonClick()
        {
            _stepTicksCount += 1;

            if (_stepTicksCount >= _addUnitButtonClicksCount)
                InitMergeStep();
        }

        private void InitMergeStep()
        {
            _addUnitButton.ButtonClickCallback -= OnAddUnitButtonClick;
            _addUnitButton.SetButtonTutorialState(false);
            _addUnitStepViewPanel.DisableView();

            _mergeStepViewPanel.EnableView();
            _cellsField.SetTutorialState(false);
            _cellsField.UnitMerged += OnUnitMerged;
        }

        private void OnUnitMerged()
        {
            _stepTicksCount += 1;

            if (_stepTicksCount >= _mergesCount)
                InitStartGameStep();
        }

        private void InitStartGameStep()
        {
            _cellsField.UnitMerged -= OnUnitMerged;
            _mergeStepViewPanel.DisableView();

            _startBattleStepViewPanel.EnableView();
            _startLevelButton.interactable = true;
        }

        private void InitIncreaseAmountStep()
        {
            _isInTutorial = true;
            _stepTicksCount = 0;
            _buyCellButton.gameObject.SetActive(false);

            _addUnitButton.SetButtonTutorialState(false);
            _increaseAmountStepViewPanel.EnableView();
            _increaseAmountButton.SetButtonTutorialState(true);
            _increaseAmountButton.ButtonClickCallback += OnIncreaceAmountButtonClick;
        }

        private void OnIncreaceAmountButtonClick()
        {
            _stepTicksCount += 1;

            if (_stepTicksCount >= _increaseAmountButtonClicksCount)
                EndIncreaseTutorial();
        }

        private void EndIncreaseTutorial()
        {
            _isInTutorial = false;

            _buyCellButton.gameObject.SetActive(true);
            _addUnitButton.SetButtonTutorialState(true);
            _increaseAmountStepViewPanel.DisableView();
            _increaseAmountButton.ButtonClickCallback -= OnIncreaceAmountButtonClick;

            CustomPlayerPrefs.SetBool(IncreaseTutorialComplete, true);
        }
    }
}
