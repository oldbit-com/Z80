namespace OldBit.Z80Cpu.OpCodes;

internal sealed class OpCodesIndex
{
    private readonly Dictionary<int, Action> _opCodeIndex = new();
    private readonly Action[] _fastOpCodeIndex = new Action[65536]; // ~30% faster access than Dictionary

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

    internal void BuildFastIndex()
    {
        Array.Fill(_fastOpCodeIndex, () => { });    // Unknown opcodes will be NOPs

        foreach (var (key, action) in _opCodeIndex)
        {
            _fastOpCodeIndex[key] = action;
        }
    }

    internal void Execute(int opCode) => _fastOpCodeIndex[opCode]();
}