using MergeAndFight.Merge;
using System;
using System.Collections.Generic;

[Serializable]
public struct MergeUnitsList
{
    private List<MergeUnitData> _mergeUnitsData;

    public IReadOnlyCollection<MergeUnitData> MergeUnitsDataList => _mergeUnitsData;

    public void AddUnitData(MergeUnitData data)
    {
        if (_mergeUnitsData == null)
            _mergeUnitsData = new List<MergeUnitData>();

        _mergeUnitsData.Add(data);
    }
}
