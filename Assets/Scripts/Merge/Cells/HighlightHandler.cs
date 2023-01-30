using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace MergeAndFight.Merge
{
    public class HighlightHandler : MonoBehaviour
    {
        //[SerializeField] private ParticleSystem _mergeableEffect;
        //[SerializeField] private ParticleSystem _mergeableWithPickedEffect;
        [Header("Transparency")]
        [SerializeField, Min(0f)] private float _currentCellHighlightTransparency = 0.4f;
        [SerializeField, Min(0f)] private float _initialCellHighlightTransparency = 0.4f;
        [SerializeField, Min(0f)] private float _mergeableCellHighlightTransparency = 0.4f;
        [SerializeField, Min(0f)] private float _transparencyChangeTime = 0.25f;
        [Header("UI")]
        [SerializeField] private Image _initialCellImage;
        [SerializeField] private Image _currentCellImage;
        [SerializeField] private Image _mergeableImage;
        [SerializeField] private Canvas _canvas;

        private Image _currentHighlightImage;
        private Tween _colorChangeTween;

        private void Awake()
        {
            DisableHighlights();
            DisableMergeableHighlight();
            DisableMergeableWithPickedHighlight();

            _canvas.worldCamera = Camera.main;
        }

        public void EnableHighlight(HighlightCellType highlightCellType)
        {
            switch (highlightCellType)
            {
                case HighlightCellType.Initial:
                    EnableInitialHighlight();
                    break;
                case HighlightCellType.Current:
                    EnableCurrentCellHighlight();
                    break;
                case HighlightCellType.Mergeable:
                    EnableMergeableHighlight();
                    break;
                case HighlightCellType.MergeableWithPicked:
                    EnableMergeableWithPickedHighlight();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void DisableHighlight(HighlightCellType highlightCellType)
        {
            switch (highlightCellType)
            {
                case HighlightCellType.Initial:
                case HighlightCellType.Current:
                    DisableHighlights();
                    break;
                case HighlightCellType.Mergeable:
                    DisableMergeableHighlight();
                    break;
                case HighlightCellType.MergeableWithPicked:
                    DisableMergeableWithPickedHighlight();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void EnableInitialHighlight() => EnableHighlight(_initialCellImage, _initialCellHighlightTransparency);

        private void EnableCurrentCellHighlight()
        {
            if (_currentHighlightImage == null)
                EnableHighlight(_currentCellImage, _currentCellHighlightTransparency);
        }

        private void EnableHighlight(Image highlightImage, float transparancy)
        {
            _colorChangeTween?.Kill();

            var newColor = highlightImage.color;
            newColor.a = transparancy;
            _colorChangeTween = highlightImage.DOColor(newColor, _transparencyChangeTime);

            _currentHighlightImage = highlightImage;
        }

        private void EnableMergeableHighlight()
        {
            _colorChangeTween?.Kill();

            var newColor = _mergeableImage.color;
            newColor.a = _mergeableCellHighlightTransparency;
            _colorChangeTween = _mergeableImage.DOColor(newColor, _transparencyChangeTime);
        }

        private void EnableMergeableWithPickedHighlight() { }

        private void DisableHighlights()
        {
            DisableHighlight(_currentCellImage);
            DisableHighlight(_initialCellImage);

            _currentHighlightImage = null;
        }

        private void DisableMergeableHighlight() => DisableHighlight(_mergeableImage);

        private void DisableHighlight(Image highlightImage)
        {
            _colorChangeTween?.Kill();

            var newColor = highlightImage.color;
            newColor.a = 0f;
            highlightImage.color = newColor;
        }

        private void DisableMergeableWithPickedHighlight() { }
    }

    public enum HighlightCellType
    {
        Initial,
        Current,
        Mergeable,
        MergeableWithPicked
    }
}
