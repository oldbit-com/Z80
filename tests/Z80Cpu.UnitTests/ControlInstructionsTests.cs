using OldBit.Z80Cpu.UnitTests.Fixtures;

namespace OldBit.Z80Cpu.UnitTests;

public class Z80ControlInstructionsTests
{
    [Fact]
    public void When_HALT_InstructionIsExecuted_CpuIsInHaltState()
    {
        var z80 = new Z80Builder()
            .Code("HALT")
            .Build();

        z80.Run(4);

        z80.Registers.PC.ShouldBe(0);
        z80.IsHalted.ShouldBeTrue();
        z80.Clock.TotalTicks.ShouldBe(4);
    }

    [Fact]
    public void When_DI_InstructionIsExecuted_InterruptFlipFlop_FlagsAreFalse()
    {
        var z80 = new Z80Builder()
            .Code(
                "EI",
                "DI")
            .Build();

        z80.Run(8);

        z80.Registers.PC.ShouldBe(2);
        z80.IFF1.ShouldBeFalse();
        z80.IFF2.ShouldBeFalse();
        z80.Clock.TotalTicks.ShouldBe(8);
    }

    [Fact]
    public void When_EI_InstructionIsExecuted_InterruptFlipFlop_FlagsAreTrue()
    {
        var z80 = new Z80Builder()
            .Code(
                "DI",
                "EI")
            .Build();

        z80.Run(8);

        z80.Registers.PC.ShouldBe(2);
        z80.IFF1.ShouldBeTrue();
        z80.IFF2.ShouldBeTrue();
        z80.Clock.TotalTicks.ShouldBe(8);
    }

    [Fact]
    public void When_IM0_InstructionIsExecuted_InterruptModeIsSetTo0()
    {
        var z80 = new Z80Builder()
            .Code(
                "IM 2",
                "IM 0")
            .Build();

        z80.Run(16);

        z80.Registers.PC.ShouldBe(4);
        z80.IM.ShouldBe(InterruptMode.Mode0);
        z80.Clock.TotalTicks.ShouldBe(16);
    }

    [Fact]
    public void When_IM1_InstructionIsExecuted_CpuInterruptModeIsSetTo1()
    {
        var z80 = new Z80Builder()
            .Code("IM 1")
            .Build();

        z80.Run(8);

        z80.Registers.PC.ShouldBe(2);
        z80.IM.ShouldBe(InterruptMode.Mode1);
        z80.Clock.TotalTicks.ShouldBe(8);
    }

    [Fact]
    public void When_IM2_InstructionIsExecuted_CpuInterruptModeIsSetTo2()
    {
        var z80 = new Z80Builder()
            .Code("IM 2")
            .Build();

        z80.Run(8);

        z80.Registers.PC.ShouldBe(2);
        z80.IM.ShouldBe(InterruptMode.Mode2);
        z80.Clock.TotalTicks.ShouldBe(8);
    }
}