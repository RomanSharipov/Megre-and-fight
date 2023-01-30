using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MergeAndFight.Fight
{
    public class GetterClosestPointOnCollider 
    {
        private LayerMask _mainTarget = LayerMask.GetMask("MainTarget");

        public bool TryGetClosestPoint(Transform self, Transform target,out Vector3 closestPoint)
        {
            Vector3 startPoint = self.transform.position;
            Vector3 endPoint = target.transform.position;
            closestPoint = new Vector3();
            
            Ray ray = new Ray(startPoint, endPoint - startPoint);
            RaycastHit hit;

            if (Physics.Raycast(ray,out hit, _mainTarget))
            {
                if (hit.collider.transform == target)
                {
                    closestPoint = hit.point;
                    return true;
                }
            }
            return false;
        }
    }
}

