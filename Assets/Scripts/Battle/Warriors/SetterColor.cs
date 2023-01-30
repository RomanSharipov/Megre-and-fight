using DG.Tweening;
using System;
using UnityEngine;

namespace MergeAndFight.Fight
{
    [Serializable]
    public class SetterColor
    {
        [SerializeField] Color _targetColor;
        [SerializeField] float _duration;
        [SerializeField] MeshRenderer[] _meshRenderers;

        public void SetColorSmooth()
        {
            for (int i = 0; i < _meshRenderers.Length; i++)
            {
                for (int j = 0; j < _meshRenderers[i].materials.Length; j++)
                {
                    _meshRenderers[i].materials[j].DOColor(_targetColor, _duration);
                }
            }
        }
    }
}
