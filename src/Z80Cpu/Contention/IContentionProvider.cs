namespace OldBit.Z80Cpu.Contention;

/// <summary>
/// Provides a mechanism to handle memory contention specific to ZX Spectrum emulation.
/// </summary>
public interface IContentionProvider
{
    /// <summary>
    /// Calculates and returns the number of T-states that should be added due to memory contention.
    /// </summary>
    /// <param name="ticks">The current number of T-states.</param>
    /// <param name="address">The memory address being accessed.</param>
    /// <returns>The number of T-states to be added due to memory contention.</returns>
    int GetMemoryContention(int ticks, Word address);

    /// <summary>
    /// Calculates and returns the number of T-states that should be added due to port contention
    /// </summary>
    /// <param name="ticks">The current number of T-states.</param>
    /// <param name="port">The port address being accessed.</param>
    /// <returns>The number of T-states to be added due to port contention.</returns>
    int GetPortContention(int ticks, Word port);

    /// <summary>
    /// Gets a value indicating whether the address is in a contended area.
    /// </summary>
    /// <param name="address">The memory address being accessed.</param>
    /// <returns>A value indicating whether the address is in a contended area.</returns>
    bool IsAddressContended(Word address);

    /// <summary>
    /// Gets a value indicating whether the port is in a contended area.
    /// </summary>
    /// <param name="port">The port address being accessed.</param>
    /// <returns>A value indicating whether the port is in a contended area.</returns>
    bool IsPortContended(Word port);
}