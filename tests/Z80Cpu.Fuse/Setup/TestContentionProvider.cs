using OldBit.Z80Cpu.Contention;

namespace OldBit.Z80Cpu.Fuse.Setup;

public class TestContentionProvider(List<InputOutputEvent> events) : IContentionProvider
{
    internal Clock Clock { get; set; } = null!;

    public int GetMemoryContention(int currentStates, Word address)
    {
        events.Add(new InputOutputEvent(Clock.CurrentTicks, "MC", address, 0));

        return 0;
    }

    public int GetPortContention(int currentStates, Word port)
    {
        events.Add(new InputOutputEvent(Clock.CurrentTicks, "PC", port, 0));

        return 0;
    }
}