using Z80.Net.Helpers;
using Z80.Net.Registers;
using static Z80.Net.Registers.Flags;

namespace Z80.Net;

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
        _opCodes["OUTI"] = () => throw new NotImplementedException();
        _opCodes["IND"] = () => Execute_INI_IND(increment: false);
        _opCodes["OUTD"] = () => throw new NotImplementedException();
        _opCodes["INIR"] = () => Execute_INIR_INDR(increment: true);
        _opCodes["OTIR"] = () => throw new NotImplementedException();
        _opCodes["INDR"] = () => Execute_INIR_INDR(increment: false);
        _opCodes["OTDR"] = () => throw new NotImplementedException();
    }

    private byte Execute_IN()
    {
        var result = ReadBus(Registers.B, Registers.C);

        Registers.F &= C;
        Registers.F |= S & (Flags)result;
        Registers.F |= Parity.Lookup[result];
        Registers.F |= result == 0 ? Z : 0;

        return result;
    }

    private void Execute_INI_IND(bool increment)
    {
        AddStates(1);
        var result = ReadBus(Registers.B, Registers.C);
        WriteByte(Registers.HL, result);

        Registers.B -= 1;
        if (increment)
        {
            Registers.HL += 1;
        }
        else
        {
            Registers.HL -= 1;
        }

        Registers.F = (Registers.F & ~Z) | N;
        Registers.F |= Registers.B == 0 ? Z : 0;
    }

    private void Execute_INIR_INDR(bool increment)
    {
        Execute_INI_IND(increment);

        if (Registers.B == 0)
        {
            return;
        }

        Registers.PC -= 2;
        AddStates(5);
    }
}