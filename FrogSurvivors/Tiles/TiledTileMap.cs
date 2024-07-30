using System.Numerics;
using System.Xml;
using FrogSurvivors.Collision;
using Raylib_cs;

namespace FrogSurvivors.Tiles;

public class TiledTileMap
{
    public readonly TiledTileSheet TileSheet;
    public readonly int Width;
    public readonly int Height;
    public readonly int[,] Tiles;
    public readonly Image Image;
    public readonly int TileWidth;
    public readonly int TileHeight;
    public readonly Texture2D Texture;


    public TiledTileMap(string path)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(path);

        string? directory = Path.GetDirectoryName(path);
        directory ??= Environment.CurrentDirectory;

        var tilesetAttribs = xmlDoc.GetElementsByTagName("tileset")[0]!.Attributes;

        string source = tilesetAttribs!["source"]?.Value!;
        TileSheet = new TiledTileSheet(Path.Combine(directory, source));

        var mapAttribs = xmlDoc.GetElementsByTagName("map")[0]?.Attributes;

        if (mapAttribs != null)
        {
            Width = int.Parse(mapAttribs.GetNamedItem("width")?.Value ??
                              throw new Exception($"Failed to get map width attribute from Tilemap {path}!"));
            Height = int.Parse(mapAttribs.GetNamedItem("height")?.Value ??
                               throw new Exception($"Failed to get height attribute from TileMap {path}!"));
            TileWidth = int.Parse(mapAttribs.GetNamedItem("tilewidth")?.Value ??
                                  throw new Exception($"Failed to get tilewidth attribute from TileMap {path}!"));
            TileHeight = int.Parse(mapAttribs.GetNamedItem("tileheight")?.Value ??
                                   throw new Exception($"Failed to get tileheight attribute from TileMap {path}!"));
        }

        Tiles = new int[Width, Height];

        var data = xmlDoc.GetElementsByTagName("data")[0];

        if (data == null)
        {
            throw new Exception($"Could not read chunk element from TileMap {path}!");
        }

        string csv = data.InnerText;

        if (string.IsNullOrWhiteSpace(csv))
        {
            throw new Exception($"Failed to read map data from chunk in map: {path}!");
        }

        string[] lines = csv.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        for (var i = 0; i < lines.Length; i++)
        {
            string[] values =
                lines[i].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            for (var v = 0; v < values.Length; v++)
            {
                Tiles[v, i] = int.Parse(values[v]);
            }
        }

        Image = Raylib.GenImageColor(Width * TileWidth, Height * TileHeight, Color.Red);

        for (var columnIndex = 0; columnIndex < Width; columnIndex++)
        {
            for (var rowIndex = 0; rowIndex < Height; rowIndex++)
            {
                int number = Tiles[columnIndex, rowIndex];

                if (number <= 0) break;

                Vector2 tilePosition = new Vector2(columnIndex * TileSheet.TileWidth,
                    rowIndex * TileSheet.TileHeight);

                Raylib.ImageDraw(ref Image, TileSheet.SheetImage,
                    new Rectangle(TileSheet.GetTilePosition(number), TileSheet.TileSize),
                    new Rectangle(tilePosition, TileSheet.TileSize), Color.White);
            }
        }

        Texture = Raylib.LoadTextureFromImage(Image);
        Raylib.ExportImage(Image, "TileMap.png");
    }
    
    public List<Collider> GetRectColliders()
    {
        var colliders = new List<Collider>();

        for (int columnIndex = 0; columnIndex < Tiles.GetLength(0); columnIndex++)
        {
            for (int rowIndex = 0; rowIndex < Tiles.GetLength(1); rowIndex++)
            {
                int number = Tiles[columnIndex, rowIndex];

                if (TileSheet.Properties[number].HasFlag(TileProperties.Collider))
                {
                    Collider collider =
                        new Collider(new Vector2(columnIndex * TileWidth, rowIndex * TileHeight),
                            TileSheet.TileSize);
                    
                    colliders.Add(collider);
                }
            }
        }

        return colliders;
    }

    public void Render(Vector2 position, float rotation, float scale, Color tint)
    {
        Raylib.DrawTextureEx(Texture, position, rotation, scale, tint);
    }
}