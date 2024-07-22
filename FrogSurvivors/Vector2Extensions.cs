using System.Numerics;

namespace FrogSurvivors;

public static class Vector2Extensions
{
    public static void Normalize(this ref Vector2 vector)
    {
        vector = Vector2.Normalize(vector);
        if (float.IsNaN(vector.X)) vector.X = 0;
        if (float.IsNaN(vector.Y)) vector.Y = 0;
    }
}