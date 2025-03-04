using OldBit.Z80Cpu.Registers;
using static OldBit.Z80Cpu.Registers.Flags;

namespace OldBit.Z80Cpu;

partial class Z80
{
    private static readonly byte[] BitMasks = [0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80];
    private static readonly byte[] InvertedBitMasks = [0xFE, 0xFD, 0xFB, 0xF7, 0xEF, 0xDF, 0xBF, 0x7F ];

    private void AddBitSetResetTestInstructions()
    {
        for (var i = 0; i <= 7; i++)
        {
            var bit = i;

            _opCodes[$"BIT {bit},A"] = () => { Execute_BIT(bit, Registers.A); };
            _opCodes[$"BIT {bit},B"] = () => { Execute_BIT(bit, Registers.B); };
            _opCodes[$"BIT {bit},C"] = () => { Execute_BIT(bit, Registers.C); };
            _opCodes[$"BIT {bit},D"] = () => { Execute_BIT(bit, Registers.D); };
            _opCodes[$"BIT {bit},E"] = () => { Execute_BIT(bit, Registers.E); };
            _opCodes[$"BIT {bit},H"] = () => { Execute_BIT(bit, Registers.H); };
            _opCodes[$"BIT {bit},L"] = () => { Execute_BIT(bit, Registers.L); };
            _opCodes[$"BIT {bit},(HL)"] = () => Execute_BIT(bit, useHL: true);

            _opCodes[$"SET {bit},A"] = () => { Registers.A = Execute_SET(bit, Registers.A); };
            _opCodes[$"SET {bit},B"] = () => { Registers.B = Execute_SET(bit, Registers.B); };
            _opCodes[$"SET {bit},C"] = () => { Registers.C = Execute_SET(bit, Registers.C); };
            _opCodes[$"SET {bit},D"] = () => { Registers.D = Execute_SET(bit, Registers.D); };
            _opCodes[$"SET {bit},E"] = () => { Registers.E = Execute_SET(bit, Registers.E); };
            _opCodes[$"SET {bit},H"] = () => { Registers.H = Execute_SET(bit, Registers.H); };
            _opCodes[$"SET {bit},L"] = () => { Registers.L = Execute_SET(bit, Registers.L); };
            _opCodes[$"SET {bit},(HL)"] = () => Execute_SET(bit, useHL: true);

            _opCodes[$"RES {bit},A"] = () => { Registers.A = Execute_RES(bit, Registers.A); };
            _opCodes[$"RES {bit},B"] = () => { Registers.B = Execute_RES(bit, Registers.B); };
            _opCodes[$"RES {bit},C"] = () => { Registers.C = Execute_RES(bit, Registers.C); };
            _opCodes[$"RES {bit},D"] = () => { Registers.D = Execute_RES(bit, Registers.D); };
            _opCodes[$"RES {bit},E"] = () => { Registers.E = Execute_RES(bit, Registers.E); };
            _opCodes[$"RES {bit},H"] = () => { Registers.H = Execute_RES(bit, Registers.H); };
            _opCodes[$"RES {bit},L"] = () => { Registers.L = Execute_RES(bit, Registers.L); };
            _opCodes[$"RES {bit},(HL)"] = () => Execute_RES(bit, useHL: true);
        }
    }

    private void Execute_BIT(int bit, byte value = 0, bool useHL = false)
    {
        Word address = 0;
        var isMemory = Registers.UseIndexRegister || useHL;

        if (isMemory)
        {
            address = (Word)(Registers.XHL + _indexRegisterOffset);
            value = ReadByte(address);

            Clock.AddMemoryContention(address, 1);
        }

        var result = value & BitMasks[bit];
        Registers.F &= C;
        Registers.F |= H;
        Registers.F |= result == 0 ? Z | P : 0;
        Registers.F |= result != 0 && bit == 7 ? S : 0;

        if (isMemory)
        {
            if (Registers.UseIndexRegister)
            {
                Registers.F |= (Flags)(address >> 8) & (X | Y);
            }
            else
            {
                Registers.F |= (Flags)value & (X | Y);
            }
        }
        else
        {
            Registers.F |= (Flags)value & (Y | X);
        }
    }

    private byte Execute_SET(int bit, byte value = 0, bool useHL = false)
    {
        Word address = 0;
        var isMemory = Registers.UseIndexRegister || useHL;

        if (isMemory)
        {
            address = (Word)(Registers.XHL + _indexRegisterOffset);
            value = ReadByte(address);

            Clock.AddMemoryContention(address, 1);
        }

        var result = (byte)(value | BitMasks[bit]);

        if (isMemory)
        {
            WriteByte(address, result);
        }

        return result;
    }

    private byte Execute_RES(int bit, byte value = 0, bool useHL = false)
    {
        Word address = 0;
        var isMemory = Registers.UseIndexRegister || useHL;

        if (isMemory)
        {
            address = (Word)(Registers.XHL + _indexRegisterOffset);
            value = ReadByte(address);

            Clock.AddMemoryContention(address, 1);
        }

        var result = (byte)(value & InvertedBitMasks[bit]);

        if (isMemory)
        {
            WriteByte(address, result);
        }

        return result;
    }
}
