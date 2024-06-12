using OldBit.Z80Cpu.Helpers;
using OldBit.Z80Cpu.Registers;
using static OldBit.Z80Cpu.Registers.Flags;

namespace OldBit.Z80Cpu;

partial class Z80
{
    private enum ShiftRotateOperation
    {
        RLC,
        RL,
        RRC,
        RR,
        SLA,
        SRA,
        SRL,
        SLL
    }

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
            Cycles.Add(4);

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
            Cycles.Add(4);

            var value = ReadByte(Registers.HL);
            var newValue = (byte)((value >> 4) | (Registers.A << 4));

            Registers.A = (byte)((value & 0x0F) | (Registers.A & 0xF0));
            WriteByte(Registers.HL, newValue);

            Registers.F &= C;
            Registers.F |= Registers.A == 0 ? Z : 0;
            Registers.F |= (S | Y | X) & (Flags)Registers.A;
            Registers.F |= Parity.Lookup[Registers.A];
        };

        _opCodes["RLC A"] = () => { Registers.A = Execute_ShiftRotate(ShiftRotateOperation.RLC, Registers.A); };
        _opCodes["RLC B"] = () => { Registers.B = Execute_ShiftRotate(ShiftRotateOperation.RLC, Registers.B); };
        _opCodes["RLC C"] = () => { Registers.C = Execute_ShiftRotate(ShiftRotateOperation.RLC, Registers.C); };
        _opCodes["RLC D"] = () => { Registers.D = Execute_ShiftRotate(ShiftRotateOperation.RLC, Registers.D); };
        _opCodes["RLC E"] = () => { Registers.E = Execute_ShiftRotate(ShiftRotateOperation.RLC, Registers.E); };
        _opCodes["RLC H"] = () => { Registers.H = Execute_ShiftRotate(ShiftRotateOperation.RLC, Registers.H); };
        _opCodes["RLC L"] = () => { Registers.L = Execute_ShiftRotate(ShiftRotateOperation.RLC, Registers.L); };
        _opCodes["RLC (HL)"] = () => Execute_ShiftRotate(ShiftRotateOperation.RLC, useHL: true);

        _opCodes["RL A"] = () => { Registers.A = Execute_ShiftRotate(ShiftRotateOperation.RL, Registers.A); };
        _opCodes["RL B"] = () => { Registers.B = Execute_ShiftRotate(ShiftRotateOperation.RL, Registers.B); };
        _opCodes["RL C"] = () => { Registers.C = Execute_ShiftRotate(ShiftRotateOperation.RL, Registers.C); };
        _opCodes["RL D"] = () => { Registers.D = Execute_ShiftRotate(ShiftRotateOperation.RL, Registers.D); };
        _opCodes["RL E"] = () => { Registers.E = Execute_ShiftRotate(ShiftRotateOperation.RL, Registers.E); };
        _opCodes["RL H"] = () => { Registers.H = Execute_ShiftRotate(ShiftRotateOperation.RL, Registers.H); };
        _opCodes["RL L"] = () => { Registers.L = Execute_ShiftRotate(ShiftRotateOperation.RL, Registers.L); };
        _opCodes["RL (HL)"] = () => Execute_ShiftRotate(ShiftRotateOperation.RL, useHL: true);

        _opCodes["RRC A"] = () => { Registers.A = Execute_ShiftRotate(ShiftRotateOperation.RRC, Registers.A); };
        _opCodes["RRC B"] = () => { Registers.B = Execute_ShiftRotate(ShiftRotateOperation.RRC, Registers.B); };
        _opCodes["RRC C"] = () => { Registers.C = Execute_ShiftRotate(ShiftRotateOperation.RRC, Registers.C); };
        _opCodes["RRC D"] = () => { Registers.D = Execute_ShiftRotate(ShiftRotateOperation.RRC, Registers.D); };
        _opCodes["RRC E"] = () => { Registers.E = Execute_ShiftRotate(ShiftRotateOperation.RRC, Registers.E); };
        _opCodes["RRC H"] = () => { Registers.H = Execute_ShiftRotate(ShiftRotateOperation.RRC, Registers.H); };
        _opCodes["RRC L"] = () => { Registers.L = Execute_ShiftRotate(ShiftRotateOperation.RRC, Registers.L); };
        _opCodes["RRC (HL)"] = () => Execute_ShiftRotate(ShiftRotateOperation.RRC, useHL: true);

        _opCodes["RR A"] = () => { Registers.A = Execute_ShiftRotate(ShiftRotateOperation.RR, Registers.A); };
        _opCodes["RR B"] = () => { Registers.B = Execute_ShiftRotate(ShiftRotateOperation.RR, Registers.B); };
        _opCodes["RR C"] = () => { Registers.C = Execute_ShiftRotate(ShiftRotateOperation.RR, Registers.C); };
        _opCodes["RR D"] = () => { Registers.D = Execute_ShiftRotate(ShiftRotateOperation.RR, Registers.D); };
        _opCodes["RR E"] = () => { Registers.E = Execute_ShiftRotate(ShiftRotateOperation.RR, Registers.E); };
        _opCodes["RR H"] = () => { Registers.H = Execute_ShiftRotate(ShiftRotateOperation.RR, Registers.H); };
        _opCodes["RR L"] = () => { Registers.L = Execute_ShiftRotate(ShiftRotateOperation.RR, Registers.L); };
        _opCodes["RR (HL)"] = () => Execute_ShiftRotate(ShiftRotateOperation.RR, useHL: true);

        _opCodes["SLA A"] = () => { Registers.A = Execute_ShiftRotate(ShiftRotateOperation.SLA, Registers.A); };
        _opCodes["SLA B"] = () => { Registers.B = Execute_ShiftRotate(ShiftRotateOperation.SLA, Registers.B); };
        _opCodes["SLA C"] = () => { Registers.C = Execute_ShiftRotate(ShiftRotateOperation.SLA, Registers.C); };
        _opCodes["SLA D"] = () => { Registers.D = Execute_ShiftRotate(ShiftRotateOperation.SLA, Registers.D); };
        _opCodes["SLA E"] = () => { Registers.E = Execute_ShiftRotate(ShiftRotateOperation.SLA, Registers.E); };
        _opCodes["SLA H"] = () => { Registers.H = Execute_ShiftRotate(ShiftRotateOperation.SLA, Registers.H); };
        _opCodes["SLA L"] = () => { Registers.L = Execute_ShiftRotate(ShiftRotateOperation.SLA, Registers.L); };
        _opCodes["SLA (HL)"] = () => Execute_ShiftRotate(ShiftRotateOperation.SLA, useHL: true);

        _opCodes["SRA A"] = () => { Registers.A = Execute_ShiftRotate(ShiftRotateOperation.SRA, Registers.A); };
        _opCodes["SRA B"] = () => { Registers.B = Execute_ShiftRotate(ShiftRotateOperation.SRA, Registers.B); };
        _opCodes["SRA C"] = () => { Registers.C = Execute_ShiftRotate(ShiftRotateOperation.SRA, Registers.C); };
        _opCodes["SRA D"] = () => { Registers.D = Execute_ShiftRotate(ShiftRotateOperation.SRA, Registers.D); };
        _opCodes["SRA E"] = () => { Registers.E = Execute_ShiftRotate(ShiftRotateOperation.SRA, Registers.E); };
        _opCodes["SRA H"] = () => { Registers.H = Execute_ShiftRotate(ShiftRotateOperation.SRA, Registers.H); };
        _opCodes["SRA L"] = () => { Registers.L = Execute_ShiftRotate(ShiftRotateOperation.SRA, Registers.L); };
        _opCodes["SRA (HL)"] = () => Execute_ShiftRotate(ShiftRotateOperation.SRA, useHL: true);

        _opCodes["SRL A"] = () => { Registers.A = Execute_ShiftRotate(ShiftRotateOperation.SRL, Registers.A); };
        _opCodes["SRL B"] = () => { Registers.B = Execute_ShiftRotate(ShiftRotateOperation.SRL, Registers.B); };
        _opCodes["SRL C"] = () => { Registers.C = Execute_ShiftRotate(ShiftRotateOperation.SRL, Registers.C); };
        _opCodes["SRL D"] = () => { Registers.D = Execute_ShiftRotate(ShiftRotateOperation.SRL, Registers.D); };
        _opCodes["SRL E"] = () => { Registers.E = Execute_ShiftRotate(ShiftRotateOperation.SRL, Registers.E); };
        _opCodes["SRL H"] = () => { Registers.H = Execute_ShiftRotate(ShiftRotateOperation.SRL, Registers.H); };
        _opCodes["SRL L"] = () => { Registers.L = Execute_ShiftRotate(ShiftRotateOperation.SRL, Registers.L); };
        _opCodes["SRL (HL)"] = () => Execute_ShiftRotate(ShiftRotateOperation.SRL, useHL: true);

        _opCodes["SLL A"] = () => { Registers.A = Execute_ShiftRotate(ShiftRotateOperation.SLL, Registers.A); };
        _opCodes["SLL B"] = () => { Registers.B = Execute_ShiftRotate(ShiftRotateOperation.SLL, Registers.B); };
        _opCodes["SLL C"] = () => { Registers.C = Execute_ShiftRotate(ShiftRotateOperation.SLL, Registers.C); };
        _opCodes["SLL D"] = () => { Registers.D = Execute_ShiftRotate(ShiftRotateOperation.SLL, Registers.D); };
        _opCodes["SLL E"] = () => { Registers.E = Execute_ShiftRotate(ShiftRotateOperation.SLL, Registers.E); };
        _opCodes["SLL H"] = () => { Registers.H = Execute_ShiftRotate(ShiftRotateOperation.SLL, Registers.H); };
        _opCodes["SLL L"] = () => { Registers.L = Execute_ShiftRotate(ShiftRotateOperation.SLL, Registers.L); };
        _opCodes["SLL (HL)"] = () => Execute_ShiftRotate(ShiftRotateOperation.SLL, useHL: true);
    }

    private byte Execute_ShiftRotate(ShiftRotateOperation operation, byte value = 0, bool useHL = false)
    {
        Word address = 0;
        var isMemory = Registers.UseIndexRegister || useHL;

        if (isMemory)
        {
            address = (Word)(Registers.XHL + _indexRegisterOffset);
            value = ReadByte(address);

            Cycles.Add(1);
        }

        var result = operation switch
        {
            ShiftRotateOperation.RLC => Execute_RLC(value),
            ShiftRotateOperation.RL => Execute_RL(value),
            ShiftRotateOperation.RRC => Execute_RRC(value),
            ShiftRotateOperation.RR => Execute_RR(value),
            ShiftRotateOperation.SLA => Execute_SLA(value),
            ShiftRotateOperation.SRA => Execute_SRA(value),
            ShiftRotateOperation.SRL => Execute_SRL(value),
            ShiftRotateOperation.SLL => Execute_SLL(value),
            _ => throw new ArgumentOutOfRangeException(nameof(operation), operation, null)
        };

        if (isMemory)
        {
            WriteByte(address, result);
        }

        return result;
    }

    private byte Execute_RLC(byte value = 0)
    {
        var bit7 = value! >> 7;
        var result = (byte)(value! << 1 | bit7);

        UpdateFlagsForShiftAndRotate(result, bit7);

        return result;
    }

    private byte Execute_RL(byte value = 0)
    {
        var bit7 = value >> 7;
        var result = (byte)(value << 1 | (byte)(Registers.F & C));

        UpdateFlagsForShiftAndRotate(result, bit7);

        return result;
    }

    private byte Execute_RRC(byte value = 0)
    {
        var bit0 = value & 1;
        var result = (byte)(value >> 1 | bit0 << 7);

        UpdateFlagsForShiftAndRotate(result, bit0);

        return result;
    }

    private byte Execute_RR(byte value = 0)
    {
        var bit0 = value & 1;
        var result = (byte)(value >> 1 | (byte)(Registers.F & C) << 7);

        UpdateFlagsForShiftAndRotate(result, bit0);

        return result;
    }

    private byte Execute_SLA(byte value = 0)
    {
        var bit7 = value >> 7;
        var result = (byte)(value << 1);

        UpdateFlagsForShiftAndRotate(result, bit7);

        return result;
    }

    private byte Execute_SRA(byte value = 0)
    {
        var bit0 = value & 1;
        var result = (byte)(value & 0x80 | value >> 1);

        UpdateFlagsForShiftAndRotate(result, bit0);

        return result;
    }

    private byte Execute_SRL(byte value = 0)
    {
        var bit0 = value & 1;
        var result = (byte)(value >> 1);

        UpdateFlagsForShiftAndRotate(result, bit0);

        return result;
    }

    private byte Execute_SLL(byte value = 0)
    {
        var bit7 = value >> 7;
        var result = (byte)(value << 1 | 1);

        UpdateFlagsForShiftAndRotate(result, bit7);

        return result;
    }

    private void UpdateFlagsForShiftAndRotate(byte result, int carry)
    {
        Registers.F = (S | Y | X) & (Flags)result;
        Registers.F |= result == 0 ? Z : 0;
        Registers.F |= (Flags)carry & C;
        Registers.F |= Parity.Lookup[result];
    }
}
