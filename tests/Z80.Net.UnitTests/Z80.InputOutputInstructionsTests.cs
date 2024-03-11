using NSubstitute;
using Z80.Net.Registers;
using Z80.Net.UnitTests.Extensions;

namespace Z80.Net.UnitTests;

public class Z80InputOutputInstructionsTests
{
    private readonly IBus _mockBus = Substitute.For<IBus>();

    [Fact]
    public void When_IN_A_n_InstructionIsExecuted_DefaultIsReturned()
    {
        var z80 = new CodeBuilder()
            .Flags(None)
            .Code(
                "LD A,0x23",
                "IN (A),0x24")
            .Build();

        z80.Run(7 + 11);

        z80.Registers.A.Should().Be(0xFF);
        z80.StatesCounter.TotalStates.Should().Be(18);
    }

    [Fact]
    public void When_IN_A_n_InstructionIsExecuted_InputIsReturned()
    {
        _mockBus.Read(0x23, 0x24).Returns((byte)0xA4);

        var z80 = new CodeBuilder()
            .Flags(None)
            .Bus(_mockBus)
            .Code(
                "LD A,0x23",
                "IN (A),0x24")
            .Build();

        z80.Run(7 + 11);

        z80.Registers.A.Should().Be(0xA4);
        z80.StatesCounter.TotalStates.Should().Be(18);
    }

    [Theory]
    [InlineData("A", 0xA5, All, S | P | C)]
    [InlineData("B", 0xA5, All, S | P | C)]
    [InlineData("C", 0xA5, All, S | P | C)]
    [InlineData("D", 0xA5, All, S | P | C)]
    [InlineData("E", 0x7E, None, P)]
    [InlineData("H", 0x7F, None, None)]
    [InlineData("L", 0, None, Z | P)]
    public void When_IN_r_C_InstructionIsExecuted_InputIsReturned(
        string register, byte value, Flags flags, Flags expectedFlags)
    {
        _mockBus.Read(0x41, 0xA5).Returns(value);

        var z80 = new CodeBuilder()
            .Flags(flags)
            .Bus(_mockBus)
            .Code(
                "LD BC,0x41A5",
                $"IN {register},(C)")
            .Build();

        z80.Run(10 + 12);

        z80.Registers.ValueOf(register).Should().Be(value);
        z80.Registers.F.Should().Be(expectedFlags);
        z80.StatesCounter.TotalStates.Should().Be(22);
    }

    [Fact]
    public void When_IN_F_C_InstructionIsExecuted_InputIsReturned()
    {
        _mockBus.Read(0x41, 0x49).Returns((byte)0x80);

        var z80 = new CodeBuilder()
            .Flags(None)
            .Bus(_mockBus)
            .Code(
                "LD BC,0x4149",
                "IN F,(C)")
            .Build();

        z80.Run(10 + 12);

        z80.Registers.F.Should().Be(S);
        z80.StatesCounter.TotalStates.Should().Be(22);
    }

    [Fact]
    public void When_INI_InstructionIsExecuted_InputIsReturned()
    {
        _mockBus.Read(0x01, 0x34).Returns((byte)0x76);

        var builder = new CodeBuilder()
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
        memory[9].Should().Be(0x76);
        z80.Registers.BC.Should().Be(0x34);
        z80.Registers.HL.Should().Be(0x0A);
        z80.Registers.F.Should().Be(Z | N | C);
        z80.StatesCounter.TotalStates.Should().Be(36);
    }

    [Fact]
    public void When_INIR_InstructionIsExecuted_InputIsReturned()
    {
        _mockBus.Read(0x05, 0x34).Returns((byte)0x71);
        _mockBus.Read(0x04, 0x34).Returns((byte)0x72);
        _mockBus.Read(0x03, 0x34).Returns((byte)0x73);
        _mockBus.Read(0x02, 0x34).Returns((byte)0x74);
        _mockBus.Read(0x01, 0x34).Returns((byte)0x75);

        var builder = new CodeBuilder()
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
        memory[09].Should().Be(0x71);
        memory[10].Should().Be(0x72);
        memory[11].Should().Be(0x73);
        memory[12].Should().Be(0x74);
        memory[13].Should().Be(0x75);
        z80.Registers.BC.Should().Be(0x34);
        z80.Registers.HL.Should().Be(0x0E);
        z80.Registers.F.Should().Be(Z | N | C);
        z80.StatesCounter.TotalStates.Should().Be(120);
    }

    [Fact]
    public void When_IND_InstructionIsExecuted_InputIsReturned()
    {
        _mockBus.Read(0x01, 0x34).Returns((byte)0x76);

        var builder = new CodeBuilder()
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
        memory[9].Should().Be(0x76);
        z80.Registers.BC.Should().Be(0x34);
        z80.Registers.HL.Should().Be(0x08);
        z80.Registers.F.Should().Be(Z | N | C);
        z80.StatesCounter.TotalStates.Should().Be(36);
    }

    [Fact]
    public void When_INDR_InstructionIsExecuted_InputIsReturned()
    {
        _mockBus.Read(0x05, 0x34).Returns((byte)0x71);
        _mockBus.Read(0x04, 0x34).Returns((byte)0x72);
        _mockBus.Read(0x03, 0x34).Returns((byte)0x73);
        _mockBus.Read(0x02, 0x34).Returns((byte)0x74);
        _mockBus.Read(0x01, 0x34).Returns((byte)0x75);

        var builder = new CodeBuilder()
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
        memory[09].Should().Be(0x75);
        memory[10].Should().Be(0x74);
        memory[11].Should().Be(0x73);
        memory[12].Should().Be(0x72);
        memory[13].Should().Be(0x71);
        z80.Registers.BC.Should().Be(0x34);
        z80.Registers.HL.Should().Be(0x08);
        z80.Registers.F.Should().Be(Z | N | C);
        z80.StatesCounter.TotalStates.Should().Be(120);
    }

    [Fact]
    public void When_OUT_A_n_InstructionIsExecuted_DataBusValueIsWritten()
    {
        var z80 = new CodeBuilder()
            .Flags(None)
            .Bus(_mockBus)
            .Code(
                "LD A,0x23",
                "OUT (0x24),A")
            .Build();

        z80.Run(7 + 11);

        _mockBus.Received().Write(z80.Registers.A, 0x24, z80.Registers.A);
        z80.StatesCounter.TotalStates.Should().Be(18);
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
        var z80 = new CodeBuilder()
            .Bus(_mockBus)
            .Code(
                $"LD {register},0xF1",
                "LD BC,0x41A5",
                $"OUT (C),{register}")
            .Build();

        z80.Run(7 + 10 + 12);

        _mockBus.Received().Write(z80.Registers.B, z80.Registers.C, expectedData);
        z80.StatesCounter.TotalStates.Should().Be(29);
    }

    [Fact]
    public void When_OUT_C_F_InstructionIsExecuted_DataBusZeroIsWritten()
    {
        var z80 = new CodeBuilder()
            .Bus(_mockBus)
            .Code(
                "LD AF,0xFFFF",
                "LD BC,0x41A5",
                "OUT (C),F")
            .Build();

        z80.Run(10 + 10 + 12);

        _mockBus.Received().Write(z80.Registers.B, z80.Registers.C, 0);
        z80.StatesCounter.TotalStates.Should().Be(32);
    }
}