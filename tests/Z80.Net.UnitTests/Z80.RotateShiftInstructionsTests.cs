using FluentAssertions;
using Z80.Net.Registers;
using Z80.Net.UnitTests.Fixtures;
using static Z80.Net.Registers.Flags;

namespace Z80.Net.UnitTests;

public class Z80RotateShiftInstructionsTests
{
    [Theory]
    [InlineData(0x55, H | N, 0xAA, Y | X)]
    [InlineData(0xAA, H | N, 0x55, C)]
    [InlineData(0x00, H | N, 0x00, None)]
    [InlineData(0xFF, H | N, 0xFF, Y | X | C)]
    public void When_RLCA_InstructionIsExecuted_AccumulatorAndFlagsAreUpdated(
        byte value, Flags flags, byte expectedValue, Flags expectedFlags)
    {
        var z80 = new CodeBuilder()
            .Flags(flags)
            .Code(
                $"LD A,{value}",
                "RLCA")
            .Build();

        z80.Run(7 + 4);

        z80.Registers.A.Should().Be(expectedValue);
        z80.Registers.F.Should().Be(expectedFlags);
        z80.CycleCounter.TotalCycles.Should().Be(11);
    }

    [Theory]
    [InlineData(0x55, H | N, 0xAA, Y | X | C)]
    [InlineData(0xAA, H | N, 0x55, None)]
    [InlineData(0x00, H | N, 0x00, None)]
    [InlineData(0xFF, H | N, 0xFF, Y | X | C)]
    public void When_RRCA_InstructionIsExecuted_AccumulatorAndFlagsAreUpdated(
        byte value, Flags flags, byte expectedValue, Flags expectedFlags)
    {
        var z80 = new CodeBuilder()
            .Flags(flags)
            .Code(
                $"LD A,{value}",
                "RRCA")
            .Build();

        z80.Run(7 + 4);

        z80.Registers.A.Should().Be(expectedValue);
        z80.Registers.F.Should().Be(expectedFlags);
        z80.CycleCounter.TotalCycles.Should().Be(11);
    }

    [Theory]
    [InlineData(0x80, H | N | C, 0x01, C)]
    [InlineData(0x55, All, 0xAB, S | Z | Y | X | P)]
    [InlineData(0x88, None, 0x10, C)]
    [InlineData(0x10, C, 0x21, Y)]
    public void When_RLA_InstructionIsExecuted_AccumulatorAndFlagsAreUpdated(
        byte value, Flags flags, byte expectedValue, Flags expectedFlags)
    {
        var z80 = new CodeBuilder()
            .Flags(flags)
            .Code(
                $"LD A,{value}",
                "RLA")
            .Build();

        z80.Run(7 + 4);

        z80.Registers.A.Should().Be(expectedValue);
        z80.Registers.F.Should().Be(expectedFlags);
        z80.CycleCounter.TotalCycles.Should().Be(11);
    }

    [Theory]
    [InlineData(0x80, H | N | C, 0xC0, None)]
    [InlineData(0x55, All, 0xAA, S | Z | Y | X | P | C)]
    [InlineData(0x89, None, 0x44, C)]
    [InlineData(0x44, C, 0xA2, Y)]
    public void When_RRA_InstructionIsExecuted_AccumulatorAndFlagsAreUpdated(
        byte value, Flags flags, byte expectedValue, Flags expectedFlags)
    {
        var z80 = new CodeBuilder()
            .Flags(flags)
            .Code(
                $"LD A,{value}",
                "RRA")
            .Build();

        z80.Run(7 + 4);

        z80.Registers.A.Should().Be(expectedValue);
        z80.Registers.F.Should().Be(expectedFlags);
        z80.CycleCounter.TotalCycles.Should().Be(11);
    }

    [Theory]
    [InlineData(0x55, Z | H | N | C, 0xAA, S | Y | X | P)]
    [InlineData(0xAA, Z | H | N | C, 0x55, P | C)]
    [InlineData(0x00, All, 0x00, Z | P)]
    [InlineData(0x80, C, 0x01, C)]
    public void When_RLC_r_InstructionIsExecuted_RegisterAndFlagsAreUpdated(
        byte value, Flags flags, byte expectedValue, Flags expectedFlags)
    {
        var z80 = new CodeBuilder()
            .Flags(flags)
            .Code(
                $"LD C,{value}",
                "RLC C")
            .Build();

        z80.Run(7 + 8);

        z80.Registers.C.Should().Be(expectedValue);
        z80.Registers.F.Should().Be(expectedFlags);
        z80.CycleCounter.TotalCycles.Should().Be(15);
    }

    [Fact]
    public void When_RLC_HL_InstructionIsExecuted_RegisterAndFlagsAreUpdated()
    {
        var builder = new CodeBuilder()
            .Flags(All)
            .Code(
                "LD HL,0x06",
                "RLC (HL)",
                "db 0,1");
        var z80 = builder.Build();

        z80.Run(10 + 15);

        builder.Memory![0x06].Should().Be(0x02);
        z80.Registers.F.Should().Be(None);
        z80.CycleCounter.TotalCycles.Should().Be(25);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_RLC_IX_InstructionIsExecuted_RegisterAndFlagsAreUpdated(string register)
    {
        var builder = new CodeBuilder()
            .Flags(All)
            .Code(
                $"LD {register},4",
                $"RLC ({register}+5)",
                "db 0,1");
        var z80 = builder.Build();

        z80.Run(14 + 23);

        builder.Memory![0x09].Should().Be(0x02);
        z80.Registers.F.Should().Be(None);
        z80.CycleCounter.TotalCycles.Should().Be(37);
    }
}