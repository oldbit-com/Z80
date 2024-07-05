using OldBit.Z80Cpu.UnitTests.Fixtures;

namespace OldBit.Z80Cpu.UnitTests;

public class RunModeTests
{
    [Fact]
    public void WhenRunModeIsAbsolute_ExecutesSGivenNumberOfTicks()
    {
        var z80 = CreateZ80();

        z80.Run(7 + 4 + 4 + 4);

        z80.Registers.A.Should().Be(0x04);
        z80.Clock.TotalTicks.Should().Be(19);
        z80.Clock.FrameTicks.Should().Be(19);
        z80.Clock.IsComplete.Should().BeTrue();
    }

    [Fact]
    public void WhenRunModeIsIncremental_ExecutesGivenNumberOfTicksIncrementally()
    {
        var z80 = CreateZ80();

        z80.Run(7, RunMode.Incremental);
        z80.Registers.A.Should().Be(0x01);
        z80.Clock.TotalTicks.Should().Be(7);
        z80.Clock.FrameTicks.Should().Be(7);
        z80.Clock.IsComplete.Should().BeTrue();

        z80.Run(11, RunMode.Incremental);
        z80.Registers.A.Should().Be(0x02);
        z80.Clock.TotalTicks.Should().Be(11);
        z80.Clock.FrameTicks.Should().Be(11);
        z80.Clock.IsComplete.Should().BeTrue();

        z80.Run(11, RunMode.Incremental);
        z80.Registers.A.Should().Be(0x02);
        z80.Clock.TotalTicks.Should().Be(11);
        z80.Clock.FrameTicks.Should().Be(11);
        z80.Clock.IsComplete.Should().BeTrue();

        z80.Run(12, RunMode.Incremental);
        z80.Registers.A.Should().Be(0x03);
        z80.Clock.TotalTicks.Should().Be(15);
        z80.Clock.FrameTicks.Should().Be(15);
        z80.Clock.IsComplete.Should().BeTrue();

        z80.Run(19, RunMode.Incremental);
        z80.Registers.A.Should().Be(0x04);
        z80.Clock.TotalTicks.Should().Be(19);
        z80.Clock.FrameTicks.Should().Be(19);
        z80.Clock.IsComplete.Should().BeTrue();

        z80.Run(20, RunMode.Incremental);
        z80.Registers.A.Should().Be(0x00);
        z80.Clock.TotalTicks.Should().Be(23);
        z80.Clock.FrameTicks.Should().Be(23);
        z80.Clock.IsComplete.Should().BeTrue();
    }

    private static Z80 CreateZ80()
    {
        return new Z80Builder()
            .Code(
                "LD A,0x01",
                "INC A",
                "INC A",
                "INC A",
                "XOR A")
            .Build();
    }
}