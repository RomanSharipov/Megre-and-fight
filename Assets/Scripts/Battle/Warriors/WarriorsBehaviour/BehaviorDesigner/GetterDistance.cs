using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace MergeAndFight.Fight
{
    public class GetterDistance : Action
    {
        [SerializeField] SharedTransform _target;

        private GetterClosestPointOnCollider _getterClosestPointOnCollider = new GetterClosestPointOnCollider();

        public SharedVector3 _totalTargetPoint;
        public SharedFloat _currentDistance;

        public override TaskStatus OnUpdate()
        {
            if (_getterClosestPointOnCollider.TryGetClosestPoint(transform,_target.Value,out Vector3 closestPoint))
            {
                _totalTargetPoint.Value = closestPoint;
            }

            else
            {
                _totalTargetPoint.Value = _target.Value.position;
            }
            _currentDistance.Value = Vector3.Distance(transform.position, _totalTargetPoint.Value);

            return TaskStatus.Success;
        }
    }
}
