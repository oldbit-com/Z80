using Z80.Net.Extensions;
using Z80.Net.Helpers;

namespace Z80.Net.Registers;

/// <summary>
/// Represents Z80 CPU registers.
/// </summary>
public sealed class CpuRegisters : StandardRegisters
{
    internal RegisterContext Context { get; set; }

    /// <summary>
    /// Gets a alternative (prime) registers set.
    /// </summary>
    public StandardRegisters Alternative { get; } = new();

    /// <summary>
    /// Gets or sets undocumented IXL register which is a low byte of the IX register.
    /// </summary>
    public byte IXL { get; set; }

    /// <summary>
    /// Gets or sets undocumented IXH register which is a high byte of the IX register.
    /// </summary>
    public byte IXH { get; set; }

    /// <summary>
    /// Gets or sets undocumented IYL register which is a low byte of the IY register.
    /// </summary>
    public byte IYL { get; set; }

    /// <summary>
    /// Gets or sets undocumented IYH register which is a high byte of the IY register.
    /// </summary>
    public byte IYH { get; set; }

    /// <summary>
    /// Gets or sets the value of the IX index register.
    /// </summary>
    public Word IX
    {
        get => TypeConverter.ToWord(IXH, IXL);
        set => (IXH, IXL) = value;
    }

    /// <summary>
    /// Gets or sets the value of the IY index register.
    /// </summary>
    public Word IY
    {
        get => TypeConverter.ToWord(IYH, IYL);
        set => (IYH, IYL) = value;
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
        get => Context switch
        {
            RegisterContext.IX => IX,
            RegisterContext.IY => IY,
            _ => HL
        };
        set
        {
            switch (Context)
            {
                case RegisterContext.IX:
                    IX = value;
                    break;
                case RegisterContext.IY:
                    IY = value;
                    break;
                default:
                    HL = value;
                    break;
            }
        }
    }

    /// <summary>
    /// Gets or sets a value either H, IXH or IYH register depending on the current context.
    /// </summary>
    internal byte XH
    {
        get => Context switch
        {
            RegisterContext.IX => IXH,
            RegisterContext.IY => IYH,
            _ => H
        };
        set
        {
            switch (Context)
            {
                case RegisterContext.IX:
                    IXH = value;
                    break;
                case RegisterContext.IY:
                    IYH = value;
                    break;
                default:
                    H = value;
                    break;
            }
        }
    }

    /// <summary>
    /// Gets or sets a value either L, IXL or IYL register depending on the current context.
    /// </summary>
    internal byte XL
    {
        get => Context switch
        {
            RegisterContext.IX => IXL,
            RegisterContext.IY => IYL,
            _ => L
        };
        set
        {
            switch (Context)
            {
                case RegisterContext.IX:
                    IXL = value;
                    break;
                case RegisterContext.IY:
                    IYL = value;
                    break;
                default:
                    L = value;
                    break;
            }
        }
    }

    internal bool UseIndexRegister => Context is RegisterContext.IX or RegisterContext.IY;
}