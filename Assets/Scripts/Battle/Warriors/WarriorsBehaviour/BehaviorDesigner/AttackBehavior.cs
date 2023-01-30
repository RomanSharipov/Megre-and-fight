using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;
using MergeAndFight.Fight;

namespace MergeAndFight.Fight
{
    [RequireComponent(typeof(Warrior))]
    public class AttackBehavior : Action
    {
        [SerializeField] SharedTransform _target;
        [SerializeField] SharedVector3 _totalTargetPoint = new SharedVector3();
        [SerializeField] SharedFloat _attackDistanceInScript;
        [SerializeField] SharedFloat _currentDistanceInScript;
        [SerializeField] private float _delayBetweenAttack;
        [SerializeField] private float _rotationSpeed;

        private GetterClosestPointOnCollider _getterClosestPointOnCollider = new GetterClosestPointOnCollider();
        private Animator _animator;
        private Warrior _warriorSelf;
        private float _lastAttackTime;

        private float _currentDistance;

        public override void OnAwake()
        {
            _animator = GetComponent<Warrior>().Animator;
        }

        public override void OnStart()
        {
            _lastAttackTime = 0;
        }

        public override TaskStatus OnUpdate()
        {
            if (_target.Value as UnityEngine.Object == null)
            {
                _animator.Play(AnimationPrefs.Idle);
                return TaskStatus.Failure;
            }

            if (_getterClosestPointOnCollider.TryGetClosestPoint(transform, _target.Value, out Vector3 closestPoint))
            {
                _totalTargetPoint.Value = closestPoint;
            }

            _currentDistanceInScript.Value = Vector3.Distance(transform.position, _totalTargetPoint.Value);

            if (_currentDistanceInScript.Value > _attackDistanceInScript.Value)
            {
                return TaskStatus.Failure;
            }

            if (_target.Value.TryGetComponent(out Warrior warrior) && warrior.IsAlive == false)
            {
                _target.Value = null;
                return TaskStatus.Failure;
            }

            TryAttack();

            return TaskStatus.Running;
        }

        private void TryAttack()
        {
            RotateTo(_totalTargetPoint.Value, _rotationSpeed);

            if (_lastAttackTime <= 0)
            {
                _animator.CrossFade(AnimationPrefs.Attack,0.1f,-1,0.0f);
                _lastAttackTime = _delayBetweenAttack;
            }

            _lastAttackTime -= Time.deltaTime;
        }

        public void RotateTo(Vector3 target, float rotationSpeed)
        {
            Vector3 direction = target - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Quaternion lookAtRotationOnly_Y = Quaternion.Euler(transform.rotation.eulerAngles.x, targetRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotationOnly_Y, rotationSpeed * Time.deltaTime);
        }
    }
}

