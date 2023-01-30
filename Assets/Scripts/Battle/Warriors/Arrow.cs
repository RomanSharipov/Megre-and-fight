using System.Collections;
using UnityEngine;

namespace MergeAndFight.Fight
{
    public class Arrow : MonoBehaviour
    {
        [SerializeField] private float _flySpeed;
        [SerializeField] private AnimationCurve _flyCurve;
        [SerializeField] private ParticleSystem _collisionEffect;
        [SerializeField] private BrokenArrow _brokenArrowTemplate;
        [SerializeField] private AudioSource _flyArrowSound;

        private int _damage = -1;
        private Vector3 _target;
        private TypeDetectableTarget _enemyWarriorType;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out AtackableTarget atackableTarget))
            {

                if (atackableTarget.TargetType == _enemyWarriorType)
                    return;

                if (other.TryGetComponent(out IAttackable attackable))
                {
                    attackable.TakeDamageWithAnimation(_damage);
                    Instantiate(_brokenArrowTemplate, transform.position, transform.rotation);

                    if (other.TryGetComponent(out Castle castle))
                    {
                        Instantiate(_collisionEffect, transform.position, transform.rotation);
                    }

                    Destroy(gameObject);
                }
            }
        }

        public void Init(Vector3 target, TypeDetectableTarget targetWarriorType, int damage)
        {
            _enemyWarriorType = targetWarriorType;
            _target = target;
            _damage = damage;
            _flyArrowSound.Play();
            StartCoroutine(FlyToTarget(_target, _flyCurve));
        }

        private IEnumerator FlyToTarget(Vector3 endPosition, AnimationCurve ascendCurve)
        {
            float currentTime = 0;
            Vector3 newPosition;
            Vector3 startPosition = transform.position;
            Vector3 previousPosition = transform.position;

            while (currentTime < _flySpeed)
            {
                currentTime += Time.deltaTime;

                newPosition = Vector3.Lerp(startPosition, endPosition, currentTime / _flySpeed);

                var currentHeight = Mathf.Lerp(startPosition.y, endPosition.y, currentTime / _flySpeed);
                newPosition.y = currentHeight + ascendCurve.Evaluate(currentTime / _flySpeed);
                transform.position = newPosition;

                Vector3 direction = (transform.position - previousPosition).normalized;
                transform.LookAt(direction + transform.position);

                previousPosition = transform.position;

                yield return null;
            }

            Destroy(gameObject);
        }
    }
}