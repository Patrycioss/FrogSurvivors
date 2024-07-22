using System.Numerics;
using Raylib_cs;

namespace FrogSurvivors;

public class Sprite
{
    private Texture2D _texture;
    private Vector2 _offset;
    private Vector2 _size;

    public Sprite(Texture2D texture, int columns, int rows, int frame, int frameSpacing, Vector2 size)
    {
        _texture = texture;
        _offset = SpriteSheetUtils.CalculateFramePosition(frame, size, columns, frameSpacing);
        _size = size;
    }

    public void Render(Vector2 position, float rotation, float scale)
    {
        Rectangle source = new Rectangle(_offset, _size);
        Rectangle destination = new Rectangle(position, source.Size * scale);

        Raylib.DrawTexturePro(_texture, source, destination, destination.Size / 2.0f, rotation,
            Color.White);
    }
}