using static Z80.Net.Registers.Flags;
using static Z80.Net.Instructions.OpCodes;

namespace Z80.Net;

partial class Z80
{
    private static readonly int[] BitMasks = [0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80];

    private void AddBitSetResetTestInstructions()
    {
        for (var i = 0; i <= 7; i++)
        {
            var bit = i;

            _opCodes[BIT_b_A | bit << 3] = () => { ExecuteBIT(bit, Registers.A); };
            _opCodes[BIT_b_B | bit << 3] = () => { ExecuteBIT(bit, Registers.B); };
            _opCodes[BIT_b_C | bit << 3] = () => { ExecuteBIT(bit, Registers.C); };
            _opCodes[BIT_b_D | bit << 3] = () => { ExecuteBIT(bit, Registers.D); };
            _opCodes[BIT_b_E | bit << 3] = () => { ExecuteBIT(bit, Registers.E); };
            _opCodes[BIT_b_H | bit << 3] = () => { ExecuteBIT(bit, Registers.H); };
            _opCodes[BIT_b_L | bit << 3] = () => { ExecuteBIT(bit, Registers.L); };
            _opCodes[BIT_b_HL | bit << 3] = () => { ExecuteMemoryBIT(bit); };

            _opCodes[SET_b_A | bit << 3] = () => { ExecuteSET(bit, Registers.A); };
            _opCodes[SET_b_B | bit << 3] = () => { ExecuteSET(bit, Registers.B); };
            _opCodes[SET_b_C | bit << 3] = () => { ExecuteSET(bit, Registers.C); };
            _opCodes[SET_b_D | bit << 3] = () => { ExecuteSET(bit, Registers.D); };
            _opCodes[SET_b_E | bit << 3] = () => { ExecuteSET(bit, Registers.E); };
            _opCodes[SET_b_H | bit << 3] = () => { ExecuteSET(bit, Registers.H); };
            _opCodes[SET_b_L | bit << 3] = () => { ExecuteSET(bit, Registers.L); };
            _opCodes[SET_b_HL | bit << 3] = () => { ExecuteMemorySET(bit); };

            _opCodes[RES_b_A | bit << 3] = () => { ExecuteRES(bit, Registers.A); };
            _opCodes[RES_b_B | bit << 3] = () => { ExecuteRES(bit, Registers.B); };
            _opCodes[RES_b_C | bit << 3] = () => { ExecuteRES(bit, Registers.C); };
            _opCodes[RES_b_D | bit << 3] = () => { ExecuteRES(bit, Registers.D); };
            _opCodes[RES_b_E | bit << 3] = () => { ExecuteRES(bit, Registers.E); };
            _opCodes[RES_b_H | bit << 3] = () => { ExecuteRES(bit, Registers.H); };
            _opCodes[RES_b_L | bit << 3] = () => { ExecuteRES(bit, Registers.L); };
            _opCodes[RES_b_HL | bit << 3] = () => { ExecuteMemoryRES(bit); };
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
        var address = (ushort)(Registers.XHL + _indexOffset);
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