using System.Numerics;
using FrogSurvivors.Collision;
using FrogSurvivors.Sprites;
using FrogSurvivors.Tiles;

namespace FrogSurvivors.GameObjects;

public class Bat : Enemy
{
    private readonly Sprite _sprite;

    public Bat(Vector2 position, GameObject target, TiledTileSheet tileSheet) : base(position,
        new Collider(position -new Vector2(5, 5), new Vector2(10, 10)), target, 0, 1.0f)
    {
        _sprite = new Sprite(tileSheet, 418);
    }

    protected override float GetSpeed()
    {
        return 10;
    }

    public override void Render()
    {
        _sprite.Render(Position, Rotation, Scale, Vector2.Zero);
        base.Render();
    }
}