using UnityEngine;

namespace MergeAndFight.Fight
{
    public abstract class AtackableTarget : MonoBehaviour 
    {
        [SerializeField] private TypeDetectableTarget _targetType;

        public TypeDetectableTarget TargetType => _targetType;
    }
}