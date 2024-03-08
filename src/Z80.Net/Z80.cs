using Z80.Net.Instructions;
using Z80.Net.Registers;

namespace Z80.Net;

public partial class Z80
{
    private readonly IMemory _memory;
    private readonly OpCodesIndex _opCodes = new();
    private bool _isExtendedInstruction;
    private sbyte _indexRegisterOffset;

    public CpuRegisters Registers { get; private set; } = new();
    public bool IsHalted { get; private set; }
    public bool IFF1 { get; internal set; }
    public bool IFF2 { get; internal set; }
    public InterruptMode InterruptMode { get; private set; }
    public StatesCounter StatesCounter { get; } = new();

    public Z80(IMemory memory)
    {
        _memory = memory;

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

    public void Run(int maxStates = 0)
    {
        StatesCounter.Limit(maxStates);

        while (!StatesCounter.IsComplete)
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
        int opCode;
        if (Registers.UseIndexRegister)
        {
            _indexRegisterOffset = (sbyte)ReadByteAndMove();
            opCode = ReadByteAndMove();
            AddStates(2);
        }
        else
        {
            opCode = FetchOpCode();
        }

        _opCodes.Execute(0xCB00 | opCode);
    }

    public void Reset()
    {
        Registers = new CpuRegisters
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

    /// <summary>
    /// Fetches an opcode of the next instruction to be executed.
    /// The operation costs 4 T-states and PC is incremented by 1.
    /// </summary>
    /// <returns>An opcode value.</returns>
    private byte FetchOpCode() => ReadByteAndMove(states: 4);

    /// <summary>
    /// Reads a byte from the memory located at the current PC address.
    /// It costs 3 T-states and PC is incremented by 1.
    /// </summary>
    /// <returns>A data byte at the current PC address.</returns>
    private byte ReadByteAndMove() => ReadByteAndMove(states: 3);

    /// <summary>
    /// Reads a byte from the memory located at the current PC address.
    /// PC is incremented by 1 afterwards.
    /// </summary>
    /// <param name="states">The number of T-states to add. The default is 3.</param>
    /// <returns>A data byte at the current PC address.</returns>
    private byte ReadByteAndMove(int states)
    {
        var value = _memory.Read(Registers.PC);
        AddStates(states);
        Registers.PC += 1;
        return value;
    }

    /// <summary>
    /// Reads a word from the memory located at the current PC address.
    /// The operation takes 6 T-states and PC is incremented by 2.
    /// </summary>
    private ushort ReadWordAndMove() => (ushort)(ReadByteAndMove() | (ReadByteAndMove() << 8));

    /// <summary>
    /// Reads a byte from the memory located at the specified address.
    /// It costs 3 T-states. PC is not changed.
    /// </summary>
    /// <param name="address">The address of the data to read.</param>
    /// <returns>A byte value.</returns>
    private byte ReadByte(int address)
    {
        var value = _memory.Read(address);
        AddStates(3);
        return value;
    }

    /// <summary>
    /// Reads a word from memory at the specified address.
    /// It costs 6 T-states (2 byte reads). PC is not changed.
    /// </summary>
    /// <param name="address">The address of the data to read.</param>
    /// <returns>A word value.</returns>
    private ushort ReadWord(int address) => (ushort)(ReadByte(address) | ReadByte((ushort)(address + 1)) << 8);

    /// <summary>
    /// Writes a byte to the memory at the specified address.
    /// It costs 6 T-states.
    /// </summary>
    /// <param name="address">The address to write to.</param>
    /// <param name="value">The value to write to the memory.</param>
    private void WriteByte(int address, byte value)
    {
        _memory.Write(address, value);
        AddStates(3);
    }

    /// <summary>
    /// Adds specified number of T-states to the current counter.
    /// </summary>
    /// <param name="states">The number of T-states to add.</param>
    private void AddStates(int states) => StatesCounter.Add(states);

    /// <summary>
    /// Reads a byte at the memory address provided in HL register. This method is aware
    /// of current HL context, e.g. will use IX / IY pair and offset if applicable.
    /// </summary>
    /// <returns>A byte value at the memory address provided in HL (or IX/IY).</returns>
    private byte ReadMemoryAtHL(int extraIndexStates) => ReadByte(CalculateHLAddress(extraIndexStates));

    /// <summary>
    /// Calculates the value of HL, IX+d or IY+d register value depending
    /// on the current context (offset will be read and added if needed).
    /// </summary>
    /// <param name="extraIndexStates">Extra T-states to add when index register is used.</param>
    /// <returns>Value of HL, IX+d or IY+d register.</returns>
    private ushort CalculateHLAddress(int extraIndexStates)
    {
        sbyte offset = 0;

        if (Registers.Context != RegisterContext.HL)
        {
            offset = (sbyte)ReadByteAndMove();
            AddStates(extraIndexStates);
        }

        return (ushort)(Registers.XHL + offset);
    }
}