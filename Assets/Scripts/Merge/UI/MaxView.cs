using DG.Tweening;
using UnityEngine;

namespace MergeAndFight.Merge
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MaxView : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float _fadeTime = 1f;
        [SerializeField] private float _ascendHeight = 1.5f;
        
        private CanvasGroup _canvasGroup;

        private void OnEnable()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            _canvasGroup.DOFade(0f, _fadeTime);
            transform.DOMoveY(transform.position.y + _ascendHeight, _fadeTime);

            Destroy(gameObject, _fadeTime + 0.01f);
        }
    }
}
