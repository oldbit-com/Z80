using OldBit.Z80Cpu.Contention;

namespace OldBit.Z80Cpu;

/// <summary>
/// Clock that counts the number of T-states executed.
/// </summary>
public sealed class Clock
{
    private int _ticksLimit;

    /// <summary>
    /// Gets or sets the contention provider that provides contention data.
    /// </summary>
    public IContentionProvider ContentionProvider { get; set; } = new ZeroContentionProvider();

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
    /// Adds the specified number of T-states respecting memory contention.
    /// </summary>
    /// <param name="address">The address of the memory that might be contended.</param>
    /// <param name="repeat">The number of contentions to repeat.</param>
    /// <param name="ticks">The number of T-states to add. Default is 1.</param>
    internal void AddMemoryContention(Word address, int repeat, int ticks = 1)
    {
        for (var i = 0; i < repeat; i++)
        {
            var contention = 0;

            if (ContentionProvider.IsAddressContended(address))
            {
                contention = ContentionProvider.GetMemoryContention(CurrentFrameTicks, address);
            }

            AddTicks(ticks + contention);
        }
    }

    internal void AddPortPreContention(Word port)
    {
        var contention = 0;

        if (ContentionProvider.IsPortContended(port))
        {
            // C:1, C:3 or C:1, C:1, C:1, C:1 pattern match
            contention = ContentionProvider.GetPortContention(CurrentFrameTicks, port);
        }

        // N:1, C:3 or N:4 pattern match
        AddTicks(1 + contention);
    }

    internal void AddPortPostContention(Word port)
    {
        if ((port & 0x01) != 0)
        {
            if (ContentionProvider.IsPortContended(port))
            {
                // C:1, C:1, C:1, C:1
                var contention = ContentionProvider.GetPortContention(CurrentFrameTicks, port);
                AddTicks(1 + contention);

                contention = ContentionProvider.GetPortContention(CurrentFrameTicks, port);
                AddTicks(1 + contention);

                contention = ContentionProvider.GetPortContention(CurrentFrameTicks, port);
                AddTicks(1 + contention);
            }
            else
            {
                // N:4
                AddTicks(3);
            }
        }
        else
        {
            // N:1, C:3 or C:1, C:3
            var contention = ContentionProvider.GetPortContention(CurrentFrameTicks, port);

            AddTicks(3 + contention);
        }
    }

    /// <summary>
    /// Sets the number of T-states executed in the frame.
    /// </summary>
    internal void SetFrameTicks() => _ticksLimit = DefaultFrameTicks;

    /// <summary>
    /// Gets a value indicating whether the current T-state is within the interrupt window.
    /// </summary>
    internal bool IsInterruptWindow =>
        CurrentFrameTicks >= 0 && CurrentFrameTicks < InterruptDuration;

    /// <summary>
    /// Resets the clock to the beginning of the frame.
    /// </summary>
    public void NewFrame() => CurrentFrameTicks = CurrentFrameTicks - _ticksLimit;

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

    /// <summary>
    /// Gets or sets the number of T-states the interrupt can last.
    /// </summary>
    public int InterruptDuration { get; set; }
}