using MergeAndFight.Fight;
using System;
using UnityEngine;

namespace MergeAndFight.Merge
{
    public class WarriorsMergeSpawner : MonoBehaviour
    {
        [SerializeField] private Warrior _swordMan;
        [SerializeField, Min(0)] private int _swordmanSpawnPercentChance = 50;
        [SerializeField] private Warrior _archer;
        [SerializeField, Min(0)] private int _archerSpawnPercentChance = 50;
        [SerializeField] private Warrior _giant;
        [SerializeField, Min(0)] private int _giantSpawnPercentChance = 20;
        [SerializeField, Min(0)] private int _giantUnlockLevel = 6;
        [SerializeField] private Warrior _elephant;
        [SerializeField, Min(0)] private int _elephantSpawnPercentChance = 10;
        [SerializeField, Min(0)] private int _elephantUnlockLevel = 13;
        [Header("Cells field")]
        [SerializeField] private CellsField _cellsField;

        private Warrior _lastWarriorSpawned;
        private int _currentLevel;

        public bool IsAbleToSpawnNewWarrior => _cellsField.DoesHaveEmptyCells();

        public event Action<bool> CellsUpdated;

        private void OnEnable()
        {
            _cellsField.UnitsOnCellsUpdated += OnCellsUpdated;
            _currentLevel = CustomPlayerPrefs.GetInt(Prefs.LevelLoadPrefs.CompletedLevels, 1);
        }

        private void OnDisable()
        {
            _cellsField.UnitsOnCellsUpdated -= OnCellsUpdated;
        }

        public void SetTutorialSpawnChances(int elephantSpawnChance, int giantSpawnChance, int swordmanSpawnChance, int archerSpawnChance)
        {
            _elephantSpawnPercentChance = elephantSpawnChance;
            _giantSpawnPercentChance = giantSpawnChance;
            _swordmanSpawnPercentChance = swordmanSpawnChance;
            _archerSpawnPercentChance = archerSpawnChance;
        }

        public void SpawnNewWarrior()
        {
            if (IsAbleToSpawnNewWarrior == false)
                throw new ArgumentOutOfRangeException($"No free cells! Call {nameof(IsAbleToSpawnNewWarrior)} first!");

            Warrior warrior = GetWarrior();

            if (warrior.TryGetComponent(out MergeObject mergeObject) == false)
                throw new NullReferenceException($"Warrior must have {nameof(MergeObject)} component!");

            _cellsField.TryAddNewWarrior(mergeObject);
        }

        private Warrior GetWarrior()
        {
            Warrior warrior = null;

            do
            {
                if (UnityEngine.Random.Range(0, 101) <= _elephantSpawnPercentChance && _lastWarriorSpawned != _elephant && _currentLevel >= _elephantUnlockLevel)
                {
                    warrior = Instantiate(_elephant, Vector3.zero, _elephant.transform.rotation);
                    _lastWarriorSpawned = _elephant;
                }
                else if (UnityEngine.Random.Range(0, 101) <= _giantSpawnPercentChance && _lastWarriorSpawned != _giant && _currentLevel >= _giantUnlockLevel)
                {
                    warrior = Instantiate(_giant, Vector3.zero, _giant.transform.rotation);
                    _lastWarriorSpawned = _giant;
                }
                else if (UnityEngine.Random.Range(0, 101) <= _swordmanSpawnPercentChance && _lastWarriorSpawned != _swordMan)
                {
                    warrior = Instantiate(_swordMan, Vector3.zero, _swordMan.transform.rotation);
                    _lastWarriorSpawned = _swordMan;
                }
                else if (_lastWarriorSpawned != _archer || _archerSpawnPercentChance == 100)
                {
                    warrior = Instantiate(_archer, Vector3.zero, _archer.transform.rotation);
                    _lastWarriorSpawned = _archer;
                }

            } while (warrior == null);

            return warrior;
        }

        private void OnCellsUpdated(bool doesHaveEmpty)
        {
            CellsUpdated?.Invoke(doesHaveEmpty);
        }
    }
}
