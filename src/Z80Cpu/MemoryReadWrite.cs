using OldBit.Z80Cpu.Registers;

namespace OldBit.Z80Cpu;

partial class Z80
{
    /// <summary>
    /// Fetches the next instruction to be executed.
    /// The operation costs 4 T-states and PC is incremented by 1.
    /// </summary>
    /// <returns>An 8-bit value representing the opcode.</returns>
    private byte FetchOpCode()
    {
        var opCode = FetchByte(ticks: 4);

        IncrementR();

        return opCode;
    }

    /// <summary>
    /// Reads an 8-bit value from the location specified by current PC value.
    /// It costs 3 T-states and PC is incremented by 1.
    /// </summary>
    /// <returns>A data byte at the current PC address.</returns>
    private byte FetchByte() => FetchByte(ticks: 3);

    /// <summary>
    /// Reads an 8-bit value from the location specified by current PC value.
    /// PC is incremented by 1.
    /// </summary>
    /// <param name="ticks">The number of T-states to add.</param>
    /// <returns>A data byte at the current PC address.</returns>
    private byte FetchByte(int ticks)
    {
        var value = ReadByte(Registers.PC, ticks);

        Registers.PC += 1;

        return value;
    }

    /// <summary>
    /// Reads a 8-bit value from the location provided.
    /// PC is not changed.
    /// </summary>
    /// <param name="address">The address of the data to read.</param>
    /// <param name="ticks">The number of T-states to add. The default is 3.</param>
    /// <returns>A byte value.</returns>
    private byte ReadByte(Word address, int ticks = 3)
    {
        Clock.Add(ticks);

        var value = _memory.Read(address);

        return value;
    }

    /// <summary>
    /// Reads a 16-bit value from the location specified by current PC value.
    /// The operation takes 6 T-states and PC is incremented by 2.
    /// </summary>
    private Word FetchWord() => (Word)(FetchByte() | (FetchByte() << 8));

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
        Clock.Add(3);

        _memory.Write(address, data);
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
    /// Calculates the value of the HL, IX+d or IY+d register depending
    /// on the current context (offset will be read and added if needed).
    /// </summary>
    /// <param name="extraIndexTicks">Extra T-states to add when index register is used.</param>
    /// <returns>Value of HL, IX+d or IY+d register.</returns>
    private Word CalculateExtendedHL(int extraIndexTicks)
    {
        sbyte offset = 0;

        if (Registers.Context != RegisterContext.HL)
        {
            offset = (sbyte)FetchByte();
            Clock.Add(extraIndexTicks);
        }

        return (Word)(Registers.XHL + offset);
    }

    /// <summary>
    /// Reads an 8-bit value at the location provided in the HL register. This method is aware
    /// of current HL context, e.g. will use IX / IY pair and offset if applicable.
    /// </summary>
    /// <returns>A byte value located at the address provided in the HL (or IX/IY) register.</returns>
    private byte ReadByteAtExtendedHL(int extraIndexTicks) => ReadByte(CalculateExtendedHL(extraIndexTicks));
}
