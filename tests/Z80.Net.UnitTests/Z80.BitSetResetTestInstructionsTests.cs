using Z80.Net.Registers;

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
        z80.States.TotalStates.Should().Be(15);
    }

    [Theory]
    [MemberData(nameof(EightBitRegistersTestData))]
    public void When_BIT_n_R_InstructionIsExecuted_FlagsSet(string register, int bit, Flags expectedFlags)
    {
        var z80 = new CodeBuilder()
            .Flags(All)
            .Code(
                $"LD {register},0xFF",
                $"BIT {bit},{register}")
            .Build();

        z80.Run(7 + 8);

        z80.Registers.F.Should().Be(expectedFlags);
        z80.States.TotalStates.Should().Be(15);
    }

    [Theory]
    [InlineData(0, H)]
    [InlineData(1, H | Z | P)]
    [InlineData(2, H)]
    [InlineData(3, H)]
    [InlineData(4, H)]
    [InlineData(5, H)]
    [InlineData(6, H)]
    [InlineData(7, H | S)]
    public void When_BIT_n_HL_InstructionIsExecuted_FlagsSet(int bit, Flags expectedFlags)
    {
        var z80 = new CodeBuilder()
            .Flags(Z | N)
            .Code(
                "LD HL,0x06",
                $"BIT {bit},(HL)",
                "NOP",
                "db 0xFD")
            .Build();

        z80.Run(10 + 12);

        z80.Registers.F.Should().Be(expectedFlags);
        z80.States.TotalStates.Should().Be(22);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_BIT_n_IXY_InstructionIsExecuted_FlagsSet(string register)
    {
        var z80 = new CodeBuilder()
            .Flags(Z | N)
            .Code(
                $"LD {register},0x07",
                $"BIT 2,({register}+2)",
                "NOP",
                "db 0xFD")
            .Build();

        z80.Run(14 + 20);

        z80.Registers.F.Should().Be(H);
        z80.States.TotalStates.Should().Be(34);
    }

    public static IEnumerable<object[]> EightBitRegistersTestData()
    {
        var bits = Enumerable.Range(0, 8);
        var registers = new[] { "A", "B", "C", "D", "E", "H", "L" };
        foreach (var bit in bits)
        {
            foreach (var register in registers)
            {
                var flags = H | C;
                flags |= bit == 7 ? S : 0;
                flags |= bit == 3 ? X : 0;
                flags |= bit == 5 ? Y : 0;

                yield return [register, bit, flags];
            }
        }
    }
}