namespace OldBit.Z80Cpu.UnitTests.Fixtures;

internal class TestBus : IBus
{
    internal List<(Word address, byte Data)> Outputs = new();


    public byte Read(Word port)
    {
        throw new NotImplementedException();
    }

    public void Write(Word port, byte data)
    {
        Outputs.Add((port, data));
    }
}