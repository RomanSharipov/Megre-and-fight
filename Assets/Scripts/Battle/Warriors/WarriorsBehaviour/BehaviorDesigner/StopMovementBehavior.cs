using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace MergeAndFight.Fight
{
    [RequireComponent(typeof(Warrior))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class StopMovementBehavior : Action 
    {
        private Animator _animator;
        private NavMeshAgent _navMeshAgent;

        public override void OnAwake()
        {
            _animator = GetComponent<Warrior>().Animator;
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public override TaskStatus OnUpdate()
        {
            _animator.SetBool(AnimationPrefs.Idle, true);
            _animator.SetBool(AnimationPrefs.Run, false);
            _navMeshAgent.isStopped = true;
            return TaskStatus.Success;
        }
    }
}

