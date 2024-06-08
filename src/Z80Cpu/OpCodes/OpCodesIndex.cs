namespace OldBit.Z80Cpu.OpCodes;

internal sealed class OpCodesIndex
{
    private readonly Dictionary<int, Action> _opCodeIndex = new();

    internal Action this[string opCode]
    {
        get => _opCodeIndex[OpCodesMapping.GetCode(opCode)];
        set => _opCodeIndex[OpCodesMapping.GetCode(opCode)] = value;
    }

    internal Action this[int opCode]
    {
        get => _opCodeIndex[opCode];
        set => _opCodeIndex[opCode] = value;
    }

    internal bool Execute(int opCode)
    {
        if (!_opCodeIndex.TryGetValue(opCode, out var executeCode))
        {
            return false;
        }

        executeCode();

        return true;
    }
}