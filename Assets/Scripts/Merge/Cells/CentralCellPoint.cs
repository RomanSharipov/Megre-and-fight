using UnityEngine;

namespace MergeAndFight.Merge
{
    public class CentralCellPoint : MonoBehaviour
    {
        private void OnValidate()
        {
            transform.localPosition = new Vector3(0f, transform.parent.localScale.y / 2, 0f);
        }
    }
}
