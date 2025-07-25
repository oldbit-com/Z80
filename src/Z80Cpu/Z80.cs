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
    private bool _isEIPending;
    private bool _isBreakpoint;

    private readonly IMemory _memory;
    private IBus? _bus;

    /// <summary>
    /// Delegate for the BeforeInstruction event.
    /// </summary>
    public delegate void BeforeInstructionEvent(Word pc);

    /// <summary>
    /// Delegate for the AfterOpCodeFetch event.
    /// </summary>
    public delegate void AfterFetchEvent(Word pc);

    /// <summary>
    /// Delegate for the BeforeOpCodeFetch event.
    /// </summary>
    public delegate void BeforeFetchEvent(Word pc);

    /// <summary>
    /// Event raised before an instruction is fetched.
    /// </summary>
    public event BeforeInstructionEvent? BeforeInstruction;

    /// <summary>
    /// Event raised after an op code is fetched.
    /// </summary>
    public event AfterFetchEvent? AfterFetch;

    /// <summary>
    /// Event raised before an op code is fetched.
    /// </summary>
    public event BeforeFetchEvent? BeforeFetch;

    /// <summary>
    /// Gets the Registers of the Z80 CPU.
    /// </summary>
    public RegisterSet Registers { get; private set; } = new();

    /// <summary>
    /// Gets a value indicating whether the CPU is halted.
    /// </summary>
    public bool IsHalted { get; set; }

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
    /// Executes a single Z80 CPU instruction.
    /// </summary>
    public void Step()
    {
        while (true)
        {
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

            break;
        }
    }

    /// <summary>
    /// Executes the Z80 CPU instructions.
    /// </summary>
    public void Run()
    {
        while (true)
        {
            if (IsHalted)
            {
                Clock.Halt();
                break;
            }

            _isEIPending = false;

            BeforeInstruction?.Invoke(Registers.PC);

            if (_isBreakpoint)
            {
                _isBreakpoint = false;
                break;
            }

            Step();

            if (Clock.IsFrameComplete)
            {
                break;
            }
        }
    }

    /// <summary>
    /// Executes the Z80 CPU instructions for the specified number of T-states.
    /// </summary>
    /// <param name="frameTicks">Specifies the number of T-states to execute per frame. Zero means no limit.</param>
    public void Run(int frameTicks)
    {
        Clock.NewFrame(frameTicks);
        Run();
    }

    /// <summary>
    /// Sets the Z80 CPU breakpoint flag, pausing execution at the next opportunity.
    /// </summary>
    public void Break() => _isBreakpoint = true;

    /// <summary>
    /// Triggers a Maskable Interrupt (INT).
    /// </summary>
    /// <param name="data">The data byte associated with the interrupt.
    /// This is used in Mode 2 to form the address of the interrupt service routine.</param>
    public void TriggerInt(byte data)
    {
        if (!IFF1 || _isEIPending)
        {
            return;
        }

        if (IsHalted)
        {
            IsHalted = false;
        }

        IFF1 = false;
        IFF2 = false;

        switch (IM)
        {
            case InterruptMode.Mode0:
                // TODO: Implement Mode 0

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

        Clock.AddTicks(7);
        IncrementR();
    }

    /// <summary>
    /// Executes a Non-Maskable Interrupt (NMI).
    /// </summary>
    public void TriggerNmi()
    {
        if (IsHalted)
        {
            IsHalted = false;
        }

        IFF2 = IFF1;
        IFF1 = false;

        PushPC();

        Registers.PC = 0x66;

        Clock.AddTicks(5);
        IncrementR();
    }

    /// <summary>
    /// Resets the Z80 CPU to its initial state.
    /// </summary>
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
        _isEIPending = false;
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

            Clock.AddTicks(2);
        }
        else
        {
            opCode = FetchOpCode();
        }

        _opCodes.Execute(0xCB00 | opCode);
    }

    private void PushPC()
    {
        Registers.SP -= 1;
        WriteByte(Registers.SP, (byte)(Registers.PC >> 8));

        Registers.SP -= 1;
        WriteByte(Registers.SP, (byte)(Registers.PC & 0xFF));
    }

    private void IncrementR() => Registers.R = (byte)(Registers.R & 0x80 | (Registers.R + 1 & 0x7F));
}