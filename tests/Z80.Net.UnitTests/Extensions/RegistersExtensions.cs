using OldBit.Z80.Net.Registers;

namespace Z80.Net.UnitTests.Extensions;

internal static class RegistersExtensions
{
    internal static Word ValueOf(this Registers registers, string register)
    {
        return register switch
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
            _ => throw new ArgumentException($"Unknown register: {register}")
        };
    }
}