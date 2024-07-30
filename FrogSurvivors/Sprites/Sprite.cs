using System.Numerics;
using FrogSurvivors.Tiles;
using Raylib_cs;

namespace FrogSurvivors.Sprites;

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

    public Sprite(TiledTileSheet sheet, int frame)
    {
        _texture = sheet.SheetTexture;
        _offset = SpriteSheetUtils.CalculateFramePosition(frame, sheet.TileSize, sheet.ColumnCount, sheet.TileSpacing);
        _size = sheet.TileSize;
    }

    public void Render(Vector2 position, float rotation, float scale, Vector2 originOffset)
    {
        Rectangle source = new Rectangle(_offset, _size);
        Rectangle destination = new Rectangle(position, source.Size * scale);

        Raylib.DrawTexturePro(_texture, source, destination, destination.Size / 2.0f + originOffset, rotation,
            Color.White);
    }
}