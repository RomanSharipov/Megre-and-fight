using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MergeAndFight.Fight
{
    public class CastleHealthView : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private ViewPanel _healthPanel;
        [SerializeField] private Image _statickHealthImage;
        [SerializeField] private Image _dynamicHealthImage;
        [SerializeField] private TMPro.TMP_Text _healthText;
        [SerializeField, Min(0f)] private float _animationTime = 0.2f;

        private Castle _castle;

        private void Awake()
        {
            _healthPanel.DisableView();
        }

        private void OnDisable()
        {
            _castle.HealthInited -= OnHealthInited;
            _castle.HealthChanged -= OnHealthChanged;
            _castle.Died -= OnCastleDied;
        }

        public void Init(Castle castle)
        {
            _castle = castle;

            _castle.HealthInited += OnHealthInited;
            _castle.HealthChanged += OnHealthChanged;
            _castle.Died += OnCastleDied;
        }

        private void OnHealthInited(int maxHealth)
        {
            _statickHealthImage.fillAmount = 1;
            _dynamicHealthImage.fillAmount = 1;
            _healthText.text = maxHealth.ToString();

            _healthPanel.EnableView();
        }

        private void OnHealthChanged(float healthLeftPercent, int currentHealth)
        {
            _healthText.text = currentHealth.ToString();
            _statickHealthImage.fillAmount = healthLeftPercent;
            _dynamicHealthImage.DOFillAmount(healthLeftPercent, _animationTime);
        }

        private void OnCastleDied() => _healthPanel.DisableView();
    }
}
