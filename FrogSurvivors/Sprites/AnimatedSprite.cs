using System.Numerics;
using FrogSurvivors.Tiles;
using Raylib_cs;

namespace FrogSurvivors.Sprites;

public class AnimatedSprite
{
    public int StartFrame { get; private set; }
    public int CycleFrames { get; private set; }

    private int _newStartFrame = -1;
    private int _newCycleFrames = -1;
    private bool _newCycle = false;

    public int FrameRate = 20;
    public bool FlipHorizontal = false;
    public bool FlipVertical = false;
    public int CurrentFrame { get; private set; }

    public readonly Vector2 FrameSize;
    public readonly Vector2[] Frames;

    private readonly Texture2D _texture;
    private float _currentDelay;

    public AnimatedSprite(Texture2D texture, int columns, Vector2 frameSize, int frameSpacing, int frameCount,
        int offset = 0)
    {
        _texture = texture;
        FrameSize = new Vector2(frameSize.X, frameSize.Y);
        CycleFrames = frameCount;
        StartFrame = 0;
        Frames = new Vector2[frameCount];
        for (int i = 0; i < frameCount; i++)
        {
            Frames[i] = SpriteSheetUtils.CalculateFramePosition(i + offset, FrameSize, columns, frameSpacing);
        }
    }

    public void Update(float deltaTime)
    {
        _currentDelay += deltaTime;

        float a = (float)1 / FrameRate;

        if (_currentDelay >= a)
        {
            int add = (int)(_currentDelay / a);

            while (add > 0)
            {
                CurrentFrame++;

                if (CurrentFrame >= CycleFrames + StartFrame)
                {
                    if (_newCycle)
                    {
                        StartFrame = _newStartFrame;
                        CycleFrames = _newCycleFrames;
                        _newCycle = false;
                    }

                    CurrentFrame = StartFrame;
                }

                add--;
            }

            _currentDelay = 0;
        }
    }

    public void SetCycle(int cycleFrames, int startFrame)
    {
        _newStartFrame = startFrame;
        _newCycleFrames = cycleFrames;
        _newCycle = true;
    }

    public void Render(Vector2 position, float rotation, float scale, Vector2 originOffset)
    {
        Vector2 sourceSize = FrameSize;

        if (FlipHorizontal) sourceSize.X *= -1;
        if (FlipVertical) sourceSize.Y *= -1;

        Rectangle source = new Rectangle(Frames[CurrentFrame], sourceSize);
        Rectangle destination = new Rectangle(position, (FrameSize) * scale);

        Raylib.DrawTexturePro(_texture, source, destination, destination.Size/2.0f + originOffset, rotation,
            Color.White);
    }
}