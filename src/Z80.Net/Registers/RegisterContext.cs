namespace OldBit.Z80.Net.Registers;

/// <summary>
/// Specifies context for the HL register which is either HL, IX or IY.
/// </summary>
public enum RegisterContext
{
    /// <summary>
    /// Use regular HL register.
    /// </summary>
    HL,

    /// <summary>
    /// Use IX register (DD prefix active).
    /// </summary>
    IX,

    /// <summary>
    /// Use IY register (FD prefix active).
    /// </summary>
    IY
}