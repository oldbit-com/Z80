namespace Z80.Net.UnitTests.Support;

internal static class To
{
    internal static Word Word(byte hi, byte lo) => (Word)((hi << 8) | lo);
}