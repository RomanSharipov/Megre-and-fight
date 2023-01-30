using SaveLoadSystem;
using System.Collections.Generic;
using UnityEngine;

namespace MergeAndFight.Merge
{
    public class MergeDataSave : MonoBehaviour
    {
        [Header("File name")]
        [SerializeField] private string _fileName = "MergeData";

        private SaveLoadSystem<MergeUnitsList> _saveLoadSystem;

        private void Awake()
        {
            _saveLoadSystem = new SaveLoadSystem<MergeUnitsList>(_fileName);
        }

        public void SaveData(List<Cell> cells)
        {
            var data = new MergeUnitsList();

            for (int i = 0; i < cells.Count; i++)
            {
                if (cells[i].IsEmpty == false)
                {
                    var id = i;
                    var level = cells[i].MergeObject.Level;
                    var amount = cells[i].MergeObject.Amount;
                    var warriorType = cells[i].MergeObject.WarriorType;

                    var unitData = new MergeUnitData(id, level, amount, warriorType);
                    data.AddUnitData(unitData);
                }
            }

            _saveLoadSystem.SaveData(data);
        }

        public MergeUnitsList GetLoadData()
        {
            return _saveLoadSystem.GetLoadedData();
        }
    }
}
