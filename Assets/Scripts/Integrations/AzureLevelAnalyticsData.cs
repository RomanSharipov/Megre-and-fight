using System;
using UnityEngine;

[Serializable]
public class AzureLevelAnalyticsData
{
    [SerializeField] private LevelDifficulty _levelDifficulty;
    [SerializeField] private LevelType _levelType;

    public string GetLevelDifficulty()
    {
        return _levelDifficulty switch
        {
            LevelDifficulty.Easy => "easy",
            LevelDifficulty.Normal => "normal",
            LevelDifficulty.Hard => "hard",
            _ => throw new NotImplementedException(),
        };
    }

    public string GetLevelType()
    {
        return _levelType switch
        {
            LevelType.Castle => "castle",
            LevelType.Boss => "boss",
            _ => throw new NotImplementedException(),
        };
    }
}

public enum LevelDifficulty
{
    Easy,
    Normal,
    Hard
}

public enum LevelType
{
    Castle,
    Boss
}
