using UnityEngine;

public static class MathHelper
{
    public static float Percentage(float value, float percent)
    {
        return value * (percent / 100f);
    }

    public static float Clamp01(float value)
    {
        return Mathf.Clamp01(value);
    }
}