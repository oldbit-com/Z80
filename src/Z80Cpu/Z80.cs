using OldBit.Z80Cpu.OpCodes;
using OldBit.Z80Cpu.Registers;

namespace OldBit.Z80Cpu;

public partial class Z80
{
    private readonly IMemory _memory;
    private IBus? _bus;

    private readonly OpCodesIndex _opCodes = new();
    private bool _isExtendedInstruction;
    private sbyte _indexRegisterOffset;

    public Registers.Registers Registers { get; private set; } = new();
    public bool IsHalted { get; private set; }
    public bool IFF1 { get; internal set; }
    public bool IFF2 { get; internal set; }
    public InterruptMode InterruptMode { get; set; }
    public CyclesCounter Cycles { get; } = new();
    public Action? Trap { get; set; }

    public Z80(IMemory memory)
    {
        Reset();
        SetupInstructions();

        _memory = memory;
    }

    /// <summary>
    /// Executes the Z80 CPU instructions.
    /// </summary>
    /// <param name="cyclesToExecute">Specifies the number of cycles to execute. Null if no limit.</param>
    public void Run(int? cyclesToExecute = null)
    {
        Cycles.Limit(cyclesToExecute);

        while (!Cycles.IsComplete || Registers.UseIndexRegister || _isExtendedInstruction)
        {
            Trap?.Invoke();

            if (IsHalted)
            {
                Cycles.Halt();
                break;
            }

            var opCode = FetchOpCode();
            IncrementR();

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
    /// Executes a Maskable Interrupt.
    /// </summary>
    /// <param name="data">The data byte associated with the interrupt.
    /// This is used in Mode 2 to form the address of the interrupt service routine.</param>
    public void Int(byte data)
    {
        IsHalted = false;
        if (!IFF1)
        {
            return;
        }

        IFF1 = false;
        IFF2 = false;

        switch (InterruptMode)
        {
            case InterruptMode.Mode0:
            case InterruptMode.Mode1:
                PushPC();
                Registers.PC = 0x38; // RST 38h
                break;

            case InterruptMode.Mode2:
                PushPC();
                var address = (Word)(Registers.I << 8 | data);
                Registers.PC = ReadWord(address);
                break;
        }

        Cycles.Add(7);
        IncrementR();
    }

    /// <summary>
    /// Executes a Non-Maskable Interrupt.
    /// </summary>
    public void Nmi()
    {
        IsHalted = false;
        IFF1 = false;
        IFF2 = false;
        PushPC();
        Registers.PC = 0x66;

        Cycles.Add(5);
        IncrementR();
    }

    public void Reset()
    {
        Registers = new Registers.Registers
        {
            A = 0xFF,
            F = Flags.All,
            PC = 0x0000,
            SP = 0xFFFF,
        };
        IFF1 = false;
        IFF2 = false;
        IsHalted = false;
    }

    public Z80 AddBus(IBus? bus)
    {
        _bus = bus;
        return this;
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

            Cycles.Add(2);
        }
        else
        {
            opCode = FetchOpCode();
        }

        IncrementR();
        _opCodes.Execute(0xCB00 | opCode);
    }

    /// <summary>
    /// Pushes the Program Counter onto the stack.
    /// </summary>
    private void PushPC() => Execute_PUSH((byte)(Registers.PC >> 8), (byte)(Registers.PC & 0xFF));

    /// <summary>
    /// Increments the R register keeping the MSB intact.
    /// </summary>
    private void IncrementR() => Registers.R = (byte)((Registers.R & 0x80) | ((Registers.R + 1) & 0x7F));
}