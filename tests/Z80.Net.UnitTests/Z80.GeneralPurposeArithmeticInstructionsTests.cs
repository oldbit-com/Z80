using OldBit.Z80.Net.Registers;

namespace Z80.Net.UnitTests;

public class Z80GeneralPurposeArithmeticInstructionsTests
{
    [Theory]
    [InlineData(0x9A, None, 0, Z | H | P | C)]
    [InlineData(0x99, None, 0x99, S | X | P)]
    [InlineData(0x8F, None, 0x95, S | H | P)]
    [InlineData(0x8F, N, 0x89, S | X | N)]
    [InlineData(0xCA, N, 0x64, Y | N | C)]
    [InlineData(0xC5, H | N, 0x5F, H | X | P | N | C)]
    [InlineData(0xCA, All, 0x64, Y | N | C)]
    public void When_DAA_InstructionIsExecuted_AccumulatorAndFlagsAreUpdated(
        byte a, Flags flags, byte expectedResult, Flags expectedFlags)
    {
        var z80 = new Z80Builder()
            .Flags(flags)
            .Code(
                $"LD A,{a}",
                "DAA")
            .Build();

        z80.Run(7 + 4);

        z80.Registers.A.Should().Be(expectedResult);
        z80.Registers.F.Should().Be(expectedFlags);
        z80.States.TotalStates.Should().Be(11);
    }

    [Fact]
    public void When_CPL_InstructionIsExecuted_AccumulatorValueIsInvertedAndFlagsAreUpdated()
    {
        var z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD A,0x5B",
                "CPL")
            .Build();

        z80.Run(7 + 4);

        z80.Registers.A.Should().Be(0xA4);
        z80.Registers.F.Should().Be(H | Y | N);
        z80.States.TotalStates.Should().Be(11);
    }

    [Theory]
    [InlineData(0x55, 0xAB, S | Y | H | X | N | C)]
    [InlineData(0, 0, Z | N)]
    [InlineData(0x80, 0x80, S | P | N | C)]
    [InlineData(0xAA, 0x56, H | N | C)]
    public void When_NEG_InstructionIsExecuted_AccumulatorValueIsNegatedAndFlagsAreUpdated(
        byte a, byte expectedResult, Flags expectedFlags)
    {
        var z80 = new Z80Builder()
            .Flags(None)
            .Code(
                $"LD A,{a}",
                "NEG")
            .Build();

        z80.Run(7 + 8);

        z80.Registers.A.Should().Be(expectedResult);
        z80.Registers.F.Should().Be(expectedFlags);
        z80.States.TotalStates.Should().Be(15);
    }

    [Fact]
    public void When_SCF_InstructionIsExecuted_CarryFlagIsSet()
    {
        var z80 = new Z80Builder()
            .Flags(S | Z | H | P | N)
            .Code("SCF")
            .Build();

        z80.Run(4);

        z80.Registers.F.Should().Be(S | Z | Y | X | P | C);
        z80.States.TotalStates.Should().Be(4);
    }

    [Theory]
    [InlineData(All, S | Z | Y | H | X | P)]
    [InlineData(Z | N, Z | Y | X | C)]
    [InlineData(Z | N | C, Z | Y | H | X)]
    public void When_CCF_InstructionIsExecuted_CarryFlagIsInverted(Flags flags, Flags expectedFlags)
    {
        var z80 = new Z80Builder()
            .Flags(flags)
            .Code("CCF")
            .Build();

        z80.Run(4);

        z80.Registers.F.Should().Be(expectedFlags);
        z80.States.TotalStates.Should().Be(4);
    }
}