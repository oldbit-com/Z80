namespace OldBit.Z80Cpu.Fuse.Setup;

public class TestBus(List<InputOutputEvent> events) : IBus
{
    public byte Read(Word port)
    {
        var value = (byte)(port >> 8);

        events.Add(new InputOutputEvent(0, "PR", port, value));

        return value;
    }

    public void Write(Word port, byte data)
    {
        events.Add(new InputOutputEvent(0, "PW", port, data));
    }
}