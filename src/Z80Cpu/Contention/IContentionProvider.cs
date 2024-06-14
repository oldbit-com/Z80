namespace OldBit.Z80Cpu.Contention;

/// <summary>
/// Provides a mechanism to handle memory contention specific to ZX Spectrum emulation.
/// </summary>
public interface IContentionProvider
{
    /// <summary>
    /// Calculates and returns the number of T-states  that should be added due to memory contention.
    /// </summary>
    /// <param name="currentStates">The current number of T-states.</param>
    /// <param name="address">The memory address being accessed.</param>
    /// <returns>The number of T-states to be added due to memory contention.</returns>
    int GetContentionStates(int currentStates, Word address);
}