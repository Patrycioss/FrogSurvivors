using System.Numerics;
using FrogSurvivors.Collision;
using FrogSurvivors.Utils;

namespace FrogSurvivors.GameObjects;

public abstract class Enemy : GameObject
{
    protected GameObject Target { get; }
    
    protected Enemy(Vector2 position, Collider collider, GameObject target, float rotation = 0, float scale = 0) : base(position, collider, rotation, scale)
    {
        Target = target;
        IsMoving = true;
    }

    protected abstract float GetSpeed();

    public override void Update()
    {
        Vector2 direction = Target.Position - Position;
        direction.Normalize();
        Velocity = direction * GetSpeed();
    }
   
    public override void Render()
    {
        base.Render();
    }
}