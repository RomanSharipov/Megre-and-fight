using System;
using System.Collections.Generic;
using UnityEngine;

namespace MergeAndFight.Fight
{
    public class GameStateHandler : MonoBehaviour
    {
        [SerializeField] private WarriorsFightSpawner _warriorsFightSpawner;
        [SerializeField] private AudioSource _winSound;
        [SerializeField] private AudioSource _failSound;
        [Header("Bounty")]
        [SerializeField] private List<int> _bountyList;
        [SerializeField] private int _afterListBounty = 1000;
        [SerializeField, Min(0f)] private float _minCurrencyPercent = 0.1f;

        private Castle _castle;
        private int _levelNumber;
        private string _levelDiff;
        private int _levelLoop;
        private int _isLevelRandom;
        private string _levelType;
        private bool _levelCompleted = false;

        public event Action<int> PlayerWon;
        public event Action<int> PlayerFail;

        private void OnEnable()
        {
            _warriorsFightSpawner.PlayerWarriorsDied += OnPlayerWarriorsDied;
        }

        private void OnDisable()
        {
            _castle.Died -= OnCastleDied;
            _warriorsFightSpawner.PlayerWarriorsDied -= OnPlayerWarriorsDied;
        }

        public void Init(Castle castle, int uniqueLevelsCount)
        {
            _castle = castle;
            _castle.Died += OnCastleDied;

            LoadAnalyticsData(uniqueLevelsCount);
            AzureAnalytics.Instance.OnLevelStart(_levelNumber, _castle.gameObject.name, _levelDiff, _levelLoop, _isLevelRandom, _levelType);
        }

        private void LoadAnalyticsData(int uniqueLevelsCount)
        {
            _levelNumber = CustomPlayerPrefs.GetInt(Prefs.LevelLoadPrefs.CompletedLevels, 1);
            _levelDiff = _castle.AnalyticsData.GetLevelDifficulty();
            _levelLoop = _levelNumber / (uniqueLevelsCount + 1);
            _isLevelRandom = _levelNumber > uniqueLevelsCount ? 1 : 0;
            _levelType = _castle.AnalyticsData.GetLevelType();
        }

        private void OnPlayerWarriorsDied()
        {
            if (_levelCompleted == false)
            {
                _failSound.Play();

                _levelCompleted = true;

                var levelProgress = (1 - _castle.HealthLeftPercent) * 100;
                AzureAnalytics.Instance.OnLevelComplete(_levelNumber, _castle.gameObject.name, _levelDiff, _levelLoop, _isLevelRandom, _levelType, "fail", (int)levelProgress);
               
                int playedLevels = CustomPlayerPrefs.GetInt(Prefs.LevelLoadPrefs.LevelCount, 1);
                playedLevels += 1;
                CustomPlayerPrefs.SetInt(Prefs.LevelLoadPrefs.LevelCount, playedLevels);

                int completedLevels = CustomPlayerPrefs.GetInt(Prefs.LevelLoadPrefs.CompletedLevels, 1);

                var currencyForLevel = GetCurrencyForLevel(completedLevels);
                var currencyByCastleHealth = Mathf.RoundToInt(currencyForLevel * (levelProgress / 100f));

                currencyForLevel = currencyByCastleHealth > _minCurrencyPercent * currencyForLevel ? currencyByCastleHealth : (int)(_minCurrencyPercent * currencyForLevel);
                CurrencyHandler.Instance.IncreaseCurrencyAmount(currencyForLevel);

                PlayerFail?.Invoke(currencyForLevel);
            }
        }

        private void OnCastleDied()
        {
            if (_levelCompleted == false)
            {
                _winSound.Play();

                _levelCompleted = true;

                AzureAnalytics.Instance.OnLevelComplete(_levelNumber, _castle.gameObject.name, _levelDiff, _levelLoop, _isLevelRandom, _levelType, "win", 100);
                
                int playedLevels = CustomPlayerPrefs.GetInt(Prefs.LevelLoadPrefs.LevelCount, 1);
                playedLevels += 1;
                CustomPlayerPrefs.SetInt(Prefs.LevelLoadPrefs.LevelCount, playedLevels);

                int completedLevels = CustomPlayerPrefs.GetInt(Prefs.LevelLoadPrefs.CompletedLevels, 1);
                completedLevels += 1;
                CustomPlayerPrefs.SetInt(Prefs.LevelLoadPrefs.CompletedLevels, completedLevels);

                var currencyForLevel = GetCurrencyForLevel(completedLevels);
                CurrencyHandler.Instance.IncreaseCurrencyAmount(currencyForLevel);


                PlayerWon?.Invoke(currencyForLevel);
            }
        }

        private int GetCurrencyForLevel(int completedLevels)
        {
            if (completedLevels <= _bountyList.Count)
            {
                return _bountyList[completedLevels - 1];
            }

            var levelsPassedAfterMax = completedLevels - _bountyList.Count;

            return _bountyList[_bountyList.Count - 1] + levelsPassedAfterMax * _afterListBounty;
        }
    }
}
