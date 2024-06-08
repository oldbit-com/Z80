namespace OldBit.Z80Cpu.UnitTests.Support;

internal static class To
{
    internal static Word Word(byte hi, byte lo) => (Word)((hi << 8) | lo);
}