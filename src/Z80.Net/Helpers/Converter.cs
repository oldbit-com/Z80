namespace OldBit.Z80.Net.Helpers;

internal static class Converter
{
    internal static Word ToWord(byte high, byte low) =>
        (Word)(high << 8 | low);
}
