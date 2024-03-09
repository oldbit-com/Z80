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

            _opCodes[$"BIT {bit},A"] = () => { ExecuteBIT(bit, Registers.A); };
            _opCodes[$"BIT {bit},B"] = () => { ExecuteBIT(bit, Registers.B); };
            _opCodes[$"BIT {bit},C"] = () => { ExecuteBIT(bit, Registers.C); };
            _opCodes[$"BIT {bit},D"] = () => { ExecuteBIT(bit, Registers.D); };
            _opCodes[$"BIT {bit},E"] = () => { ExecuteBIT(bit, Registers.E); };
            _opCodes[$"BIT {bit},H"] = () => { ExecuteBIT(bit, Registers.H); };
            _opCodes[$"BIT {bit},L"] = () => { ExecuteBIT(bit, Registers.L); };
            _opCodes[$"BIT {bit},(HL)"] = () => { ExecuteMemoryBIT(bit); };

            _opCodes[$"SET {bit},A"] = () => { ExecuteSET(bit, Registers.A); };
            _opCodes[$"SET {bit},B"] = () => { ExecuteSET(bit, Registers.B); };
            _opCodes[$"SET {bit},C"] = () => { ExecuteSET(bit, Registers.C); };
            _opCodes[$"SET {bit},D"] = () => { ExecuteSET(bit, Registers.D); };
            _opCodes[$"SET {bit},E"] = () => { ExecuteSET(bit, Registers.E); };
            _opCodes[$"SET {bit},H"] = () => { ExecuteSET(bit, Registers.H); };
            _opCodes[$"SET {bit},L"] = () => { ExecuteSET(bit, Registers.L); };
            _opCodes[$"SET {bit},(HL)"] = () => { ExecuteMemorySET(bit); };

            _opCodes[$"RES {bit},A"] = () => { ExecuteRES(bit, Registers.A); };
            _opCodes[$"RES {bit},B"] = () => { ExecuteRES(bit, Registers.B); };
            _opCodes[$"RES {bit},C"] = () => { ExecuteRES(bit, Registers.C); };
            _opCodes[$"RES {bit},D"] = () => { ExecuteRES(bit, Registers.D); };
            _opCodes[$"RES {bit},E"] = () => { ExecuteRES(bit, Registers.E); };
            _opCodes[$"RES {bit},H"] = () => { ExecuteRES(bit, Registers.H); };
            _opCodes[$"RES {bit},L"] = () => { ExecuteRES(bit, Registers.L); };
            _opCodes[$"RES {bit},(HL)"] = () => { ExecuteMemoryRES(bit); };
        }
    }

    private void ExecuteBIT(int bit, byte value)
    {
        var result = value & BitMasks[bit];
        Registers.F &= C;
        Registers.F |= H;
        Registers.F |= result == 0 ? Z | P : 0;
        Registers.F |= bit == 7 && result != 0 ? S : 0;
        Registers.F |= bit == 5 && result != 0 ? Y : 0;
        Registers.F |= bit == 3 && result != 0 ? X : 0;
    }

    private void ExecuteMemoryBIT(int bit)
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

    private void ExecuteSET(int bit, byte value)
    {

    }

    private void ExecuteMemorySET(int bit)
    {

    }

    private void ExecuteRES(int bit, byte value)
    {

    }

    private void ExecuteMemoryRES(int bit)
    {

    }
}