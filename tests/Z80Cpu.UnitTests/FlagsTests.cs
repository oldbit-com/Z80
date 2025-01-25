using OldBit.Z80Cpu.Extensions;
using OldBit.Z80Cpu.Registers;

namespace OldBit.Z80Cpu.UnitTests;

public class FlagsTests
{
    [Theory]
    [InlineData(All, C, true)]
    [InlineData(All, X, true)]
    [InlineData(All, All, true)]
    [InlineData(None, None, true)]
    [InlineData(C, C, true)]
    [InlineData(None, C, false)]
    [InlineData(Z, C, false)]
    [InlineData(N | H, N, true)]
    [InlineData(N | H, H, true)]
    [InlineData(N | H, N | H, true)]
    public void ShouldCorrectlyCheckIfFlagIsSet(Flags flags, Flags flag, bool expected)
    {
        var result = flags.IsSet(flag);

        result.ShouldBe(expected);
    }
}
