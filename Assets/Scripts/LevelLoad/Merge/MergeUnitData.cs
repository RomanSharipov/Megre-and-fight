using System;

[Serializable]
public struct MergeUnitData
{
    public int CellID { get; private set; }
    public int Level { get; private set; }
    public int Amount { get; private set; }
    public WarriorType WarriorType { get; private set; }

    public MergeUnitData(int cellID, int level, int amount, WarriorType warriorType)
    {
        if (cellID < 0)
            throw new ArgumentOutOfRangeException($"{nameof(cellID)} can't be less, than 0! It equals {cellID} now!");

        if (level < 0)
            throw new ArgumentOutOfRangeException($"{nameof(level)} can't be less, than 0! It equals {level} now!");

        if (amount < 0)
            throw new ArgumentOutOfRangeException($"{nameof(amount)} can't be less, than 0! It equals {amount} now!");

        CellID = cellID;
        Level = level;
        Amount = amount;
        WarriorType = warriorType;
    }
}

[Serializable]
public enum WarriorType
{
    Swordman,
    Archer,
    Giant,
    Elephant
}
