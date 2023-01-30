using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;
using UnityEngine.AI;

namespace MergeAndFight.Fight
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class CheckTargetIsNull : Conditional
    {
        [SerializeField] private SharedTransform _target;

        private NavMeshAgent _navMeshAgent;

        public override void OnAwake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public override TaskStatus OnUpdate()
        {
            if (_target.Value as UnityEngine.Object != null)
            {
                return TaskStatus.Success;
            }

            else
            {
                _navMeshAgent.isStopped = true;
                return TaskStatus.Failure;
            }
        }
    }
}
