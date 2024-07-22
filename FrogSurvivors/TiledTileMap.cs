using System.Numerics;
using System.Xml;
using Raylib_cs;

namespace FrogSurvivors;

public class TiledTileMap
{
    public struct Tileset(int first, TiledTileSheet tileSheet)
    {
        public readonly int First = first;
        public readonly TiledTileSheet TileSheet = tileSheet;
    }

    public readonly List<Tileset> Tilesets;
    public readonly int Width;
    public readonly int Height;
    public readonly int[,] Tiles;
    public readonly Image Image;
    public readonly int TileWidth;
    public readonly int TileHeight;
    public readonly Texture2D Texture;

    public TiledTileMap(string path)
    {
        Tilesets = [];

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(path);

        string? directory = Path.GetDirectoryName(path);
        directory ??= Environment.CurrentDirectory;

        var tilesets = xmlDoc.GetElementsByTagName("tileset");

        for (var i = 0; i < tilesets.Count; i++)
        {
            var tileset = tilesets[i];
            var tilesetAttributes = tileset!.Attributes;

            string? source = tilesetAttributes!["source"]?.Value;
            if (source == null)
            {
                Console.WriteLine($"Unable to load source path!");
                continue;
            }

            Console.WriteLine($"source: {source}");

            Tilesets.Add(new Tileset(int.Parse(tilesetAttributes!["firstgid"]!.Value),
                new TiledTileSheet(Path.Combine(directory, source))));
        }

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

                foreach (var tileset in Tilesets)
                {
                    int setFirst = tileset.First;
                    int setLast = setFirst + tileset.TileSheet.TileCount;

                    if (number >= setFirst && number <= setLast)
                    {
                        var tileSheet = tileset.TileSheet;

                        Vector2 tilePosition = new Vector2(columnIndex * tileSheet.TileWidth,
                            rowIndex * tileSheet.TileHeight);

                        Raylib.ImageDraw(ref Image, tileSheet.SheetImage,new Rectangle(tileSheet.GetTilePosition(number), tileSheet.TileSize), new Rectangle(tilePosition, tileSheet.TileSize), Color.White);
                        break;
                    }
                }
            }
        }

        Texture = Raylib.LoadTextureFromImage(Image);
        Raylib.ExportImage(Image, "TileMap.png");
    }

    public void Render(Vector2 position, float rotation, float scale, Color tint)
    {
        Raylib.DrawTextureEx(Texture, position, rotation, scale, tint);
       
    }
}