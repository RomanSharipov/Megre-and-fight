using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;
using UnityEngine.AI;

namespace MergeAndFight.Fight
{
    [RequireComponent(typeof(Warrior))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class MovementToTargetBehavior : Action
    {
        [SerializeField] private SharedTransform _currentTarget;
        [SerializeField] private SharedFloat _attackDistanceInScript;
        [SerializeField] private SharedFloat _currentDistanceInScript;
        [SerializeField] private SharedVector3 _totalTargetPoint;

        private Animator _animator;
        private NavMeshAgent _navMeshAgent;

        public override void OnAwake()
        {
            _animator = GetComponent<Warrior>().Animator;
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public override TaskStatus OnUpdate()
        {
            MoveToTarget(_totalTargetPoint.Value);

            return TaskStatus.Success;
        }

        private void MoveToTarget(Vector3 point)
        {
            _animator.SetBool(AnimationPrefs.Run, true);
            _animator.SetBool(AnimationPrefs.Idle, false);
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(point);
        }
    }
}
