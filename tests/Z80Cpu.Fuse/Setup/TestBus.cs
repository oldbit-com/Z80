namespace OldBit.Z80Cpu.Fuse.Setup;

public class TestBus(List<InputOutputEvent> events) : IBus
{
    public byte Read(Word address)
    {
        var value = (byte)(address >> 8);

        events.Add(new InputOutputEvent(0, "PR", address, value));

        return value;
    }

    public void Write(Word address, byte data)
    {
        events.Add(new InputOutputEvent(0, "PW", address, data));
    }
}