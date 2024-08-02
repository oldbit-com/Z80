using OldBit.Z80Cpu.UnitTests.Fixtures;

namespace OldBit.Z80Cpu.UnitTests;

public class InterruptTests
{
    [Fact]
    public void TestMaskableInterrupt_WhenNotHalted()
    {
        var builder = new Z80Builder()
            .StartAddress(0x1234)
            .SetSP(0x04)
            .Iff1(false)
            .Code(
                "block 0x2344",
                "db 0x42,0x78");
        var z80 = builder.Build();

        z80.INT(0);

        z80.Registers.SP.Should().Be(0x04);
        z80.Registers.PC.Should().Be(0x1234);

        z80.Clock.TotalTicks.Should().Be(0);
    }

    [Fact]
    public void TestMaskableInterrupt_Im1()
    {
        var builder = new Z80Builder()
            .StartAddress(0x1234)
            .SetSP(0x04)
            .Iff1(true)
            .Iff2(true)
            .Halt(true)
            .Im(InterruptMode.Mode1)
            .Code(
                "block 0x2344",
                "db 0x42,0x78");
        var z80 = builder.Build();
        var memory = builder.Memory!;

        z80.INT(0);

        z80.Registers.SP.Should().Be(0x02);
        z80.Registers.PC.Should().Be(0x38);

        memory.Read(0x03).Should().Be(0x12);
        memory.Read(0x02).Should().Be(0x35);

        z80.IsHalted.Should().BeFalse();
        z80.IFF1.Should().BeFalse();
        z80.IFF2.Should().BeFalse();

        z80.Clock.TotalTicks.Should().Be(13);
    }

    [Fact]
    public void TestMaskableInterrupt_Im2()
    {
        var builder = new Z80Builder()
            .StartAddress(0x1234)
            .SetSP(0x04)
            .SetI(0x23)
            .Iff1(true)
            .Iff2(true)
            .Halt(true)
            .Im(InterruptMode.Mode2)
            .Code(
                "block 0x2344",
                "db 0x42,0x78");
        var z80 = builder.Build();
        var memory = builder.Memory!;

        z80.INT(0x44);

        z80.Registers.SP.Should().Be(0x02);
        z80.Registers.PC.Should().Be(0x7842);

        memory.Read(0x03).Should().Be(0x12);
        memory.Read(0x02).Should().Be(0x35);

        z80.IsHalted.Should().BeFalse();
        z80.IFF1.Should().BeFalse();
        z80.IFF2.Should().BeFalse();

        z80.Clock.TotalTicks.Should().Be(19);
    }

    [Fact]
    public void TestNonMaskableInterrupt()
    {
        var builder = new Z80Builder()
            .StartAddress(0x1234)
            .SetSP(0x04)
            .Iff1(true)
            .Code("db 0,0,0,0");
        var z80 = builder.Build();
        var memory = builder.Memory!;

        z80.NMI();

        z80.Registers.SP.Should().Be(0x02);
        z80.Registers.PC.Should().Be(0x66);

        memory.Read(0x03).Should().Be(0x12);
        memory.Read(0x02).Should().Be(0x34);

        z80.IsHalted.Should().BeFalse();
        z80.IFF1.Should().BeFalse();
        z80.IFF2.Should().BeFalse();

        z80.Clock.TotalTicks.Should().Be(11);
    }
}