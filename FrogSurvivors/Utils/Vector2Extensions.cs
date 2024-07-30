using System.Numerics;

namespace FrogSurvivors.Utils;

public static class Vector2Extensions
{
    public static void Normalize(this ref Vector2 vector)
    {
        vector = Vector2.Normalize(vector);
        if (float.IsNaN(vector.X)) vector.X = 0;
        if (float.IsNaN(vector.Y)) vector.Y = 0;
    }

    public static Vector2 SafeNormalized(this Vector2 vector)
    {
        vector = Vector2.Normalize(vector);
        if (float.IsNaN(vector.X)) vector.X = 0;
        if (float.IsNaN(vector.Y)) vector.Y = 0;
        return vector;
    }
}