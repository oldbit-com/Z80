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
        // Unknown opcodes will be NOPs
        Array.Fill(_fastOpCodeIndex, this["NOP"]);

        foreach (var (opCode, action) in _opCodeIndex)
        {
            _fastOpCodeIndex[opCode] = action;
        }
    }

    internal void Execute(int opCode) => _fastOpCodeIndex[opCode]();
}