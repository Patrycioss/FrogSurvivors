using System.Numerics;

namespace FrogSurvivors.Collision;

public struct CollisionInfo
{
    public Vector2 ContactPoint;
    public Vector2 ContactNormal;
    public float ContactTime;
}