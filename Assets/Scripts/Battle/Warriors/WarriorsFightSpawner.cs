using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MergeAndFight.Fight
{
    [RequireComponent(typeof(RaycastCheckUnderMouse))]
    public class WarriorsFightSpawner : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _spawnUnitParticle;
        [SerializeField] private GameStateHandler _gameStateHandler;
        [SerializeField, Min(0f)] private float _spawnDelay = 0.2f;

        private RaycastCheckUnderMouse _raycastChecker;
        private List<WarriorPickView> _warriorPickViews;
        private List<Warrior> _archers;
        private List<Warrior> _swordmans;
        private List<Warrior> _giants;
        private List<Warrior> _elephants;
        private List<Warrior> _currentWarriors;
        private WarriorPickView _currentWarriorPickView;
        private int _spawnedWarriorsCount = 0;
        private Coroutine _spawnCoroutine;

        public event Action PlayerWarriorsDied;
        public event Action WarriorSpawned;

        private void Awake()
        {
            _raycastChecker = GetComponent<RaycastCheckUnderMouse>();
        }

        private void OnEnable()
        {
            foreach (var warriorPickView in _warriorPickViews)
            {
                warriorPickView.WarriorTypeChanged += OnWarriorTypeChanged;
            }
        }

        private void OnDisable()
        {
            foreach (var warriorPickView in _warriorPickViews)
            {
                warriorPickView.WarriorTypeChanged -= OnWarriorTypeChanged;
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && _raycastChecker.TryGetWarriorPosition(out Vector3 spawnPosition))
                _spawnCoroutine = StartCoroutine(SpawnUnits());

            if (_spawnCoroutine != null && Input.GetMouseButtonUp(0))
                StopCoroutine(_spawnCoroutine);
        }

        public void Init(ref List<Warrior> archers, ref List<Warrior> swordmans, ref List<Warrior> giants, ref List<Warrior> elephants, ref List<WarriorPickView> warriorPickViews)
        {
            _archers = archers;
            _swordmans = swordmans;
            _giants = giants;
            _elephants = elephants;

            _warriorPickViews = warriorPickViews;
        }

        private void OnWarriorTypeChanged(WarriorType warriorType, WarriorPickView warriorPickView)
        {
            if (_currentWarriorPickView != null)
                _currentWarriorPickView.TryEnableView(_currentWarriors.Count);

            _currentWarriorPickView = warriorPickView;

            switch (warriorType)
            {
                case WarriorType.Archer:
                    _currentWarriors = _archers;
                    break;
                case WarriorType.Swordman:
                    _currentWarriors = _swordmans;
                    break;
                case WarriorType.Giant:
                    _currentWarriors = _giants;
                    break;
                case WarriorType.Elephant:
                    _currentWarriors = _elephants;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unknown warrior type {warriorType}!");
            }
        }

        private IEnumerator SpawnUnits()
        {
            var tick = new WaitForSeconds(_spawnDelay);

            while (Input.GetMouseButtonUp(0) == false)
            {
                if (_raycastChecker.TryGetWarriorPosition(out Vector3 spawnPosition))
                    TrySpawnNewWarrior(spawnPosition);

                yield return tick;
            }
        }

        private bool TrySpawnNewWarrior(Vector3 spawnPosition)
        {
            if (_currentWarriors == null || _currentWarriors.Count == 0)
                return false;

            var warrior = _currentWarriors[0];
            Warrior newWarrior = Instantiate(warrior, spawnPosition, warrior.transform.rotation);
            newWarrior.Init(_gameStateHandler);
            newWarrior.WarriorDied += OnWarriorDied;

            _spawnedWarriorsCount += 1;

            Instantiate(_spawnUnitParticle, spawnPosition, _spawnUnitParticle.transform.rotation);

            if (_currentWarriorPickView == null)
                throw new NullReferenceException($"{nameof(_currentWarriorPickView)} is null! Check {nameof(OnWarriorTypeChanged)} method!");

            _currentWarriors.Remove(warrior);
            _currentWarriorPickView.UpdateWarriorsCount(_currentWarriors.Count);

            WarriorSpawned?.Invoke();

            return true;
        }

        private void OnWarriorDied(Warrior warrior)
        {
            _spawnedWarriorsCount -= 1;
            warrior.WarriorDied -= OnWarriorDied;

            var warriorsLeftInPockets = _archers.Count + _swordmans.Count + _giants.Count + _elephants.Count;

            if (_spawnedWarriorsCount <= 0 && warriorsLeftInPockets <= 0)
                PlayerWarriorsDied?.Invoke();
        }
    }
}
