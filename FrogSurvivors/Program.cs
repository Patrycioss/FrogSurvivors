using System.Numerics;
using Raylib_cs;

namespace FrogSurvivors;

public static class Program
{
    public static Vector2 Resolution = new Vector2(1920, 1080);
    public static Vector2 MousePosition = new Vector2(0, 0);

    public static void Main()
    {
        Raylib.SetConfigFlags(ConfigFlags.VSyncHint);
        // Raylib.SetConfigFlags(ConfigFlags.BorderlessWindowMode | ConfigFlags.VSyncHint | ConfigFlags.FullscreenMode);

        Raylib.InitWindow((int)Resolution.X, (int)Resolution.Y, "Frog Survivors");

        RenderTexture2D renderTexture = Raylib.LoadRenderTexture((int)Resolution.X, (int)Resolution.Y);
        Raylib.SetTextureFilter(renderTexture.Texture, TextureFilter.Bilinear);

        TiledTileMap mainMap = new TiledTileMap("Resources/first.tmx");

        TiledTileSheet tileSheet = mainMap.Tilesets[0].TileSheet;

        var playerSprite = new AnimatedSprite(tileSheet.SheetTexture, tileSheet.ColumnCount, tileSheet.RowCount,
            tileSheet.TileSize, tileSheet.TileSpacing, 6, 459);
        playerSprite.FrameRate = 10;
        Player player = new Player(playerSprite, new Vector2(1550, 1600));

        // Sprite test = SpriteSheetUtils.SpriteFromSheet(tileSheet, 4);

        Camera2D playerCamera = new Camera2D
        {
            Target = player.Position,
            Offset = new Vector2(Resolution.X / 2.0f, Resolution.Y / 2.0f),
            Rotation = 0,
            Zoom = 3,
        };

        var enemies = new List<Enemy>();

        int number = 1;


        while (!Raylib.WindowShouldClose())
        {
            int screenWidth = Raylib.GetScreenWidth();
            int screenHeight = Raylib.GetScreenHeight();
            float deltaTime = Raylib.GetFrameTime();
            float scale = Math.Min(screenWidth / Resolution.X, screenHeight / Resolution.Y);

            Vector2 mousePosition = Raylib.GetMousePosition();
            MousePosition.X = (mousePosition.X - (screenWidth - (Resolution.X * scale)) * 0.5f) / scale;
            MousePosition.Y = (mousePosition.Y - (screenHeight - (Resolution.Y * scale)) * 0.5f) / scale;
            MousePosition = Vector2.Clamp(MousePosition, Resolution, Resolution);

            foreach (var enemy in enemies)
            {
                enemy.Update(deltaTime);
            }

            player.Update(deltaTime);

            if (Raylib.IsKeyPressed(KeyboardKey.B))
            {
                number++;
                Console.WriteLine(number);
            }
            else if (Raylib.IsKeyPressed(KeyboardKey.Q))
            {
                number--;
                Console.WriteLine(number);
            }

            playerCamera.Target = player.Position;

            Raylib.ClearBackground(Color.White);

            Raylib.BeginTextureMode(renderTexture);
            Raylib.BeginDrawing();

            Raylib.ClearBackground(Color.Red);

            Raylib.BeginMode2D(playerCamera);

            mainMap.Render(Vector2.Zero, 0, 1, Color.White);

            foreach (var enemy in enemies)
            {
                enemy.Render();
            }

            player.Render();
            // test.Render(player.Position, 0, 10);
            
            Raylib.EndMode2D();
            Raylib.EndTextureMode();

            Raylib.DrawTexturePro(renderTexture.Texture,
                new Rectangle(0, 0, renderTexture.Texture.Width, -renderTexture.Texture.Height),
                new Rectangle(screenWidth - (Resolution.X * scale), screenHeight - (Resolution.Y * scale),
                    Resolution * scale),
                Vector2.Zero, 0, Color.White);

            Raylib.EndDrawing();
        }

        Raylib.UnloadRenderTexture(renderTexture);
        Raylib.CloseWindow();
    }

    private static void SetupWindow()
    {
    }
}