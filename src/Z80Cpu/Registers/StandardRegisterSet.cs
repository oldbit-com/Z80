using OldBit.Z80Cpu.Extensions;
using OldBit.Z80Cpu.Helpers;

namespace OldBit.Z80Cpu.Registers;

/// <summary>
/// Represents the standard Z80 registers that have prime counterpart.
/// </summary>
public class StandardRegisterSet
{
    private readonly WordRegister _bc = new();
    private readonly WordRegister _de = new();
    private readonly WordRegister _hl = new();

    internal WordRegister HLReg => _hl;

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
    public byte B { get => _bc.H; set => _bc.H = value; }

    /// <summary>
    /// Gets or sets the value of the C register.
    /// </summary>
    public byte C { get => _bc.L; set  => _bc.L = value; }

    /// <summary>
    /// Gets or sets the value of the D register.
    /// </summary>
    public byte D { get => _de.H; set  => _de.H = value; }

    /// <summary>
    /// Gets or sets the value of the E register.
    /// </summary>
    public byte E { get => _de.L; set  => _de.L = value; }

    /// <summary>
    /// Gets or sets the value of the H register.
    /// </summary>
    public byte H { get => _hl.H; set  => _hl.H = value; }

    /// <summary>
    /// Gets or sets the value of the L register.
    /// </summary>
    public byte L { get => _hl.L; set   => _hl.L = value; }

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
        get => _bc.Value;
        set => _bc.Value = value;
    }

    /// <summary>
    /// Gets or sets the value of the DE register.
    /// </summary>
    public Word DE
    {
        get => _de.Value;
        set => _de.Value = value;
    }

    /// <summary>
    /// Gets or sets the value of the HL register.
    /// </summary>
    public Word HL
    {
        get => _hl.Value;
        set => _hl.Value = value;
    }
}