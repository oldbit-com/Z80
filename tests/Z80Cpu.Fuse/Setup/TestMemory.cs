namespace OldBit.Z80Cpu.Fuse.Setup;

public class TestMemory : IMemory
{
    private readonly List<InputOutputEvent> _events;
    private readonly byte[] _memory = new byte [65536];

    internal StatesCounter States { get; set; } = null!;

    public TestMemory(List<(Word, byte)> memory, List<InputOutputEvent> events)
    {
        _events = events;
        foreach (var (address, data) in memory)
        {
            _memory[address] = data;
        }
    }

    public byte Read(Word address)
    {
        var value = _memory[address];

        _events.Add(new InputOutputEvent(States.CurrentStates, "MR", address, value));

        return value;
    }

    public void Write(Word address, byte data)
    {
        _events.Add(new InputOutputEvent(States.CurrentStates, "MW", address, data));

        _memory[address] = data;
    }
}