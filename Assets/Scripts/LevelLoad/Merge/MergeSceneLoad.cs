using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MergeAndFight.Merge
{
    [RequireComponent(typeof(MergeDataSave))]
    public class MergeSceneLoad : MonoBehaviour
    {
        [SerializeField] private CellsField _cellsField;
        [SerializeField] private CellBuyHandler _cellBuyHandler;
        [SerializeField] private List<MergeObject> _mergeObjects;

        private MergeDataSave _dataSave;
        private Vector3 _initialPosition = new Vector3(-1000f, -1000f, - 1000f);

        private void Start()
        {
            _dataSave = GetComponent<MergeDataSave>();

            _cellBuyHandler.LoadCells();
            LoadUnitsData();
        }

        private void LoadUnitsData()
        {
            var data = _dataSave.GetLoadData();

            if (data.MergeUnitsDataList != null)
            {
                foreach (var unit in data.MergeUnitsDataList)
                {
                    var mergeObjectToInstantiate = _mergeObjects.FirstOrDefault(mergeObj => mergeObj.WarriorType == unit.WarriorType
                        && mergeObj.Level == unit.Level);

                    if (mergeObjectToInstantiate == null)
                        throw new ArgumentNullException($"Can't find unit with parametrs: unit type {unit.WarriorType}, unit level {unit.Level}! " +
                            $"Check {nameof(MergeSceneLoad)} merge objects spawn list!");

                    var mergeObject = Instantiate(mergeObjectToInstantiate, _initialPosition, mergeObjectToInstantiate.transform.rotation);
                    mergeObject.SetAmount(unit.Amount);

                    _cellsField.AddLoadedWarrior(mergeObject, unit.CellID);
                }
            }
        }
    }
}
