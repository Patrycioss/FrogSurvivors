using System.Numerics;
using FrogSurvivors.Rendering;
using FrogSurvivors.Utils;
using Raylib_cs;

namespace FrogSurvivors;

public static class Program
{
    private static Game _game = null!;

    public static void Main()
    {
        Window.Initialize(new Vector2(1920, 1200), true);

        _game = new Game();

        while (!Window.ShouldClose())
        {
            Time.Update();
            Window.Update();

            _game.Update();
            _game.Render();
            _game.Draw();
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}