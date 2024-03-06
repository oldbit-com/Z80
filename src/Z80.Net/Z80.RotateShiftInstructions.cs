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
            AddStates(4);

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
            // A                        (HL)
            // a7 a6 a5 a4 a3 a2 a1 a0  m7 m6 m5 m4 m3 m2 m1 m0  Before
            // a7 a6 a5 a4 m3 m2 m1 m0  a3 a2 a1 a0 m7 a6 a5 a4  After
            AddStates(4);

            var value = ReadByte(Registers.HL);
            var newValue = (byte)((value >> 4) | (Registers.A << 4));

            Registers.A = (byte)((value & 0x0F) | (Registers.A & 0xF0));
            WriteByte(Registers.HL, newValue);

            Registers.F &= C;
            Registers.F |= Registers.A == 0 ? Z : 0;
            Registers.F |= (S | Y | X) & (Flags)Registers.A;
            Registers.F |= Parity.Lookup[Registers.A];
        };

        _opCodes[RLC_A] = () => { Registers.A = ExecuteRLC(Registers.A); };
        _opCodes[RLC_B] = () => { Registers.B = ExecuteRLC(Registers.B); };
        _opCodes[RLC_C] = () => { Registers.C = ExecuteRLC(Registers.C); };
        _opCodes[RLC_D] = () => { Registers.D = ExecuteRLC(Registers.D); };
        _opCodes[RLC_E] = () => { Registers.E = ExecuteRLC(Registers.E); };
        _opCodes[RLC_H] = () => { Registers.H = ExecuteRLC(Registers.H); };
        _opCodes[RLC_L] = () => { Registers.L = ExecuteRLC(Registers.L); };
        _opCodes[RLC_HL] = () => ShiftRotateMemory(ExecuteRLC);

        _opCodes[RL_A] = () => { Registers.A = ExecuteRL(Registers.A); };
        _opCodes[RL_B] = () => { Registers.B = ExecuteRL(Registers.B); };
        _opCodes[RL_C] = () => { Registers.C = ExecuteRL(Registers.C); };
        _opCodes[RL_D] = () => { Registers.D = ExecuteRL(Registers.D); };
        _opCodes[RL_E] = () => { Registers.E = ExecuteRL(Registers.E); };
        _opCodes[RL_H] = () => { Registers.H = ExecuteRL(Registers.H); };
        _opCodes[RL_L] = () => { Registers.L = ExecuteRL(Registers.L); };
        _opCodes[RL_HL] = () => ShiftRotateMemory(ExecuteRL);

        _opCodes[RRC_A] = () => { Registers.A = ExecuteRRC(Registers.A); };
        _opCodes[RRC_B] = () => { Registers.B = ExecuteRRC(Registers.B); };
        _opCodes[RRC_C] = () => { Registers.C = ExecuteRRC(Registers.C); };
        _opCodes[RRC_D] = () => { Registers.D = ExecuteRRC(Registers.D); };
        _opCodes[RRC_E] = () => { Registers.E = ExecuteRRC(Registers.E); };
        _opCodes[RRC_H] = () => { Registers.H = ExecuteRRC(Registers.H); };
        _opCodes[RRC_L] = () => { Registers.L = ExecuteRRC(Registers.L); };
        _opCodes[RRC_HL] = () => ShiftRotateMemory(ExecuteRRC);

        _opCodes[RR_A] = () => { Registers.A = ExecuteRR(Registers.A); };
        _opCodes[RR_B] = () => { Registers.B = ExecuteRR(Registers.B); };
        _opCodes[RR_C] = () => { Registers.C = ExecuteRR(Registers.C); };
        _opCodes[RR_D] = () => { Registers.D = ExecuteRR(Registers.D); };
        _opCodes[RR_E] = () => { Registers.E = ExecuteRR(Registers.E); };
        _opCodes[RR_H] = () => { Registers.H = ExecuteRR(Registers.H); };
        _opCodes[RR_L] = () => { Registers.L = ExecuteRR(Registers.L); };
        _opCodes[RR_HL] = () => ShiftRotateMemory(ExecuteRR);

        _opCodes[SLA_A] = () => { Registers.A = ExecuteSLA(Registers.A); };
        _opCodes[SLA_B] = () => { Registers.B = ExecuteSLA(Registers.B); };
        _opCodes[SLA_C] = () => { Registers.C = ExecuteSLA(Registers.C); };
        _opCodes[SLA_D] = () => { Registers.D = ExecuteSLA(Registers.D); };
        _opCodes[SLA_E] = () => { Registers.E = ExecuteSLA(Registers.E); };
        _opCodes[SLA_H] = () => { Registers.H = ExecuteSLA(Registers.H); };
        _opCodes[SLA_L] = () => { Registers.L = ExecuteSLA(Registers.L); };
        _opCodes[SLA_HL] = () => ShiftRotateMemory(ExecuteSLA);

        _opCodes[SRA_A] = () => { Registers.A = ExecuteSRA(Registers.A); };
        _opCodes[SRA_B] = () => { Registers.B = ExecuteSRA(Registers.B); };
        _opCodes[SRA_C] = () => { Registers.C = ExecuteSRA(Registers.C); };
        _opCodes[SRA_D] = () => { Registers.D = ExecuteSRA(Registers.D); };
        _opCodes[SRA_E] = () => { Registers.E = ExecuteSRA(Registers.E); };
        _opCodes[SRA_H] = () => { Registers.H = ExecuteSRA(Registers.H); };
        _opCodes[SRA_L] = () => { Registers.L = ExecuteSRA(Registers.L); };
        _opCodes[SRA_HL] = () => ShiftRotateMemory(ExecuteSRA);

        _opCodes[SRL_A] = () => { Registers.A = ExecuteSRL(Registers.A); };
        _opCodes[SRL_B] = () => { Registers.B = ExecuteSRL(Registers.B); };
        _opCodes[SRL_C] = () => { Registers.C = ExecuteSRL(Registers.C); };
        _opCodes[SRL_D] = () => { Registers.D = ExecuteSRL(Registers.D); };
        _opCodes[SRL_E] = () => { Registers.E = ExecuteSRL(Registers.E); };
        _opCodes[SRL_H] = () => { Registers.H = ExecuteSRL(Registers.H); };
        _opCodes[SRL_L] = () => { Registers.L = ExecuteSRL(Registers.L); };
        _opCodes[SRL_HL] = () => ShiftRotateMemory(ExecuteSRL);

        _opCodes[SLL_A] = () => { Registers.A = ExecuteSLL(Registers.A); };
        _opCodes[SLL_B] = () => { Registers.B = ExecuteSLL(Registers.B); };
        _opCodes[SLL_C] = () => { Registers.C = ExecuteSLL(Registers.C); };
        _opCodes[SLL_D] = () => { Registers.D = ExecuteSLL(Registers.D); };
        _opCodes[SLL_E] = () => { Registers.E = ExecuteSLL(Registers.E); };
        _opCodes[SLL_H] = () => { Registers.H = ExecuteSLL(Registers.H); };
        _opCodes[SLL_L] = () => { Registers.L = ExecuteSLL(Registers.L); };
        _opCodes[SLL_HL] = () => ShiftRotateMemory(ExecuteSLL);
    }

    private byte ExecuteRLC(byte value)
    {
        var bit7 = value >> 7;
        var result = (byte)(value << 1 | bit7);

        UpdateFlagsForShiftAndRotate(result, bit7);

        return result;
    }

    private byte ExecuteRL(byte value)
    {
        var bit7 = value >> 7;
        var result = (byte)(value << 1 | (byte)(Registers.F & C));

        UpdateFlagsForShiftAndRotate(result, bit7);

        return result;
    }

    private byte ExecuteRRC(byte value)
    {
        var bit0 = value & 1;
        var result = (byte)(value >> 1 | bit0 << 7);

        UpdateFlagsForShiftAndRotate(result, bit0);

        return result;
    }

    private byte ExecuteRR(byte value)
    {
        var bit0 = value & 1;
        var result = (byte)(value >> 1 | (byte)(Registers.F & C) << 7);

        UpdateFlagsForShiftAndRotate(result, bit0);

        return result;
    }

    private byte ExecuteSLA(byte value)
    {
        var bit7 = value >> 7;
        var result = (byte)(value << 1);

        UpdateFlagsForShiftAndRotate(result, bit7);

        return result;
    }

    private byte ExecuteSRA(byte value)
    {
        var bit0 = value & 1;
        var result = (byte)(value & 0x80 | value >> 1);

        UpdateFlagsForShiftAndRotate(result, bit0);

        return result;
    }

    private byte ExecuteSRL(byte value)
    {
        var bit0 = value & 1;
        var result = (byte)(value >> 1);

        UpdateFlagsForShiftAndRotate(result, bit0);

        return result;
    }

    private byte ExecuteSLL(byte value) => (byte)(ExecuteSLA(value) | 1);

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
        AddStates(1);
        var result = calculate(value);
        WriteByte(address, result);
    }
}