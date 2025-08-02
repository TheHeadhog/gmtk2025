public struct TimeRange
{
    public readonly int BeginTick;
    public readonly int EndTick;

    public TimeRange(int beginTick, int endTick)
    {
        BeginTick = beginTick;
        EndTick = endTick;
    }

    public bool Overlaps(TimeRange other)
    {
        return BeginTick < other.EndTick && other.BeginTick < EndTick;
    }
}