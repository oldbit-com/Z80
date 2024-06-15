namespace OldBit.Z80Cpu.Fuse.Setup;

public class TestBus(List<InputOutputEvent> events) : IBus
{
    public byte Read(Word address)
    {
        PreContend(address);

        var value = (byte)(address >> 8);
        events.Add(new InputOutputEvent(0, "PR", address, value));

        PostContend(address);

        return value;
    }

    public void Write(Word address, byte data)
    {
        PreContend(address);

        events.Add(new InputOutputEvent(0, "PW", address, data));

        PostContend(address);
    }

    private void PreContend(Word address)
    {
        if ((address & 0xC000) == 0x4000)
        {
            events.Add(new InputOutputEvent(0, "PC", address, 0));
        }
    }

    private void PostContend(Word address)
    {
        if ((address & 0x0001) != 0)
        {
            if ((address & 0xC000) == 0x4000)
            {
                events.Add(new InputOutputEvent(0, "PC", address, 0));
                events.Add(new InputOutputEvent(0, "PC", address, 0));
                events.Add(new InputOutputEvent(0, "PC", address, 0));
            }
        }
        else
        {
            events.Add(new InputOutputEvent(0, "PC", address, 0));
        }
    }
}