using OldBit.Z80Cpu.UnitTests.Fixtures;

namespace OldBit.Z80Cpu.UnitTests;

public class StepExecutionTests
{
    [Fact]
    public void WhenStepIsCalled_SingleInstructionIsExecuted()
    {
        var z80 = new Z80Builder()
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

        // LD BC,0xFFFF
        z80.Step();
        z80.Registers.BC.ShouldBe(0xFFFF);
        z80.Registers.DE.ShouldBe(0);
        z80.Registers.HL.ShouldBe(0);
        z80.Registers.SP.ShouldBe(0xFFFF);
        z80.Registers.IX.ShouldBe(0);
        z80.Registers.IY.ShouldBe(0);

        // DEC BC
        z80.Step();
        z80.Registers.BC.ShouldBe(0xFFFE);
        z80.Registers.DE.ShouldBe(0);
        z80.Registers.HL.ShouldBe(0);
        z80.Registers.SP.ShouldBe(0xFFFF);
        z80.Registers.IX.ShouldBe(0);
        z80.Registers.IY.ShouldBe(0);

        // LD DE,0x0000
        z80.Step();
        z80.Registers.BC.ShouldBe(0xFFFE);
        z80.Registers.DE.ShouldBe(0);
        z80.Registers.HL.ShouldBe(0);
        z80.Registers.SP.ShouldBe(0xFFFF);
        z80.Registers.IX.ShouldBe(0);
        z80.Registers.IY.ShouldBe(0);

        // DEC DE
        z80.Step();
        z80.Registers.BC.ShouldBe(0xFFFE);
        z80.Registers.DE.ShouldBe(0xFFFF);
        z80.Registers.HL.ShouldBe(0);
        z80.Registers.SP.ShouldBe(0xFFFF);
        z80.Registers.IX.ShouldBe(0);
        z80.Registers.IY.ShouldBe(0);

        // LD HL,0xFFFE
        z80.Step();
        z80.Registers.BC.ShouldBe(0xFFFE);
        z80.Registers.DE.ShouldBe(0xFFFF);
        z80.Registers.HL.ShouldBe(0xFFFE);
        z80.Registers.SP.ShouldBe(0xFFFF);
        z80.Registers.IX.ShouldBe(0);
        z80.Registers.IY.ShouldBe(0);

        // DEC HL
        z80.Step();
        z80.Registers.BC.ShouldBe(0xFFFE);
        z80.Registers.DE.ShouldBe(0xFFFF);
        z80.Registers.HL.ShouldBe(0xFFFD);
        z80.Registers.SP.ShouldBe(0xFFFF);
        z80.Registers.IX.ShouldBe(0);
        z80.Registers.IY.ShouldBe(0);

        // LD SP,0x00F9
        z80.Step();
        z80.Registers.BC.ShouldBe(0xFFFE);
        z80.Registers.DE.ShouldBe(0xFFFF);
        z80.Registers.HL.ShouldBe(0xFFFD);
        z80.Registers.SP.ShouldBe(0x00F9);
        z80.Registers.IX.ShouldBe(0);
        z80.Registers.IY.ShouldBe(0);

        // DEC SP
        z80.Step();
        z80.Registers.BC.ShouldBe(0xFFFE);
        z80.Registers.DE.ShouldBe(0xFFFF);
        z80.Registers.HL.ShouldBe(0xFFFD);
        z80.Registers.SP.ShouldBe(0x00F8);
        z80.Registers.IX.ShouldBe(0);
        z80.Registers.IY.ShouldBe(0);

        // LD IX,0xFF00
        z80.Step();
        z80.Registers.BC.ShouldBe(0xFFFE);
        z80.Registers.DE.ShouldBe(0xFFFF);
        z80.Registers.HL.ShouldBe(0xFFFD);
        z80.Registers.SP.ShouldBe(0x00F8);
        z80.Registers.IX.ShouldBe(0xFF00);
        z80.Registers.IY.ShouldBe(0);

        // DEC IX
        z80.Step();
        z80.Registers.BC.ShouldBe(0xFFFE);
        z80.Registers.DE.ShouldBe(0xFFFF);
        z80.Registers.HL.ShouldBe(0xFFFD);
        z80.Registers.SP.ShouldBe(0x00F8);
        z80.Registers.IX.ShouldBe(0xFEFF);
        z80.Registers.IY.ShouldBe(0);

        // LD IY,0xFF01
        z80.Step();
        z80.Registers.BC.ShouldBe(0xFFFE);
        z80.Registers.DE.ShouldBe(0xFFFF);
        z80.Registers.HL.ShouldBe(0xFFFD);
        z80.Registers.SP.ShouldBe(0x00F8);
        z80.Registers.IX.ShouldBe(0xFEFF);
        z80.Registers.IY.ShouldBe(0xFF01);

        // DEC IY
        z80.Step();
        z80.Registers.BC.ShouldBe(0xFFFE);
        z80.Registers.DE.ShouldBe(0xFFFF);
        z80.Registers.HL.ShouldBe(0xFFFD);
        z80.Registers.SP.ShouldBe(0x00F8);
        z80.Registers.IX.ShouldBe(0xFEFF);
        z80.Registers.IY.ShouldBe(0xFF00);
    }
}