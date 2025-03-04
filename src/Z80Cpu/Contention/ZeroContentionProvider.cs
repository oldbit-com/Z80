namespace OldBit.Z80Cpu.Contention;

internal class ZeroContentionProvider : IContentionProvider
{
    public int GetMemoryContention(int ticks, Word address) => 0;

    public int GetPortContention(int ticks, Word port) => 0;

    public bool IsAddressContended(Word address) => false;

    public bool IsPortContended(Word port) => false;
}