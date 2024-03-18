namespace OldBit.Z80.Net;

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
    }
}