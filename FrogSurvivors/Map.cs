using System.Numerics;
using FrogSurvivors.Collision;
using FrogSurvivors.Tiles;
using Raylib_cs;

namespace FrogSurvivors;

public class Map
{
    public TiledTileSheet TileSheet { get; }
    
    private readonly TiledTileMap _map;

    public Map(string path)
    {
        _map = new TiledTileMap(path);
        TileSheet = _map.TileSheet;
    }

    public List<Collider> GetColliders()
    {
        return _map.GetRectColliders();
    }

    public void Draw()
    {
        _map.Render(Vector2.Zero, 0, 1, Color.White);
    }
}