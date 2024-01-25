namespace Z80.Net.Extensions;

public static class WordExtensions
{
    public static void Deconstruct(this ushort value, out byte highByte, out byte lowByte)
    {
        highByte = (byte)(value >> 8);
        lowByte = (byte)(value & 0xFF);
    }
}