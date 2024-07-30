using System.Numerics;
using FrogSurvivors.Sprites;

namespace FrogSurvivors.Tiles;

public class SpriteSheetUtils
{
    public static Vector2 CalculateFramePosition(int frame, Vector2 frameSize, int columnCount, int frameSpacing)
    {
        int startRow = frame / columnCount;
        float startY = startRow * frameSize.Y + startRow * frameSpacing;
        int startColumn = frame % columnCount;
        float startX = startColumn * frameSize.X + (startColumn * frameSpacing);

        return new Vector2(startX, startY);
    }

    public static Sprite SpriteFromSheet(TiledTileSheet tileSheet, int frame)
    {
        return new Sprite(tileSheet.SheetTexture, tileSheet.ColumnCount, tileSheet.RowCount, frame,
            tileSheet.TileSpacing, tileSheet.TileSize);
    }
}