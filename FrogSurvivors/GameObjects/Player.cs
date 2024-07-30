using System.Numerics;
using FrogSurvivors.Collision;
using FrogSurvivors.Sprites;
using FrogSurvivors.Tiles;
using FrogSurvivors.Utils;
using Raylib_cs;

namespace FrogSurvivors.GameObjects;

public class Player : GameObject
{
    private const float HopDuration = 0.3f;
    private const float Speed = 100;
    private const float HopCooldown = 0.2f;

    private readonly AnimatedSprite _animatedSprite;

    private float _lastHopTime;
    private float _lastMoveTime;

    private bool _ableToMove = true;

    private Vector2 _moveDirection;
    private float _facingDirection = 1;

    public Player(Vector2 position, TiledTileSheet tileSheet) : base(position,
        new Collider(position + new Vector2(-6,-6), new Vector2(12, 10)), 0, 1.0f)
    {
        var playerSprite = new AnimatedSprite(tileSheet.SheetTexture, tileSheet.ColumnCount,
            tileSheet.TileSize, tileSheet.TileSpacing, 6, 459);
        playerSprite.FrameRate = 10;

        Texture2D playerTexture = Raylib.LoadTexture("Resources/Player.png");

        _animatedSprite = new AnimatedSprite(playerTexture, 3, new Vector2(16, 16), 0, 3, 0);
        _animatedSprite.FrameRate = 10;
        _animatedSprite.SetCycle(1, 0);

        _lastMoveTime = -HopCooldown;
    }

    public override void Update()
    {
        _animatedSprite.Update(Time.DeltaTime);

        if (_ableToMove && Time.ElapsedTime >= _lastMoveTime + HopCooldown)
        {
            if (CheckForPlayerInput(out Vector2 direction))
            {
                _moveDirection = direction;
                _ableToMove = false;
                IsMoving = true;
                _lastHopTime = Time.ElapsedTime;

                if (_moveDirection.X != 0)
                {
                    _facingDirection = _moveDirection.X > 0 ? 1 : -1;
                }

                _animatedSprite.FlipHorizontal = _facingDirection < 0;
                _animatedSprite.SetCycle(3, 0);
            }
        }

        if (IsMoving)
        {
            Velocity = _moveDirection * Speed;

            float moveTimeLeft = _lastHopTime + HopDuration - Time.ElapsedTime;

            if (moveTimeLeft < 0)
            {
                _animatedSprite.SetCycle(1, 0);
                IsMoving = false;
                _ableToMove = true;
                _lastMoveTime = Time.ElapsedTime;
            }
        }
    }
    
    private bool CheckForPlayerInput(out Vector2 direction)
    {
        direction = Vector2.Zero;

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

        if (direction.X != 0 || direction.Y != 0)
        {
            direction.Normalize();
            return true;
        }

        return false;
    }

    public override void Render()
    {
        _animatedSprite.Render(Position, Rotation, Scale, new Vector2(0,4));

        base.Render();
    }
}