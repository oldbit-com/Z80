using OldBit.Z80Cpu.UnitTests.Fixtures;

namespace OldBit.Z80Cpu.UnitTests;

public class BreakpointTests
{
    [Fact]
    public void WhenBreakIsCalled_ExecutionIsSuspended()
    {
        var z80 = new Z80Builder()
            .Code(
                "XOR A",        // PC: 0x00
                "INC A",        // PC: 0x01
                "INC A",        // PC: 0x02
                "INC A",        // PC: 0x03
                "INC A")        // PC: 0x04
            .Build();

        List<Word> breakpoints = [0x02, 0x04];

        z80.BeforeInstruction += pc =>
        {
            if (!breakpoints.Contains(pc))
            {
                return;
            }

            z80.Break();
            breakpoints.Remove(pc);
        };

        z80.Run();

        z80.Registers.PC.ShouldBe(0x02);
        z80.Registers.A.ShouldBe(0x01);

        z80.Run();

        z80.Registers.PC.ShouldBe(0x04);
        z80.Registers.A.ShouldBe(0x03);

        z80.Run(4);
        z80.Registers.PC.ShouldBe(0x05);
        z80.Registers.A.ShouldBe(0x04);
    }
}