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
        var previousFrameTicks = FrameTicks;

        FrameTicks += ticks;

        TicksAdded?.Invoke(ticks, previousFrameTicks, FrameTicks);
    }

    /// <summary>
    /// Handles the HALT instruction where normally CPU executes NOPs.
    /// </summary>
    public void Halt()
    {
        var remaining = _ticksLimit - FrameTicks;
        AddTicks(remaining > 4 ? remaining : 4 - remaining);
    }

    /// <summary>
    /// Resets the clock to the beginning of the frame.
    /// <param name="frameTicks">The number of T-states per frame.</param>
    /// </summary>
    public void NewFrame(int frameTicks)
    {
        FrameTicks -= _ticksLimit;
        _ticksLimit = frameTicks;
    }

    /// <summary>
    /// Gets a value indicating whether the frame is complete.
    /// </summary>
    public bool IsFrameComplete => _ticksLimit != 0 && FrameTicks >= _ticksLimit;

    /// <summary>
    /// Gets the number of T-states executed in the current frame execution.
    /// </summary>
    public int FrameTicks { get; private set; }
}