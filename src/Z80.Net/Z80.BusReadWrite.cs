namespace OldBit.Z80.Net;

partial class Z80
{
    /// <summary>
    /// Reads a value from the data bus.
    /// </summary>
    /// <param name="topHalf">The top 8 bits of the data bus address (A8-A15).</param>
    /// <param name="bottomHalf">The bottom 8 bits of the data bus address (A0-A7).</param>
    /// <returns>The value from the data bus.</returns>
    private byte ReadBus(byte topHalf, byte bottomHalf)
    {
        Cycles.Add(4);

        return _bus?.Read((Word)((topHalf << 8) | bottomHalf)) ?? 0xFF;
    }

    /// <summary>
    /// Writes a value to the data bus.
    /// </summary>
    /// <param name="topHalf">The top 8 bits of the data bus address (A8-A15).</param>
    /// <param name="bottomHalf">The bottom 8 bits of the data bus address (A0-A7).</param>
    /// <param name="data">The data to be written</param>
    private void WriteBus(byte topHalf, byte bottomHalf, byte data)
    {
        Cycles.Add(4);

        _bus?.Write((Word)((topHalf << 8) | bottomHalf), data);
    }
}
