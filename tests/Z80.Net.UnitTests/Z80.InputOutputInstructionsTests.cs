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
}