namespace OldBit.Z80Cpu;

/// <summary>
/// Utility class to count the number of T-states executed.
/// </summary>
public sealed class StatesCounter
{
    private int _statesLimit;

    /// <summary>
    /// Adds the specified number of T-states.
    /// </summary>
    /// <param name="states">The number of T-states to add.</param>
    public void Add(int states)
    {
        TotalStates += states;
        CurrentStates += states;
    }

    /// <summary>
    /// Handles the HALT instruction where normally CPU executes NOPs.
    /// </summary>
    public void Halt()
    {
        var remaining = _statesLimit - CurrentStates;
        Add(remaining >= 4 ? 4 : remaining);
    }

    /// <summary>
    /// Limits the number of T-states that should be executed.
    /// </summary>
    /// <param name="statesLimit">The maximum number of T-states to execute.</param>
    public void Limit(int statesLimit)
    {
        var remaining = _statesLimit - CurrentStates;
        _statesLimit = statesLimit + remaining;
        CurrentStates = 0;
    }

    /// <summary>
    /// Returns true if number of executed T-states reached the maximum.
    /// </summary>
    public bool IsComplete => _statesLimit != 0 && CurrentStates >= _statesLimit;

    /// <summary>
    /// Gets the total number of T-states since boot or hard reset.
    /// </summary>
    public long TotalStates { get; private set; }

    /// <summary>
    /// Gets the number of T-states executed in the current run.
    /// </summary>
    public int CurrentStates { get; private set; }
}