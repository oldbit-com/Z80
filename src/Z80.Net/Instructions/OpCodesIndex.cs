namespace Z80.Net.Instructions;

public class OpCodesIndex
{
    private readonly Dictionary<int, Action> _opCodeIndex = new();

    public Action this[string opCode]
    {
        get => _opCodeIndex[OpCodesMap.GetCode(opCode)];
        set => _opCodeIndex[OpCodesMap.GetCode(opCode)] = value;
    }

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