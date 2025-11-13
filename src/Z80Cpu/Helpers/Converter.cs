namespace OldBit.Z80Cpu.Helpers;

internal static class Converter
{
    internal static Word ToWord(byte high, byte low) => (Word)(high << 8 | low);
}
