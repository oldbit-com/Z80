namespace OldBit.Z80Cpu.Contention;

internal class ZeroContentionProvider : IContentionProvider
{
    public int GetContentionStates(int currentStates, Word address) => 0;
}