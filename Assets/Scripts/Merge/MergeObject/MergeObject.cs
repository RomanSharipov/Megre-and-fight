using MergeAndFight.Fight;
using System;
using UnityEngine;

namespace MergeAndFight.Merge
{
    public class MergeObject : MonoBehaviour
    {
        private const string PickedUpAnimation = "PickedUp";

        [Tooltip("Left next level object empty if it's max level object.")]
        [SerializeField] private MergeObject _nextLevelObject;
        [SerializeField] private Warrior _warrior;
        [SerializeField] private WarriorType _warriorType;
        [SerializeField] private Animator _animator;
        [Header("Max amount")]
        [SerializeField, Min(1)] private int _amount = 1;
        [SerializeField, Min(1)] private int _maxAmount = 5;
        [Header("Movement")]
        [SerializeField] private float _placeHeight = 0.5f;
        [SerializeField] private float _ascendHeight = 1f;

        private bool _isPickedUp = false;

        public Warrior Warrior => _warrior;
        public MergeObject NextLevelObject => _nextLevelObject;
        public int Amount => _amount;
        public int MaxAmount => _maxAmount;
        public WarriorType WarriorType => _warriorType;
        public int Level => _warrior.CurrentLevel;

        public event Action<int, bool> AmountUpdated;
        public event Action SwitchedToBattleState;

        private void Awake()
        {
            _animator.applyRootMotion = false;
        }

        public void MoveTowardsPointer(Vector3 pointerPosition)
        {
            if (_isPickedUp == false)
            {
                _isPickedUp = true;
                _animator.SetBool(PickedUpAnimation, _isPickedUp);
            }

            transform.position = new Vector3(pointerPosition.x, _placeHeight + _ascendHeight, pointerPosition.z);
        }

        public void PlaceOnCentralPoint(Vector3 centralPoint)
        {
            _isPickedUp = false;
            _animator.SetBool(PickedUpAnimation, _isPickedUp);

            transform.position = new Vector3(centralPoint.x, _placeHeight, centralPoint.z);
        }

        public void SetAmount(int amount)
        {
            if (amount < 1)
                throw new ArgumentOutOfRangeException($"{nameof(amount)} can't be less, than 1! Now it equals {amount}!");

            _amount = amount;

            if (_amount > 1)
                AmountUpdated?.Invoke(_amount, _amount >= _maxAmount);
        }

        public void SwitchToBattleState() => SwitchedToBattleState?.Invoke();
    }
}
