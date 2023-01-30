using UnityEngine;

namespace MergeAndFight.Fight
{
    public class BrokenArrow : MonoBehaviour
    {
        [SerializeField] private SetterColor _setterColor = new SetterColor();
        [SerializeField] private ArrowSegment[] _arrowSegments;
        [SerializeField] private float _lifeTime;

        private void OnEnable()
        {
            _setterColor.SetColorSmooth();
            Destroy(gameObject, _lifeTime);
        }
    }
}
