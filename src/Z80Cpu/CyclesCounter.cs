namespace OldBit.Z80Cpu;

public sealed class CyclesCounter
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

    public void Halt()
    {
        var remaining = _maxCycles - CurrentCycles;
        Add(remaining >= 4 ? 4 : remaining);
    }

    /// <summary>
    /// Limits the number of T-states that should execute.
    /// </summary>
    /// <param name="maxCycles">The maximum number of T-states to execute.</param>
    public void Limit(int maxCycles)
    {
        var remaining = _maxCycles - CurrentCycles;
        _maxCycles = maxCycles + remaining;
        CurrentCycles = 0;
    }

    /// <summary>
    /// Returns true if number of executed T-states reached the maximum.
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