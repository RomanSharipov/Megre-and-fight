using InvokeWithDelay;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MergeAndFight.Fight
{
    public class FightTutorial : MonoBehaviour
    {
        private const string FightTutorialShown = "FightTutorialShown";

        [Header("Pick warrior type step")]
        [SerializeField] private ViewPanel _pickWarriorStepPanel;
        [SerializeField] private ViewPanel _finger;
        [SerializeField] private List<WarriorPickView> _warriorPickViews;
        [Header("Spawn warrior step")]
        [SerializeField] private ViewPanel _buttonHidePanel;
        [SerializeField] private ViewPanel _spawnWarriorStepPanel;
        [SerializeField] private WarriorsFightSpawner _warriorsFightSpawner;
        [Header("Keep spawn step")]
        [SerializeField] private ViewPanel _keepSpawnViewPanel;
        [SerializeField, Min(0f)] private float _viewDisableTime = 3f;

        private void Start()
        {
            _buttonHidePanel.DisableView();
            _pickWarriorStepPanel.DisableView();
            _spawnWarriorStepPanel.DisableView();
            _keepSpawnViewPanel.DisableView();
            _finger.DisableView();

            if (CustomPlayerPrefs.GetBool(FightTutorialShown) == false)
                StartTutorial();
        }

        private void StartTutorial()
        {
            _pickWarriorStepPanel.EnableView();
            _finger.EnableView();

            foreach (var warriorPickView in _warriorPickViews)
            {
                warriorPickView.WarriorTypeChanged += OnWarriorTypeChanged;
            }
        }

        private void OnWarriorTypeChanged(WarriorType warriorType, WarriorPickView warriorPickView)
        {
            _pickWarriorStepPanel.DisableView();
            _finger.DisableView();

            foreach (var pickView in _warriorPickViews)
            {
                pickView.WarriorTypeChanged -= OnWarriorTypeChanged;
            }

            _spawnWarriorStepPanel.EnableView();
            _buttonHidePanel.EnableView();
            _warriorsFightSpawner.WarriorSpawned += OnWarriorSpawned;
        }

        private void OnWarriorSpawned()
        {
            _spawnWarriorStepPanel.DisableView();
            _buttonHidePanel.DisableView();
            _warriorsFightSpawner.WarriorSpawned -= OnWarriorSpawned;

            CustomPlayerPrefs.SetBool(FightTutorialShown, true);
            _keepSpawnViewPanel.EnableView();
            this.Invoke(() => _keepSpawnViewPanel.DisableView(), _viewDisableTime);
        }
    }
}
