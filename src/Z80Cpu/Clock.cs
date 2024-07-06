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
    /// Adds the specified number of T-states.
    /// </summary>
    /// <param name="ticks">The number of T-states to add.</param>
    public void Add(int ticks)
    {
        TotalTicks += ticks;
        FrameTicks += ticks;
    }

    /// <summary>
    /// Adds the specified number of T-states respecting contention.
    /// </summary>
    /// <param name="address">The address of the memory that might be contended.</param>
    /// <param name="repeat">The number of repeated conentions to repeat.</param>
    /// <param name="ticks">The number of T-states to add. Default is 1.</param>
    public void MemoryContention(Word address, int repeat, int ticks = 1)
    {
        for (var i = 0; i < repeat; i++)
        {
            var contentionStates = contentionProvider.GetMemoryContention(FrameTicks, address);

            TotalTicks += ticks + contentionStates;
            FrameTicks += ticks + contentionStates;
        }
    }

    public void PortContention(Word port, int repeat, int ticks = 1)
    {
        for (var i = 0; i < repeat; i++)
        {
            var contentionStates = contentionProvider.GetPortContention(FrameTicks, port);

            TotalTicks += ticks + contentionStates;
            FrameTicks += ticks + contentionStates;
        }
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