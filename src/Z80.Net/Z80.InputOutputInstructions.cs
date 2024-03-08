namespace Z80.Net;

partial class Z80
{
    private void AddInputOutputInstructions()
    {
        _opCodes["OUT (n),A"] = () => throw new NotImplementedException();

        _opCodes["IN A,(n)"] = () => throw new NotImplementedException();

        _opCodes["IN A,(C)"] = () => throw new NotImplementedException();
        _opCodes["IN B,(C)"] = () => throw new NotImplementedException();
        _opCodes["IN C,(C)"] = () => throw new NotImplementedException();
        _opCodes["IN D,(C)"] = () => throw new NotImplementedException();
        _opCodes["IN E,(C)"] = () => throw new NotImplementedException();
        _opCodes["IN H,(C)"] = () => throw new NotImplementedException();
        _opCodes["IN L,(C)"] = () => throw new NotImplementedException();
        _opCodes["IN F,(C)"] = () => throw new NotImplementedException();

        _opCodes["OUT (C),A"] = () => throw new NotImplementedException();
        _opCodes["OUT (C),B"] = () => throw new NotImplementedException();
        _opCodes["OUT (C),C"] = () => throw new NotImplementedException();
        _opCodes["OUT (C),D"] = () => throw new NotImplementedException();
        _opCodes["OUT (C),E"] = () => throw new NotImplementedException();
        _opCodes["OUT (C),H"] = () => throw new NotImplementedException();
        _opCodes["OUT (C),L"] = () => throw new NotImplementedException();
        _opCodes["OUT (C),F"] = () => throw new NotImplementedException();

        _opCodes["INI"] = () => throw new NotImplementedException();
        _opCodes["OUTI"] = () => throw new NotImplementedException();
        _opCodes["IND"] = () => throw new NotImplementedException();
        _opCodes["OUTD"] = () => throw new NotImplementedException();
        _opCodes["INIR"] = () => throw new NotImplementedException();
        _opCodes["OTIR"] = () => throw new NotImplementedException();
        _opCodes["INDR"] = () => throw new NotImplementedException();
        _opCodes["OTDR"] = () => throw new NotImplementedException();
    }
}