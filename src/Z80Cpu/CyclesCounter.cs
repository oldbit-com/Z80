namespace OldBit.Z80Cpu;

/// <summary>
/// Utility class to count the number of T-states executed.
/// </summary>
public sealed class CyclesCounter
{
    private int _cyclesLimit;

    public void Reset()
    {
        TotalCycles = 0;
        CurrentCycles = 0;
    }

    /// <summary>
    /// Adds the specified number of T-states.
    /// </summary>
    /// <param name="cycles">The number of T-states to add.</param>
    public void Add(int cycles)
    {
        TotalCycles += cycles;
        CurrentCycles += cycles;
    }

    /// <summary>
    /// Handles the HALT instruction where normally CPU executes NOPs.
    /// </summary>
    public void Halt()
    {
        var remaining = _cyclesLimit - CurrentCycles;
        Add(remaining >= 4 ? 4 : remaining);
    }

    /// <summary>
    /// Limits the number of T-states that should be executed.
    /// </summary>
    /// <param name="cyclesLimit">The maximum number of T-states to execute.</param>
    public void Limit(int cyclesLimit)
    {
        var remaining = _cyclesLimit - CurrentCycles;
        _cyclesLimit = cyclesLimit + remaining;
        CurrentCycles = 0;
    }

    /// <summary>
    /// Returns true if number of executed T-states reached the maximum.
    /// </summary>
    public bool IsComplete => _cyclesLimit != 0 && CurrentCycles >= _cyclesLimit;

    /// <summary>
    /// Gets the total number of T-states since boot or hard reset.
    /// </summary>
    public long TotalCycles { get; private set; }

    /// <summary>
    /// Gets the number of T-states executed in the current run.
    /// </summary>
    public int CurrentCycles { get; private set; }
}