using UnityEngine;

namespace MergeAndFight.Fight
{
    public class Elephant : Warrior
    {
        [SerializeField] private DamageCollisionHandler _weapon;

        private void Start()
        {
            _weapon.Init(base.SelfTargetType, AttackPower);
        }

        public override void Attack()
        {
            if (Enemy.Value as UnityEngine.Object == null)
                return;

            Debug.Log("Attack");
        }
    }
}
