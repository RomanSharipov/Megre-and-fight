using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;
using UnityEngine.AI;


namespace MergeAndFight.Fight
{
    [RequireComponent(typeof(Warrior))]
    public class FindTargetsBehavior : Action
    {
        [SerializeField, Min(0)] SharedFloat _maxDistanceForFind;
        [SerializeField] private SharedTransform _findedTarget;
        [SerializeField] private TypeDetectableTarget _typeTarget;

        private Animator _animator;
        private IDetector _detectorTarget;
        private NavMeshAgent _navMeshAgent;

        public override void OnStart()
        {
            _animator.SetBool(AnimationPrefs.Idle, true);
        }

        public override void OnAwake()
        {
            _animator = GetComponent<Warrior>().Animator;
            _navMeshAgent = GetComponent<NavMeshAgent>();

            switch (_typeTarget)
            {
                case TypeDetectableTarget.Player:
                    _detectorTarget = new DetectorTarget<PlayerTag>();
                    break;
                case TypeDetectableTarget.Enemy:
                    _detectorTarget = new DetectorTarget<EnemyTag>();
                    break;
                case TypeDetectableTarget.Castle:
                    _detectorTarget = new DetectorTarget<CastleTag>();
                    break;
                default:
                    break;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (_findedTarget.Value as UnityEngine.Object != null)
            {
                return TaskStatus.Failure;
            }

            if (_detectorTarget.TryGetNearbyTarget(transform, _maxDistanceForFind.Value, out Transform target))
            {
                _findedTarget.Value = target;

                return TaskStatus.Success;
            }

            _animator.SetBool(AnimationPrefs.Idle, true);
            _animator.SetBool(AnimationPrefs.Run, false);

            if (_navMeshAgent != null && _navMeshAgent.enabled == true)
                _navMeshAgent.isStopped = true;

            return TaskStatus.Running;
        }
    }
}
