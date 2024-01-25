using Z80.Net.Instructions;
using Z80.Net.Registers;

namespace Z80.Net;

public partial class Z80
{
    private readonly IMemory _memory;
    private readonly OpCodesIndex _opCodes = new();
    private byte _opCodePrefix;
    private sbyte _indexOffset;

    public CpuRegisters Registers { get; private set; } = new();
    public bool IsHalted { get; private set; }
    public bool IFF1 { get; internal set; }
    public bool IFF2 { get; internal set; }
    public InterruptMode InterruptMode { get; private set; }
    public CycleCounter CycleCounter { get; } = new();

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
        AddSpecialInstructions();
        AddGeneralPurposeArithmeticInstructions();
        Add8BitLoadInstructions();
        Add8BitArithmeticInstructions();
        Add16BitLoadInstructions();
        Add16BitArithmeticInstructions();
        AddRotateShiftInstructions();
        AddUndocumentedInstructions();
    }

    public void Run(int maxCycles = 0)
    {
        CycleCounter.SetLimit(maxCycles);

        while (!CycleCounter.IsComplete)
        {
            if (IsHalted)
            {
                break;
            }

            var opCode = FetchOpCode();

            if (opCode == OpCodes.CB)
            {
                ProcessBitOpCodes();
            }
            else
            {
                var prefixedOpCode = _opCodePrefix << 8 | opCode;
                if (_opCodes.Execute(prefixedOpCode))
                {
                    if (OpCodes.AllOpCodes.TryGetValue(prefixedOpCode, out var opCode1))
                    {
                        Console.WriteLine($"{CycleCounter.TotalCycles} : {opCode1.Mnemonic}");
                    }

                    if (IsPrefixOpCode(opCode))
                    {
                        continue;
                    }
                }
            }

            Registers.HLContext = HLContext.HL;
            _opCodePrefix = 0;
            _indexOffset = 0;
        }
    }

    /// <summary>
    /// Processes CB prefixed op codes. They require slightly different handling,
    /// because for IX and IY registers, offset precedes the actual op code. This
    /// is different than standard op codes where offset is after the op code.
    /// </summary>
    private void ProcessBitOpCodes()
    {
        int opCode;
        if (Registers.UseIndexRegister)
        {
            _indexOffset = (sbyte)ReadNextByte();
            opCode = ReadNextByte();
            AddCycles(2);
        }
        else
        {
            opCode = FetchOpCode();
        }

        _opCodes.Execute(0xCB << 8 | opCode);
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

    private bool IsPrefixOpCode(byte opCode) =>
        (opCode == OpCodes.IX || opCode == OpCodes.IY ||
         opCode == OpCodes.ED || opCode == OpCodes.CB);

    /// <summary>
    /// Fetches an opcode of the next instruction to be executed.
    /// The operation takes 4 cycles and PC is incremented by 1 afterwards.
    /// </summary>
    /// <returns>An opcode value.</returns>
    private byte FetchOpCode() => ReadNextByte(cycles: 4);

    /// <summary>
    /// Reads a byte from the memory located at the current PC address.
    /// The operation takes 3 cycles and PC is incremented by 1 afterwards.
    /// </summary>
    /// <returns>A data byte at the current PC address.</returns>
    private byte ReadNextByte() => ReadNextByte(cycles: 3);

    /// <summary>
    /// Reads a byte from the memory located at the current PC address.
    /// PC is incremented by 1 afterwards.
    /// </summary>
    /// <param name="cycles">The number of cycles to add. The default is 3.</param>
    /// <returns>A data byte at the current PC address.</returns>
    private byte ReadNextByte(int cycles)
    {
        var value = _memory.Read(Registers.PC);
        AddCycles(cycles);
        Registers.PC += 1;
        return value;
    }

    /// <summary>
    /// Reads a word from the memory located at the current PC address.
    /// The operation takes 6 cycles and PC is incremented by 2 afterwards.
    /// </summary>
    private ushort ReadNextWord() => (ushort)(ReadNextByte() | (ReadNextByte() << 8));

    /// <summary>
    /// Peeks a byte from the memory located at the specified address.
    /// The operation takes 3 cycles. PC remains unchanged.
    /// </summary>
    /// <param name="address">The address of the data to read.</param>
    /// <returns>A byte value.</returns>
    private byte ReadByte(int address)
    {
        var value = _memory.Read(address);
        AddCycles(3);
        return value;
    }

    /// <summary>
    /// Peeks a word from memory at the specified address. The operation takes 6 cycles. PC is not changed.
    /// </summary>
    /// <param name="address">The address of the data to read.</param>
    /// <returns>A word value.</returns>
    private ushort PeekWord(int address) => (ushort)(ReadByte(address) | ReadByte((ushort)(address + 1)) << 8);

    /// <summary>
    /// Writes a byte to the memory at the specified address. The operation takes 6 cycles.
    /// </summary>
    /// <param name="address">The address to write to.</param>
    /// <param name="value">The value to write to the memory.</param>
    private void WriteByte(int address, byte value)
    {
        _memory.Write(address, value);
        AddCycles(3);
    }

    /// <summary>
    /// Adds specified number of cycles to the current cycles counter.
    /// </summary>
    /// <param name="cycles">The number of cycles.</param>
    private void AddCycles(int cycles) => CycleCounter.Add(cycles);

    /// <summary>
    /// Reads a byte at the memory address provided in HL register. This method is aware
    /// of current HL context, e.g. will use IX / IY pair and offset if applicable.
    /// </summary>
    /// <returns>A byte value at the memory address provided in HL (or IX/IY).</returns>
    private byte ReadMemoryAtHL() => ReadByte(CalculateHLAddress());

    /// <summary>
    /// Calculates the value of HL, IX+d or IY+d register value depending
    /// on the current context (offset will be read and added if needed).
    /// </summary>
    /// <returns>Value of HL, IX+d or IY+d register.</returns>
    private ushort CalculateHLAddress()
    {
        sbyte offset = 0;
        if (Registers.HLContext != HLContext.HL)
        {
            offset = (sbyte)ReadNextByte();
            AddCycles(5);
        }

        return (ushort)(Registers.XHL + offset);
    }
}