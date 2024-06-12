using OldBit.Z80Cpu.Registers;
using OldBit.Z80Cpu.UnitTests.Extensions;
using OldBit.Z80Cpu.UnitTests.Fixtures;

namespace OldBit.Z80Cpu.UnitTests;

public class Z80CallReturnInstructionsTests
{
    [Fact]
    public void When_CALL_InstructionIsExecuted_CodeAtJumpLocationIsExecutedAndStackUpdated()
    {
        var builder = new Z80Builder()
            .Code(
                "LD SP,0x0010",
                "CALL 0x0009",
                "LD A,0xAA",
                "HALT",
                "LD A,0x55",
                "db 0xFF, 0xFF, 0xFF, 0xFF, 0xFF");

        var z80 = builder.Build();
        z80.Run(10 + 17 + 7);

        var memory = builder.Memory!;
        z80.Registers.A.Should().Be(0x55);
        z80.Registers.SP.Should().Be(0x0E);
        z80.Registers.PC.Should().Be(0x000B);
        memory![0x0F].Should().Be(0x00);
        memory[0x0E].Should().Be(0x06);
        z80.Cycles.TotalCycles.Should().Be(34);
    }

    [Theory]
    [InlineData("c", C, true)]
    [InlineData("z", Z, true)]
    [InlineData("m", S, true)]
    [InlineData("pe", P, true)]
    [InlineData("nc", None, true)]
    [InlineData("nz", None, true)]
    [InlineData("p", None, true)]
    [InlineData("po", None, true)]
    [InlineData("c", None, false)]
    [InlineData("z", None, false)]
    [InlineData("m", None, false)]
    [InlineData("pe", None, false)]
    [InlineData("nc", C, false)]
    [InlineData("nz", Z, false)]
    [InlineData("p", S, false)]
    [InlineData("po", P, false)]
    public void When_CALL_cc_InstructionIsExecuted_DependingOnFlagCondition_CodeAtJumpLocationIsExecutedAndStackUpdated(string condition, Flags flags, bool shouldCall)
    {
        var builder = new Z80Builder()
            .Flags(flags)
            .Code(
                "LD SP,0x10",
                $"CALL {condition},0x09",
                "LD A,0xAA",
                "HALT",
                "LD A,0x55",
                "db 0xFF, 0xFF, 0xFF, 0xFF, 0xFF");
        var z80 = builder.Build();

        var cycles = shouldCall ? 10 + 17 + 7 : 10 + 10 + 7;
        z80.Run(cycles);

        var memory = builder.Memory!;
        if (shouldCall)
        {
            z80.Registers.A.Should().Be(0x55);
            z80.Registers.SP.Should().Be(0x0E);
            z80.Registers.PC.Should().Be(0x000B);
            memory[0x0F].Should().Be(0x00);
            memory[0x0E].Should().Be(0x06);
        }
        else
        {
            z80.Registers.A.Should().Be(0xAA);
            z80.Registers.PC.Should().Be(0x0008);
        }
        z80.Cycles.TotalCycles.Should().Be(cycles);
    }

    [Fact]
    public void When_RET_InstructionIsExecuted_CodeAtReturnAddressIsExecuted()
    {
        var z80 = new Z80Builder()
            .Code(
                "LD SP,0x000A",
                "RET",
                "LD A,0xAA",
                "HALT",
                "LD A,0x55",
                "NOP",
                "db 0x07, 0x00")
            .Build();

        z80.Run(10 + 10 + 7);

        z80.Registers.A.Should().Be(0x55);
        z80.Registers.SP.Should().Be(0x0C);
        z80.Registers.PC.Should().Be(0x0009);
        z80.Cycles.TotalCycles.Should().Be(27);
    }

    [Theory]
    [InlineData("c", C, true)]
    [InlineData("z", Z, true)]
    [InlineData("m", S, true)]
    [InlineData("pe", P, true)]
    [InlineData("nc", None, true)]
    [InlineData("nz", None, true)]
    [InlineData("p", None, true)]
    [InlineData("po", None, true)]
    [InlineData("c", None, false)]
    [InlineData("z", None, false)]
    [InlineData("m", None, false)]
    [InlineData("pe", None, false)]
    [InlineData("nc", C, false)]
    [InlineData("nz", Z, false)]
    [InlineData("p", S, false)]
    [InlineData("po", P, false)]
    public void When_RET_cc_InstructionIsExecuted_DependingOnFlagCondition_CodeAtReturnAddressIsExecuted(string condition, Flags flags, bool shouldReturn)
    {
        var z80 = new Z80Builder()
            .Flags(flags)
            .Code(
                "LD SP,0x000A",
                $"RET {condition}",
                "LD A,0xAA",
                "HALT",
                "LD A,0x55",
                "NOP",
                "db 0x07, 0x00")
            .Build();

        var cycles = shouldReturn ? 10 + 11 + 7 : 10 + 5 + 7;
        z80.Run(cycles);

        z80.Registers.A.Should().Be(shouldReturn ? 0x55 : 0xAA);
        z80.Registers.SP.Should().Be(shouldReturn ? 0x0C : 0x0A);
        z80.Registers.PC.Should().Be(shouldReturn ? 0x0009 : 0x0006);
        z80.Cycles.TotalCycles.Should().Be(cycles);
    }

    [Fact]
    public void When_RETN_InstructionIsExecuted_CodeAtReturnAddressIsExecuted()
    {
        var z80 = new Z80Builder()
            .Iff2(true)
            .Code(
                "LD SP,0x000B",
                "RETN",
                "LD A,0xAA",
                "HALT",
                "LD A,0x55",
                "NOP",
                "db 0x08, 0x00")
            .Build();

        z80.Run(10 + 14 + 7);

        z80.Registers.A.Should().Be(0x55);
        z80.Registers.SP.Should().Be(0x000D);
        z80.Registers.PC.Should().Be(0x000A);
        z80.IFF1.Should().Be(true);
        z80.Cycles.TotalCycles.Should().Be(31);
    }

    [Fact]
    public void When_RST_xx_InstructionIsExecuted_CodeAtSpecifiedAddressIsExecuted()
    {
        var z80 = new Z80Builder()
            .StartAddress(0x40)
            .Code(
                "LD A,0x01",
                "RET",
                "db 0,0,0,0,0",
                "ADD A,0x02",
                "RET",
                "db 0,0,0,0,0",
                "ADD A,0x04",
                "RET",
                "db 0,0,0,0,0",
                "ADD A,0x08",
                "RET",
                "db 0,0,0,0,0",
                "ADD A,0x10",
                "RET",
                "db 0,0,0,0,0",
                "ADD A,0x20",
                "RET",
                "db 0,0,0,0,0",
                "ADD A,0x40",
                "RET",
                "db 0,0,0,0,0",
                "ADD A,0x80",
                "RET",
                "db 0,0,0,0,0",
                "LD SP,0x0050",
                "RST 00h",
                "RST 08h",
                "RST 10h",
                "RST 18h",
                "RST 20h",
                "RST 28h",
                "RST 30h",
                "RST 38h",
                "LD B,0x55",
                "db 0,0,0")
            .Build();

        z80.Run(10 + 8 * (11 + 7 + 10) + 7);
        z80.Registers.A.Should().Be(0xFF);
        z80.Registers.B.Should().Be(0x55);
        z80.Cycles.TotalCycles.Should().Be(241);
    }
}