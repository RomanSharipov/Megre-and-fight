using UnityEngine;

public static class TargetFramerateSetting
{
    [RuntimeInitializeOnLoadMethod()]
    private static void SetTargetFramerate()
    {
        Application.targetFrameRate = 60;
    }
}
