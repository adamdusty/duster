namespace Duster.App;

public struct TimeTracker
{
    public double Accumulator;
    public double FrameTime;
    public double Begin;
    public double End;
    public double InterpolatedTime;
    public double FixedDeltaTime;

    public TimeTracker(double fixedDeltaTime)
    {
        FixedDeltaTime = fixedDeltaTime;
        Accumulator = 0;
        FrameTime = 0;
        Begin = 0;
        End = 0;
        InterpolatedTime = 0;
    }
}