namespace Z80.Net.ComponentTests.Fixtures;

public class TestMemory : IMemory
{
    private readonly byte[] _memory = new byte [65536];

    public byte Read(int address) => _memory[address];

    public void Write(int address, byte value) => _memory[address] = value;
}