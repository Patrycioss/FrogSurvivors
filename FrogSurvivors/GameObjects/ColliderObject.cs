using System.Numerics;
using FrogSurvivors.Collision;

namespace FrogSurvivors.GameObjects;

public class ColliderObject : GameObject
{
    public ColliderObject(Vector2 position, Collider collider) :
        base(position, collider, 0, 1)
    {
    }
}