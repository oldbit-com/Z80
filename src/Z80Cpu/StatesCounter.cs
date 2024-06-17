using OldBit.Z80Cpu.Contention;

namespace OldBit.Z80Cpu;

/// <summary>
/// Utility class to count the number of T-states executed.
/// </summary>
/// <param name="contentionProvider">Provides contention states data.</param>
public sealed class StatesCounter(IContentionProvider contentionProvider)
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
    /// Adds the specified number of T-states respecting contention.
    /// </summary>
    /// <param name="address">The address of the memory that might be contended.</param>
    /// <param name="repeat">The number of repeated conentions to repeat.</param>
    /// <param name="states">The number of T-states to add. Default is 1.</param>
    public void MemoryContention(Word address, int repeat, int states = 1)
    {
        for (var i = 0; i < repeat; i++)
        {
            var contentionStates = contentionProvider.GetMemoryContention(CurrentStates, address);

            TotalStates += states + contentionStates;
            CurrentStates += states + contentionStates;
        }
    }

    public void PortContention(Word port, int repeat, int states = 1)
    {
        for (var i = 0; i < repeat; i++)
        {
            var contentionStates = contentionProvider.GetPortContention(CurrentStates, port);

            TotalStates += states + contentionStates;
            CurrentStates += states + contentionStates;
        }
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