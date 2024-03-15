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
    private RegisterContext RegisterContext { get; set; }

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

    /// <summary>
    /// Fetches the next instruction to be executed.
    /// The operation costs 4 T-states and PC is incremented by 1.
    /// </summary>
    /// <returns>An 8-bit value representing the opcode.</returns>
    private byte FetchOpCode() => FetchByte(states: 4);

    /// <summary>
    /// Reads an 8-bit value from the location specified by current PC value.
    /// It costs 3 T-states and PC is incremented by 1.
    /// </summary>
    /// <returns>A data byte at the current PC address.</returns>
    private byte FetchByte() => FetchByte(states: 3);

    /// <summary>
    /// Reads an 8-bit value from the location specified by current PC value.
    /// PC is incremented by 1.
    /// </summary>
    /// <param name="states">The number of T-states to add. The default is 3.</param>
    /// <returns>A data byte at the current PC address.</returns>
    private byte FetchByte(int states)
    {
        var value = _memory.Read(Registers.PC);

        States.Add(states);
        Registers.PC += 1;

        return value;
    }

    /// <summary>
    /// Reads a 16-bit value from the location specified by current PC value.
    /// The operation takes 6 T-states and PC is incremented by 2.
    /// </summary>
    private Word FetchWord() => (Word)(FetchByte() | (FetchByte() << 8));

    /// <summary>
    /// Reads a 8-bit value from the location provided.
    /// It costs 3 T-states. PC is not changed.
    /// </summary>
    /// <param name="address">The address of the data to read.</param>
    /// <returns>A byte value.</returns>
    private byte ReadByte(Word address)
    {
        var value = _memory.Read((Word)address);

        States.Add(3);

        return value;
    }

    /// <summary>
    /// Reads a 16-bit value from the location provided.
    /// It costs 6 T-states (2 byte reads). PC is not changed.
    /// </summary>
    /// <param name="address">The address of the data to read.</param>
    /// <returns>A word value.</returns>
    private Word ReadWord(Word address) => (Word)(ReadByte(address) | ReadByte((Word)(address + 1)) << 8);

    /// <summary>
    /// Writes an 8-bit value to the specified location.
    /// It costs 6 T-states.
    /// </summary>
    /// <param name="address">The address to write to.</param>
    /// <param name="data">The value to write to the memory.</param>
    private void WriteByte(Word address, byte data)
    {
        _memory.Write(address, data);

        States.Add(3);
    }

    /// <summary>
    /// Writes a 16-bit value to the specified location.
    /// It costs 12 T-states (2 byte writes).
    /// </summary>
    /// <param name="address">The address to write to.</param>
    /// <param name="data">The value to write to the memory.</param>
    private void WriteWord(Word address, Word data)
    {
        WriteByte(address, (byte)data);
        WriteByte((Word)(address + 1), (byte)(data >> 8));
    }

    /// <summary>
    /// Reads a value from the data bus.
    /// </summary>
    /// <param name="topHalf">The top 8 bits of the data bus address (A8-A15).</param>
    /// <param name="bottomHalf">The bottom 8 bits of the data bus address (A0-A7).</param>
    /// <returns>The value from the data bus.</returns>
    private byte ReadBus(byte topHalf, byte bottomHalf)
    {
        States.Add(4);

        return _bus?.Read((Word)((topHalf << 8) | bottomHalf)) ?? 0xFF;
    }

    /// <summary>
    /// Writes a value to the data bus.
    /// </summary>
    /// <param name="topHalf">The top 8 bits of the data bus address (A8-A15).</param>
    /// <param name="bottomHalf">The bottom 8 bits of the data bus address (A0-A7).</param>
    /// <param name="data">The data to be written</param>
    private void WriteBus(byte topHalf, byte bottomHalf, byte data)
    {
        States.Add(4);

        _bus?.Write((Word)((topHalf << 8) | bottomHalf), data);
    }

    /// <summary>
    /// Reads an 8-bit value at the location provided in the HL register. This method is aware
    /// of current HL context, e.g. will use IX / IY pair and offset if applicable.
    /// </summary>
    /// <returns>A byte value located at the address provided in the HL (or IX/IY) register.</returns>
    private byte ReadMemoryAtHL(int extraIndexStates) => ReadByte(CalculateHLAddress(extraIndexStates));

    /// <summary>
    /// Calculates the value of HL, IX+d or IY+d register value depending
    /// on the current context (offset will be read and added if needed).
    /// </summary>
    /// <param name="extraIndexStates">Extra T-states to add when index register is used.</param>
    /// <returns>Value of HL, IX+d or IY+d register.</returns>
    private Word CalculateHLAddress(int extraIndexStates)
    {
        sbyte offset = 0;

        if (Registers.Context != RegisterContext.HL)
        {
            offset = (sbyte)FetchByte();
            States.Add(extraIndexStates);
        }

        return (Word)(Registers.XHL + offset);
    }
}