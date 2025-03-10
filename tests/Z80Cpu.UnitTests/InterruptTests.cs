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

        z80.TriggerInt(0);

        z80.Registers.SP.ShouldBe(0x04);
        z80.Registers.PC.ShouldBe(0x1234);

        z80.Clock.TotalTicks.ShouldBe(0);
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

        z80.TriggerInt(0);

        z80.Registers.SP.ShouldBe(0x02);
        z80.Registers.PC.ShouldBe(0x38);

        memory.Read(0x03).ShouldBe(0x12);
        memory.Read(0x02).ShouldBe(0x34);

        z80.IsHalted.ShouldBeFalse();
        z80.IFF1.ShouldBeFalse();
        z80.IFF2.ShouldBeFalse();

        z80.Clock.TotalTicks.ShouldBe(13);
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

        z80.TriggerInt(0x44);

        z80.Registers.SP.ShouldBe(0x02);
        z80.Registers.PC.ShouldBe(0x7842);

        memory.Read(0x03).ShouldBe(0x12);
        memory.Read(0x02).ShouldBe(0x34);

        z80.IsHalted.ShouldBeFalse();
        z80.IFF1.ShouldBeFalse();
        z80.IFF2.ShouldBeFalse();

        z80.Clock.TotalTicks.ShouldBe(19);
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

        z80.TriggerNmi();

        z80.Registers.SP.ShouldBe(0x02);
        z80.Registers.PC.ShouldBe(0x66);

        memory.Read(0x03).ShouldBe(0x12);
        memory.Read(0x02).ShouldBe(0x34);

        z80.IsHalted.ShouldBeFalse();
        z80.IFF1.ShouldBeFalse();
        z80.IFF2.ShouldBeFalse();

        z80.Clock.TotalTicks.ShouldBe(11);
    }
}