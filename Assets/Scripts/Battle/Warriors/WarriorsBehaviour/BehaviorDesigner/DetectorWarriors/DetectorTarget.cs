using System.Linq;
using UnityEngine;

namespace MergeAndFight.Fight
{
    public class DetectorTarget<T> : IDetector where T : AtackableTarget
    {
        Collider[] _overlapColliders = new Collider[256];

        public bool TryGetNearbyTarget(Transform selfTransform, float maxDistanceForFind, out Transform target)
        {
            Transform[] targets = new Transform[5];
            int targetsIterator = 0;

            target = null;
            int overlapCount = Physics.OverlapSphereNonAlloc(selfTransform.position, maxDistanceForFind, _overlapColliders);

            float nearbyObjectDistance = float.PositiveInfinity;

            for (int colliderIterator = 0; colliderIterator < overlapCount; colliderIterator += 1)
            {
                Collider overlapCollider = _overlapColliders[colliderIterator];

                if (overlapCollider.gameObject == selfTransform.gameObject)
                    continue;

                if (overlapCollider.TryGetComponent(out Warrior warrior) && warrior.IsAlive == false)
                    continue;

                if (overlapCollider.TryGetComponent(out T detectedObject))
                {
                    float distance = Vector3.Distance(selfTransform.position, detectedObject.transform.position);

                    if (distance < nearbyObjectDistance)
                    {
                        target = detectedObject.transform;
                        nearbyObjectDistance = distance;

                        targets[targetsIterator] = target;
                        targetsIterator += 1;

                        if (targetsIterator >= 3)
                        {
                            targetsIterator = 0;
                        }
                    }
                }
            }

            if (targets.Any(trg => trg != null))
            {
                var nonNullTargets = targets.Where(trg => trg != null).ToList();
                target = nonNullTargets[Random.Range(0, nonNullTargets.Count)];
            }

            return target == null ? false : true;
        }
    }
}
