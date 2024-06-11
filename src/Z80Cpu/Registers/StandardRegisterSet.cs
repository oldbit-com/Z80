using OldBit.Z80Cpu.Extensions;
using OldBit.Z80Cpu.Helpers;

namespace OldBit.Z80Cpu.Registers;

/// <summary>
/// Represents the standard Z80 registers that have prime counterpart.
/// </summary>
public class StandardRegisterSet
{
    /// <summary>
    /// Gets or sets the value of the Accumulator register.
    /// </summary>
    public byte A { get; set; }

    /// <summary>
    /// Gets or sets the value of the Flags register.
    /// </summary>
    public Flags F { get; set; }

    /// <summary>
    /// Gets or sets the value of the B register.
    /// </summary>
    public byte B { get; set; }

    /// <summary>
    /// Gets or sets the value of the C register.
    /// </summary>
    public byte C { get; set; }

    /// <summary>
    /// Gets or sets the value of the D register.
    /// </summary>
    public byte D { get; set; }

    /// <summary>
    /// Gets or sets the value of the E register.
    /// </summary>
    public byte E { get; set; }

    /// <summary>
    /// Gets or sets the value of the H register.
    /// </summary>
    public byte H { get; set; }

    /// <summary>
    /// Gets or sets the value of the L register.
    /// </summary>
    public byte L { get; set; }

    /// <summary>
    /// Gets or sets the value of the AF register.
    /// </summary>
    public Word AF
    {
        get => Converter.ToWord(A, (byte)F);
        set
        {
            var (a, f) = value;
            A = a;
            F = (Flags)f;
        }
    }

    /// <summary>
    /// Gets or sets the value of the BC register.
    /// </summary>
    public Word BC
    {
        get => Converter.ToWord(B, C);
        set => (B, C) = value;
    }

    /// <summary>
    /// Gets or sets the value of the DE register.
    /// </summary>
    public Word DE
    {
        get => Converter.ToWord(D, E);
        set => (D, E) = value;
    }

    /// <summary>
    /// Gets or sets the value of the HL register.
    /// </summary>
    public Word HL
    {
        get => Converter.ToWord(H, L);
        set => (H, L) = value;
    }
}