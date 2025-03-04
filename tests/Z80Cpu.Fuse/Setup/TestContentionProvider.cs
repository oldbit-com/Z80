using OldBit.Z80Cpu.Contention;

namespace OldBit.Z80Cpu.Fuse.Setup;

public class TestContentionProvider(List<InputOutputEvent> events) : IContentionProvider
{
    internal Clock Clock { get; set; } = null!;

    public int GetMemoryContention(int ticks, Word address)
    {
        events.Add(new InputOutputEvent(Clock.CurrentFrameTicks, "MC", address, 0));

        return 0;
    }

    public int GetPortContention(int ticks, Word port)
    {
        events.Add(new InputOutputEvent(Clock.CurrentFrameTicks, "PC", port, 0));

        return 0;
    }

    public bool IsAddressContended(Word address) => true;

    public bool IsPortContended(Word port) => (port & 0xC000) == 0x4000;
}