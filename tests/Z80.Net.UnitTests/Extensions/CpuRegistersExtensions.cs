using Z80.Net.Registers;

namespace Z80.Net.UnitTests.Extensions;

internal static class CpuRegistersExtensions
{
    internal static ushort ValueOf(this CpuRegisters registers, string register)
    {
        return register switch
        {
            "IX" => registers.IX,
            "IY" => registers.IY,
            _ => throw new ArgumentException($"Unknown register: {register}")
        };
    }
}