using System.Numerics;
using System.Xml;
using Raylib_cs;

namespace FrogSurvivors;

public struct TileProperties
{
    public readonly bool IsCollider;

    public TileProperties(bool isCollider)
    {
        if (isCollider)
        {
            Console.WriteLine($"ye");
        }

        IsCollider = isCollider;
    }
}

public class TiledTileSheet
{
    public Vector2 TileSize => new Vector2(TileWidth, TileHeight);
    public Texture2D SheetTexture { get; private set; }
    public Image SheetImage { get; private set; }
    public int TileWidth { get; private set; }
    public int TileHeight { get; private set; }
    public int TileSpacing { get; private set; }
    public int TileCount { get; private set; }
    public int ColumnCount { get; private set; }
    public int RowCount { get; private set; }

    public TileProperties[] TileProperties { get; private set; } = [];

    public TiledTileSheet(string path)
    {
        Console.WriteLine($"Loading tile sheet from: {path}");

        XmlDocument sheetXml = new XmlDocument();
        sheetXml.Load(path);

        LoadSourceImage(path, sheetXml);
        ParseSheetAttributes(sheetXml);
        ParseTileProperties(sheetXml, path);
    }

    private void ParseTileProperties(XmlDocument sheetXml, string path)
    {
        var tiles = sheetXml.GetElementsByTagName("tile");

        TileProperties = new TileProperties[TileCount];
        for (var i = 0; i < TileProperties.Length; i++)
        {
            TileProperties[i] = new TileProperties(false);
        }

        for (var i = 0; i < tiles.Count; i++)
        {
            var tile = tiles[i];

            if (tile == null)
            {
                throw new Exception($"Tile {i} not found in sheet {path}");
            }

            var tileAttribs = tile.Attributes;

            if (tileAttribs == null)
            {
                throw new Exception($"Tile {i} attributes not found in sheet {path}");
            }

            int number = int.Parse(tileAttribs.GetNamedItem("id")?.Value ??
                                   throw new Exception(
                                       $"Failed to get attribute value from tile element in tile sheet {path}"));

            var childNodes = tile.ChildNodes;

            XmlNodeList? tileProperties = null;

            foreach (XmlNode? childNode in childNodes)
            {
                if (childNode is { Name: "properties" })
                {
                    tileProperties = childNode.ChildNodes;
                }
            }

            var isCollider = false;

            if (tileProperties != null)
            {
                foreach (XmlNode property in tileProperties)
                {
                    string name = property.Attributes!.GetNamedItem("name")?.Value ??
                                  throw new Exception(
                                      $"Failed to get attribute name from property in tile sheet {path}");
                    string value = property.Attributes!.GetNamedItem("value")?.Value ??
                                   throw new Exception(
                                       $"Failed to get attribute value from property in tile sheet {path}");

                    isCollider = name switch
                    {
                        "IsCollider" => value == "true",
                        _ => isCollider
                    };
                }
            }

            TileProperties[number] = new TileProperties(isCollider);
        }
    }

    private void ParseSheetAttributes(XmlDocument sheetXml)
    {
        var tilesetAttribs = sheetXml.GetElementsByTagName("tileset")[0]?.Attributes;

        var tileWidthAttrib = tilesetAttribs?.GetNamedItem("tilewidth");
        TileWidth = int.Parse(tileWidthAttrib?.Value ??
                              throw new Exception("Couldn't parse tile width attribute from tilesheet"));

        var tileHeightAttrib = tilesetAttribs?.GetNamedItem("tileheight");
        TileHeight = int.Parse(tileHeightAttrib?.Value ??
                               throw new Exception("Couldn't parse tile height attribute from tilesheet"));

        var tileSpacingAttrib = tilesetAttribs?.GetNamedItem("spacing");
        TileSpacing = int.Parse(tileSpacingAttrib?.Value ??
                                throw new Exception("Couldn't parse tile spacing attribute from tilesheet"));

        var tileCountAttrib = tilesetAttribs?.GetNamedItem("tilecount");
        TileCount = int.Parse(tileCountAttrib?.Value ??
                              throw new Exception("Couldn't parse tile count attribute from tilesheet"));

        var columnCountAttrib = tilesetAttribs?.GetNamedItem("columns");
        ColumnCount = int.Parse(columnCountAttrib?.Value ??
                                throw new Exception("Couldn't parse columns attribute from tilesheet"));

        RowCount = (int)Math.Ceiling(TileCount / (float)ColumnCount);
    }

    private void LoadSourceImage(string path, XmlDocument sheetXml)
    {
        string? directory = Path.GetDirectoryName(path);

        string? imagePath = Path.Join(directory,
            sheetXml.GetElementsByTagName("image").Item(0)?.Attributes?.GetNamedItem("source")?.Value);

        if (imagePath == null)
        {
            throw new Exception($"Couldn't read source image path from tilesheet {imagePath}.");
        }

        SheetImage = Raylib.LoadImage(imagePath);
        SheetTexture = Raylib.LoadTexture(imagePath);
        Raylib.SetTextureFilter(SheetTexture, TextureFilter.Point);
    }

    public Vector2 GetTilePosition(int number)
    {
        if (number > TileCount || number < 1)
        {
            throw new Exception(
                $"Tile number {number} is not a valid tile. It has to be bigger than 0 and at most be {TileCount}.");
        }

        number--;
        
        Vector2 position = Vector2.Zero;
        
        int row = number / ColumnCount;
        position.Y = row * TileHeight + row * TileSpacing;

        int column = number % ColumnCount;
        position.X = column * TileWidth + (column * TileSpacing);

        return position;
    }

    public void RenderTile(int number, int posX, int posY, float rotation, float scale = 1.0f)
    {
        if (number > TileCount || number < 1)
        {
            throw new Exception(
                $"Tile number {number} is not a valid tile. It has to be bigger than 0 and at most be {TileCount}.");
        }

        number--;

        int row = number / ColumnCount;
        int y = row * TileHeight + row * TileSpacing;

        int column = number % ColumnCount;
        int x = column * TileWidth + (column * TileSpacing);

        Raylib.DrawTexturePro(SheetTexture, new Rectangle(x, y, TileWidth, TileHeight),
            new Rectangle(posX, posY, TileWidth * scale, TileHeight * scale),
            new Vector2((TileWidth * scale) / 2.0f, (TileHeight * scale) / 2.0f),
            rotation, Color.White);
    }
}