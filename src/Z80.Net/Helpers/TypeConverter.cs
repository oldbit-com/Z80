namespace Z80.Net.Helpers;

internal static class TypeConverter
{
    internal static ushort ToWord(byte high, byte low) =>
        (ushort)(high << 8 | low);

    internal static (byte High, byte Low) ToBytes(ushort value) =>
        (High(value), Low(value));

    internal static byte High(ushort value) => (byte)(value >> 8);

    internal static byte Low(ushort value) => (byte)(value & 0xFF);
}
