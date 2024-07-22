using System.Numerics;
using Raylib_cs;

namespace FrogSurvivors;

public class AnimatedSprite
{
    public struct Frame
    {
        public readonly Vector2 Position;

        public Frame(Vector2 position)
        {
            Position = position;
        }
    }

    public int StartFrame = 0;
    public int CycleFrames;
    public int FrameRate = 20;
    public bool FlipHorizontal = false;
    public bool FlipVertical = false;
    public int CurrentFrame { get; private set; }

    public readonly int Columns;
    public readonly int Rows;
    public readonly Vector2 FrameSize;
    public readonly Frame[] Frames;

    private readonly Texture2D _texture;
    private float _currentDelay;

    public AnimatedSprite(Texture2D texture, int columns, int rows, Vector2 frameSize, int frameSpacing, int frameCount,
        int offset = 0)
    {
        _texture = texture;
        Columns = columns;
        Rows = rows;
        FrameSize = new Vector2(frameSize.X, frameSize.Y);
        CycleFrames = frameCount;
        Frames = new Frame[frameCount];
        for (int i = 0; i < frameCount; i++)
        {
            Frames[i] = new Frame(SpriteSheetUtils.CalculateFramePosition(i + offset, FrameSize, Columns, frameSpacing));
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
                    CurrentFrame = StartFrame;
                }

                add--;
            }

            _currentDelay = 0;
        }
        
      
    }

    public void Render(Vector2 position, float rotation, float scale)
    {
        Frame frame = Frames[CurrentFrame];
        Vector2 sourceSize = FrameSize;
        
        if (FlipHorizontal) sourceSize.X *= -1;
        if (FlipVertical) sourceSize.Y *= -1;
        
        Rectangle source = new Rectangle(frame.Position, sourceSize);
        Rectangle destination = new Rectangle(position, FrameSize * scale);

        Raylib.DrawTexturePro(_texture, source, destination, (FrameSize * scale) / 2.0f, rotation,
            Color.White);
    }
}