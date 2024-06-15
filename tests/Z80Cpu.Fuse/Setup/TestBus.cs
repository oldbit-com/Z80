namespace OldBit.Z80Cpu.Fuse.Setup;

public class TestBus(List<InputOutputEvent> events) : IBus
{
    public byte Read(Word port)
    {
        PreContend(port);

        var value = (byte)(port >> 8);
        events.Add(new InputOutputEvent(0, "PR", port, value));

        PostContend(port);

        return value;
    }

    public void Write(Word port, byte data)
    {
        PreContend(port);

        events.Add(new InputOutputEvent(0, "PW", port, data));

        PostContend(port);
    }

    private void PreContend(Word port)
    {
        if ((port & 0xC000) == 0x4000)
        {
            events.Add(new InputOutputEvent(0, "PC", port, 0));
        }
    }

    private void PostContend(Word port)
    {
        if ((port & 0x0001) != 0)
        {
            if ((port & 0xC000) == 0x4000)
            {
                events.Add(new InputOutputEvent(0, "PC", port, 0));
                events.Add(new InputOutputEvent(0, "PC", port, 0));
                events.Add(new InputOutputEvent(0, "PC", port, 0));
            }
        }
        else
        {
            events.Add(new InputOutputEvent(0, "PC", port, 0));
        }
    }
}