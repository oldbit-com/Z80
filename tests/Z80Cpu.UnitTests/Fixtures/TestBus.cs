namespace OldBit.Z80Cpu.UnitTests.Fixtures;

internal class TestBus : IBus
{
    internal List<(Word address, byte Data)> Outputs = new();


    public byte Read(Word address)
    {
        throw new NotImplementedException();
    }

    public void Write(Word address, byte data)
    {
        Outputs.Add((address, data));
    }
}