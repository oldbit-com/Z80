using Z80.Net.OpCodes;
using Z80.Net.Registers;

namespace Z80.Net;

public partial class Z80
{
    private readonly IMemory _memory;
    private readonly IBus? _bus;

    private readonly OpCodesIndex _opCodes = new();
    private bool _isExtendedInstruction;
    private sbyte _indexRegisterOffset;

    public Registers.Registers Registers { get; private set; } = new();
    public bool IsHalted { get; private set; }
    public bool IFF1 { get; internal set; }
    public bool IFF2 { get; internal set; }
    public InterruptMode InterruptMode { get; private set; }
    public StatesCounter States { get; } = new();

    public Z80(IMemory memory, IBus? bus = null)
    {
        _memory = memory;
        _bus = bus;

        Reset();
        SetupInstructions();
    }

    private void SetupInstructions()
    {
        AddControlInstructions();
        AddCallAndReturnInstructions();
        AddJumpInstructions();
        AddExchangeBlockInstructions();
        AddGeneralPurposeArithmeticInstructions();
        Add8BitLoadInstructions();
        Add8BitArithmeticInstructions();
        Add16BitLoadInstructions();
        Add16BitArithmeticInstructions();
        AddRotateShiftInstructions();
        AddBitSetResetTestInstructions();
        AddInputOutputInstructions();
        AddUndocumentedInstructions();
    }

    public void Run(int statesToExecute = 0)
    {
        States.Limit(statesToExecute);

        while (!States.IsComplete)
        {
            if (IsHalted)
            {
                break;
            }

            var opCode = FetchOpCode();
            switch (opCode)
            {
                case 0xCB:
                    ExecuteBitOpCodes();
                    break;

                case 0xED:
                    _isExtendedInstruction = true;
                    continue;

                case 0xDD:
                    Registers.Context = RegisterContext.IX;
                    continue;

                case 0xFD:
                    Registers.Context = RegisterContext.IY;
                    continue;

                default:
                    _opCodes.Execute(_isExtendedInstruction ? 0xED00 | opCode : opCode);

                    break;
            }

            Registers.Context = RegisterContext.HL;
            _isExtendedInstruction = false;
            _indexRegisterOffset = 0;
        }
    }

    /// <summary>
    /// Processes CB prefixed op codes. They require slightly different handling,
    /// because for IX and IY registers, offset precedes the actual op code. This
    /// is different than standard op codes where offset is after the op code.
    /// </summary>
    private void ExecuteBitOpCodes()
    {
        byte opCode;
        if (Registers.UseIndexRegister)
        {
            _indexRegisterOffset = (sbyte)FetchByte();
            opCode = FetchByte();

            States.Add(2);
        }
        else
        {
            opCode = FetchOpCode();
        }

        _opCodes.Execute(0xCB00 | opCode);
    }

    public void Reset()
    {
        Registers = new Registers.Registers
        {
            A = 0xFF,
            F = Flags.All,
            PC = 0x000,
            SP = 0xFFF,
        };
        IFF1 = false;
        IFF2 = false;
        IsHalted = false;
    }
}
