using System.Numerics;
using Raylib_cs;

namespace FrogSurvivors.Rendering;

public static class Window
{
    public static Vector2 Resolution { get; private set; }
    public static int Width { get; private set; }
    public static int Height { get; private set; }
    
    public static Vector2 MousePosition { get; private set; }
    
    public static bool ResolutionChanged { get; private set; }

    public static void Initialize(Vector2 startResolution, bool fullScreen)
    {
        Resolution = startResolution;

        ConfigFlags configFlags;

        if (fullScreen)
        {
            configFlags = ConfigFlags.FullscreenMode | ConfigFlags.BorderlessWindowMode | ConfigFlags.VSyncHint;
        }
        else
        {
            configFlags = ConfigFlags.VSyncHint;
        }

        Raylib.SetConfigFlags(configFlags);
        Raylib.InitWindow((int)startResolution.X, (int)startResolution.Y, "Frog Survivors");
        
        GetWindowSize();
    }

    public static void Update()
    {
        ResolutionChanged = false;
        MousePosition = Raylib.GetMousePosition();
        GetWindowSize();
    }

    public static bool ShouldClose()
    {
        return Raylib.WindowShouldClose();
    }


    public static void SetResolution(Vector2 resolution)
    {
        Resolution = resolution;
        ResolutionChanged = true;
    }

    private static void GetWindowSize()
    {
        Width = Raylib.GetScreenWidth();
        Height = Raylib.GetScreenHeight();
    }
}