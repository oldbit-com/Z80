namespace OldBit.Z80Cpu.Contention;

internal class ZeroContentionProvider : IContentionProvider
{
    public int GetMemoryContention(int ticks, Word address) => 0;

    public int GetPortContention(int ticks, Word port) => 0;
}