using Z80.Net.Helpers;
using Z80.Net.Registers;
using static Z80.Net.Instructions.OpCodes;
using static Z80.Net.Registers.Flags;

namespace Z80.Net;

partial class Z80
{
    private void AddRotateShiftInstructions()
    {
        _opCodes[RLCA] = () =>
        {
            var bit7 = Registers.A >> 7;
            Registers.A = (byte)(Registers.A << 1 | bit7);
            Registers.F &= S | Z | P;
            Registers.F |= (Flags)bit7;
            Registers.F |= (Y | X) & (Flags)Registers.A;
        };

        _opCodes[RRCA] = () =>
        {
            var bit0 = Registers.A & 0x01;
            Registers.A = (byte)(Registers.A >> 1 | bit0 << 7);
            Registers.F &= S | Z | P;
            Registers.F |= (Flags)bit0;
            Registers.F |= (Y | X) & (Flags)Registers.A;
        };

        _opCodes[RLA] = () =>
        {
            var bit7 = Registers.A >> 7;
            Registers.A = (byte)(Registers.A << 1 | (byte)(Registers.F & C));
            Registers.F &= S | Z | P;
            Registers.F |= (Flags)bit7;
            Registers.F |= (Y | X) & (Flags)Registers.A;
        };

        _opCodes[RRA] = () =>
        {
            var bit0 = Registers.A & 0x01;
            Registers.A = (byte)(Registers.A >> 1 | (byte)(Registers.F & C) << 7);
            Registers.F &= S | Z | P;
            Registers.F |= (Flags)bit0;
            Registers.F |= (Y | X) & (Flags)Registers.A;
        };

        _opCodes[RLC_A] = () => { Registers.A = Rlc(Registers.A); };
        _opCodes[RLC_B] = () => { Registers.B = Rlc(Registers.B); };
        _opCodes[RLC_C] = () => { Registers.C = Rlc(Registers.C); };
        _opCodes[RLC_D] = () => { Registers.D = Rlc(Registers.D); };
        _opCodes[RLC_E] = () => { Registers.E = Rlc(Registers.E); };
        _opCodes[RLC_H] = () => { Registers.H = Rlc(Registers.H); };
        _opCodes[RLC_L] = () => { Registers.L = Rlc(Registers.L); };
        _opCodes[RLC_HL] = () =>
        {
            var address = (ushort)(Registers.XHL + _indexOffset);
            var value = ReadByte(address);
            AddCycles(1);
            WriteByte(address, Rlc(value));
        };

        _opCodes[RL_A] = () => { Registers.A = Rl(Registers.A); };
        _opCodes[RL_B] = () => { Registers.B = Rl(Registers.B); };
        _opCodes[RL_C] = () => { Registers.C = Rl(Registers.C); };
        _opCodes[RL_D] = () => { Registers.D = Rl(Registers.D); };
        _opCodes[RL_E] = () => { Registers.E = Rl(Registers.E); };
        _opCodes[RL_H] = () => { Registers.H = Rl(Registers.H); };
        _opCodes[RL_L] = () => { Registers.L = Rl(Registers.L); };
        _opCodes[RL_HL] = () =>
        {
            var address = (ushort)(Registers.XHL + _indexOffset);
            var value = ReadByte(address);
            AddCycles(1);
            WriteByte(address, Rl(value));
        };
    }

    private byte Rlc(byte value)
    {
        var bit7 = value >> 7;
        var result = (byte)(value << 1 | bit7);

        Registers.F = (S | Y | X) & (Flags)result;
        Registers.F |= result == 0 ? Z : 0;
        Registers.F |= (Flags)bit7;
        Registers.F |= Parity.Lookup[result];

        return result;
    }

    private byte Rl(byte value)
    {
        return 0;
    }
}