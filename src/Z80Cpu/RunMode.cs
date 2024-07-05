namespace OldBit.Z80Cpu;

/// <summary>
/// Represents the run mode of the Z80 CPU.
/// </summary>
public enum RunMode
{
    /// <summary>
    /// Execute absolute number of T-states.
    /// </summary>
    Absolute,

    /// <summary>
    /// Execute until the specified number of T-states is reached.
    /// </summary>
    Incremental,
}