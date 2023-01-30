using UnityEngine;

public class CustomPlayerPrefs : PlayerPrefs
{
    private static int IntIfTrueValue = 101;
    private static int IntIfFalseValue = 0;

    public static void SetBool(string key, bool value)
    {
        if (value)
            SetInt(key, IntIfTrueValue);
        else
            SetInt(key, IntIfFalseValue);
    }

    public static bool GetBool(string key)
    {
        return GetInt(key, IntIfFalseValue) == IntIfTrueValue;
    }
}
