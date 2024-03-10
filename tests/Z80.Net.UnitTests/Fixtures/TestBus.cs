namespace Z80.Net.UnitTests.Fixtures;

internal class TestBus : IBus
{
    internal List<(byte Top, byte Bottom, byte Data)> Outputs = new();


    public byte Read(byte top, byte bottom)
    {
        throw new NotImplementedException();
    }

    public void Write(byte top, byte bottom, byte data)
    {
        Outputs.Add((top, bottom, data));
    }
}