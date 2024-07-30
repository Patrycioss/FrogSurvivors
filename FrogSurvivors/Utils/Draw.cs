using System.Numerics;
using Raylib_cs;

namespace FrogSurvivors.Utils;

/// <summary>
/// Helper class to make Raylib primitive shape drawing functions less of a pain to call.
/// </summary>
public static class Draw
{
    /// <summary>
    /// <inheritdoc cref="Raylib.DrawRectangle"/>
    /// </summary>
    public static void Rectangle(Vector2 position, Vector2 size, Color color)
    {
        Raylib.DrawRectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y, color);
    }

    /// <summary>
    /// <inheritdoc cref="Raylib.DrawRectangle"/>
    /// </summary>
    public static void Rectangle(Vector2 position, float width, float height, Color color)
    {
        Rectangle(position, new Vector2(width, height), color);
    }

    /// <summary>
    /// <inheritdoc cref="Raylib.DrawRectangle"/>
    /// </summary>
    public static void Rectangle(float x, float y, float width, float height, Color color)
    {
        Rectangle(new Vector2(x, y), width, height, color);
    }

    /// <summary>
    /// <inheritdoc cref="Raylib.DrawRectangle"/>
    /// </summary>
    public static void Rectangle(float x, float y, Vector2 size, Color color)
    {
        Rectangle(new Vector2(x, y), size, color);
    }

    /// <summary>
    /// <inheritdoc cref="Raylib.DrawLine"/>
    /// </summary>
    public static void DrawLine(Vector2 start, Vector2 end, Color color)
    {
        Raylib.DrawLine((int)start.X, (int)start.Y, (int)end.X, (int)end.Y, color);
    }

    /// <summary>
    /// <inheritdoc cref="Raylib.DrawCircle"/>
    /// </summary>
    public static void Circle(Vector2 position, float radius, Color color)
    {
        Raylib.DrawCircle((int)position.X, (int)position.Y, radius, color);
    }
}