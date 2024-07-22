using System.Numerics;
using Raylib_cs;

namespace FrogSurvivors;

public class Player : IGameObject
{
    public Vector2 Position { get; private set; }
    private readonly float _speed;

    private readonly AnimatedSprite _animatedSprite;

    public Player(AnimatedSprite animatedSprite, Vector2 position)
    {
        _animatedSprite = animatedSprite;
        _speed = 150f;
        Position = position;
    }

    public void Update(float deltaTime)
    {
        _animatedSprite.Update(deltaTime);
        
        Vector2 direction = Vector2.Zero;

        if (Raylib.IsKeyDown(KeyboardKey.W))
        {
            direction += new Vector2(0, -1);
        }

        if (Raylib.IsKeyDown(KeyboardKey.S))
        {
            direction += new Vector2(0, 1);
        }

        if (Raylib.IsKeyDown(KeyboardKey.A))
        {
            direction += new Vector2(-1, 0);
        }

        if (Raylib.IsKeyDown(KeyboardKey.D))
        {
            direction += new Vector2(1, 0);
        }
        
        _animatedSprite.FlipHorizontal = direction.X < 0;

        direction.Normalize();

        direction *= _speed * deltaTime;

        Position += direction;
    }

    public void Render()
    {
        _animatedSprite.Render(Position, 0, 1);
    }
}