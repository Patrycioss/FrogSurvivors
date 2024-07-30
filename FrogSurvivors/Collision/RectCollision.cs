using System.Numerics;

namespace FrogSurvivors.Collision;

public static class RectCollision
{
    public static bool Overlaps(this Collider collider, Vector2 point)
    {
        return point.X >= collider.Position.X && point.X < collider.Position.X + collider.Size.X && point.Y >= collider.Position.Y &&
               point.Y < collider.Position.Y + collider.Size.Y;
    }

    public static bool Overlaps(this Collider incoming, Collider target)
    {
        return incoming.Position.X < target.Position.X + target.Size.X &&
               incoming.Position.X + incoming.Size.X > target.Position.X &&
               incoming.Position.Y < target.Position.Y + target.Size.Y &&
               incoming.Position.Y + incoming.Size.Y > target.Position.Y;
    }

    public static bool Overlaps(this Collider collider, Vector2 rayOrigin, Vector2 rayDirection,
        out CollisionInfo collisionInfo)
    {
        collisionInfo = new CollisionInfo();

        Vector2 near = (collider.Position - rayOrigin) / rayDirection;
        Vector2 far = (collider.Position + collider.Size - rayOrigin) / rayDirection;

        if (float.IsNaN(near.X) || float.IsNaN(near.Y)) return false;
        if (float.IsNaN(far.X) || float.IsNaN(far.Y)) return false;

        if (near.X > far.X) (near.X, far.X) = (far.X, near.X);
        if (near.Y > far.Y) (near.Y, far.Y) = (far.Y, near.Y);

        if (near.X > far.Y || near.Y > far.X)
        {
            return false;
        }

        collisionInfo.ContactTime = Math.Max(near.X, near.Y);
        float hitFar = Math.Min(far.X, far.Y);

        if (hitFar < 0) return false;

        collisionInfo.ContactPoint = rayOrigin + (collisionInfo.ContactTime * rayDirection);

        if (near.X > near.Y)
        {
            if (rayDirection.X < 0)
                collisionInfo.ContactNormal = new Vector2(1, 0);
            else
                collisionInfo.ContactNormal = new Vector2(-1, 0);
        }
        else if (near.X < near.Y)
        {
            if (rayDirection.Y < 0)
                collisionInfo.ContactNormal = new Vector2(0, 1);
            else
                collisionInfo.ContactNormal = new Vector2(0, -1);
        }

        return true;
    }

    public static bool Overlaps(this Collider incoming, Collider target, float deltaTime, out CollisionInfo collisionInfo)
    {
        collisionInfo = new CollisionInfo();

        if (MathF.Abs(incoming.Velocity.X) < 0.001f && MathF.Abs(incoming.Velocity.Y) < 0.001f)
        {
            return false;
        }
        
        Collider expandedTarget = new Collider(target.Position - (incoming.Size / 2.0f), target.Size + incoming.Size);

        if (expandedTarget.Overlaps(incoming.Position + incoming.Size / 2.0f, incoming.Velocity * deltaTime,
                out collisionInfo))
        {
            if (collisionInfo.ContactTime <= 1.0f && collisionInfo.ContactTime >= -1.0f)
            {
                return true;
            }
        }

        return false;
    }
}