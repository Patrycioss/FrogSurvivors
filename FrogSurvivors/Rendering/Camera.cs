using System.Numerics;
using Raylib_cs;

namespace FrogSurvivors.Rendering;

public class Camera
{
    public bool ZoomChanged { get; private set; }
    public Vector2 Target => _camera.Target;
    public Vector2 Offset => _camera.Offset;
    public float Zoom => _camera.Zoom;
    public float Rotation => _camera.Rotation;

    private Camera2D _camera;

    public Camera(Vector2 target, Vector2 offset, float rotation, float zoom)
    {
        _camera = new Camera2D(offset, target, rotation, zoom);
    }

    public void SetZoom(float zoom)
    {
        _camera.Zoom = zoom;
        ZoomChanged = true;
    }

    public void Update()
    {
        ZoomChanged = false;
    }

    public void SetTarget(Vector2 target)
    {
        _camera.Target = target;
    }

    public static void Begin(Camera camera)
    {
        Raylib.BeginMode2D(camera._camera);
    }

    public static void End()
    {
        Raylib.EndMode2D();
    }
}