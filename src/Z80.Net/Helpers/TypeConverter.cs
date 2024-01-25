namespace Z80.Net.Helpers;

internal static class TypeConverter
{
    internal static ushort ToWord(byte high, byte low) =>
        (ushort)(high << 8 | low);
}
