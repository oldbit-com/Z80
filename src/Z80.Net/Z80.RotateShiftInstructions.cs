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

        _opCodes[RLD] = () =>
        {   // A                        (HL)
            // a7 a6 a5 a4 a3 a2 a1 a0  m7 m6 m5 m4 m3 m2 m1 m0  Before
            // a7 a6 a5 a4 m7 m6 m5 m4  m3 m2 m1 m0 a3 a2 a1 a0  After
            // A = (HL) >> 4 | A & 0xF0
            // (HL) = (HL) << 4 | A & 0x0F
            AddCycles(4);

            var value = ReadByte(Registers.HL);
            var newValue = (byte)((value << 4) | (Registers.A & 0x0F));
            Registers.A = (byte)((value >> 4) | (Registers.A & 0xF0));

            WriteByte(Registers.HL, newValue);

            Registers.F &= C;
            Registers.F |= Registers.A == 0 ? Z : 0;
            Registers.F |= (S | Y | X) & (Flags)Registers.A;
            Registers.F |= Parity.Lookup[Registers.A];
        };

        _opCodes[RRD] = () =>
        {

        };

        _opCodes[RLC_A] = () => { Registers.A = RLC(Registers.A); };
        _opCodes[RLC_B] = () => { Registers.B = RLC(Registers.B); };
        _opCodes[RLC_C] = () => { Registers.C = RLC(Registers.C); };
        _opCodes[RLC_D] = () => { Registers.D = RLC(Registers.D); };
        _opCodes[RLC_E] = () => { Registers.E = RLC(Registers.E); };
        _opCodes[RLC_H] = () => { Registers.H = RLC(Registers.H); };
        _opCodes[RLC_L] = () => { Registers.L = RLC(Registers.L); };
        _opCodes[RLC_HL] = () => ShiftRotateMemory(RLC);

        _opCodes[RL_A] = () => { Registers.A = RL(Registers.A); };
        _opCodes[RL_B] = () => { Registers.B = RL(Registers.B); };
        _opCodes[RL_C] = () => { Registers.C = RL(Registers.C); };
        _opCodes[RL_D] = () => { Registers.D = RL(Registers.D); };
        _opCodes[RL_E] = () => { Registers.E = RL(Registers.E); };
        _opCodes[RL_H] = () => { Registers.H = RL(Registers.H); };
        _opCodes[RL_L] = () => { Registers.L = RL(Registers.L); };
        _opCodes[RL_HL] = () => ShiftRotateMemory(RL);

        _opCodes[RRC_A] = () => { Registers.A = RRC(Registers.A); };
        _opCodes[RRC_B] = () => { Registers.B = RRC(Registers.B); };
        _opCodes[RRC_C] = () => { Registers.C = RRC(Registers.C); };
        _opCodes[RRC_D] = () => { Registers.D = RRC(Registers.D); };
        _opCodes[RRC_E] = () => { Registers.E = RRC(Registers.E); };
        _opCodes[RRC_H] = () => { Registers.H = RRC(Registers.H); };
        _opCodes[RRC_L] = () => { Registers.L = RRC(Registers.L); };
        _opCodes[RRC_HL] = () => ShiftRotateMemory(RRC);

        _opCodes[RR_A] = () => { Registers.A = RR(Registers.A); };
        _opCodes[RR_B] = () => { Registers.B = RR(Registers.B); };
        _opCodes[RR_C] = () => { Registers.C = RR(Registers.C); };
        _opCodes[RR_D] = () => { Registers.D = RR(Registers.D); };
        _opCodes[RR_E] = () => { Registers.E = RR(Registers.E); };
        _opCodes[RR_H] = () => { Registers.H = RR(Registers.H); };
        _opCodes[RR_L] = () => { Registers.L = RR(Registers.L); };
        _opCodes[RR_HL] = () => ShiftRotateMemory(RR);

        _opCodes[SLA_A] = () => { Registers.A = SLA(Registers.A); };
        _opCodes[SLA_B] = () => { Registers.B = SLA(Registers.B); };
        _opCodes[SLA_C] = () => { Registers.C = SLA(Registers.C); };
        _opCodes[SLA_D] = () => { Registers.D = SLA(Registers.D); };
        _opCodes[SLA_E] = () => { Registers.E = SLA(Registers.E); };
        _opCodes[SLA_H] = () => { Registers.H = SLA(Registers.H); };
        _opCodes[SLA_L] = () => { Registers.L = SLA(Registers.L); };
        _opCodes[SLA_HL] = () => ShiftRotateMemory(SLA);

        _opCodes[SRA_A] = () => { Registers.A = SRA(Registers.A); };
        _opCodes[SRA_B] = () => { Registers.B = SRA(Registers.B); };
        _opCodes[SRA_C] = () => { Registers.C = SRA(Registers.C); };
        _opCodes[SRA_D] = () => { Registers.D = SRA(Registers.D); };
        _opCodes[SRA_E] = () => { Registers.E = SRA(Registers.E); };
        _opCodes[SRA_H] = () => { Registers.H = SRA(Registers.H); };
        _opCodes[SRA_L] = () => { Registers.L = SRA(Registers.L); };
        _opCodes[SRA_HL] = () => ShiftRotateMemory(SRA);

        _opCodes[SRL_A] = () => { Registers.A = SRL(Registers.A); };
        _opCodes[SRL_B] = () => { Registers.B = SRL(Registers.B); };
        _opCodes[SRL_C] = () => { Registers.C = SRL(Registers.C); };
        _opCodes[SRL_D] = () => { Registers.D = SRL(Registers.D); };
        _opCodes[SRL_E] = () => { Registers.E = SRL(Registers.E); };
        _opCodes[SRL_H] = () => { Registers.H = SRL(Registers.H); };
        _opCodes[SRL_L] = () => { Registers.L = SRL(Registers.L); };
        _opCodes[SRL_HL] = () => ShiftRotateMemory(SRL);

        _opCodes[SLL_A] = () => { Registers.A = SLL(Registers.A); };
        _opCodes[SLL_B] = () => { Registers.B = SLL(Registers.B); };
        _opCodes[SLL_C] = () => { Registers.C = SLL(Registers.C); };
        _opCodes[SLL_D] = () => { Registers.D = SLL(Registers.D); };
        _opCodes[SLL_E] = () => { Registers.E = SLL(Registers.E); };
        _opCodes[SLL_H] = () => { Registers.H = SLL(Registers.H); };
        _opCodes[SLL_L] = () => { Registers.L = SLL(Registers.L); };
        _opCodes[SLL_HL] = () => ShiftRotateMemory(SLL);
    }

    private byte RLC(byte value)
    {
        var bit7 = value >> 7;
        var result = (byte)(value << 1 | bit7);

        UpdateFlagsForShiftAndRotate(result, bit7);

        return result;
    }

    private byte RL(byte value)
    {
        var bit7 = value >> 7;
        var result = (byte)(value << 1 | (byte)(Registers.F & C));

        UpdateFlagsForShiftAndRotate(result, bit7);

        return result;
    }

    private byte RRC(byte value)
    {
        var bit0 = value & 1;
        var result = (byte)(value >> 1 | bit0 << 7);

        UpdateFlagsForShiftAndRotate(result, bit0);

        return result;
    }

    private byte RR(byte value)
    {
        var bit0 = value & 1;
        var result = (byte)(value >> 1 | (byte)(Registers.F & C) << 7);

        UpdateFlagsForShiftAndRotate(result, bit0);

        return result;
    }

    private byte SLA(byte value)
    {
        var bit7 = value >> 7;
        var result = (byte)(value << 1);

        UpdateFlagsForShiftAndRotate(result, bit7);

        return result;
    }

    private byte SRA(byte value)
    {
        var bit0 = value & 1;
        var result = (byte)(value & 0x80 | value >> 1);

        UpdateFlagsForShiftAndRotate(result, bit0);

        return result;
    }

    private byte SRL(byte value)
    {
        var bit0 = value & 1;
        var result = (byte)(value >> 1);

        UpdateFlagsForShiftAndRotate(result, bit0);

        return result;
    }

    private byte SLL(byte value) => (byte)(SLA(value) | 1);

    private void UpdateFlagsForShiftAndRotate(byte result, int carry)
    {
        Registers.F = (S | Y | X) & (Flags)result;
        Registers.F |= result == 0 ? Z : 0;
        Registers.F |= (Flags)carry & C;
        Registers.F |= Parity.Lookup[result];
    }

    private void ShiftRotateMemory(Func<byte, byte> calculate)
    {
        var address = (ushort)(Registers.XHL + _indexOffset);
        var value = ReadByte(address);
        AddCycles(1);
        var result = calculate(value);
        WriteByte(address, result);
    }
}