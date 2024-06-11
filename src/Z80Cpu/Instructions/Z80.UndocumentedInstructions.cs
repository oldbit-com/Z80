namespace OldBit.Z80Cpu;

partial class Z80
{
    private void AddUndocumentedInstructions()
    {
        _opCodes[0xED54] = _opCodes["NEG"];
        _opCodes[0xED64] = _opCodes["NEG"];
        _opCodes[0xED74] = _opCodes["NEG"];
        _opCodes[0xED4C] = _opCodes["NEG"];
        _opCodes[0xED5C] = _opCodes["NEG"];
        _opCodes[0xED6C] = _opCodes["NEG"];
        _opCodes[0xED7C] = _opCodes["NEG"];

        _opCodes[0xED55] = _opCodes["RETN"];
        _opCodes[0xED5D] = _opCodes["RETN"];
        _opCodes[0xED65] = _opCodes["RETN"];
        _opCodes[0xED6D] = _opCodes["RETN"];
        _opCodes[0xED75] = _opCodes["RETN"];
        _opCodes[0xED7D] = _opCodes["RETN"];

        _opCodes[0xED4E] = _opCodes["IM 0"];
        _opCodes[0xED66] = _opCodes["IM 0"];
        _opCodes[0xED76] = _opCodes["IM 1"];
        _opCodes[0xED7E] = _opCodes["IM 2"];
    }
}