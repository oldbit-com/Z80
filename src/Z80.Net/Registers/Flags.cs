namespace Z80.Net.Registers;

/// <summary>
/// Specifies Z80 CPU flags.
/// </summary>
[Flags]
public enum Flags : byte
{
    /// <summary>
    /// No flag is set.
    /// </summary>
    None = 0,

    /// <summary>
    /// Carry Flag
    /// </summary>
    C = 0x01,

    /// <summary>
    /// Add/Subtract Flag
    /// </summary>
    N = 0x02,

    /// <summary>
    /// Parity/Overflow Flag
    /// </summary>
    P = 0x04,

    /// <summary>
    /// Undocumented X Flag
    /// </summary>
    X = 0x08,

    /// <summary>
    /// Half Carry Flag
    /// </summary>
    H = 0x10,

    /// <summary>
    /// Undocumented Y Flag
    /// </summary>
    Y = 0x20,

    /// <summary>
    /// Zero Flag
    /// </summary>
    Z = 0x40,

    /// <summary>
    /// Sign Flag
    /// </summary>
    S = 0x80,

    /// <summary>
    /// All flags
    /// </summary>
    All = 0xFF
}
