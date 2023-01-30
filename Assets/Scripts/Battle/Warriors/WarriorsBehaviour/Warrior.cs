using BehaviorDesigner.Runtime;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

namespace MergeAndFight.Fight
{
    [RequireComponent(typeof(AtackableTarget))]
    public abstract class Warrior : MonoBehaviour, IAttackable
    {
        private const int DefaultAttackPower = -100;
        private const string DeathAnimation = "Dead";
        protected const string EnemyTransform = "_enemyTransform";

        [Header("Warrior behavior")]
        [SerializeField] private WarriorData _warriorData;
        [SerializeField] private BehaviorTree _behaviorTree;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField, Min(1)] private int _currentLevel = 1;
        [SerializeField] private bool _isBoss = false;
        [SerializeField] private Vector3 _unitFightScale;
        [Tooltip("Sound might be null for enemies")]
        [SerializeField] private AudioSource _spawnSound;
        [Header("Warrior death")]
        [SerializeField, Min(0f)] private float _deathTime = 3f;
        [SerializeField] private DeathMaterial _deathMaterial;

        private Animator _animator;
        protected SharedTransform _enemy;
        private AtackableTarget _selfAttackable;
        private int _currentHealth = -1;
        private int _attackPower = DefaultAttackPower;
        private GameStateHandler _gameStateHandler;
        private Coroutine _deathCoroutine;

        protected BehaviorTree BehaviorTree => _behaviorTree;
        protected SharedTransform Enemy => _enemy;

        public int Health => _warriorData.Health * _currentLevel;
        public int AttackPower => _attackPower;
        public int CurrentLevel => _currentLevel;
        public Animator Animator => _animator;
        public bool IsAlive => _currentHealth > 0;
        public TypeDetectableTarget SelfTargetType => _selfAttackable.TargetType;

        public event Action<int> WarriorDamaged;
        public event Action<Warrior> WarriorDied;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _selfAttackable = GetComponent<AtackableTarget>();

            if (_spawnSound != null)
                _spawnSound.playOnAwake = false;

            _currentHealth = _currentHealth == -1 ? Health : _currentHealth;
            _attackPower = _attackPower == DefaultAttackPower ? _warriorData.AttackPower * _currentLevel : _attackPower;
        }

        private void OnDisable()
        {
            if (_gameStateHandler != null)
            {
                _gameStateHandler.PlayerWon -= OnPlayerWon;
            }
        }

        public virtual void Init(GameStateHandler gameStateHandler)
        {
            _spawnSound.Play();
            transform.localScale = _unitFightScale;

            _gameStateHandler = gameStateHandler;
            _gameStateHandler.PlayerWon += OnPlayerWon;

            _behaviorTree.enabled = true;
            _navMeshAgent.enabled = true;
            _enemy = (SharedTransform)BehaviorTree.GetVariable(EnemyTransform);
        }

        public WarriorType GetWarriorType()
        {
            return this.GetType().Name switch
            {
                nameof(SwordMan) => WarriorType.Swordman,
                nameof(Archer) => WarriorType.Archer,
                nameof(Giant) => WarriorType.Giant,
                nameof(Elephant) => WarriorType.Elephant,
                _ => throw new NotImplementedException("Unknown warrior type!"),
            };
        }

        public void SetHealth(int health)
        {
            if (health <= 0)
                throw new ArgumentOutOfRangeException($"{nameof(health)} can't be less, than 1! Now it equals {health}");

            _currentHealth = health;
        }

        public void SetAttackPower(int attackPower)
        {
            if (attackPower <= 0)
                throw new ArgumentOutOfRangeException($"{nameof(attackPower)} can't be less, than 1! Now it equals {attackPower}");

            _attackPower = attackPower;
        }

        public abstract void Attack();

        public void TakeDamageWithAnimation(int damage)
        {
            if (damage < 0)
                throw new ArgumentOutOfRangeException($"Damage can't be less, than 0! It's now {damage} on {gameObject}");

            if (_isBoss)
                return;

            WarriorDamaged?.Invoke(damage);
            _currentHealth -= damage;

            if (_currentHealth <= 0 && _deathCoroutine == null)
                Die();
        }

        public void Die()
        {
            _deathCoroutine = StartCoroutine(DeathCoroutine());
        }

        public IEnumerator DeathCoroutine()
        {
            if (_gameStateHandler != null)
            {
                _gameStateHandler.PlayerWon -= OnPlayerWon;
            }

            WarriorDied?.Invoke(this);
            _animator.SetTrigger(DeathAnimation);
            _navMeshAgent.enabled = false;
            _behaviorTree.enabled = false;

            if (_deathMaterial.IsMaterialAbleToChanged)
                _deathMaterial.SetDieMaterial();

            yield return new WaitForSeconds(_deathTime);

            transform.DOMove(transform.position + Vector3.down * _deathTime * transform.localScale.y, _deathTime);
            Destroy(gameObject, _deathTime);
        }

        private void OnPlayerWon(int currencyForLevel)
        {
            _behaviorTree.enabled = false;
            _animator.Play(AnimationPrefs.Victory, 0, UnityEngine.Random.value);
        }
    }
}
