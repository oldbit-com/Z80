using OldBit.Z80Cpu.Contention;

namespace OldBit.Z80Cpu;

/// <summary>
/// Clock that counts the number of T-states executed.
/// </summary>
/// <param name="contentionProvider">Provides contention states data.</param>
public sealed class Clock(IContentionProvider contentionProvider)
{
    private int _ticksLimit;
    private int _extraFrameTicks;

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
    public void Add(int ticks)
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
    /// Adds the specified number of T-states respecting memory contention.
    /// </summary>
    /// <param name="address">The address of the memory that might be contended.</param>
    /// <param name="repeat">The number of contentions to repeat.</param>
    /// <param name="ticks">The number of T-states to add. Default is 1.</param>
    public void MemoryContention(Word address, int repeat, int ticks = 1)
    {
        for (var i = 0; i < repeat; i++)
        {
            var contentionStates = contentionProvider.GetMemoryContention(FrameTicks, address);

            Add(ticks + contentionStates);
        }
    }

    /// <summary>
    /// Adds the specified number of T-states respecting port contention.
    /// </summary>
    /// <param name="port">The address of the port that might be contended.</param>
    /// <param name="repeat">The number of contentions to repeat.</param>
    /// <param name="ticks">The number of T-states to add. Default is 1.</param>
    public void PortContention(Word port, int repeat, int ticks = 1)
    {
        for (var i = 0; i < repeat; i++)
        {
            var contentionStates = contentionProvider.GetPortContention(FrameTicks, port);

            Add(ticks + contentionStates);
        }
    }

    /// <summary>
    /// Handles the HALT instruction where normally CPU executes NOPs.
    /// </summary>
    public void Halt()
    {
        var remaining = _ticksLimit - CurrentFrameTicks;
        Add(remaining > 4 ? remaining : 4 - remaining);
    }

    /// <summary>
    /// Limits the number of T-states that should be executed in the frame.
    /// </summary>
    internal void InitFrameLimiter()
    {
        _ticksLimit = DefaultFrameTicks + _extraFrameTicks;
    }

    /// <summary>
    /// Resets the clock to the beginning of the frame.
    /// </summary>
    public void NewFrame()
    {
        _extraFrameTicks = _ticksLimit - CurrentFrameTicks;
        CurrentFrameTicks = 0;
    }

    /// <summary>
    /// Returns true if number of executed T-states reached the maximum.
    /// </summary>
    public bool IsComplete => _ticksLimit != 0 && CurrentFrameTicks >= _ticksLimit;

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