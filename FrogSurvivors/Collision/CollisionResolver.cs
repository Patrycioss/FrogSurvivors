using System.Numerics;
using FrogSurvivors.Utils;

namespace FrogSurvivors.Collision;

public static class CollisionResolver
{
    public static void ResolveCollisions(List<GameObject> gameObjects)
    {
        for (int i = gameObjects.Count - 1; i >= 0; i--)
        {
            if (!gameObjects[i].IsMoving)
            {
                continue;
            }

            Collider colliderI = gameObjects[i].Collider;
            List<(Collider, float)> collisionsToCheck = [];

            for (int j = gameObjects.Count - 1; j >= 0; j--)
            {
                if (j == i) continue;
                var collider = gameObjects[j].Collider;
                
                if ((collider.Position - colliderI.Position).LengthSquared() > 100 * 100)
                {
                    continue;
                }
                
                if (colliderI.Overlaps(collider, Time.DeltaTime, out CollisionInfo intialCheckInfo))
                {
                    collisionsToCheck.Add((collider, intialCheckInfo.ContactTime));
                }
            }
            
            if (collisionsToCheck.Count > 1)
            {
                for (int aColIndex = collisionsToCheck.Count - 1; aColIndex >= 0; --aColIndex)
                {
                    for (int bColIndex = aColIndex - 1; bColIndex >= 0; --bColIndex)
                    {
                        var a = collisionsToCheck[aColIndex];
                        var b = collisionsToCheck[bColIndex];

                        if (a.Item2 < b.Item2)
                        {
                            collisionsToCheck[aColIndex] = b;
                            collisionsToCheck[bColIndex] = a;
                        }
                    }
                }
            }
            
            foreach (var pair in collisionsToCheck)
            {
                if (colliderI.Overlaps(pair.Item1, Time.DeltaTime, out CollisionInfo actualInfo))
                {
                    colliderI.Velocity += actualInfo.ContactNormal *
                                      new Vector2(Math.Abs(colliderI.Velocity.X), Math.Abs(colliderI.Velocity.Y)) *
                                      (1 - actualInfo.ContactTime);
                }
            }

            gameObjects[i].Position += colliderI.Velocity * Time.DeltaTime;
        }
    }
}