using BehaviorDesigner.Runtime;
using UnityEngine;

namespace MergeAndFight.Fight
{
    [RequireComponent(typeof(AtackableTarget))]
    public class Archer : Warrior
    {
        [SerializeField] private Arrow _arrowTemplate;

        public override void Attack()
        {
            _enemy = (SharedTransform)BehaviorTree.GetVariable(EnemyTransform);
            if (Enemy.Value as UnityEngine.Object == null)
                return;

            ShootArrow(SelfTargetType);
        }

        private void ShootArrow(TypeDetectableTarget targetType)
        {
            Arrow newArrow = Instantiate(_arrowTemplate, transform.position, transform.rotation);
            newArrow.Init(Enemy.Value.position, targetType, AttackPower);
        }
    }
}
