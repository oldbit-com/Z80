namespace Z80.Net.Helpers;

internal static class TypeConverter
{
    internal static Word ToWord(byte high, byte low) =>
        (Word)(high << 8 | low);
}
