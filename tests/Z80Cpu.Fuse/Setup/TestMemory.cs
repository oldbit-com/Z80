namespace OldBit.Z80Cpu.Fuse.Setup;

public class TestMemory : IMemory
{
    private readonly List<InputOutputEvent> _events;
    private readonly byte[] _memory = new byte [65536];

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

        _events.Add(new InputOutputEvent(0, "MC", address, 0));
        _events.Add(new InputOutputEvent(0, "MR", address, value));

        return value;
    }

    public void Write(Word address, byte data)
    {
        _events.Add(new InputOutputEvent(0, "MC", address, 0));
        _events.Add(new InputOutputEvent(0, "MW", address, data));

        _memory[address] = data;
    }
}