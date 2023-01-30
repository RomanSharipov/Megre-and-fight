using System;
using System.Collections.Generic;
using UnityEngine;

namespace MergeAndFight.Fight
{
    [Serializable]
    public class DeathMaterial
    {
        [SerializeField] private List<SkinnedMeshRenderer> _skinnedMeshRenderers;
        [SerializeField] private Material _dieMaterial;

        public bool IsMaterialAbleToChanged => _dieMaterial != null && _skinnedMeshRenderers.Count > 0;

        public void SetDieMaterial()
        {
            _skinnedMeshRenderers.ForEach(skinnedMeshRenderer => UpdateMaterial(skinnedMeshRenderer));
        }

        private void UpdateMaterial(SkinnedMeshRenderer skinnedMeshRenderer)
        {
            Material[] newMaterials = new Material[skinnedMeshRenderer.materials.Length];

            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = _dieMaterial;
            }

            skinnedMeshRenderer.materials = newMaterials;
        }
    }
}