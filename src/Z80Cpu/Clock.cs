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
        var previousFrameTicks = FrameTicks;

        FrameTicks += ticks;

        TicksAdded?.Invoke(ticks, previousFrameTicks, FrameTicks);
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
                contention = ContentionProvider.GetMemoryContention(FrameTicks, address);
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
            contention = ContentionProvider.GetPortContention(FrameTicks, port);
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
                var contention = ContentionProvider.GetPortContention(FrameTicks, port);
                AddTicks(1 + contention);

                contention = ContentionProvider.GetPortContention(FrameTicks, port);
                AddTicks(1 + contention);

                contention = ContentionProvider.GetPortContention(FrameTicks, port);
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
            var contention = ContentionProvider.GetPortContention(FrameTicks, port);

            AddTicks(3 + contention);
        }
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
    /// Gets a value indicating whether the current T-state is within the interrupt window.
    /// </summary>
    internal bool IsInterruptWindow => FrameTicks >= 0 && FrameTicks < InterruptDuration;

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

    /// <summary>
    /// Gets or sets the number of T-states the interrupt can last.
    /// </summary>
    public int InterruptDuration { get; set; }
}