namespace OldBit.Z80.Net;

public sealed class StatesCounter
{
    private int _maxStates;

    public void Reset()
    {
        TotalStates = 0;
        CurrentStates = 0;
    }

    public void Add(int states)
    {
        TotalStates += states;
        CurrentStates += states;
    }

    /// <summary>
    /// Limits the number of T-states that should execute.
    /// </summary>
    /// <param name="maxCycles">The maximum number of T-states to execute.</param>
    public void Limit(int maxCycles)
    {
        var remaining = _maxStates - CurrentStates;
        _maxStates = maxCycles + remaining;
        CurrentStates = 0;
    }

    /// <summary>
    /// Returns true if number of executed T-states reached the maximum.
    /// </summary>
    public bool IsComplete => _maxStates != 0 && CurrentStates >= _maxStates;

    /// <summary>
    /// Gets the total number of T-states since boot or hard reset.
    /// </summary>
    public long TotalStates { get; private set; }

    /// <summary>
    /// Gets the number of T-states executed in the current run.
    /// </summary>
    public int CurrentStates { get; private set; }
}