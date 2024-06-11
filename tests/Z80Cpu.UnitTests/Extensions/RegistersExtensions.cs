using OldBit.Z80Cpu.Registers;

namespace OldBit.Z80Cpu.UnitTests.Extensions;

internal static class RegistersExtensions
{
    internal static Word ValueOf(this RegisterSet registers, string registerName)
    {
        return registerName switch
        {
            "A" => registers.A,
            "B" => registers.B,
            "C" => registers.C,
            "D" => registers.D,
            "E" => registers.E,
            "H" => registers.H,
            "L" => registers.L,
            "IX" => registers.IX,
            "IY" => registers.IY,
            _ => throw new ArgumentException($"Unknown register: {registerName}")
        };
    }
}