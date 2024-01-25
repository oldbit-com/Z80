using Z80.Net.Helpers;

namespace Z80.Net.Registers;

/// <summary>
/// Represents Z80 CPU registers.
/// </summary>
public sealed class CpuRegisters : CommonRegisters
{
    internal HLContext HLContext { get; set; }

    /// <summary>
    /// Gets a alternative (prime) registers set.
    /// </summary>
    public CommonRegisters Alternative { get; } = new();

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
    public ushort IX
    {
        get => TypeConverter.ToWord(IXH, IXL);
        set => (IXH, IXL) = TypeConverter.ToBytes(value);
    }

    /// <summary>
    /// Gets or sets the value of the IY index register.
    /// </summary>
    public ushort IY
    {
        get => TypeConverter.ToWord(IYH, IYL);
        set => (IYH, IYL) = TypeConverter.ToBytes(value);
    }

    /// <summary>
    /// Gets or sets the value of the Program Counter register.
    /// </summary>
    public ushort PC { get; set; }

    /// <summary>
    /// Gets or sets the value of the Stack Pointer register.
    /// </summary>
    public ushort SP { get; set; }

    /// <summary>
    /// Gets or sets a value of either HL, IX or IY register depending on the current context.
    /// </summary>
    internal ushort XHL
    {
        get => HLContext switch
        {
            HLContext.IX => IX,
            HLContext.IY => IY,
            _ => HL
        };
        set
        {
            switch (HLContext)
            {
                case HLContext.IX:
                    IX = value;
                    break;
                case HLContext.IY:
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
        get => HLContext switch
        {
            HLContext.IX => IXH,
            HLContext.IY => IYH,
            _ => H
        };
        set
        {
            switch (HLContext)
            {
                case HLContext.IX:
                    IXH = value;
                    break;
                case HLContext.IY:
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
        get => HLContext switch
        {
            HLContext.IX => IXL,
            HLContext.IY => IYL,
            _ => L
        };
        set
        {
            switch (HLContext)
            {
                case HLContext.IX:
                    IXL = value;
                    break;
                case HLContext.IY:
                    IYL = value;
                    break;
                default:
                    L = value;
                    break;
            }
        }
    }

    internal bool UseIndexRegister => HLContext is HLContext.IX or HLContext.IY;
}