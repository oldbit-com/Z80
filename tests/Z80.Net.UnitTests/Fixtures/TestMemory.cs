namespace Z80.Net.UnitTests.Fixtures;

public class TestMemory : IMemory
{
    private readonly byte[] _memory;

    public TestMemory(params byte[] memory)
    {
        _memory = memory;
    }

    public byte Read(int address)
    {
        return _memory[address];
    }

    public void Write(int address, byte value)
    {
        _memory[address] = value;
    }

    public byte this[int address] => _memory[address];
}