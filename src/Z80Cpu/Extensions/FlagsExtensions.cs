using OldBit.Z80Cpu.Registers;

namespace OldBit.Z80Cpu.Extensions;

/// <summary>
/// Extension methods for the Flags enumeration.
/// </summary>
public static class FlagsExtensions
{
    /// <summary>
    /// Determines whether the specified flag is set in the given Flags enumeration.
    /// </summary>
    /// <param name="flags">The Flags enumeration to check.</param>
    /// <param name="flag">The specific flag to check for.</param>
    /// <returns>true if the specified flag is set; otherwise, false.</returns>
    public static bool IsSet(this Flags flags, Flags flag) => (flags & flag) == flag;
}