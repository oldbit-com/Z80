namespace OldBit.Z80Cpu.Contention;

internal class ZeroContentionProvider : IContentionProvider
{
    public int GetMemoryContention(int currentStates, Word address) => 0;

    public int GetPortContention(int currentStates, Word port) => 0;
}