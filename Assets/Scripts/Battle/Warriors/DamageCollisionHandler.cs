using UnityEngine;

namespace MergeAndFight.Fight
{
    public class DamageCollisionHandler : MonoBehaviour
    {
        [SerializeField] private AudioSource _collisionSound;

        private int _damage;
        private TypeDetectableTarget _targetType;

        public void Init(TypeDetectableTarget targetType, int damage)
        {
            _targetType = targetType;
            _damage = damage;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IAttackable attackable) && other.TryGetComponent(out AtackableTarget target) && target.TargetType != _targetType)
            {
                attackable.TakeDamageWithAnimation(_damage);
                _collisionSound.Play();
            }
        }
    }
}
