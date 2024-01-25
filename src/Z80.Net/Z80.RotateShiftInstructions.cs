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

        _opCodes[RLC_A] = () => { Registers.A = RLC(Registers.A); };
        _opCodes[RLC_B] = () => { Registers.B = RLC(Registers.B); };
        _opCodes[RLC_C] = () => { Registers.C = RLC(Registers.C); };
        _opCodes[RLC_D] = () => { Registers.D = RLC(Registers.D); };
        _opCodes[RLC_E] = () => { Registers.E = RLC(Registers.E); };
        _opCodes[RLC_H] = () => { Registers.H = RLC(Registers.H); };
        _opCodes[RLC_L] = () => { Registers.L = RLC(Registers.L); };
        _opCodes[RLC_HL] = () =>
        {
            var address = (ushort)(Registers.XHL + _indexOffset);
            var value = ReadByte(address);
            AddCycles(1);
            WriteByte(address, RLC(value));
        };

        _opCodes[RL_A] = () => { Registers.A = RL(Registers.A); };
        _opCodes[RL_B] = () => { Registers.B = RL(Registers.B); };
        _opCodes[RL_C] = () => { Registers.C = RL(Registers.C); };
        _opCodes[RL_D] = () => { Registers.D = RL(Registers.D); };
        _opCodes[RL_E] = () => { Registers.E = RL(Registers.E); };
        _opCodes[RL_H] = () => { Registers.H = RL(Registers.H); };
        _opCodes[RL_L] = () => { Registers.L = RL(Registers.L); };
        _opCodes[RL_HL] = () =>
        {
            var address = (ushort)(Registers.XHL + _indexOffset);
            var value = ReadByte(address);
            AddCycles(1);
            WriteByte(address, RL(value));
        };

        _opCodes[RRC_A] = () => { Registers.A = RRC(Registers.A); };
        _opCodes[RRC_B] = () => { Registers.B = RRC(Registers.B); };
        _opCodes[RRC_C] = () => { Registers.C = RRC(Registers.C); };
        _opCodes[RRC_D] = () => { Registers.D = RRC(Registers.D); };
        _opCodes[RRC_E] = () => { Registers.E = RRC(Registers.E); };
        _opCodes[RRC_H] = () => { Registers.H = RRC(Registers.H); };
        _opCodes[RRC_L] = () => { Registers.L = RRC(Registers.L); };
        _opCodes[RRC_HL] = () =>
        {
            var address = (ushort)(Registers.XHL + _indexOffset);
            var value = ReadByte(address);
            AddCycles(1);
            WriteByte(address, RRC(value));
        };

        _opCodes[RR_A] = () => { Registers.A = RR(Registers.A); };
        _opCodes[RR_B] = () => { Registers.B = RR(Registers.B); };
        _opCodes[RR_C] = () => { Registers.C = RR(Registers.C); };
        _opCodes[RR_D] = () => { Registers.D = RR(Registers.D); };
        _opCodes[RR_E] = () => { Registers.E = RR(Registers.E); };
        _opCodes[RR_H] = () => { Registers.H = RR(Registers.H); };
        _opCodes[RR_L] = () => { Registers.L = RR(Registers.L); };
        _opCodes[RR_HL] = () =>
        {
            var address = (ushort)(Registers.XHL + _indexOffset);
            var value = ReadByte(address);
            AddCycles(1);
            WriteByte(address, RR(value));
        };

        _opCodes[SLA_A] = () => { Registers.A = SLA(Registers.A); };
        _opCodes[SLA_B] = () => { Registers.B = SLA(Registers.B); };
        _opCodes[SLA_C] = () => { Registers.C = SLA(Registers.C); };
        _opCodes[SLA_D] = () => { Registers.D = SLA(Registers.D); };
        _opCodes[SLA_E] = () => { Registers.E = SLA(Registers.E); };
        _opCodes[SLA_H] = () => { Registers.H = SLA(Registers.H); };
        _opCodes[SLA_L] = () => { Registers.L = SLA(Registers.L); };
        _opCodes[SLA_HL] = () =>
        {
            var address = (ushort)(Registers.XHL + _indexOffset);
            var value = ReadByte(address);
            AddCycles(1);
            WriteByte(address, SLA(value));
        };

        _opCodes[SRA_A] = () => { Registers.A = SRA(Registers.A); };
        _opCodes[SRA_B] = () => { Registers.B = SRA(Registers.B); };
        _opCodes[SRA_C] = () => { Registers.C = SRA(Registers.C); };
        _opCodes[SRA_D] = () => { Registers.D = SRA(Registers.D); };
        _opCodes[SRA_E] = () => { Registers.E = SRA(Registers.E); };
        _opCodes[SRA_H] = () => { Registers.H = SRA(Registers.H); };
        _opCodes[SRA_L] = () => { Registers.L = SRA(Registers.L); };
        _opCodes[SRA_HL] = () =>
        {
            var address = (ushort)(Registers.XHL + _indexOffset);
            var value = ReadByte(address);
            AddCycles(1);
            WriteByte(address, SRA(value));
        };
    }

    private byte RLC(byte value)
    {
        var bit7 = value >> 7;
        var result = (byte)(value << 1 | bit7);

        Registers.F = (S | Y | X) & (Flags)result;
        Registers.F |= result == 0 ? Z : 0;
        Registers.F |= (Flags)bit7 & C;
        Registers.F |= Parity.Lookup[result];

        return result;
    }

    private byte RL(byte value)
    {
        var bit7 = value >> 7;
        var result = (byte)(value << 1 | (byte)(Registers.F & C));

        Registers.F = (S | Y | X) & (Flags)result;
        Registers.F |= result == 0 ? Z : 0;
        Registers.F |= (Flags)bit7 & C;
        Registers.F |= Parity.Lookup[result];

        return result;
    }

    private byte RRC(byte value)
    {
        var bit0 = value & 1;
        var result = (byte)(value >> 1 | bit0 << 7);

        Registers.F = (S | Y | X) & (Flags)result;
        Registers.F |= result == 0 ? Z : 0;
        Registers.F |= (Flags)bit0 & C;
        Registers.F |= Parity.Lookup[result];

        return result;
    }

    private byte RR(byte value)
    {
        var bit0 = value & 1;
        var result = (byte)(value >> 1 | (byte)(Registers.F & C) << 7);

        Registers.F = (S | Y | X) & (Flags)result;
        Registers.F |= result == 0 ? Z : 0;
        Registers.F |= (Flags)bit0 & C;
        Registers.F |= Parity.Lookup[result];

        return result;
    }

    private byte SLA(byte value)
    {
        var bit7 = value >> 7;
        var result = (byte)(value << 1);

        Registers.F = (S | Y | X) & (Flags)result;
        Registers.F |= result == 0 ? Z : 0;
        Registers.F |= (Flags)bit7 & C;
        Registers.F |= Parity.Lookup[result];

        return result;
    }

    private byte SRA(byte value)
    {
        var bit0 = value & 1;
        var result = (byte)(value & 0x80 | value >> 1);

        Registers.F = (S | Y | X) & (Flags)result;
        Registers.F |= result == 0 ? Z : 0;
        Registers.F |= (Flags)bit0 & C;
        Registers.F |= Parity.Lookup[result];

        return result;
    }

    private void UpdateFlagsForShiftAndRotate(byte result, byte carry)
    {
        Registers.F = (S | Y | X) & (Flags)result;
        Registers.F |= result == 0 ? Z : 0;
        Registers.F |= (Flags)carry & C;
        Registers.F |= Parity.Lookup[result];
    }
}