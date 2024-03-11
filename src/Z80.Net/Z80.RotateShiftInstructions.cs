using Z80.Net.Helpers;
using Z80.Net.Registers;
using static Z80.Net.Registers.Flags;

namespace Z80.Net;

partial class Z80
{
    private void AddRotateShiftInstructions()
    {
        _opCodes["RLCA"] = () =>
        {
            var bit7 = Registers.A >> 7;
            Registers.A = (byte)(Registers.A << 1 | bit7);
            Registers.F &= S | Z | P;
            Registers.F |= (Flags)bit7;
            Registers.F |= (Y | X) & (Flags)Registers.A;
        };

        _opCodes["RRCA"] = () =>
        {
            var bit0 = Registers.A & 0x01;
            Registers.A = (byte)(Registers.A >> 1 | bit0 << 7);
            Registers.F &= S | Z | P;
            Registers.F |= (Flags)bit0;
            Registers.F |= (Y | X) & (Flags)Registers.A;
        };

        _opCodes["RLA"] = () =>
        {
            var bit7 = Registers.A >> 7;
            Registers.A = (byte)(Registers.A << 1 | (byte)(Registers.F & C));
            Registers.F &= S | Z | P;
            Registers.F |= (Flags)bit7;
            Registers.F |= (Y | X) & (Flags)Registers.A;
        };

        _opCodes["RRA"] = () =>
        {
            var bit0 = Registers.A & 0x01;
            Registers.A = (byte)(Registers.A >> 1 | (byte)(Registers.F & C) << 7);
            Registers.F &= S | Z | P;
            Registers.F |= (Flags)bit0;
            Registers.F |= (Y | X) & (Flags)Registers.A;
        };

        _opCodes["RLD"] = () =>
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

        _opCodes["RRD"] = () =>
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

        _opCodes["RLC A"] = () => { Registers.A = Execute_RLC(Registers.A); };
        _opCodes["RLC B"] = () => { Registers.B = Execute_RLC(Registers.B); };
        _opCodes["RLC C"] = () => { Registers.C = Execute_RLC(Registers.C); };
        _opCodes["RLC D"] = () => { Registers.D = Execute_RLC(Registers.D); };
        _opCodes["RLC E"] = () => { Registers.E = Execute_RLC(Registers.E); };
        _opCodes["RLC H"] = () => { Registers.H = Execute_RLC(Registers.H); };
        _opCodes["RLC L"] = () => { Registers.L = Execute_RLC(Registers.L); };
        _opCodes["RLC (HL)"] = () => ShiftRotateMemory(Execute_RLC);

        _opCodes["RL A"] = () => { Registers.A = Execute_RL(Registers.A); };
        _opCodes["RL B"] = () => { Registers.B = Execute_RL(Registers.B); };
        _opCodes["RL C"] = () => { Registers.C = Execute_RL(Registers.C); };
        _opCodes["RL D"] = () => { Registers.D = Execute_RL(Registers.D); };
        _opCodes["RL E"] = () => { Registers.E = Execute_RL(Registers.E); };
        _opCodes["RL H"] = () => { Registers.H = Execute_RL(Registers.H); };
        _opCodes["RL L"] = () => { Registers.L = Execute_RL(Registers.L); };
        _opCodes["RL (HL)"] = () => ShiftRotateMemory(Execute_RL);

        _opCodes["RRC A"] = () => { Registers.A = Execute_RRC(Registers.A); };
        _opCodes["RRC B"] = () => { Registers.B = Execute_RRC(Registers.B); };
        _opCodes["RRC C"] = () => { Registers.C = Execute_RRC(Registers.C); };
        _opCodes["RRC D"] = () => { Registers.D = Execute_RRC(Registers.D); };
        _opCodes["RRC E"] = () => { Registers.E = Execute_RRC(Registers.E); };
        _opCodes["RRC H"] = () => { Registers.H = Execute_RRC(Registers.H); };
        _opCodes["RRC L"] = () => { Registers.L = Execute_RRC(Registers.L); };
        _opCodes["RRC (HL)"] = () => ShiftRotateMemory(Execute_RRC);

        _opCodes["RR A"] = () => { Registers.A = Execute_RR(Registers.A); };
        _opCodes["RR B"] = () => { Registers.B = Execute_RR(Registers.B); };
        _opCodes["RR C"] = () => { Registers.C = Execute_RR(Registers.C); };
        _opCodes["RR D"] = () => { Registers.D = Execute_RR(Registers.D); };
        _opCodes["RR E"] = () => { Registers.E = Execute_RR(Registers.E); };
        _opCodes["RR H"] = () => { Registers.H = Execute_RR(Registers.H); };
        _opCodes["RR L"] = () => { Registers.L = Execute_RR(Registers.L); };
        _opCodes["RR (HL)"] = () => ShiftRotateMemory(Execute_RR);

        _opCodes["SLA A"] = () => { Registers.A = Execute_SLA(Registers.A); };
        _opCodes["SLA B"] = () => { Registers.B = Execute_SLA(Registers.B); };
        _opCodes["SLA C"] = () => { Registers.C = Execute_SLA(Registers.C); };
        _opCodes["SLA D"] = () => { Registers.D = Execute_SLA(Registers.D); };
        _opCodes["SLA E"] = () => { Registers.E = Execute_SLA(Registers.E); };
        _opCodes["SLA H"] = () => { Registers.H = Execute_SLA(Registers.H); };
        _opCodes["SLA L"] = () => { Registers.L = Execute_SLA(Registers.L); };
        _opCodes["SLA (HL)"] = () => ShiftRotateMemory(Execute_SLA);

        _opCodes["SRA A"] = () => { Registers.A = Execute_SRA(Registers.A); };
        _opCodes["SRA B"] = () => { Registers.B = Execute_SRA(Registers.B); };
        _opCodes["SRA C"] = () => { Registers.C = Execute_SRA(Registers.C); };
        _opCodes["SRA D"] = () => { Registers.D = Execute_SRA(Registers.D); };
        _opCodes["SRA E"] = () => { Registers.E = Execute_SRA(Registers.E); };
        _opCodes["SRA H"] = () => { Registers.H = Execute_SRA(Registers.H); };
        _opCodes["SRA L"] = () => { Registers.L = Execute_SRA(Registers.L); };
        _opCodes["SRA (HL)"] = () => ShiftRotateMemory(Execute_SRA);

        _opCodes["SRL A"] = () => { Registers.A = Execute_SRL(Registers.A); };
        _opCodes["SRL B"] = () => { Registers.B = Execute_SRL(Registers.B); };
        _opCodes["SRL C"] = () => { Registers.C = Execute_SRL(Registers.C); };
        _opCodes["SRL D"] = () => { Registers.D = Execute_SRL(Registers.D); };
        _opCodes["SRL E"] = () => { Registers.E = Execute_SRL(Registers.E); };
        _opCodes["SRL H"] = () => { Registers.H = Execute_SRL(Registers.H); };
        _opCodes["SRL L"] = () => { Registers.L = Execute_SRL(Registers.L); };
        _opCodes["SRL (HL)"] = () => ShiftRotateMemory(Execute_SRL);

        _opCodes["SLL A"] = () => { Registers.A = Execute_SLL(Registers.A); };
        _opCodes["SLL B"] = () => { Registers.B = Execute_SLL(Registers.B); };
        _opCodes["SLL C"] = () => { Registers.C = Execute_SLL(Registers.C); };
        _opCodes["SLL D"] = () => { Registers.D = Execute_SLL(Registers.D); };
        _opCodes["SLL E"] = () => { Registers.E = Execute_SLL(Registers.E); };
        _opCodes["SLL H"] = () => { Registers.H = Execute_SLL(Registers.H); };
        _opCodes["SLL L"] = () => { Registers.L = Execute_SLL(Registers.L); };
        _opCodes["SLL (HL)"] = () => ShiftRotateMemory(Execute_SLL);
    }

    private byte Execute_RLC(byte value)
    {
        var bit7 = value >> 7;
        var result = (byte)(value << 1 | bit7);

        UpdateFlagsForShiftAndRotate(result, bit7);

        return result;
    }

    private byte Execute_RL(byte value)
    {
        var bit7 = value >> 7;
        var result = (byte)(value << 1 | (byte)(Registers.F & C));

        UpdateFlagsForShiftAndRotate(result, bit7);

        return result;
    }

    private byte Execute_RRC(byte value)
    {
        var bit0 = value & 1;
        var result = (byte)(value >> 1 | bit0 << 7);

        UpdateFlagsForShiftAndRotate(result, bit0);

        return result;
    }

    private byte Execute_RR(byte value)
    {
        var bit0 = value & 1;
        var result = (byte)(value >> 1 | (byte)(Registers.F & C) << 7);

        UpdateFlagsForShiftAndRotate(result, bit0);

        return result;
    }

    private byte Execute_SLA(byte value)
    {
        var bit7 = value >> 7;
        var result = (byte)(value << 1);

        UpdateFlagsForShiftAndRotate(result, bit7);

        return result;
    }

    private byte Execute_SRA(byte value)
    {
        var bit0 = value & 1;
        var result = (byte)(value & 0x80 | value >> 1);

        UpdateFlagsForShiftAndRotate(result, bit0);

        return result;
    }

    private byte Execute_SRL(byte value)
    {
        var bit0 = value & 1;
        var result = (byte)(value >> 1);

        UpdateFlagsForShiftAndRotate(result, bit0);

        return result;
    }

    private byte Execute_SLL(byte value) => (byte)(Execute_SLA(value) | 1);

    private void UpdateFlagsForShiftAndRotate(byte result, int carry)
    {
        Registers.F = (S | Y | X) & (Flags)result;
        Registers.F |= result == 0 ? Z : 0;
        Registers.F |= (Flags)carry & C;
        Registers.F |= Parity.Lookup[result];
    }

    private void ShiftRotateMemory(Func<byte, byte> calculate)
    {
        var address = (Word)(Registers.XHL + _indexRegisterOffset);
        var value = ReadByte(address);
        AddStates(1);
        var result = calculate(value);
        WriteByte(address, result);
    }
}