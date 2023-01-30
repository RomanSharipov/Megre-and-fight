using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MergeAndFight.Fight
{
    public class CastleRigidbodyHandler : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float _pushForce = 0.01f;
        [SerializeField, Min(1)] private int _partsToPushInProgress = 3;
        [SerializeField] private Vector2 _xDirectionRange;
        [SerializeField] private Vector2 _zDirectionRange;

        private List<Rigidbody> _rigidbodies = new List<Rigidbody>();

        private void Awake()
        {
            AttachRigidbodyToMeshes();
        }

        public void PushAllRemainRigidbodies()
        {
            foreach (var rigidbody in _rigidbodies)
            {
                rigidbody.useGravity = true;
                rigidbody.isKinematic = false;

                Vector3 pushDirection = new Vector3(Random.Range(_xDirectionRange.x, _xDirectionRange.y), 0f
                    , Random.Range(_zDirectionRange.x, _zDirectionRange.y));
                rigidbody.AddForce(pushDirection * _pushForce, ForceMode.Impulse);
            }
        }

        public void PushCoupleParts()
        {
            for (int rigidbodyIndex = 0; rigidbodyIndex <_partsToPushInProgress; rigidbodyIndex += 1)
            {
                var rigidbody = _rigidbodies[rigidbodyIndex];
                _rigidbodies.RemoveAt(rigidbodyIndex);

                rigidbody.useGravity = true;
                rigidbody.isKinematic = false;

                Vector3 pushDirection = new Vector3(Random.Range(_xDirectionRange.x, _xDirectionRange.y), 0f
                    , Random.Range(_zDirectionRange.x, _zDirectionRange.y));

                rigidbody.AddForce(pushDirection * _pushForce, ForceMode.Impulse);
            }
        }

        private void AttachRigidbodyToMeshes()
        {
            _rigidbodies.Clear();
            var meshes = GetComponentsInChildren<MeshRenderer>().ToList();

            meshes.ForEach(mesh =>
            {
                var collider = mesh.gameObject.AddComponent<MeshCollider>();
                collider.convex = true;

                var rigidbody = mesh.gameObject.AddComponent<Rigidbody>();
                _rigidbodies.Add(rigidbody);
                rigidbody.useGravity = false;
                rigidbody.isKinematic = true;
                rigidbody.mass = 100000f;
            });
        }
    }
}
