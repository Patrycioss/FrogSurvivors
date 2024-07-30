namespace FrogSurvivors.Utils;

public static class MathHelpers
{
    public static float MapValue(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}