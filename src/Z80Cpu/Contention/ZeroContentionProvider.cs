using System.Runtime.CompilerServices;

namespace OldBit.Z80Cpu.Contention;

internal class ZeroContentionProvider : IContentionProvider
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetMemoryContention(int ticks, Word address) => 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetPortContention(int ticks, Word port) => 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsAddressContended(Word address) => false;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsPortContended(Word port) => false;
}