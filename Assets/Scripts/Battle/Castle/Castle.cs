using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace MergeAndFight.Fight
{
    public class Castle : MonoBehaviour, IAttackable
    {
        [SerializeField] private List<Warrior> _defenders;
        [SerializeField] private List<WarriorType> _defendersTypesToCountHealth;
        [SerializeField] private CastleRigidbodyHandler _castleRigidbodyHandler;
        [Header("Castle settings")]
        [SerializeField, Min(0)] private int _levelCompleteTime = 30;
        [SerializeField, Min(0)] private int _levelFailTime = 45;
        [SerializeField, Min(0)] private int _maxAttackPower = 5;
        [SerializeField, Min(0f)] private float _defendersHealthPercent = 0.75f;
        [Header("Animation")]
        [SerializeField, Min(0f)] private float _castlePartsDropStep = 0.1f;
        [SerializeField] private bool _isNeedAnimation = true;
        [SerializeField, Min(0)] private float _shakeTime = 0.25f;
        [SerializeField] private Vector2 _shakePerlinVector;
        [SerializeField] private AzureLevelAnalyticsData _analyticsData;

        private float _nextPartDropStep; 
        private int _currentHealth;
        private int _maxHealth;
        private bool _isAlive = true;
        private Coroutine _castleShakeCoroutine;
        private Vector3 _defaultPosition;
        private List<Warrior> _defendersToObserveDamageList;

        public AzureLevelAnalyticsData AnalyticsData => _analyticsData;
        public int WarriorsCount => _defenders.Count;
        public float HealthLeftPercent => (float)_currentHealth / _maxHealth;
        public int LevelCompleteTime => _levelCompleteTime;
        public int LevelFailTime => _levelFailTime;
        public int MaxAttackPower => _maxAttackPower;

        public event Action<int> HealthInited;
        public event Action<float, int> HealthChanged;
        public event UnityAction Died;

        private void OnDisable()
        {
            foreach (var defender in _defendersToObserveDamageList)
            {
                defender.WarriorDamaged -= OnDefenderDamaged;
                defender.WarriorDied -= OnDefenderDied;
            }
        }

        private void Start()
        {
            HealthInited?.Invoke(_maxHealth);
            _defaultPosition = transform.position;
            _nextPartDropStep = 1f - _castlePartsDropStep;
        }

        public void Init(int health, int attackPower)
        {
            _defendersToObserveDamageList = new List<Warrior>();

            foreach (var warriorType in _defendersTypesToCountHealth)
            {
                foreach (var defender in _defenders)
                {
                    if (defender.GetWarriorType() == warriorType)
                    {
                        _defendersToObserveDamageList.Add(defender);

                        defender.WarriorDamaged += OnDefenderDamaged;
                        defender.WarriorDied += OnDefenderDied;
                    }
                }
            }

            var warriorHealth = _defendersToObserveDamageList.Count > 1 ? (int)((health * _defendersHealthPercent) / _defendersToObserveDamageList.Count) : _defenders[0].Health;
            _currentHealth = health;
            _maxHealth = _currentHealth;

            foreach (var defender in _defenders)
            {
                defender.SetAttackPower(attackPower);
                defender.SetHealth(warriorHealth);
            }
        }

        public void TakeDamageWithAnimation(int damage)
        {
            if (_castleShakeCoroutine == null && _isNeedAnimation)
            {
                _castleShakeCoroutine = StartCoroutine(ShakeAnimation());
            }

            if (_castleRigidbodyHandler != null && HealthLeftPercent <= _nextPartDropStep)
            {
                _castleRigidbodyHandler.PushCoupleParts();
                _nextPartDropStep -= _castlePartsDropStep;
            }

            TakeDamge(damage);
        }

        public void Die()
        {
            _isAlive = false;

            foreach (var defender in _defenders)
            {
                if (defender != null)
                    Destroy(defender.gameObject);
            }

            if (_castleRigidbodyHandler != null)
                _castleRigidbodyHandler.PushAllRemainRigidbodies();
        }

        private void TakeDamge(int damage)
        {
            if (damage < 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(damage)} can't be less, than 0! It equals {damage} now!");
            }

            _currentHealth -= damage;
            HealthChanged?.Invoke(HealthLeftPercent, _currentHealth);

            if (_currentHealth <= 0 && _isAlive)
            {
                Died?.Invoke();
                Die();
            }
        }

        private void OnDefenderDamaged(int damage) => TakeDamge(damage);

        private void OnDefenderDied(Warrior warrior)
        {
            warrior.WarriorDamaged -= OnDefenderDamaged;
            warrior.WarriorDied -= OnDefenderDied;
        }

        private IEnumerator ShakeAnimation()
        {
            var time = 0f;

            while (time < _shakeTime)
            {
                yield return null;

                var newPosition = _defaultPosition;
                newPosition.x += Mathf.PerlinNoise(_shakePerlinVector.x, _shakePerlinVector.y) * time;
                newPosition.z += Mathf.PerlinNoise(_shakePerlinVector.x, _shakePerlinVector.y) * time;
                transform.position = newPosition;

                time += Time.deltaTime;
            }

            transform.position = _defaultPosition;
            _castleShakeCoroutine = null;
        }

#if UNITY_EDITOR
        public void FillDefenders()
        {
            _defenders = GetComponentsInChildren<Warrior>().ToList();
            Save();
        }

        private void Save() => UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
