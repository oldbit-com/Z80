namespace OldBit.Z80Cpu;

/// <summary>
/// Event arguments for the TicksAdded event.
/// </summary>
/// <param name="previousFrameTicks">Specifies the number of ticks before the update.</param>
/// <param name="currentFrameTicks">Specifies the number of ticks after the update.</param>
public class TicksAddedEventArgs(int previousFrameTicks, int currentFrameTicks)
{
    /// <summary>
    /// Gets the number of ticks before the update.
    /// </summary>
    public int PreviousFrameTicks { get; } = previousFrameTicks;

    /// <summary>
    /// Gets the number of ticks after the update.
    /// </summary>
    public int CurrentFrameTicks { get; } = currentFrameTicks;
}

public delegate void TicksAddedEventHandler(TicksAddedEventArgs e);

/// <summary>
/// Clock that counts the number of T-states executed.
/// </summary>
public sealed class Clock
{
    private int _ticksLimit;
    private int _extraFrameTicks;

    /// <summary>
    /// Event raised when T-states are added.
    /// </summary>
    public event TicksAddedEventHandler? TicksAdded;

    /// <summary>
    /// Adds the specified number of T-states.
    /// </summary>
    /// <param name="ticks">The number of T-states to add.</param>
    public void Add(int ticks)
    {
        var previousFrameTicks = FrameTicks;

        TotalTicks += ticks;
        FrameTicks += ticks;

        TicksAdded?.Invoke(new TicksAddedEventArgs(previousFrameTicks, FrameTicks));
    }

    /// <summary>
    /// Handles the HALT instruction where normally CPU executes NOPs.
    /// </summary>
    public void Halt()
    {
        var remaining = _ticksLimit - FrameTicks;
        Add(remaining >= 4 ? 4 : remaining);
    }

    /// <summary>
    /// Limits the number of T-states that should be executed.
    /// </summary>
    /// <param name="ticksLimit">The maximum number of T-states to execute.</param>
    public void SetLimit(int ticksLimit)
    {
        _ticksLimit = ticksLimit + _extraFrameTicks;
    }

    /// <summary>
    /// Resets the clock to the beginning of the frame.
    /// </summary>
    public void NewFrame()
    {
        _extraFrameTicks = _ticksLimit - FrameTicks;
        FrameTicks = 0;
    }

    /// <summary>
    /// Returns true if number of executed T-states reached the maximum.
    /// </summary>
    public bool IsComplete => _ticksLimit != 0 && FrameTicks >= _ticksLimit;

    /// <summary>
    /// Gets the total number of T-states since boot or hard reset.
    /// </summary>
    public long TotalTicks { get; private set; }

    /// <summary>
    /// Gets the number of T-states executed in the current frame execution.
    /// </summary>
    public int FrameTicks { get; private set; }
}