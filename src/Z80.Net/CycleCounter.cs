namespace Z80.Net;

public class CycleCounter
{
    private int _maxCycles;

    public void Reset()
    {
        TotalCycles = 0;
        CurrentCycles = 0;
    }

    public void Add(int cycles)
    {
        TotalCycles += cycles;
        CurrentCycles += cycles;
    }

    /// <summary>
    /// Limits the number of cycles that should run to the specified maximum.
    /// </summary>
    /// <param name="maxCycles">The maximum number of cycles to run.</param>
    public void SetLimit(int maxCycles)
    {
        var remaining = _maxCycles - CurrentCycles;
        _maxCycles = maxCycles + remaining;
        CurrentCycles = 0;
    }

    /// <summary>
    /// Returns true if number of executed cycles reached the maximum.
    /// </summary>
    public bool IsComplete => _maxCycles != 0 && CurrentCycles >= _maxCycles;

    /// <summary>
    /// Gets the total number of T-states since boot or hard reset.
    /// </summary>
    public long TotalCycles { get; private set; }

    /// <summary>
    /// Gets the number of T-states executed in the current run.
    /// </summary>
    public int CurrentCycles { get; private set; }
}