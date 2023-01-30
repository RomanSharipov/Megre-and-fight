using UnityEngine;

namespace MergeAndFight.Fight
{
    public class Giant : Warrior
    {
        [SerializeField] private DamageCollisionHandler _weapon;

        private void Start()
        {
            _weapon.Init(base.SelfTargetType, AttackPower);
        }

        public override void Attack()
        {
            if (Enemy.Value as Object == null)
                return;

            Debug.Log("Attack");
        }
    }
}
