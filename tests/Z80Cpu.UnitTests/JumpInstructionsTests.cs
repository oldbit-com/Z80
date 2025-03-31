using OldBit.Z80Cpu.Registers;
using OldBit.Z80Cpu.UnitTests.Extensions;
using OldBit.Z80Cpu.UnitTests.Fixtures;

namespace OldBit.Z80Cpu.UnitTests;

public class Z80JumpInstructionsTests
{
    [Fact]
    public void When_JP_InstructionIsExecuted_CodeAtJumpLocationIsExecuted()
    {
        var z80 = new Z80Builder()
            .Code(
                "JP 0x06",
                "LD A,0xAA",
                "HALT",
                "LD A,0x55")
            .Build();

        z80.Run(10 + 7);

        z80.Registers.A.ShouldBe(0x55);
        z80.Registers.PC.ShouldBe(0x0008);
        z80.Clock.FrameTicks.ShouldBe(17);
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
    public void When_JP_cc_InstructionIsExecuted_DependingOnFlagCondition_CodeAtJumpLocationIsExecuted(string condition, Flags flags, bool shouldJump)
    {
        var z80 = new Z80Builder()
            .Flags(flags)
            .Code(
                $"JP {condition},0x06",
                "LD A,0xAA",
                "HALT",
                "LD A,0x55")
            .Build();

        z80.Run(10 + 7);

        z80.Registers.A.ShouldBe(shouldJump ? 0x55 : 0xAA);
        z80.Registers.PC.ShouldBe(shouldJump ? 0x0008 : 0x0005);
        z80.Clock.FrameTicks.ShouldBe(17);
    }

    [Fact]
    public void When_JR_InstructionIsExecuted_CodeAtJumpLocationIsExecuted()
    {
        var z80 = new Z80Builder()
            .Code(
                $"JR 0x03",
                "LD C,0x11",
                "HALT",
                "LD D,0x22")
            .Build();

        z80.Run(12 + 7);

        z80.Registers.C.ShouldBe(0x00);
        z80.Registers.D.ShouldBe(0x22);
        z80.Registers.PC.ShouldBe(0x07);
        z80.Clock.FrameTicks.ShouldBe(19);
    }

    [Theory]
    [InlineData("c", C, true)]
    [InlineData("z", Z, true)]
    [InlineData("nc", None, true)]
    [InlineData("nz", None, true)]
    [InlineData("c", None, false)]
    [InlineData("z", None, false)]
    [InlineData("nc", C, false)]
    [InlineData("nz", Z, false)]
    public void When_JR_cc_InstructionIsExecuted_DependingOnFlagCondition_CodeAtJumpLocationIsExecuted(string condition, Flags flags, bool shouldJump)
    {
        // Forward jump
        var z80 = new Z80Builder()
            .Flags(flags)
            .Code(
                $"JR {condition},2",
                "LD B,0xAB",
                "LD C,0x54")
            .Build();

        var ticks = shouldJump ? 12 + 7 : 7 + 7;
        z80.Run(ticks);

        z80.Registers.B.ShouldBe(shouldJump ? 0x00 : 0xAB);
        z80.Registers.C.ShouldBe(shouldJump ? 0x54 : 0x00);
        z80.Registers.PC.ShouldBe(shouldJump ? 0x0006 : 0x0004);
        z80.Clock.FrameTicks.ShouldBe(ticks);

        // Backward jump
        z80 = new Z80Builder()
            .Flags(flags)
            .StartAddress(0x02)
            .Code(
                "LD A,0x47",
                $"JR {condition},-4")
            .Build();

        ticks = shouldJump ? 12 + 7 : 7;
        z80.Run(ticks);

        z80.Registers.A.ShouldBe(shouldJump ? 0x47 : 0xFF);
        z80.Registers.PC.ShouldBe(shouldJump ? 0x0002 : 0x0004);
        z80.Clock.FrameTicks.ShouldBe(ticks);
    }

    [Theory]
    [InlineData("HL", 0x07, 10 + 4 + 7)]
    [InlineData("IX", 0x09, 14 + 8 + 7)]
    [InlineData("IY", 0x09, 14 + 8 + 7)]
    public void When_JP_RR_InstructionIsExecuted_CodeAtJumpLocationIsExecuted(string register, int address, int ticks)
    {
        var z80 = new Z80Builder()
            .Code(
                $"LD {register},{address}",
                $"JP ({register})",
                "LD A,0xAA",
                "HALT",
                "LD A,0x55")
            .Build();

        z80.Run(ticks);
        z80.Registers.A.ShouldBe(0x55);
        z80.Clock.FrameTicks.ShouldBe(ticks);
    }

    [Fact]
    public void When_DJNZ_InstructionIsExecuted_LoopIsExecuted()
    {
        var z80 = new Z80Builder()
            .Code(
                "LD B,0x20",
                "LD A,0",
                "INC A",
                "DJNZ -3")
            .Build();

        z80.Run(7 + 7 + 4 + 0x1F * (4 + 13) + 8);

        z80.Registers.A.ShouldBe(0x20);
        z80.Registers.B.ShouldBe(0);
        z80.Clock.FrameTicks.ShouldBe(553);

        z80 = new Z80Builder()
            .Code(
                "LD B,0xFF",
                "LD A,1",
                "DJNZ 1",
                "HALT",
                "INC A",
                "JR -6")
            .Build();

        z80.Run(7 + 7 + 0xFE * (13 + 4 + 12) + 8);

        z80.Registers.A.ShouldBe(0xFF);
        z80.Registers.B.ShouldBe(0);
        z80.Clock.FrameTicks.ShouldBe(7388);
    }
}