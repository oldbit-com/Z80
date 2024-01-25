using FluentAssertions;
using Z80.Net.Registers;
using Z80.Net.UnitTests.Extensions;
using Z80.Net.UnitTests.Fixtures;
using static Z80.Net.Registers.Flags;

namespace Z80.Net.UnitTests;

public class Z8016BitArithmeticInstructionsTests
{
    [Fact]
    public void When_ADD_HL_BC_InstructionIsExecuted_HLIsUpdatedAndFlagsSet()
    {
        var z80 = new CodeBuilder()
            .Flags(None)
            .Code(
                "LD HL,0xFFFF",
                "LD BC,0x0001",
                "ADD HL,BC")
            .Build();

        z80.Run(10 + 10 + 11);

        z80.Registers.HL.Should().Be(0x00);
        z80.Registers.F.Should().Be(H | C);
        z80.CycleCounter.TotalCycles.Should().Be(31);
    }

    [Theory]
    [InlineData(All, 0x4241, 0x1111, S | Z | P, 0x5352)]
    [InlineData(All, 0xFEFF, 0x0100, S | Z | Y | X | P, 0xFFFF)]
    [InlineData(None, 0xFEFF, 0x0100, Y | X, 0xFFFF)]
    public void When_ADD_HL_DE_InstructionIsExecuted_HLIsUpdatedAndFlagsSet(
        Flags flags, ushort hl, ushort de, Flags expectedFlags, ushort expectedHL)
    {
        var z80 = new CodeBuilder()
            .Flags(flags)
            .Code(
                $"LD HL,{hl}",
                $"LD DE,{de}",
                "ADD HL,DE")
            .Build();

        z80.Run(10 + 10 + 11);

        z80.Registers.HL.Should().Be(expectedHL);
        z80.Registers.F.Should().Be(expectedFlags);
        z80.CycleCounter.TotalCycles.Should().Be(31);
    }

    [Fact]
    public void When_ADD_HL_HL_InstructionIsExecuted_HLIsUpdatedAndFlagsSet()
    {
        var z80 = new CodeBuilder()
            .Flags(None)
            .Code(
                "LD HL,0x4241",
                "ADD HL,HL")
            .Build();

        z80.Run(10 + 11);

        z80.Registers.HL.Should().Be(0x8482);
        z80.Registers.F.Should().Be(None);
        z80.CycleCounter.TotalCycles.Should().Be(21);
    }

    [Fact]
    public void When_ADD_HL_SP_InstructionIsExecuted_HLIsUpdatedAndFlagsSet()
    {
        var z80 = new CodeBuilder()
            .Flags(None)
            .Code(
                "LD HL,0xFFFE",
                "LD SP,0x0002",
                "ADD HL,SP")
            .Build();

        z80.Run(10 + 10 + 11);

        z80.Registers.HL.Should().Be(0);
        z80.Registers.F.Should().Be(H | C);
        z80.CycleCounter.TotalCycles.Should().Be(31);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_ADD_IXY_SP_InstructionIsExecuted_IXIsUpdatedAndFlagsSet(string register)
    {
        var z80 = new CodeBuilder()
            .Flags(None)
            .Code(
                $"LD {register},0xFFFE",
                "LD SP,0x0003",
                $"ADD {register},SP")
            .Build();

        z80.Run(14 + 10 + 15);

        z80.Registers.ValueOf(register).Should().Be(0x0001);
        z80.Registers.F.Should().Be(H | C);
        z80.CycleCounter.TotalCycles.Should().Be(39);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_ADD_IXY_IXY_InstructionIsExecuted_IXIsUpdatedAndFlagsSet(string register)
    {
        var z80 = new CodeBuilder()
            .Flags(None)
            .Code(
                $"LD {register},0xFFFE",
                $"ADD {register},{register}")
            .Build();

        z80.Run(14 + 15);

        z80.Registers.ValueOf(register).Should().Be(0xFFFC);
        z80.Registers.F.Should().Be(Y | H | X | C);
        z80.CycleCounter.TotalCycles.Should().Be(29);
    }

    [Fact]
    public void When_ADC_HL_BC_InstructionIsExecuted_HLIsUpdatedAndFlagsSet()
    {
        var z80 = new CodeBuilder()
            .Flags(C)
            .Code(
                "LD HL,0xFFFE",
                "LD BC,0x0001",
                "ADC HL,BC")
            .Build();

        z80.Run(10 + 10 + 15);

        z80.Registers.HL.Should().Be(0x00);
        z80.Registers.F.Should().Be(Z | H | C);
        z80.CycleCounter.TotalCycles.Should().Be(35);
    }

    [Fact]
    public void When_ADC_HL_DE_InstructionIsExecuted_HLIsUpdatedAndFlagsSet()
    {
        var z80 = new CodeBuilder()
            .Flags(C)
            .Code(
                "LD HL,0x63C0",
                "LD DE,0x8AD0",
                "ADC HL,DE")
            .Build();

        z80.Run(10 + 10 + 15);

        z80.Registers.HL.Should().Be(0xEE91);
        z80.Registers.F.Should().Be(S | Y | X);
        z80.CycleCounter.TotalCycles.Should().Be(35);
    }

    [Fact]
    public void When_ADC_HL_HL_InstructionIsExecuted_HLIsUpdatedAndFlagsSet()
    {
        var z80 = new CodeBuilder()
            .Flags(All)
            .Code(
                "LD HL,0xF1FF",
                "ADC HL,HL")
            .Build();

        z80.Run(10 + 15);

        z80.Registers.HL.Should().Be(0xE3FF);
        z80.Registers.F.Should().Be(S | Y | C);
        z80.CycleCounter.TotalCycles.Should().Be(25);
    }

    [Fact]
    public void When_ADC_HL_SP_InstructionIsExecuted_HLIsUpdatedAndFlagsSet()
    {
        var z80 = new CodeBuilder()
            .Flags(C)
            .Code(
                "LD HL,0x7F18",
                "LD SP,0x7748",
                "ADC HL,SP")
            .Build();

        z80.Run(10 + 10 + 15);

        z80.Registers.HL.Should().Be(0xF661);
        z80.Registers.F.Should().Be(S | Y | H | P );
        z80.CycleCounter.TotalCycles.Should().Be(35);
    }

    [Fact]
    public void When_SBC_HL_BC_InstructionIsExecuted_HLIsUpdatedAndFlagsSet()
    {
        var z80 = new CodeBuilder()
            .Flags(C)
            .Code(
                "LD HL,0xFFFE",
                "LD BC,0xFFFD",
                "SBC HL,BC")
            .Build();

        z80.Run(10 + 10 + 15);

        z80.Registers.HL.Should().Be(0);
        z80.Registers.F.Should().Be(Z | N);
        z80.CycleCounter.TotalCycles.Should().Be(35);
    }

    [Fact]
    public void When_SBC_HL_DE_InstructionIsExecuted_HLIsUpdatedAndFlagsSet()
    {
        var z80 = new CodeBuilder()
            .Flags(C)
            .Code(
                "LD HL,0x0001",
                "LD DE,0x7FFD",
                "SBC HL,DE")
            .Build();

        z80.Run(10 + 10 + 15);

        z80.Registers.HL.Should().Be(0x8003);
        z80.Registers.F.Should().Be(S | H | N | C);
        z80.CycleCounter.TotalCycles.Should().Be(35);
    }

    [Fact]
    public void When_SBC_HL_HL_InstructionIsExecuted_HLIsUpdatedAndFlagsSet()
    {
        var z80 = new CodeBuilder()
            .Flags(C)
            .Code(
                "LD HL,0xFFFF",
                "SBC HL,HL")
            .Build();

        z80.Run(10 + 15);

        z80.Registers.HL.Should().Be(0xFFFF);
        z80.Registers.F.Should().Be(S | Y | H | X | N | C);
        z80.CycleCounter.TotalCycles.Should().Be(25);
    }

    [Fact]
    public void When_SBC_HL_SP_InstructionIsExecuted_HLIsUpdatedAndFlagsSet()
    {
        var z80 = new CodeBuilder()
            .Flags(C)
            .Code(
                "LD HL,0x7001",
                "LD SP,0x8FFD",
                "SBC HL,SP")
            .Build();

        z80.Run(10 + 10 + 15);

        z80.Registers.HL.Should().Be(0xE003);
        z80.Registers.F.Should().Be(S | Y | H | P | N | C);
        z80.CycleCounter.TotalCycles.Should().Be(35);
    }

    [Fact]
    public void When_INC_RR_InstructionIsExecuted_RRIsUpdatedAndFlagsSet()
    {
        var z80 = new CodeBuilder()
            .Flags(C)
            .Code(
                "LD BC,0xFFFF",
                "INC BC",
                "LD DE,0x0000",
                "INC DE",
                "LD HL,0xFFFE",
                "INC HL",
                "LD SP,0x00F9",
                "INC SP",
                "LD IX,0xFF00",
                "INC IX",
                "LD IY,0xFF01",
                "INC IY")
            .Build();

        z80.Run(4 * (10 + 6) + 2 * (14 + 10));

        z80.Registers.BC.Should().Be(0x0000);
        z80.Registers.DE.Should().Be(0x0001);
        z80.Registers.HL.Should().Be(0xFFFF);
        z80.Registers.SP.Should().Be(0x00FA);
        z80.Registers.IX.Should().Be(0xFF01);
        z80.Registers.IY.Should().Be(0xFF02);
        z80.CycleCounter.TotalCycles.Should().Be(112);
    }

    [Fact]
    public void When_DEC_RR_InstructionIsExecuted_RRIsUpdatedAndFlagsSet()
    {
        var z80 = new CodeBuilder()
            .Flags(C)
            .Code(
                "LD BC,0xFFFF",
                "DEC BC",
                "LD DE,0x0000",
                "DEC DE",
                "LD HL,0xFFFE",
                "DEC HL",
                "LD SP,0x00F9",
                "DEC SP",
                "LD IX,0xFF00",
                "DEC IX",
                "LD IY,0xFF01",
                "DEC IY")
            .Build();

        z80.Run(4 * (10 + 6) + 2 * (14 + 10));

        z80.Registers.BC.Should().Be(0xFFFE);
        z80.Registers.DE.Should().Be(0xFFFF);
        z80.Registers.HL.Should().Be(0xFFFD);
        z80.Registers.SP.Should().Be(0x00F8);
        z80.Registers.IX.Should().Be(0xFEFF);
        z80.Registers.IY.Should().Be(0xFF00);
        z80.CycleCounter.TotalCycles.Should().Be(112);
    }
}