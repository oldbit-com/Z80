using OldBit.Z80Cpu.Registers;

namespace OldBit.Z80Cpu;

/// <summary>
/// Represents the Z80 CPU emulator.
/// </summary>
public partial class Z80
{
    private const int RST38 = 0x0038;

    private bool _isExtendedInstruction;
    private sbyte _indexRegisterOffset;

    private readonly IMemory _memory;
    private IBus? _bus;

    /// <summary>
    /// Gets the Registers of the Z80 CPU.
    /// </summary>
    public RegisterSet Registers { get; private set; } = new();

    /// <summary>
    /// Gets a value indicating whether the CPU is halted.
    /// </summary>
    public bool IsHalted { get; internal set; }

    /// <summary>
    /// Gets or sets a value of the Interrupt Flip-Flop 1.
    /// </summary>
    public bool IFF1 { get; set; }

    /// <summary>
    /// Gets or sets a value of the Interrupt Flip-Flop 2.
    /// </summary>
    public bool IFF2 { get; set; }

    /// <summary>
    /// Gets or sets the Interrupt Mode (0, 1 or 2).
    /// </summary>
    public InterruptMode IM { get; set; }

    /// <summary>
    /// Gets the clock that keeps track of the number of T-states executed.
    /// </summary>
    public Clock Clock { get; } = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Z80"/> class.
    /// </summary>
    /// <param name="memory">An instance of IMemory that represents the memory used by the Z80 CPU.</param>
    public Z80(IMemory memory)
    {
        Reset();
        SetupInstructions();

        _memory = memory;
    }

    /// <summary>
    /// Executes the Z80 CPU instructions for the specified number of T-states.
    /// </summary>
    /// <param name="ticks">Specifies the number of T-states to execute. Zero means no limit.</param>
    public void Run(int ticks = 0)
    {
        Clock.SetLimit(ticks);

        while (true)
        {
            if (IsHalted)
            {
                Clock.Halt();
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

            if (Clock.IsComplete)
            {
                break;
            }
        }
    }

    /// <summary>
    /// Executes a Maskable Interrupt.
    /// </summary>
    /// <param name="data">The data byte associated with the interrupt.
    /// This is used in Mode 2 to form the address of the interrupt service routine.</param>
    public void Int(byte data)
    {
        if (IsHalted)
        {
            IsHalted = false;
            Registers.PC += 1;
        }

        if (!IFF1)
        {
            return;
        }

        IFF1 = false;
        IFF2 = false;

        switch (IM)
        {
            case InterruptMode.Mode0:
            case InterruptMode.Mode1:
                PushPC();
                Registers.PC = RST38;
                break;

            case InterruptMode.Mode2:
                PushPC();
                var address = (Word)(Registers.I << 8 | data);
                Registers.PC = ReadWord(address);
                break;
        }

        Clock.Add(7);
        IncrementR();
    }

    /// <summary>
    /// Executes a Non-Maskable Interrupt.
    /// </summary>
    public void Nmi()
    {
        if (IsHalted)
        {
            IsHalted = false;
            Registers.PC += 1;
        }

        IFF1 = false;
        IFF2 = false;
        PushPC();
        Registers.PC = 0x66;

        Clock.Add(5);
        IncrementR();
    }

    public void Reset()
    {
        Registers = new RegisterSet
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

    /// <summary>
    /// Adds a bus to the Z80 CPU.
    /// </summary>
    /// <param name="bus">An instance of IBus that represents the bus to be added to the Z80 CPU.</param>
    /// <returns>Returns the Z80 instance after adding the bus.</returns>
    public Z80 AddBus(IBus? bus)
    {
        _bus = bus;
        return this;
    }

    /// <summary>
    /// Processes CB prefixed op codes. They require slightly different handling,
    /// because for IX and IY registers, offset precedes the actual op code. This
    /// is different from standard op codes where offset is after the op code.
    /// </summary>
    private void ExecuteBitOpCodes()
    {
        byte opCode;
        if (Registers.UseIndexRegister)
        {
            _indexRegisterOffset = (sbyte)FetchByte();
            opCode = FetchByte();

            Clock.Add(2);
        }
        else
        {
            opCode = FetchOpCode();
        }

        _opCodes.Execute(0xCB00 | opCode);
    }

    private void PushPC() => Execute_PUSH((byte)(Registers.PC >> 8), (byte)(Registers.PC & 0xFF));

    private void IncrementR() => Registers.R = (byte)((Registers.R & 0x80) | ((Registers.R + 1) & 0x7F));
}