namespace Z80.Net.Instructions;

public sealed class OpCodesIndex
{
    private readonly Dictionary<int, Action> _opCodeIndex = new();

    public Action this[int opCode]
    {
        get => _opCodeIndex[opCode];
        set => _opCodeIndex[opCode] = value;
    }

    public bool Execute(int opCode)
    {
        if (!_opCodeIndex.TryGetValue(opCode, out var executeCode))
        {
            return false;
        }

        executeCode();

        return true;
    }
}