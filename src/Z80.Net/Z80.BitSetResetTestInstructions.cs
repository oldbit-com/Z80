using static Z80.Net.Registers.Flags;

namespace Z80.Net;

partial class Z80
{
    private static readonly int[] BitMasks = [0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80];

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
            _opCodes[$"BIT {bit},(HL)"] = () => { Execute_BIT(bit); };

            _opCodes[$"SET {bit},A"] = () => { Execute_SET(bit, Registers.A); };
            _opCodes[$"SET {bit},B"] = () => { Execute_SET(bit, Registers.B); };
            _opCodes[$"SET {bit},C"] = () => { Execute_SET(bit, Registers.C); };
            _opCodes[$"SET {bit},D"] = () => { Execute_SET(bit, Registers.D); };
            _opCodes[$"SET {bit},E"] = () => { Execute_SET(bit, Registers.E); };
            _opCodes[$"SET {bit},H"] = () => { Execute_SET(bit, Registers.H); };
            _opCodes[$"SET {bit},L"] = () => { Execute_SET(bit, Registers.L); };
            _opCodes[$"SET {bit},(HL)"] = () => { Execute_SET(bit); };

            _opCodes[$"RES {bit},A"] = () => { Execute_RES(bit, Registers.A); };
            _opCodes[$"RES {bit},B"] = () => { Execute_RES(bit, Registers.B); };
            _opCodes[$"RES {bit},C"] = () => { Execute_RES(bit, Registers.C); };
            _opCodes[$"RES {bit},D"] = () => { Execute_RES(bit, Registers.D); };
            _opCodes[$"RES {bit},E"] = () => { Execute_RES(bit, Registers.E); };
            _opCodes[$"RES {bit},H"] = () => { Execute_RES(bit, Registers.H); };
            _opCodes[$"RES {bit},L"] = () => { Execute_RES(bit, Registers.L); };
            _opCodes[$"RES {bit},(HL)"] = () => { Execute_RES(bit); };
        }
    }

    private void Execute_BIT(int bit, byte value)
    {
        var result = value & BitMasks[bit];
        Registers.F &= C;
        Registers.F |= H;
        Registers.F |= result == 0 ? Z | P : 0;
        Registers.F |= bit == 7 && result != 0 ? S : 0;
        Registers.F |= bit == 5 && result != 0 ? Y : 0;
        Registers.F |= bit == 3 && result != 0 ? X : 0;
    }

    private void Execute_BIT(int bit)
    {
        var address = (Word)(Registers.XHL + _indexRegisterOffset);
        var value = ReadByte(address);
        AddStates(1);

        var result = value & BitMasks[bit];
        Registers.F &= C;
        Registers.F |= H;
        Registers.F |= result == 0 ? Z | P : 0;
        Registers.F |= bit == 7 && result != 0 ? S : 0;
        Registers.F |= bit == 5 && Registers.H != 0 ? Y : 0;
        Registers.F |= bit == 3 && Registers.H != 0 ? X : 0;
    }

    private void Execute_SET(int bit, byte value)
    {
        // TODO: Implement
    }

    private void Execute_SET(int bit)
    {
        // TODO: Implement
    }

    private void Execute_RES(int bit, byte value)
    {
        // TODO: Implement
    }

    private void Execute_RES(int bit)
    {
        // TODO: Implement
    }
}