namespace Z80.Net.ComponentTests.Fixtures;

public class TestMemory : IMemory
{
    private readonly byte[] _memory = new byte [65536];

    public byte Read(Word address) => _memory[address];

    public void Write(Word address, byte data) => _memory[address] = data;
}