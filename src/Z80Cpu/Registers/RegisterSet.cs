using OldBit.Z80Cpu.Extensions;
using OldBit.Z80Cpu.Helpers;

namespace OldBit.Z80Cpu.Registers;

/// <summary>
/// Represents Z80 CPU registers.
/// </summary>
public sealed class RegisterSet : StandardRegisterSet
{
    private readonly WordRegister _ix = new();
    private readonly WordRegister _iy = new();

    private RegisterContext _context;

    private WordRegister _active;

    public RegisterSet()
    {
        _active = HLReg;
    }

    internal RegisterContext Context
    {
        get => _context;
        set
        {
            _context = value;

            switch (_context)
            {
                case RegisterContext.IX:
                    _active = _ix;
                    break;

                case RegisterContext.IY:
                    _active = _iy;
                    break;

                default:
                    _active = HLReg;
                    break;
            }
        }
    }

    /// <summary>
    /// Gets or sets the value of the I register.
    /// </summary>
    public byte I { get; set; }

    /// <summary>
    /// Gets or sets the value of the R register.
    /// </summary>
    public byte R { get; set; }

    /// <summary>
    /// Gets a alternative (prime) registers set.
    /// </summary>
    public StandardRegisterSet Prime { get; } = new();

    /// <summary>
    /// Gets or sets undocumented IXL register which is a low byte of the IX register.
    /// </summary>
    public byte IXL { get => _ix.L; set => _ix.L = value; }

    /// <summary>
    /// Gets or sets undocumented IXH register which is a high byte of the IX register.
    /// </summary>
    public byte IXH { get => _ix.H; set  => _ix.H = value; }

    /// <summary>
    /// Gets or sets undocumented IYL register which is a low byte of the IY register.
    /// </summary>
    public byte IYL { get => _iy.L; set  => _iy.L = value; }

    /// <summary>
    /// Gets or sets undocumented IYH register which is a high byte of the IY register.
    /// </summary>
    public byte IYH { get => _iy.H; set  => _iy.H = value; }

    /// <summary>
    /// Gets or sets the value of the IX index register.
    /// </summary>
    public Word IX
    {
        get => _ix.Value;
        set => _ix.Value = value;
    }

    /// <summary>
    /// Gets or sets the value of the IY index register.
    /// </summary>
    public Word IY
    {
        get => _iy.Value;
        set => _iy.Value = value;
    }

    /// <summary>
    /// Gets or sets the value of the Program Counter register.
    /// </summary>
    public Word PC { get; set; }

    /// <summary>
    /// Gets or sets the value of the Stack Pointer register.
    /// </summary>
    public Word SP { get; set; }

    /// <summary>
    /// Gets or sets a value of either HL, IX or IY register depending on the current context.
    /// </summary>
    internal Word XHL
    {
        get => _active.Value;
        set => _active.Value = value;
    }

    /// <summary>
    /// Gets or sets a value either H, IXH or IYH register depending on the current context.
    /// </summary>
    internal byte XH
    {
        get => _active.H;
        set => _active.H = value;
    }

    /// <summary>
    /// Gets or sets a value either L, IXL or IYL register depending on the current context.
    /// </summary>
    internal byte XL
    {
        get => _active.L;
        set => _active.L = value;
    }

    /// <summary>
    /// Gets the value of the virtual IR register.
    /// </summary>
    internal Word IR => Converter.ToWord(I, R);

    /// <summary>
    /// Indicates whether IX or IY index register is in the current context.
    /// </summary>
    internal bool UseIndexRegister => Context is RegisterContext.IX or RegisterContext.IY;
}