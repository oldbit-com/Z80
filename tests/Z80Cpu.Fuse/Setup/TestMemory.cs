namespace OldBit.Z80Cpu.Fuse.Setup;

public class TestMemory : IMemory
{
    private readonly byte[] _memory = new byte [65536];

    public TestMemory(List<(Word, byte)> memory)
    {
        foreach (var (address, data) in memory)
        {
            _memory[address] = data;
        }
    }

    public byte Read(Word address) => _memory[address];

    public void Write(Word address, byte data) => _memory[address] = data;
}