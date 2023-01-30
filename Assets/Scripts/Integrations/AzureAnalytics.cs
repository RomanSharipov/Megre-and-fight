using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AzureAnalytics : Singleton<AzureAnalytics>
{
    private int _levelComplitionTime = 0;
    private Coroutine _timer;

    public void OnLevelStart(int levelNumber, string levelName, string levelDiff, int levelLoop, int isLevelRandom,
        string levelType)
    {
        var levelCount = CustomPlayerPrefs.GetInt(Prefs.LevelLoadPrefs.LevelCount, 1);

        Dictionary<string, object> levelProperty = new Dictionary<string, object>
        {
            {"level_number", levelNumber},
            {"level_name", levelName},
            {"level_count", levelCount},
            {"level_diff", levelDiff},
            {"level_loop", levelLoop},
            {"level_random", isLevelRandom},
            {"level_type", levelType}
        };

        DebugDictionary(levelProperty);

        AppMetrica.Instance.ReportEvent("level_start", levelProperty);
        AppMetrica.Instance.SendEventsBuffer();

        _timer = StartCoroutine(StartTimer());
    }

    public void OnLevelComplete(int levelNumber, string levelName, string levelDiff, int levelLoop, int isLevelRandom, string levelType, string result, int progress)
    {
        var levelCount = CustomPlayerPrefs.GetInt(Prefs.LevelLoadPrefs.LevelCount, 1);
        StopCoroutine(_timer);

        Dictionary<string, object> userInfo = new Dictionary<string, object>
        {
            {"level_number", levelNumber},
            {"level_name", levelName},
            {"level_count", levelCount},
            {"level_diff", levelDiff},
            {"level_loop", levelLoop},
            {"level_random", isLevelRandom},
            {"level_type", levelType},
            {"time", _levelComplitionTime},
            {"result", result},
            {"progress", progress}
        };
        DebugDictionary(userInfo);

        AppMetrica.Instance.ReportEvent("level_finish", userInfo);
        AppMetrica.Instance.SendEventsBuffer();
    }

    private void DebugDictionary(Dictionary<string, object> _dictionary)
    {
        StringBuilder mesange = new StringBuilder();
        mesange.Append("Sending data: \n");
        foreach (var value in _dictionary)
        {
            mesange.Append(value.Key);
            mesange.Append(" - ");
            mesange.Append(value.Value);
            mesange.Append(", \n");
        }

        Debug.Log(mesange);
    }

    private IEnumerator StartTimer()
    {
        _levelComplitionTime = 0;
        var tick = new WaitForSecondsRealtime(1f);

        while (enabled)
        {
            _levelComplitionTime += 1;
            yield return tick;
        }
    }
}