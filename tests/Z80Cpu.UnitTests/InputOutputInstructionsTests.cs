using NSubstitute;
using OldBit.Z80Cpu.Registers;
using OldBit.Z80Cpu.UnitTests.Extensions;
using OldBit.Z80Cpu.UnitTests.Fixtures;
using OldBit.Z80Cpu.UnitTests.Support;

namespace OldBit.Z80Cpu.UnitTests;

public class Z80InputOutputInstructionsTests
{
    private readonly IBus _mockBus = Substitute.For<IBus>();

    [Fact]
    public void When_IN_A_n_InstructionIsExecuted_DefaultIsReturned()
    {
        var z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD A,0x23",
                "IN (A),0x24")
            .Build();

        z80.Run(7 + 11);

        z80.Registers.A.ShouldBe(0xFF);
        z80.Clock.TotalTicks.ShouldBe(18);
    }

    [Fact]
    public void When_IN_A_n_InstructionIsExecuted_InputIsReturned()
    {
        const byte data = 0xA4;
        _mockBus.Read(0x2324).Returns(data);

        var z80 = new Z80Builder()
            .Flags(None)
            .Bus(_mockBus)
            .Code(
                "LD A,0x23",
                "IN (A),0x24")
            .Build();

        z80.Run(7 + 11);

        z80.Registers.A.ShouldBe(data);
        z80.Clock.TotalTicks.ShouldBe(18);
    }

    [Theory]
    [InlineData("A", 0xA5, All, S | P | C | Y)]
    [InlineData("B", 0xA5, All, S | P | C | Y)]
    [InlineData("C", 0xA5, All, S | P | C | Y)]
    [InlineData("D", 0xA5, All, S | P | C | Y)]
    [InlineData("E", 0x7E, None, P | Y | X)]
    [InlineData("H", 0x7F, None, Y | X)]
    [InlineData("L", 0, None, Z | P)]
    public void When_IN_r_C_InstructionIsExecuted_InputIsReturned(
        string register, byte data, Flags flags, Flags expectedFlags)
    {
        _mockBus.Read(0x41A5).Returns(data);

        var z80 = new Z80Builder()
            .Flags(flags)
            .Bus(_mockBus)
            .Code(
                "LD BC,0x41A5",
                $"IN {register},(C)")
            .Build();

        z80.Run(10 + 12);

        z80.Registers.ValueOf(register).ShouldBe(data);
        z80.Registers.F.ShouldBe(expectedFlags);
        z80.Clock.TotalTicks.ShouldBe(22);
    }

    [Fact]
    public void When_IN_F_C_InstructionIsExecuted_InputIsReturned()
    {
        const byte data = 0x80;
        _mockBus.Read(0x4149).Returns(data);

        var z80 = new Z80Builder()
            .Flags(None)
            .Bus(_mockBus)
            .Code(
                "LD BC,0x4149",
                "IN F,(C)")
            .Build();

        z80.Run(10 + 12);

        z80.Registers.F.ShouldBe(S);
        z80.Clock.TotalTicks.ShouldBe(22);
    }

    [Fact]
    public void When_INI_InstructionIsExecuted_InputValueIsStoredInMemory()
    {
        const byte data = 0x76;
        _mockBus.Read(0x0134).Returns(data);

        var builder = new Z80Builder()
            .Flags(C)
            .Bus(_mockBus)
            .Code(
                "LD HL,0x09",
                "LD BC,0x0134",
                "INI",
                "NOP",
                "db 0x01");
        var z80 = builder.Build();
        var memory = builder.Memory!;

        z80.Run(10 + 10 + 16);
        memory[9].ShouldBe(data);
        z80.Registers.BC.ShouldBe(0x34);
        z80.Registers.HL.ShouldBe(0x0A);
        z80.Registers.F.ShouldBe(Z | P);
        z80.Clock.TotalTicks.ShouldBe(36);
    }

    [Fact]
    public void When_INIR_InstructionIsExecuted_InputValuesAreStoredInMemory()
    {
        _mockBus.Read(0x0534).Returns((byte)0x71);
        _mockBus.Read(0x0434).Returns((byte)0x72);
        _mockBus.Read(0x0334).Returns((byte)0x73);
        _mockBus.Read(0x0234).Returns((byte)0x74);
        _mockBus.Read(0x0134).Returns((byte)0x75);

        var builder = new Z80Builder()
            .Flags(C)
            .Bus(_mockBus)
            .Code(
                "LD HL,0x09",
                "LD BC,0x0534",
                "INIR",
                "NOP",
                "db 0x01,0x02,0x03,0x04,0x05");
        var z80 = builder.Build();
        var memory = builder.Memory!;

        z80.Run(10 + 10 + 21 + 21 + 21 + 21 + 16);
        memory[09].ShouldBe(0x71);
        memory[10].ShouldBe(0x72);
        memory[11].ShouldBe(0x73);
        memory[12].ShouldBe(0x74);
        memory[13].ShouldBe(0x75);
        z80.Registers.BC.ShouldBe(0x34);
        z80.Registers.HL.ShouldBe(0x0E);
        z80.Registers.F.ShouldBe(Z);
        z80.Clock.TotalTicks.ShouldBe(120);
    }

    [Fact]
    public void When_IND_InstructionIsExecuted_InputValueIsStoredInMemory()
    {
        const byte data = 0x76;
        _mockBus.Read(0x0134).Returns(data);

        var builder = new Z80Builder()
            .Flags(C)
            .Bus(_mockBus)
            .Code(
                "LD HL,0x09",
                "LD BC,0x0134",
                "IND",
                "NOP",
                "db 0x01");
        var z80 = builder.Build();
        var memory = builder.Memory!;

        z80.Run(10 + 10 + 16);
        memory[9].ShouldBe(data);
        z80.Registers.BC.ShouldBe(0x34);
        z80.Registers.HL.ShouldBe(0x08);
        z80.Registers.F.ShouldBe(Z);
        z80.Clock.TotalTicks.ShouldBe(36);
    }

    [Fact]
    public void When_INDR_InstructionIsExecuted_InputValuesAreStoredInMemory()
    {
        _mockBus.Read(0x0534).Returns((byte)0x71);
        _mockBus.Read(0x0434).Returns((byte)0x72);
        _mockBus.Read(0x0334).Returns((byte)0x73);
        _mockBus.Read(0x0234).Returns((byte)0x74);
        _mockBus.Read(0x0134).Returns((byte)0x75);

        var builder = new Z80Builder()
            .Flags(C)
            .Bus(_mockBus)
            .Code(
                "LD HL,0x0D",
                "LD BC,0x0534",
                "INDR",
                "NOP",
                "db 0x01,0x02,0x03,0x04,0x05");
        var z80 = builder.Build();
        var memory = builder.Memory!;

        z80.Run(10 + 10 + 21 + 21 + 21 + 21 + 16);
        memory[09].ShouldBe(0x75);
        memory[10].ShouldBe(0x74);
        memory[11].ShouldBe(0x73);
        memory[12].ShouldBe(0x72);
        memory[13].ShouldBe(0x71);
        z80.Registers.BC.ShouldBe(0x34);
        z80.Registers.HL.ShouldBe(0x08);
        z80.Registers.F.ShouldBe(P | Z);
        z80.Clock.TotalTicks.ShouldBe(120);
    }

    [Fact]
    public void When_OUT_A_n_InstructionIsExecuted_DataBusValueIsWritten()
    {
        var z80 = new Z80Builder()
            .Flags(None)
            .Bus(_mockBus)
            .Code(
                "LD A,0x23",
                "OUT (0x24),A")
            .Build();

        z80.Run(7 + 11);

        _mockBus.Received().Write(To.Word(z80.Registers.A, 0x24), z80.Registers.A);
        z80.Clock.TotalTicks.ShouldBe(18);
    }

    [Theory]
    [InlineData("A", 0xF1)]
    [InlineData("B", 0x41)]
    [InlineData("C", 0xA5)]
    [InlineData("D", 0xF1)]
    [InlineData("E", 0xF1)]
    [InlineData("H", 0xF1)]
    [InlineData("L", 0xF1)]
    public void When_OUT_C_r_InstructionIsExecuted_DataBusValueIsWritten(
        string register, byte expectedData)
    {
        var z80 = new Z80Builder()
            .Bus(_mockBus)
            .Code(
                $"LD {register},0xF1",
                "LD BC,0x41A5",
                $"OUT (C),{register}")
            .Build();

        z80.Run(7 + 10 + 12);

        _mockBus.Received().Write(To.Word(z80.Registers.B, z80.Registers.C), expectedData);
        z80.Clock.TotalTicks.ShouldBe(29);
    }

    [Fact]
    public void When_OUT_C_F_InstructionIsExecuted_DataBusZeroIsWritten()
    {
        var z80 = new Z80Builder()
            .Bus(_mockBus)
            .Code(
                "LD AF,0xFFFF",
                "LD BC,0x41A5",
                "OUT (C),F")
            .Build();

        z80.Run(10 + 10 + 12);

        _mockBus.Received().Write(To.Word(z80.Registers.B, z80.Registers.C), 0);
        z80.Clock.TotalTicks.ShouldBe(32);
    }

    [Fact]
    public void When_OUTI_InstructionIsExecuted_MemoryValueIsWritten()
    {
        const byte data = 0x87;
        var z80 = new Z80Builder()
            .Flags(C)
            .Bus(_mockBus)
            .Code(
                "LD HL,0x09",
                "LD BC,0x0134",
                "OUTI",
                "NOP",
                $"db {data}")
            .Build();

        z80.Run(10 + 10 + 16);
        _mockBus.Received().Write(To.Word(z80.Registers.B, z80.Registers.C), data);
        z80.Registers.BC.ShouldBe(0x34);
        z80.Registers.HL.ShouldBe(0x0A);
        z80.Registers.F.ShouldBe(Z | N);
        z80.Clock.TotalTicks.ShouldBe(36);
    }

    [Fact]
    public void When_OTIR_InstructionIsExecuted_MemoryValuesAreWritten()
    {
        var z80 = new Z80Builder()
            .Flags(C)
            .Bus(_mockBus)
            .Code(
                "LD HL,0x09",
                "LD BC,0x0534",
                "OTIR",
                "NOP",
                "db 0x01,0x02,0x03,0x04,0x05")
            .Build();

        z80.Run(10 + 10 + 21 + 21 + 21 + 21 + 16);
        _mockBus.Received().Write(To.Word(0x04, z80.Registers.C), 0x01);
        _mockBus.Received().Write(To.Word(0x03, z80.Registers.C), 0x02);
        _mockBus.Received().Write(To.Word(0x02, z80.Registers.C), 0x03);
        _mockBus.Received().Write(To.Word(0x01, z80.Registers.C), 0x04);
        _mockBus.Received().Write(To.Word(0x00, z80.Registers.C), 0x05);
        z80.Registers.BC.ShouldBe(0x34);
        z80.Registers.HL.ShouldBe(0x0E);
        z80.Registers.F.ShouldBe(Z | P);
        z80.Clock.TotalTicks.ShouldBe(120);
    }

    [Fact]
    public void When_OUTD_InstructionIsExecuted_MemoryValueIsWritten()
    {
        const byte data = 0x87;
        var z80 = new Z80Builder()
            .Flags(C)
            .Bus(_mockBus)
            .Code(
                "LD HL,0x09",
                "LD BC,0x0134",
                "OUTD",
                "NOP",
                $"db {data}")
            .Build();

        z80.Run(10 + 10 + 16);
        _mockBus.Received().Write(To.Word(z80.Registers.B, z80.Registers.C), data);
        z80.Registers.BC.ShouldBe(0x34);
        z80.Registers.HL.ShouldBe(0x08);
        z80.Registers.F.ShouldBe(Z | N);
        z80.Clock.TotalTicks.ShouldBe(36);
    }

    [Fact]
    public void When_OTDR_InstructionIsExecuted_MemoryValuesAreWritten()
    {
        var z80 = new Z80Builder()
            .Flags(C)
            .Bus(_mockBus)
            .Code(
                "LD HL,0x0D",
                "LD BC,0x0534",
                "OTDR",
                "NOP",
                "db 0x01,0x02,0x03,0x04,0x05")
            .Build();

        z80.Run(10 + 10 + 21 + 21 + 21 + 21 + 16);
        _mockBus.Received().Write(To.Word(0x04, z80.Registers.C), 0x05);
        _mockBus.Received().Write(To.Word(0x03, z80.Registers.C), 0x04);
        _mockBus.Received().Write(To.Word(0x02, z80.Registers.C), 0x03);
        _mockBus.Received().Write(To.Word(0x01, z80.Registers.C), 0x02);
        _mockBus.Received().Write(To.Word(0x00, z80.Registers.C), 0x01);
        z80.Registers.BC.ShouldBe(0x34);
        z80.Registers.HL.ShouldBe(0x08);
        z80.Registers.F.ShouldBe(Z);
        z80.Clock.TotalTicks.ShouldBe(120);
    }
}