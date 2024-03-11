using Z80.Net.Helpers;
using Z80.Net.Registers;
using static Z80.Net.Registers.Flags;

namespace Z80.Net;

partial class Z80
{
    private void AddInputOutputInstructions()
    {
        _opCodes["IN A,(C)"] = () => Registers.A = ExecuteIN();
        _opCodes["IN B,(C)"] = () => Registers.B = ExecuteIN();
        _opCodes["IN C,(C)"] = () => Registers.C = ExecuteIN();
        _opCodes["IN D,(C)"] = () => Registers.D = ExecuteIN();
        _opCodes["IN E,(C)"] = () => Registers.E = ExecuteIN();
        _opCodes["IN H,(C)"] = () => Registers.H = ExecuteIN();
        _opCodes["IN L,(C)"] = () => Registers.L = ExecuteIN();
        _opCodes["IN F,(C)"] = () => ExecuteIN();
        _opCodes["IN A,(n)"] = () => Registers.A = ReadBus(Registers.A, FetchByte());

        _opCodes["OUT (C),A"] = () => WriteBus(Registers.B, Registers.C, Registers.A);
        _opCodes["OUT (C),B"] = () => WriteBus(Registers.B, Registers.C, Registers.B);
        _opCodes["OUT (C),C"] = () => WriteBus(Registers.B, Registers.C, Registers.C);
        _opCodes["OUT (C),D"] = () => WriteBus(Registers.B, Registers.C, Registers.D);
        _opCodes["OUT (C),E"] = () => WriteBus(Registers.B, Registers.C, Registers.E);
        _opCodes["OUT (C),H"] = () => WriteBus(Registers.B, Registers.C, Registers.H);
        _opCodes["OUT (C),L"] = () => WriteBus(Registers.B, Registers.C, Registers.L);
        _opCodes["OUT (C),F"] = () => throw new NotImplementedException();
        _opCodes["OUT (n),A"] = () => WriteBus(Registers.A, FetchByte(), Registers.A);

        _opCodes["INI"] = () => throw new NotImplementedException();
        _opCodes["OUTI"] = () => throw new NotImplementedException();
        _opCodes["IND"] = () => throw new NotImplementedException();
        _opCodes["OUTD"] = () => throw new NotImplementedException();
        _opCodes["INIR"] = () => throw new NotImplementedException();
        _opCodes["OTIR"] = () => throw new NotImplementedException();
        _opCodes["INDR"] = () => throw new NotImplementedException();
        _opCodes["OTDR"] = () => throw new NotImplementedException();
    }

    private byte ExecuteIN()
    {
        var result = ReadBus(Registers.B, Registers.C);

        Registers.F &= C;
        Registers.F |= S & (Flags)result;
        Registers.F |= Parity.Lookup[result];
        Registers.F |= result == 0 ? Z : 0;

        return result;
    }
}