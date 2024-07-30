using System.Numerics;
using Raylib_cs;

namespace FrogSurvivors.Collision;


public class Collider
{
    /// <summary>
    /// Position of the top left corner of the shape.
    /// </summary>
    public Vector2 Position;

    /// <summary>
    /// Velocity of the shape.
    /// </summary>
    public Vector2 Velocity;
    
    /// <summary>
    /// Size of the rectangle.
    /// </summary>
    public Vector2 Size;
    
    public Collider(Vector2 position, Vector2 size)
    {
        Position = position;
        Velocity = Vector2.Zero;
        Size = size;
    }
    
    /// <summary>
    /// Draw the collider to the screen. Useful for debugging.
    /// </summary>
    /// <param name="color">Color to use for drawing the shape.</param>
    public void Draw(Color color)
    {
        Utils.Draw.Rectangle(Position, Size, color);
    }
}