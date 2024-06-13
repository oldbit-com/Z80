using OldBit.Z80Cpu.Helpers;
using OldBit.Z80Cpu.Registers;
using static OldBit.Z80Cpu.Registers.Flags;

namespace OldBit.Z80Cpu;

partial class Z80
{
    private void AddInputOutputInstructions()
    {
        _opCodes["IN A,(C)"] = () => Registers.A = Execute_IN();
        _opCodes["IN B,(C)"] = () => Registers.B = Execute_IN();
        _opCodes["IN C,(C)"] = () => Registers.C = Execute_IN();
        _opCodes["IN D,(C)"] = () => Registers.D = Execute_IN();
        _opCodes["IN E,(C)"] = () => Registers.E = Execute_IN();
        _opCodes["IN H,(C)"] = () => Registers.H = Execute_IN();
        _opCodes["IN L,(C)"] = () => Registers.L = Execute_IN();
        _opCodes["IN F,(C)"] = () => Execute_IN();
        _opCodes["IN A,(n)"] = () => Registers.A = ReadBus(Registers.A, FetchByte());

        _opCodes["OUT (C),A"] = () => WriteBus(Registers.B, Registers.C, Registers.A);
        _opCodes["OUT (C),B"] = () => WriteBus(Registers.B, Registers.C, Registers.B);
        _opCodes["OUT (C),C"] = () => WriteBus(Registers.B, Registers.C, Registers.C);
        _opCodes["OUT (C),D"] = () => WriteBus(Registers.B, Registers.C, Registers.D);
        _opCodes["OUT (C),E"] = () => WriteBus(Registers.B, Registers.C, Registers.E);
        _opCodes["OUT (C),H"] = () => WriteBus(Registers.B, Registers.C, Registers.H);
        _opCodes["OUT (C),L"] = () => WriteBus(Registers.B, Registers.C, Registers.L);
        _opCodes["OUT (C),F"] = () => WriteBus(Registers.B, Registers.C, 0);
        _opCodes["OUT (n),A"] = () => WriteBus(Registers.A, FetchByte(), Registers.A);

        _opCodes["INI"] = () => Execute_INI_IND(increment: true);
        _opCodes["OUTI"] = () => Execute_OUTI_OUTD(increment: true);
        _opCodes["IND"] = () => Execute_INI_IND(increment: false);
        _opCodes["OUTD"] = () => Execute_OUTI_OUTD(increment: false);
        _opCodes["INIR"] = () =>  ExecuteRepeated_OUT(increment: true, Execute_INI_IND);
        _opCodes["OTIR"] = () => ExecuteRepeated_OUT(increment: true, Execute_OUTI_OUTD);
        _opCodes["INDR"] = () => ExecuteRepeated_OUT(increment: false, Execute_INI_IND);
        _opCodes["OTDR"] = () => ExecuteRepeated_OUT(increment: false, Execute_OUTI_OUTD);
    }

    private byte Execute_IN()
    {
        var result = ReadBus(Registers.B, Registers.C);

        Registers.F &= C;
        Registers.F |= S & (Flags)result;
        Registers.F |= Parity.Lookup[result];
        Registers.F |= result == 0 ? Z : 0;
        Registers.F |= (Flags)result & (Y | X);

        return result;
    }

    private void Execute_INI_IND(bool increment)
    {
        States.Add(1);

        var result = ReadBus(Registers.B, Registers.C);
        WriteByte(Registers.HL, result);

        Registers.B -= 1;
        byte temp = 0;
        if (increment)
        {
            Registers.HL += 1;
            temp = (byte)(Registers.C + result + 1);
        }
        else
        {
            Registers.HL -= 1;
            temp = (byte)(Registers.C + result - 1);
        }

        Registers.F = Registers.B == 0 ? Z : 0;
        Registers.F |= (result & 0x80) != 0 ? N : 0;
        Registers.F |= temp < result ? H | C : 0;
        Registers.F |= Parity.Lookup[(temp & 0x07) ^ Registers.B];
        Registers.F |= (S | Y | X) & (Flags)Registers.B;
    }

    private void Execute_OUTI_OUTD(bool increment)
    {
        States.Add(1);

        var data = ReadByte(Registers.HL);

        Registers.B -= 1;
        WriteBus(Registers.B, Registers.C, data);

        if (increment)
        {
            Registers.HL += 1;
        }
        else
        {
            Registers.HL -= 1;
        }

        var temp = (byte)(data + Registers.L);
        Registers.F = Registers.B == 0 ? Z : 0;
        Registers.F |= (data & 0x80) != 0 ? N : 0;
        Registers.F |= temp < data ? H | C : 0;
        Registers.F |= Parity.Lookup[(temp & 0x07) ^ Registers.B];
        Registers.F |= (S | Y | X) & (Flags)Registers.B;
    }

    private void ExecuteRepeated_OUT(bool increment, Action<bool> outInstruction)
    {
        outInstruction(increment);

        if (Registers.B == 0)
        {
            return;
        }

        Registers.PC -= 2;

        States.Add(5);
    }
}