using System.Numerics;
using Raylib_cs;

namespace FrogSurvivors.Rendering;

public class RenderTexture
{
    private readonly RenderTexture2D _renderTexture;

    public RenderTexture(int width, int height, TextureFilter textureFilter = TextureFilter.Bilinear)
    {
        _renderTexture = Raylib.LoadRenderTexture(width, height);
        Raylib.SetTextureFilter(_renderTexture.Texture, textureFilter);
    }

    public RenderTexture(Vector2 resolution, TextureFilter textureFilter = TextureFilter.Bilinear)
    {
        _renderTexture = Raylib.LoadRenderTexture((int)resolution.X, (int)resolution.Y);
        Raylib.SetTextureFilter(_renderTexture.Texture, textureFilter);
    }

    public static void Enable(RenderTexture renderTexture)
    {
        Raylib.BeginTextureMode(renderTexture._renderTexture);
    }

    public static void Disable()
    {
        Raylib.EndTextureMode();
    }

    public void Draw(Vector2 size, float scale)
    {
        Vector2 texturePosition = size / 2.0f;
        Vector2 textureSize = size * scale;
        texturePosition -= textureSize / 2.0f;

        Raylib.DrawTexturePro(_renderTexture.Texture,
            new Rectangle(0, 0, _renderTexture.Texture.Width, -_renderTexture.Texture.Height),
            new Rectangle(texturePosition, textureSize),
            Vector2.Zero, 0, Color.White);
    }

    ~RenderTexture()
    {
        Raylib.UnloadRenderTexture(_renderTexture);
    }
}