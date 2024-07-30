using Raylib_cs;

namespace FrogSurvivors.Utils;

public static class Time
{
    public static float DeltaTime { get; private set; }
    public static float ElapsedTime { get; private set; }

    public static void Update()
    {
        DeltaTime = Raylib.GetFrameTime();
        ElapsedTime = (float) Raylib.GetTime();
    }
}