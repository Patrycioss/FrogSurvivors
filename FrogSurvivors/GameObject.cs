using System.Numerics;
using FrogSurvivors.Collision;
using FrogSurvivors.Utils;
using Raylib_cs;

namespace FrogSurvivors;

public class GameObject
{
    public bool IsMoving { get; protected set; }
    
    public Vector2 Position
    {
        get => _position;
        set
        {
            Collider.Position = (Collider.Position - _position) + value;
            _position = value;
        }
    }

    public Vector2 Velocity
    {
        get => Collider.Velocity;
        set => Collider.Velocity = value;
    }

    public Collider Collider { get; }
    public bool EnableDebug = false;
    
    // Not sure what to do with these yet...
    protected float Rotation;
    protected float Scale;
    
    private Vector2 _position;

    protected GameObject(Vector2 position, Collider collider, float rotation, float scale)
    {
        _position = position;
        Collider = collider;

        Rotation = rotation;
        Scale = scale;
    }

    public virtual void Update()
    {
    }

    public virtual void Render()
    {
        if (EnableDebug)
        {
            Collider.Draw(Color.Red);
            
            Draw.Circle(Position, 1, Color.Gold);
        }
    }
}