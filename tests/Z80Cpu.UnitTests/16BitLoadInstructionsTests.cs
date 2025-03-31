using OldBit.Z80Cpu.UnitTests.Fixtures;

namespace OldBit.Z80Cpu.UnitTests;

public class Z8016BitLoadInstructionsTests
{
    [Fact]
    public void When_LD_RR_nn_InstructionIsExecuted_RegisterValueIsUpdated()
    {
        var z80 = new Z80Builder()
            .Code(
                "LD BC,0x0201",
                "LD DE,0x0403",
                "LD HL,0x0605",
                "LD SP,0x0807",
                "LD IX,0x0A09",
                "LD IY,0x0C0B")
            .Build();

        z80.Run(4 * 10 + 2 * 14);

        z80.Registers.BC.ShouldBe(0x0201);
        z80.Registers.DE.ShouldBe(0x0403);
        z80.Registers.HL.ShouldBe(0x0605);
        z80.Registers.SP.ShouldBe(0x0807);
        z80.Registers.IX.ShouldBe(0x0A09);
        z80.Registers.IY.ShouldBe(0x0C0B);
        z80.Clock.FrameTicks.ShouldBe(68);
    }

    [Fact]
    public void When_LD_HL_IX_IY_mm_InstructionIsExecuted_RegisterValueIsUpdated()
    {
        var z80 = new Z80Builder()
            .Code(
                "LD HL,(0x000B)",
                "LD IX,(0x000D)",
                "LD IY,(0x000F)",
                "db 0x01, 0x02, 0x03, 0x04, 0x05, 0x06")
            .Build();

        z80.Run(16 + 2 * 20);

        z80.Registers.HL.ShouldBe(0x0201);
        z80.Registers.IX.ShouldBe(0x0403);
        z80.Registers.IY.ShouldBe(0x0605);
        z80.Clock.FrameTicks.ShouldBe(56);
    }

    [Fact]
    public void When_LD_RR_mm_InstructionIsExecuted_RegisterValueIsUpdated()
    {
        var z80 = new Z80Builder()
            .Code(
                "LD BC,(0x0010)",
                "LD DE,(0x0012)",
                "db 0xED, 0x6B, 0x14, 0x00", // long form of LD HL,(0x0014)
                "LD SP,(0x0016)",
                "db 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08")
            .Build();

        z80.Run(4 * 20);

        z80.Registers.BC.ShouldBe(0x0201);
        z80.Registers.DE.ShouldBe(0x0403);
        z80.Registers.HL.ShouldBe(0x0605);
        z80.Registers.SP.ShouldBe(0x0807);
        z80.Clock.FrameTicks.ShouldBe(80);
    }

    [Fact]
    public void When_PUSH_RR_InstructionIsExecuted_RegisterValueIsStoredAtStack()
    {
        var builder = new Z80Builder()
            .Flags(S | C)
            .Code(
                "LD SP,0x002A",
                "LD A,0x98",
                "LD BC,0x1234",
                "LD DE,0x1335",
                "LD HL,0x1436",
                "LD IX,0x1537",
                "LD IY,0x1638",
                "PUSH AF",
                "PUSH BC",
                "PUSH DE",
                "PUSH HL",
                "PUSH IX",
                "PUSH IY",
                "db 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00");
        var z80 = builder.Build();

        z80.Run(10 + 7 + 3 * 10 + 2 * 14 + 4 * 11 + 2 * 15);

        var memory = builder.Memory!;
        memory[0x29].ShouldBe(z80.Registers.A);
        memory[0x28].ShouldBe((byte)z80.Registers.F);
        memory[0x27].ShouldBe(z80.Registers.B);
        memory[0x26].ShouldBe(z80.Registers.C);
        memory[0x25].ShouldBe(z80.Registers.D);
        memory[0x24].ShouldBe(z80.Registers.E);
        memory[0x23].ShouldBe(z80.Registers.H);
        memory[0x22].ShouldBe(z80.Registers.L);
        memory[0x21].ShouldBe(z80.Registers.IXH);
        memory[0x20].ShouldBe(z80.Registers.IXL);
        memory[0x1F].ShouldBe(z80.Registers.IYH);
        memory[0x1E].ShouldBe(z80.Registers.IYL);
        z80.Clock.FrameTicks.ShouldBe(149);
    }

    [Fact]
    public void When_POP_RR_InstructionIsExecuted_RegisterValueIsRestoredFromStack()
    {
        var z80 = new Z80Builder()
            .Code(
                "LD SP,0x000C",
                "POP AF",
                "POP BC",
                "POP DE",
                "POP HL",
                "POP IX",
                "POP IY",
                "NOP",
                "db 0x43, 0x21, 0x44, 0x22, 0x45, 0x23, 0x46, 0x24, 0x47, 0x25, 0x48, 0x26")
            .Build();

        z80.Run(5 * 10 + 2 * 14);

        z80.Registers.A.ShouldBe(0x21);
        z80.Registers.F.ShouldBe(C | N | Z);
        z80.Registers.B.ShouldBe(0x22);
        z80.Registers.C.ShouldBe(0x44);
        z80.Registers.D.ShouldBe(0x23);
        z80.Registers.E.ShouldBe(0x45);
        z80.Registers.H.ShouldBe(0x24);
        z80.Registers.L.ShouldBe(0x46);
        z80.Registers.IXH.ShouldBe(0x25);
        z80.Registers.IXL.ShouldBe(0x47);
        z80.Registers.IYH.ShouldBe(0x26);
        z80.Registers.IYL.ShouldBe(0x48);
        z80.Registers.SP.ShouldBe(0x18);
        z80.Clock.FrameTicks.ShouldBe(78);
    }

    [Fact]
    public void When_LD_SP_HL_InstructionIsExecuted_StackPointerIsUpdated()
    {
        var z80 = new Z80Builder()
            .Code(
                "LD HL,0x1234",
                "LD SP,HL")
            .Build();

        z80.Run(10 + 6);

        z80.Registers.SP.ShouldBe(0x1234);
        z80.Clock.FrameTicks.ShouldBe(16);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_LD_SP_IXY_InstructionIsExecuted_StackPointerIsUpdated(string register)
    {
        var z80 = new Z80Builder()
            .Code(
                $"LD {register},0x1234",
                $"LD SP,{register}")
            .Build();

        z80.Run(14 + 10);

        z80.Registers.SP.ShouldBe(0x1234);
        z80.Clock.FrameTicks.ShouldBe(24);
    }

    [Fact]
    public void When_LD_mm_HL_InstructionIsExecuted_MemoryIsUpdated()
    {
        var builder = new Z80Builder()
            .Code(
                "LD HL,0x1234",
                "LD (6),HL",
                "db 0,0");
        var z80 = builder.Build();
        var memory = builder.Memory!;

        z80.Run(10 + 16);

        memory[6].ShouldBe(0x34);
        memory[7].ShouldBe(0x12);
        z80.Clock.FrameTicks.ShouldBe(26);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_LD_mm_IXY_InstructionIsExecuted_MemoryIsUpdated(string opCode)
    {
        var builder = new Z80Builder()
            .Code(
                $"LD {opCode},0x1234",
                $"LD (8),{opCode}",
                "db 0,0");
        var z80 = builder.Build();
        var memory = builder.Memory!;

        z80.Run(14 + 20);

        memory[8].ShouldBe(0x34);
        memory[9].ShouldBe(0x12);
        z80.Clock.FrameTicks.ShouldBe(34);
    }

    [Fact]
    public void When_LD_mm_RR_InstructionIsExecuted_RegisterValueIsUpdated()
    {
        var builder = new Z80Builder()
            .Code(
                "LD BC,0x0102",
                "LD DE,0x0304",
                "LD HL,0x0506",
                "LD SP,0x0708",
                "LD (0x1C),BC",
                "LD (0x1E),DE",
                "db 0xED, 0x63, 0x20, 0x00", // long form of LD (4),HL
                "LD (0x22),SP",
                "db 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00");

        var z80 = builder.Build();
        var memory = builder.Memory!;

        z80.Run(4 * 10 + 4 * 20);

        memory[0x1C].ShouldBe(0x02);
        memory[0x1D].ShouldBe(0x01);
        memory[0x1E].ShouldBe(0x04);
        memory[0x1F].ShouldBe(0x03);
        memory[0x20].ShouldBe(0x06);
        memory[0x21].ShouldBe(0x05);
        memory[0x22].ShouldBe(0x08);
        memory[0x23].ShouldBe(0x07);
        z80.Clock.FrameTicks.ShouldBe(120);
    }
}