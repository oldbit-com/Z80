using OldBit.Z80Cpu.UnitTests.Fixtures;

namespace OldBit.Z80Cpu.UnitTests;

public class Z80ExchangeBlockInstructionsTests
{
    [Fact]
    public void When_EX_DE_HL_InstructionIsExecuted_RegisterValuesAreSwapped()
    {
        var z80 = new Z80Builder()
            .Code(
                "LD HL,0x0201",
                "LD DE,0x0403",
                "EX DE,HL")
            .Build();

        z80.Run(20 + 4);

        z80.Registers.HL.ShouldBe(0x0403);
        z80.Registers.DE.ShouldBe(0x0201);
        z80.Clock.FrameTicks.ShouldBe(24);
    }

    [Fact]
    public void When_EX_AF_AF_InstructionIsExecuted_RegisterValuesAreSwapped()
    {
        var z80 = new Z80Builder()
            .SetAF(0xCC35, 0x5555)
            .Code("EX AF,AF")
            .Build();

        z80.Run(4);

        z80.Registers.AF.ShouldBe(0x5555);
        z80.Registers.Prime.AF.ShouldBe(0xCC35);
        z80.Clock.FrameTicks.ShouldBe(4);
    }

    [Fact]
    public void When_EXX_InstructionIsExecuted_RegisterValuesAreSwapped()
    {
        var z80 = new Z80Builder()
            .SetBC(0x0102, 0x1112)
            .SetDE(0x0304, 0x1314)
            .SetHL(0x0506, 0x1516)
            .Code("EXX")
            .Build();

        z80.Run(4);

        z80.Registers.BC.ShouldBe(0x1112);
        z80.Registers.Prime.BC.ShouldBe(0x0102);
        z80.Registers.DE.ShouldBe(0x1314);
        z80.Registers.Prime.DE.ShouldBe(0x0304);
        z80.Registers.HL.ShouldBe(0x1516);
        z80.Registers.Prime.HL.ShouldBe(0x0506);
        z80.Clock.FrameTicks.ShouldBe(4);
    }

    [Fact]
    public void When_EX_SP_HL_InstructionIsExecuted_HLAndMemoryValuesAreSwapped()
    {
        var builder = new Z80Builder()
            .Code(
                "LD HL,0x7012",
                "LD SP,0x0008",
                "EX SP,HL",
                "NOP",
                "db 0x11, 0x22");
        var z80 = builder.Build();

        z80.Run(2 * 10 + 19);

        z80.Registers.HL.ShouldBe(0x2211);
        builder.Memory![0x08].ShouldBe(0x12);
        builder.Memory![0x09].ShouldBe(0x70);
        z80.Clock.FrameTicks.ShouldBe(39);
    }

    [Fact]
    public void When_EX_SP_IX_InstructionIsExecuted_HLAndMemoryValuesAreSwapped()
    {
        var builder = new Z80Builder()
            .Code(
                "LD IX,0x7012",
                "LD SP,0x000A",
                "EX SP,IX",
                "NOP",
                "db 0x11, 0x22");
        var z80 = builder.Build();

        z80.Run(14 + 10 + 23);

        z80.Registers.IX.ShouldBe(0x2211);
        builder.Memory![0x0A].ShouldBe(0x12);
        builder.Memory![0x0B].ShouldBe(0x70);
        z80.Clock.FrameTicks.ShouldBe(47);
    }

    [Fact]
    public void When_EX_SP_IY_InstructionIsExecuted_HLAndMemoryValuesAreSwapped()
    {
        var builder = new Z80Builder()
            .Code(
                "LD IY,0x7012",
                "LD SP,0x000A",
                "EX SP,IY",
                "NOP",
                "db 0x11, 0x22");
        var z80 = builder.Build();

        z80.Run(14 + 10 + 23);

        z80.Registers.IY.ShouldBe(0x2211);
        builder.Memory![0x0A].ShouldBe(0x12);
        builder.Memory![0x0B].ShouldBe(0x70);
        z80.Clock.FrameTicks.ShouldBe(47);
    }

    [Fact]
    public void When_LDI_InstructionIsExecuted_RegistersAndMemoryValuesAreUpdated()
    {
        var builder = new Z80Builder()
            .Flags(All)
            .Code(
                "LD HL,0x0C",
                "LD DE,0x0D",
                "LD BC,0x01",
                "LDI",
                "NOP",
                "db 0xA5, 0x00");
        var z80 = builder.Build();

        z80.Run(3 * 10 + 16);

        builder.Memory![0x0D].ShouldBe(0xA5);
        z80.Registers.BC.ShouldBe(0x00);
        z80.Registers.DE.ShouldBe(0x0E);
        z80.Registers.HL.ShouldBe(0x0D);
        z80.Registers.F.ShouldBe(S | Z | C);
        z80.Clock.FrameTicks.ShouldBe(46);

        builder = new Z80Builder()
            .Flags(None)
            .Code(
                "LD HL,0x0C",
                "LD DE,0x0D",
                "LD BC,0x02",
                "LDI",
                "NOP",
                "db 0xA5, 0x00");
        z80 = builder.Build();

        z80.Run(3 * 10 + 16);

        builder.Memory![0x0D].ShouldBe(0xA5);
        z80.Registers.BC.ShouldBe(0x01);
        z80.Registers.DE.ShouldBe(0x0E);
        z80.Registers.HL.ShouldBe(0x0D);
        z80.Registers.F.ShouldBe(P);
        z80.Clock.FrameTicks.ShouldBe(46);
    }

    [Fact]
    public void When_LDIR_InstructionIsExecuted_RegistersAndMemoryValuesAreUpdated()
    {
        var builder = new Z80Builder()
            .Flags(All)
            .Code(
                "LD HL,0x0C",
                "LD DE,0x0F",
                "LD BC,0x03",
                "LDIR",
                "NOP",
                "db 0x88, 0x36, 0xA5, 0x00, 0x00, 0x00");
        var z80 = builder.Build();

        z80.Run(3 * 10 + 2 * 21 + 16);

        var memory = builder.Memory!;
        memory[0x0F].ShouldBe(0x88);
        memory[0x10].ShouldBe(0x36);
        memory[0x11].ShouldBe(0xA5);
        z80.Registers.BC.ShouldBe(0x00);
        z80.Registers.DE.ShouldBe(0x12);
        z80.Registers.HL.ShouldBe(0x0F);
        z80.Registers.F.ShouldBe(S | Z | C);
        z80.Clock.FrameTicks.ShouldBe(88);
    }

    [Fact]
    public void When_LDD_InstructionIsExecuted_RegistersAndMemoryValuesAreUpdated()
    {
        var builder = new Z80Builder()
            .Flags(All)
            .Code(
                "LD HL,0x0C",
                "LD DE,0x0D",
                "LD BC,0x01",
                "LDD",
                "NOP",
                "db 0xA5, 0x00");
        var z80 = builder.Build();

        z80.Run(3 * 10 + 16);

        builder.Memory![0x0D].ShouldBe(0xA5);
        z80.Registers.BC.ShouldBe(0x00);
        z80.Registers.DE.ShouldBe(0x0C);
        z80.Registers.HL.ShouldBe(0x0B);
        z80.Registers.F.ShouldBe(S | Z | C);
        z80.Clock.FrameTicks.ShouldBe(46);

        builder = new Z80Builder()
            .Flags(None)
            .Code(
                "LD HL,0x0C",
                "LD DE,0x0D",
                "LD BC,0x02",
                "LDD",
                "NOP",
                "db 0xA5, 0x00");
        z80 = builder.Build();

        z80.Run(3 * 10 + 16);

        builder.Memory![0x0D].ShouldBe(0xA5);
        z80.Registers.BC.ShouldBe(0x01);
        z80.Registers.DE.ShouldBe(0x0C);
        z80.Registers.HL.ShouldBe(0x0B);
        z80.Registers.F.ShouldBe(P);
        z80.Clock.FrameTicks.ShouldBe(46);
    }

    [Fact]
    public void When_LDDR_InstructionIsExecuted_RegistersAndMemoryValuesAreUpdated()
    {
        var builder = new Z80Builder()
            .Flags(All)
            .Code(
                "LD HL,0x0E",
                "LD DE,0x11",
                "LD BC,0x03",
                "LDDR",
                "NOP",
                "db 0x88, 0x36, 0xA5, 0x00, 0x00, 0x00");
        var z80 = builder.Build();

        z80.Run(3 * 10 + 2 * 21 + 16);

        var memory = builder.Memory!;
        memory[0x0F].ShouldBe(0x88);
        memory[0x10].ShouldBe(0x36);
        memory[0x11].ShouldBe(0xA5);
        z80.Registers.BC.ShouldBe(0x00);
        z80.Registers.DE.ShouldBe(0x0E);
        z80.Registers.HL.ShouldBe(0x0B);
        z80.Registers.F.ShouldBe(S | Z | Y | C);
        z80.Clock.FrameTicks.ShouldBe(88);
    }

    [Fact]
    public void When_CPI_InstructionIsExecuted_FlagsAreUpdated()
    {
        var builder = new Z80Builder()
            .Flags(C)
            .Code(
                "LD HL,0x0B",
                "LD BC,0x03",
                "LD A,0x88",
                "CPI",
                "NOP",
                "db 0x88");
        var z80 = builder.Build();

        z80.Run(10 + 10 + 7 + 16);

        z80.Registers.BC.ShouldBe(0x02);
        z80.Registers.HL.ShouldBe(0x0C);
        z80.Registers.F.ShouldBe(Z | P | N | C);
        z80.Clock.FrameTicks.ShouldBe(43);

        builder = new Z80Builder()
            .Flags(None)
            .Code(
                "LD HL,0x0B",
                "LD BC,0x01",
                "LD A,0x88",
                "CPI",
                "NOP",
                "db 0x89");
        z80 = builder.Build();

        z80.Run(10 + 10 + 7 + 16);

        z80.Registers.BC.ShouldBe(0x00);
        z80.Registers.HL.ShouldBe(0x0C);
        z80.Registers.F.ShouldBe(S | Y | H | X | N);
        z80.Clock.FrameTicks.ShouldBe(43);
    }

    [Fact]
    public void When_CPIR_InstructionIsExecuted_FlagsAreUpdated()
    {
        var builder = new Z80Builder()
            .Flags(None)
            .Code(
                "LD HL,0x0B",
                "LD BC,0xFF",
                "LD A,0x88",
                "CPIR",
                "NOP",
                "db 0x02, 0x04, 0x80, 0x88, 0x90");
        var z80 = builder.Build();

        z80.Run(10 + 10 + 7 + 21 + 21 + 21 + 16);

        z80.Registers.BC.ShouldBe(0xFB);
        z80.Registers.HL.ShouldBe(0x0F);
        z80.Registers.F.ShouldBe(Z | P | N);
        z80.Clock.FrameTicks.ShouldBe(106);
    }

    [Fact]
    public void When_CPD_InstructionIsExecuted_FlagsAreUpdated()
    {
        var builder = new Z80Builder()
            .Flags(C)
            .Code(
                "LD HL,0x0B",
                "LD BC,0x03",
                "LD A,0x88",
                "CPD",
                "NOP",
                "db 0x88");
        var z80 = builder.Build();

        z80.Run(10 + 10 + 7 + 16);

        z80.Registers.BC.ShouldBe(0x02);
        z80.Registers.HL.ShouldBe(0x0A);
        z80.Registers.F.ShouldBe(Z | P | N | C);
        z80.Clock.FrameTicks.ShouldBe(43);

        builder = new Z80Builder()
            .Flags(None)
            .Code(
                "LD HL,0x0B",
                "LD BC,0x01",
                "LD A,0x88",
                "CPD",
                "NOP",
                "db 0x89");
        z80 = builder.Build();

        z80.Run(10 + 10 + 7 + 16);

        z80.Registers.BC.ShouldBe(0x00);
        z80.Registers.HL.ShouldBe(0x0A);
        z80.Registers.F.ShouldBe(S | Y | H | X | N);
        z80.Clock.FrameTicks.ShouldBe(43);
    }

    [Fact]
    public void When_CPDR_InstructionIsExecuted_FlagsAreUpdated()
    {
        var builder = new Z80Builder()
            .Flags(None)
            .Code(
                "LD HL,0x0F",
                "LD BC,0xFF",
                "LD A,0x88",
                "CPDR",
                "NOP",
                "db 0x02, 0x04, 0x80, 0x88, 0x90");
        var z80 = builder.Build();

        z80.Run(10 + 10 + 7 + 21 + 16);

        z80.Registers.BC.ShouldBe(0xFD);
        z80.Registers.HL.ShouldBe(0x0D);
        z80.Registers.F.ShouldBe(Z | P | N);
        z80.Clock.FrameTicks.ShouldBe(64);
    }
}
