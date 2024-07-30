using System.Numerics;
using FrogSurvivors.Collision;
using FrogSurvivors.GameObjects;
using FrogSurvivors.Rendering;
using FrogSurvivors.Utils;
using Raylib_cs;

namespace FrogSurvivors;

public class Game
{
    public Vector2 MousePosition { get; private set; }
    public float Scale { get; private set; }

    private readonly List<GameObject> _gameObjects = [];
    private readonly Camera _camera;
    private readonly Map _map;
    private readonly RenderTexture _renderTexture;

    private Vector2 _cameraOffset;

    public Game()
    {
        _map = new Map("Resources/first.tmx");

        Player player = new Player(new Vector2(1550, 1600), _map.TileSheet);
        _gameObjects.Add(player);

        var mapColliders = _map.GetColliders();
        _gameObjects.Capacity = 1 + mapColliders.Count;
        foreach (var collider in mapColliders)
        {
            _gameObjects.Add(new ColliderObject(collider.Position + collider.Size / 2.0f,
                new Collider(collider.Position, collider.Size)));
        }

        _camera = new Camera(_gameObjects[0].Position, Window.Resolution / 2.0f, 0, 3);
        _renderTexture = new RenderTexture(Window.Resolution);

        RecalculateGameScale();
        RecalculateCameraOffset();

        Bat bat = new Bat(new Vector2(1800, 1600), player, _map.TileSheet);
        _gameObjects.Add(bat);
    }

    public void Update()
    {
        _camera.Update();

        if (Raylib.IsWindowResized())
        {
            RecalculateGameScale();
            RecalculateCameraOffset();
        }

        if (_camera.ZoomChanged)
        {
            RecalculateCameraOffset();
        }

        if (Window.ResolutionChanged)
        {
            RecalculateGameScale();
            RecalculateCameraOffset();
        }

        MousePosition = new Vector2(MathHelpers.MapValue(Window.MousePosition.X, 0, Window.Width,
                _camera.Target.X - _cameraOffset.X,
                _camera.Target.X + _cameraOffset.X),
            MathHelpers.MapValue(Window.MousePosition.Y, 0, Window.Height,
                _camera.Target.Y - _cameraOffset.Y,
                _camera.Target.Y + _cameraOffset.Y));

        foreach (var gameObject in _gameObjects)
        {
            gameObject.Update();
        }

        CollisionResolver.ResolveCollisions(_gameObjects);

        _camera.SetTarget(_gameObjects[0].Position);
    }

    public void Render()
    {
        Raylib.ClearBackground(Color.White);

        RenderTexture.Enable(_renderTexture);
        Raylib.BeginDrawing();

        Raylib.ClearBackground(Color.Red);

        Camera.Begin(_camera);

        _map.Draw();

        foreach (var gameObject in _gameObjects)
        {
            gameObject.Render();
        }

        Camera.End();
        RenderTexture.Disable();
    }

    public void Draw()
    {
        _renderTexture.Draw(new Vector2(Window.Width, Window.Height), Scale);
    }

    private void RecalculateGameScale()
    {
        Scale = Math.Min(Window.Width / Window.Resolution.X, Window.Height / Window.Resolution.Y);
    }

    private void RecalculateCameraOffset()
    {
        _cameraOffset = Window.Resolution / _camera.Zoom / Scale / 2.0f;
    }
}