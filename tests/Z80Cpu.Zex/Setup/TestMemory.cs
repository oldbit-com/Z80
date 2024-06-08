namespace OldBit.Z80Cpu.Zex.Setup;

internal class TestMemory : IMemory
{
    private readonly byte[] _memory = new byte [65536];

    public byte Read(Word address) => _memory[address];

    public void Write(Word address, byte data) => _memory[address] = data;

    internal void CopyFrom(byte[] data, Word startAddress)
    {
        Array.Copy(data, 0, _memory, startAddress, data.Length);
    }
}
