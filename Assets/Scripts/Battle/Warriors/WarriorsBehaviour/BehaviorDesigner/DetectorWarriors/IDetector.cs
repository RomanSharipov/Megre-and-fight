using UnityEngine;

namespace MergeAndFight.Fight
{
    interface IDetector
    {
        public bool TryGetNearbyTarget(Transform selfTransform, float maxDistanceForFind, out Transform target);
    }
}
