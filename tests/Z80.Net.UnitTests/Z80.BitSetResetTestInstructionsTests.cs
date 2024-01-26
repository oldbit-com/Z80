using FluentAssertions;
using Z80.Net.Registers;
using Z80.Net.UnitTests.Fixtures;
using static Z80.Net.Registers.Flags;

namespace Z80.Net.UnitTests;

public class Z80BitSetResetTestInstructionsTests
{
    [Theory]
    [InlineData(6, 0x40, Z | N | C, H | C)]
    [InlineData(0, 0xFE, None, Z | H | P)]
    [InlineData(5, 0x20, All, Y | H | C)]
    public void When_BIT_n_A_InstructionIsExecuted_FlagsAreSet(
        int bit, byte value, Flags flags, Flags expectedFlags)
    {
        var z80 = new CodeBuilder()
            .Flags(flags)
            .Code(
                $"LD A,{value}",
                $"BIT {bit},A")
            .Build();

        z80.Run(7 + 8);

        z80.Registers.F.Should().Be(expectedFlags);
        z80.CycleCounter.TotalCycles.Should().Be(15);
    }

    [Theory]
    [MemberData(nameof(GetBitRegisterData))]
    public void When_BIT_n_R_InstructionIsExecuted_FlagsSet(string register, int bit)
    {
        var expectedFlags = H | C;
        expectedFlags |= bit == 7 ? S : 0;
        expectedFlags |= bit == 3 ? X : 0;
        expectedFlags |= bit == 5 ? Y : 0;

        var z80 = new CodeBuilder()
            .Flags(All)
            .Code(
                $"LD {register},0xFF",
                $"BIT {bit},{register}")
            .Build();

        z80.Run(7 + 8);

        z80.Registers.F.Should().Be(expectedFlags);
        z80.CycleCounter.TotalCycles.Should().Be(15);
    }

    public static IEnumerable<object[]> GetBitRegisterData()
    {
        var bits = Enumerable.Range(0,8);
        var registers = new[] { "A", "B", "C", "D", "E", "H", "L" };
        foreach (var bit in bits)
        {
            foreach (var register in registers)
            {
                yield return [register, bit];
            }
        }
    }
}