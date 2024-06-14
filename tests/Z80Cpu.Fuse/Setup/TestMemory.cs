namespace OldBit.Z80Cpu.Fuse.Setup;

public class TestMemory : IMemory
{
    private readonly byte[] _memory = new byte [65536];
    internal List<(string Event, Word Address, byte Value)> ReadEvents { get; } = [];

    public TestMemory(List<(Word, byte)> memory)
    {
        foreach (var (address, data) in memory)
        {
            _memory[address] = data;
        }
    }

    public byte Read(Word address)
    {
        var value = _memory[address];

        //ReadEvents.Add(("MC", address, 0));
        ReadEvents.Add(("MR", address, value));

        return value;
    }

    public void Write(Word address, byte data) => _memory[address] = data;
}