using System.Numerics;
using Raylib_cs;

namespace FrogSurvivors.Sprites;

public static class SpriteLoader
{
    private static Dictionary<string, AnimatedSprite?> _animatedSprites = new();

    public static AnimatedSprite? LoadAnimatedSprite(string name, Texture2D texture, int columns, int rows,
        Vector2 frameSize, int frameSpacing, int frameCount, int offset = 0)
    {
        AnimatedSprite? animatedSprite;
        if (_animatedSprites.TryGetValue(name, out animatedSprite))
        {
            return animatedSprite;
        }

        animatedSprite = new AnimatedSprite(texture, columns, frameSize, frameSpacing, frameCount, offset);
        _animatedSprites.Add(name, animatedSprite);
        return animatedSprite;
    }

    public static bool TryGetAnimatedSprite(string name, out AnimatedSprite? animatedSprite)
    {
        return _animatedSprites.TryGetValue(name, out animatedSprite);
    }
}