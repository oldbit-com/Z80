using System.Runtime.InteropServices;

namespace OldBit.Z80Cpu.Registers;

[StructLayout(LayoutKind.Explicit)]
internal sealed class WordRegister(Word value = 0)
{
    [FieldOffset(0)] internal Word Value = value;

    [FieldOffset(0)] internal byte L;

    [FieldOffset(1)] internal byte H;

    // public static implicit operator WordRegister(Word value) => new(value);
    //
    // public static implicit operator Word(WordRegister register) => register.Value;
}