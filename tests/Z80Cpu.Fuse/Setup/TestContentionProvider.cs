using OldBit.Z80Cpu.Contention;

namespace OldBit.Z80Cpu.Fuse.Setup;

public class TestContentionProvider(List<InputOutputEvent> events) : IContentionProvider
{
    public int GetContentionStates(int currentStates, Word address)
    {
        events.Add(new InputOutputEvent(0, "MC", address, 0));

        return 0;
    }
}