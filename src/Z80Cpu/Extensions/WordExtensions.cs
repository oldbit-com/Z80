using OldBit.Z80Cpu.Registers;

namespace OldBit.Z80Cpu.Extensions;

public static class WordExtensions
{
    public static void Deconstruct(this Word value, out byte highByte, out byte lowByte)
    {
        highByte = (byte)(value >> 8);
        lowByte = (byte)(value & 0xFF);
    }
}