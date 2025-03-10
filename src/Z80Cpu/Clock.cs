namespace OldBit.Z80Cpu;

/// <summary>
/// Clock that counts the number of T-states executed.
/// </summary>
public sealed class Clock
{
    private int _ticksLimit;

    /// <summary>
    /// Delegate for the TicksAdded event.
    /// </summary>
    public delegate void TicksAddedEvent(int addedTicks, int previousFrameTicks, int currentFrameTicks);

    /// <summary>
    /// Event raised when T-states are added.
    /// </summary>
    public event TicksAddedEvent? TicksAdded;

    /// <summary>
    /// Adds the specified number of T-states.
    /// </summary>
    /// <param name="ticks">The number of T-states to add.</param>
    public void AddTicks(int ticks)
    {
        var previousFrameTicks = CurrentFrameTicks;

        unchecked
        {
            TotalTicks += ticks;
            CurrentFrameTicks += ticks;
        }

        TicksAdded?.Invoke(ticks, previousFrameTicks, CurrentFrameTicks);
    }

    /// <summary>
    /// Handles the HALT instruction where normally CPU executes NOPs.
    /// </summary>
    public void Halt()
    {
        var remaining = _ticksLimit - CurrentFrameTicks;
        AddTicks(remaining > 4 ? remaining : 4 - remaining);
    }

    /// <summary>
    /// Sets the number of T-states executed in the frame.
    /// </summary>
    internal void SetFrameTicksLimit() => _ticksLimit = DefaultFrameTicks;

    /// <summary>
    /// Resets the clock to the beginning of the frame.
    /// </summary>
    public void NewFrame() => CurrentFrameTicks -= _ticksLimit;

    /// <summary>
    /// Gets a value indicating whether the frame is complete.
    /// </summary>
    public bool IsFrameComplete => _ticksLimit != 0 && CurrentFrameTicks >= _ticksLimit;

    /// <summary>
    /// Gets the total number of T-states since boot or hard reset.
    /// </summary>
    public long TotalTicks { get; private set; }

    /// <summary>
    /// Gets the number of T-states executed in the current frame execution.
    /// </summary>
    public int CurrentFrameTicks { get; private set; }

    /// <summary>
    /// Gets or sets the default number of T-states executed in the frame.
    /// </summary>
    public int DefaultFrameTicks { get; set; }
}