using IJunior.TypedScenes;
using MergeAndFight.Merge;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MergeAndFight.Fight
{
    public class BattleSceneLoad : MonoBehaviour, ISceneLoadHandler<List<MergeObject>>
    {
        [Header("Warriors spawner")]
        [SerializeField] private WarriorsFightSpawner _warriorsSpawner;
        [Header("Warriors pick view")]
        [SerializeField] private WarriorPickView _archersPickView;
        [SerializeField] private WarriorPickView _swordmansPickView;
        [SerializeField] private WarriorPickView _giantsPickView;
        [SerializeField] private WarriorPickView _elephantsPickView;
        [Header("Castle")]
        [SerializeField] private List<Castle> _castlesList;
        [SerializeField] private Transform _castleSpawnPoint;
        [SerializeField, Min(1)] private int _castleIndexStartRandomIndex = 1;
        [SerializeField] private CastleHealthView _castleHealthView;
        [Header("Game state handler")]
        [SerializeField] private GameStateHandler _gameStateHandler;

        private Castle _currentCastle;

        public void OnSceneLoaded(List<MergeObject> argument)
        {
            ParseMergeObjectsList(argument);
        }

        private void ParseMergeObjectsList(List<MergeObject> mergeObjects)
        {
            List<Warrior> archers = new List<Warrior>();
            List<Warrior> swordmans = new List<Warrior>();
            List<Warrior> giants = new List<Warrior>();
            List<Warrior> elephants = new List<Warrior>();

            foreach (MergeObject mergeObject in mergeObjects)
            {
                SceneManager.MoveGameObjectToScene(mergeObject.gameObject, SceneManager.GetActiveScene());
                mergeObject.SwitchToBattleState();
            }

            mergeObjects.ForEach(mergeObject =>
            {
                if (mergeObject.Warrior.GetType() == typeof(Archer))
                {
                    for (int i = 0; i < mergeObject.Amount; i++)
                    {
                        archers.Add((Archer)mergeObject.Warrior);
                    }
                }
            });

            mergeObjects.ForEach(mergeObject =>
            {
                if (mergeObject.Warrior.GetType() == typeof(SwordMan))
                    for (int i = 0; i < mergeObject.Amount; i++)
                    {
                        swordmans.Add((SwordMan)mergeObject.Warrior);
                    }
            });

            mergeObjects.ForEach(mergeObject =>
            {
                if (mergeObject.Warrior.GetType() == typeof(Giant))
                    for (int i = 0; i < mergeObject.Amount; i++)
                    {
                        giants.Add((Giant)mergeObject.Warrior);
                    }
            });

            mergeObjects.ForEach(mergeObject =>
            {
                if (mergeObject.Warrior.GetType() == typeof(Elephant))
                {
                    elephants.Add((Elephant)mergeObject.Warrior);
                }
            });

            List<WarriorPickView> warriorPickViews = new List<WarriorPickView>();
            warriorPickViews.Add(_archersPickView);
            warriorPickViews.Add(_swordmansPickView);
            warriorPickViews.Add(_giantsPickView);
            warriorPickViews.Add(_elephantsPickView);

            _warriorsSpawner.Init(ref archers, ref swordmans, ref giants, ref elephants, ref warriorPickViews);

            InitViews(archers.Count, swordmans.Count, giants.Count, elephants.Count);
            InitCastle(mergeObjects);

            _gameStateHandler.Init(_currentCastle, _castlesList.Count);
            _castleHealthView.Init(_currentCastle);
        }

        private void InitViews(int archersCount, int swordmansCount, int giantsCount, int elephantsCount)
        {
            _archersPickView.Init(archersCount);
            _swordmansPickView.Init(swordmansCount);
            _giantsPickView.Init(giantsCount);
            _elephantsPickView.Init(elephantsCount);
        }

        private void InitCastle(List<MergeObject> mergeObjects)
        {
            int completedLevels = CustomPlayerPrefs.GetInt(Prefs.LevelLoadPrefs.CompletedLevels, 1);
            completedLevels -= 1;

            completedLevels = completedLevels >= _castlesList.Count ? Random.Range(_castleIndexStartRandomIndex - 1, _castlesList.Count) : completedLevels;

            _currentCastle = Instantiate(_castlesList[completedLevels], _castleSpawnPoint);

            var castleHealth = CastleParametrsFormula.GetCastleHealth(mergeObjects, _currentCastle.LevelCompleteTime);
            var castleAttackPower = CastleParametrsFormula.GetCastleAttackPower(mergeObjects, _currentCastle.WarriorsCount, _currentCastle.LevelFailTime, _currentCastle.MaxAttackPower);

            _currentCastle.Init(castleHealth, castleAttackPower);
        }
    }
}
