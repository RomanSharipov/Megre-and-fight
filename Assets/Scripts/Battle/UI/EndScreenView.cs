using IJunior.TypedScenes;
using InvokeWithDelay;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MergeAndFight.Fight
{
    public class EndScreenView : MonoBehaviour
    {
        private const string MoneyIncreaseAnimation = "IsMoneyIncreasing";

        [SerializeField] private GameStateHandler _gameStateHandler;
        [SerializeField] private ViewPanel _warriorPickView;
        [SerializeField, Min(0f)] private float _moneyIncreaseTime = 2f;
        [SerializeField, Min(0f)] private float _delayTime = 1f;
        [Header("UI")]
        [SerializeField] private Animator _animator;
        [SerializeField] private ViewPanel _endScreenViewPanel;
        [SerializeField] private TMPro.TMP_Text _currencyText;
        [SerializeField] private Button _mergeLevelLoadButton;
        [SerializeField] private ViewPanel _loadingScreenPanel;
        [SerializeField] private ViewPanel _winTopPanel;
        [SerializeField] private ViewPanel _failTopPanel;

        private void Awake()
        {
            _endScreenViewPanel.DisableView();
            _loadingScreenPanel.DisableView();
            _winTopPanel.DisableView();
            _failTopPanel.DisableView();
        }

        private void OnEnable()
        {
            _gameStateHandler.PlayerWon += OnPlayerWon;
            _gameStateHandler.PlayerFail += OnPlayerFail;
            _mergeLevelLoadButton.onClick.AddListener(OnMergeLevelLoadButtonClick);
        }

        private void OnDisable()
        {
            _gameStateHandler.PlayerWon -= OnPlayerWon;
            _gameStateHandler.PlayerFail -= OnPlayerFail;
            _mergeLevelLoadButton.onClick.RemoveListener(OnMergeLevelLoadButtonClick);
        }

        private void OnPlayerWon(int moneyCount)
        {
            this.Invoke(() =>
            {
                _winTopPanel.EnableView();
                EnableEndScreen(moneyCount);
            }, _delayTime);
        }

        private void OnPlayerFail(int moneyCount)
        {
            this.Invoke(() =>
            {
                _failTopPanel.EnableView();
                EnableEndScreen(moneyCount);
            }, _delayTime);
        }

        private void EnableEndScreen(int moneyCount)
        {
            _warriorPickView.DisableView();
            _endScreenViewPanel.EnableView();

            StartCoroutine(MoneyIncreaceCoroutine(moneyCount));
        }

        private IEnumerator MoneyIncreaceCoroutine(int moneyCount)
        {
            var time = 0f;
            _animator.SetBool(MoneyIncreaseAnimation, true);

            while (time <= _moneyIncreaseTime)
            {
                int currentMoneyCount = (int)Mathf.Lerp(0f, moneyCount, time / _moneyIncreaseTime);
                _currencyText.text = currentMoneyCount.ToString();
                time += Time.deltaTime;

                yield return null;
            }

            _currencyText.text = moneyCount.ToString();
            _animator.SetBool(MoneyIncreaseAnimation, false);
        }

        private void OnMergeLevelLoadButtonClick()
        {
            _loadingScreenPanel.EnableView();
            MergeScene.Load();
        }
    }
}