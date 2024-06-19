namespace OldBit.Z80Cpu.Fuse.Setup;

public class TestBus(List<InputOutputEvent> events, Clock clock) : IBus
{
    public byte Read(Word port)
    {
        var value = (byte)(port >> 8);
        events.Add(new InputOutputEvent(clock.CurrentStates, "PR", port, value));
        return value;
    }

    public void Write(Word port, byte data)
    {
        events.Add(new InputOutputEvent(clock.CurrentStates, "PW", port, data));
    }
}