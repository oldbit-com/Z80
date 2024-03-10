namespace Z80.Net.UnitTests.Fixtures;

internal class TestMemory : IMemory
{
    private readonly byte[] _memory;

    internal TestMemory(params byte[] memory)
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

    internal byte this[int address] => _memory[address];
}