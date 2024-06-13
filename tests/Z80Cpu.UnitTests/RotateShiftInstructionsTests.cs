using OldBit.Z80Cpu.Registers;
using OldBit.Z80Cpu.UnitTests.Extensions;
using OldBit.Z80Cpu.UnitTests.Fixtures;

namespace OldBit.Z80Cpu.UnitTests;

public class Z80RotateShiftInstructionsTests
{
    [Theory]
    [InlineData(0x55, H | N, 0xAA, Y | X)]
    [InlineData(0xAA, H | N, 0x55, C)]
    [InlineData(0x00, H | N, 0x00, None)]
    [InlineData(0xFF, H | N, 0xFF, Y | X | C)]
    public void When_RLCA_InstructionIsExecuted_AccumulatorAndFlagsAreUpdated(
        byte value, Flags flags, byte expectedValue, Flags expectedFlags)
    {
        var z80 = new Z80Builder()
            .Flags(flags)
            .Code(
                $"LD A,{value}",
                "RLCA")
            .Build();

        z80.Run(7 + 4);

        z80.Registers.A.Should().Be(expectedValue);
        z80.Registers.F.Should().Be(expectedFlags);
        z80.States.TotalStates.Should().Be(11);
    }

    [Theory]
    [InlineData(0x55, H | N, 0xAA, Y | X | C)]
    [InlineData(0xAA, H | N, 0x55, None)]
    [InlineData(0x00, H | N, 0x00, None)]
    [InlineData(0xFF, H | N, 0xFF, Y | X | C)]
    public void When_RRCA_InstructionIsExecuted_AccumulatorAndFlagsAreUpdated(
        byte value, Flags flags, byte expectedValue, Flags expectedFlags)
    {
        var z80 = new Z80Builder()
            .Flags(flags)
            .Code(
                $"LD A,{value}",
                "RRCA")
            .Build();

        z80.Run(7 + 4);

        z80.Registers.A.Should().Be(expectedValue);
        z80.Registers.F.Should().Be(expectedFlags);
        z80.States.TotalStates.Should().Be(11);
    }

    [Theory]
    [InlineData(0x80, H | N | C, 0x01, C)]
    [InlineData(0x55, All, 0xAB, S | Z | Y | X | P)]
    [InlineData(0x88, None, 0x10, C)]
    [InlineData(0x10, C, 0x21, Y)]
    public void When_RLA_InstructionIsExecuted_AccumulatorAndFlagsAreUpdated(
        byte value, Flags flags, byte expectedValue, Flags expectedFlags)
    {
        var z80 = new Z80Builder()
            .Flags(flags)
            .Code(
                $"LD A,{value}",
                "RLA")
            .Build();

        z80.Run(7 + 4);

        z80.Registers.A.Should().Be(expectedValue);
        z80.Registers.F.Should().Be(expectedFlags);
        z80.States.TotalStates.Should().Be(11);
    }

    [Theory]
    [InlineData(0x80, H | N | C, 0xC0, None)]
    [InlineData(0x55, All, 0xAA, S | Z | Y | X | P | C)]
    [InlineData(0x89, None, 0x44, C)]
    [InlineData(0x44, C, 0xA2, Y)]
    public void When_RRA_InstructionIsExecuted_AccumulatorAndFlagsAreUpdated(
        byte value, Flags flags, byte expectedValue, Flags expectedFlags)
    {
        var z80 = new Z80Builder()
            .Flags(flags)
            .Code(
                $"LD A,{value}",
                "RRA")
            .Build();

        z80.Run(7 + 4);

        z80.Registers.A.Should().Be(expectedValue);
        z80.Registers.F.Should().Be(expectedFlags);
        z80.States.TotalStates.Should().Be(11);
    }

    [Theory]
    [InlineData(0x55, Z | H | N | C, 0xAA, S | Y | X | P)]
    [InlineData(0xAA, Z | H | N | C, 0x55, P | C)]
    [InlineData(0x00, All, 0x00, Z | P)]
    [InlineData(0x80, C, 0x01, C)]
    public void When_RLC_C_InstructionIsExecuted_RegisterAndFlagsAreUpdated(
        byte value, Flags flags, byte expectedValue, Flags expectedFlags)
    {
        var z80 = new Z80Builder()
            .Flags(flags)
            .Code(
                $"LD C,{value}",
                "RLC C")
            .Build();

        z80.Run(7 + 8);

        z80.Registers.C.Should().Be(expectedValue);
        z80.Registers.F.Should().Be(expectedFlags);
        z80.States.TotalStates.Should().Be(15);
    }

    [Theory]
    [InlineData("A")]
    [InlineData("B")]
    [InlineData("C")]
    [InlineData("D")]
    [InlineData("E")]
    [InlineData("H")]
    [InlineData("L")]
    public void When_RLC_r_InstructionIsExecuted_RegisterAndFlagsAreUpdated(string register)
    {
        var z80 = new Z80Builder()
            .Flags(All)
            .Code(
                $"LD {register},0xAA",
                $"RLC {register}")
            .Build();

        z80.Run(7 + 8);

        z80.Registers.ValueOf(register).Should().Be(0x55);
        z80.Registers.F.Should().Be(P | C);
        z80.States.TotalStates.Should().Be(15);
    }

    [Fact]
    public void When_RLC_HL_InstructionIsExecuted_RegisterAndFlagsAreUpdated()
    {
        var builder = new Z80Builder()
            .Flags(All)
            .Code(
                "LD HL,0x06",
                "RLC (HL)",
                "db 0,1");
        var z80 = builder.Build();

        z80.Run(10 + 15);

        builder.Memory![0x06].Should().Be(0x02);
        z80.Registers.F.Should().Be(None);
        z80.States.TotalStates.Should().Be(25);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_RLC_IXY_InstructionIsExecuted_RegisterAndFlagsAreUpdated(string register)
    {
        var builder = new Z80Builder()
            .Flags(All)
            .Code(
                $"LD {register},4",
                $"RLC ({register}+5)",
                "db 0,1");
        var z80 = builder.Build();

        z80.Run(14 + 23);

        builder.Memory![0x09].Should().Be(0x02);
        z80.Registers.F.Should().Be(None);
        z80.States.TotalStates.Should().Be(37);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_RLC_IXY_r_InstructionIsExecuted_RegisterAndFlagsAreUpdated(string register)
    {
        var builder = new Z80Builder()
            .Flags(All)
            .Code(
                $"LD {register},4",
                $"RLC ({register}+5),L",
                "db 0,1");
        var z80 = builder.Build();

        z80.Run(14 + 23);

        builder.Memory![0x09].Should().Be(0x02);
        z80.Registers.F.Should().Be(None);
        z80.Registers.L.Should().Be(0x02);
        z80.States.TotalStates.Should().Be(37);
    }

    [Theory]
    [InlineData(0x55, Z | H | N | C, 0xAB, S | Y | X)]
    [InlineData(0xAA, Z | H | N | C, 0x55, P | C)]
    [InlineData(0x80, None, 0x00, Z | P | C)]
    [InlineData(0x80, C, 0x01, C)]
    public void When_RL_D_InstructionIsExecuted_RegisterAndFlagsAreUpdated(
        byte value, Flags flags, byte expectedValue, Flags expectedFlags)
    {
        var z80 = new Z80Builder()
            .Flags(flags)
            .Code(
                $"LD D,{value}",
                "RL D")
            .Build();

        z80.Run(7 + 8);

        z80.Registers.D.Should().Be(expectedValue);
        z80.Registers.F.Should().Be(expectedFlags);
        z80.States.TotalStates.Should().Be(15);
    }

    [Theory]
    [InlineData("A")]
    [InlineData("B")]
    [InlineData("C")]
    [InlineData("D")]
    [InlineData("E")]
    [InlineData("H")]
    [InlineData("L")]
    public void When_RL_r_InstructionIsExecuted_RegisterAndFlagsAreUpdated(string register)
    {
        var z80 = new Z80Builder()
            .Flags(All)
            .Code(
                $"LD {register},0xAA",
                $"RL {register}")
            .Build();

        z80.Run(7 + 8);

        z80.Registers.ValueOf(register).Should().Be(0x55);
        z80.Registers.F.Should().Be(P | C);
        z80.States.TotalStates.Should().Be(15);
    }

    [Fact]
    public void When_RL_HL_InstructionIsExecuted_RegisterAndFlagsAreUpdated()
    {
        var builder = new Z80Builder()
            .Flags(None)
            .Code(
                "LD HL,0x06",
                "RL (HL)",
                "db 0,0x81");
        var z80 = builder.Build();

        z80.Run(10 + 15);

        builder.Memory![0x06].Should().Be(0x02);
        z80.Registers.F.Should().Be(C);
        z80.States.TotalStates.Should().Be(25);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_RL_IXY_InstructionIsExecuted_RegisterAndFlagsAreUpdated(string register)
    {
        var builder = new Z80Builder()
            .Flags(All)
            .Code(
                $"LD {register},4",
                $"RL ({register}+5)",
                "db 0,2");
        var z80 = builder.Build();

        z80.Run(14 + 23);

        builder.Memory![0x09].Should().Be(0x05);
        z80.Registers.F.Should().Be(P);
        z80.States.TotalStates.Should().Be(37);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_RL_IXY_r_InstructionIsExecuted_RegisterAndFlagsAreUpdated(string register)
    {
        var builder = new Z80Builder()
            .Flags(All)
            .Code(
                $"LD {register},4",
                $"RL ({register}+5),H",
                "db 0,2");
        var z80 = builder.Build();

        z80.Run(14 + 23);

        builder.Memory![0x09].Should().Be(0x05);
        z80.Registers.F.Should().Be(P);
        z80.Registers.H.Should().Be(0x05);
        z80.States.TotalStates.Should().Be(37);
    }

    [Theory]
    [InlineData(0x55, Z | H | N, 0xAA, S | Y | X | P | C)]
    [InlineData(0xAA, S | Z | H | N | C, 0x55, P)]
    [InlineData(0x00, None, 0x00, Z | P)]
    [InlineData(0x80, C, 0x40, None)]
    public void When_RRC_D_InstructionIsExecuted_RegisterAndFlagsAreUpdated(
        byte value, Flags flags, byte expectedValue, Flags expectedFlags)
    {
        var z80 = new Z80Builder()
            .Flags(flags)
            .Code(
                $"LD D,{value}",
                "RRC D")
            .Build();

        z80.Run(7 + 8);

        z80.Registers.D.Should().Be(expectedValue);
        z80.Registers.F.Should().Be(expectedFlags);
        z80.States.TotalStates.Should().Be(15);
    }

    [Theory]
    [InlineData("A")]
    [InlineData("B")]
    [InlineData("C")]
    [InlineData("D")]
    [InlineData("E")]
    [InlineData("H")]
    [InlineData("L")]
    public void When_RRC_r_InstructionIsExecuted_RegisterAndFlagsAreUpdated(string register)
    {
        var z80 = new Z80Builder()
            .Flags(All)
            .Code(
                $"LD {register},0xAA",
                $"RRC {register}")
            .Build();

        z80.Run(7 + 8);

        z80.Registers.ValueOf(register).Should().Be(0x55);
        z80.Registers.F.Should().Be(P);
        z80.States.TotalStates.Should().Be(15);
    }

    [Fact]
    public void When_RRC_HL_InstructionIsExecuted_RegisterAndFlagsAreUpdated()
    {
        var builder = new Z80Builder()
            .Flags(None)
            .Code(
                "LD HL,0x06",
                "RRC (HL)",
                "db 0,1");
        var z80 = builder.Build();

        z80.Run(10 + 15);

        builder.Memory![0x06].Should().Be(0x80);
        z80.Registers.F.Should().Be(S | C);
        z80.States.TotalStates.Should().Be(25);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_RRC_IXY_InstructionIsExecuted_RegisterAndFlagsAreUpdated(string register)
    {
        var builder = new Z80Builder()
            .Flags(All)
            .Code(
                $"LD {register},4",
                $"RRC ({register}+5)",
                "db 0,0x7F");
        var z80 = builder.Build();

        z80.Run(14 + 23);

        builder.Memory![0x09].Should().Be(0xBF);
        z80.Registers.F.Should().Be(S | Y | X | C);
        z80.States.TotalStates.Should().Be(37);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_RRC_IXY_r_InstructionIsExecuted_RegisterAndFlagsAreUpdated(string register)
    {
        var builder = new Z80Builder()
            .Flags(All)
            .Code(
                $"LD {register},4",
                $"RRC ({register}+5),A",
                "db 0,0x7F");
        var z80 = builder.Build();

        z80.Run(14 + 23);

        builder.Memory![0x09].Should().Be(0xBF);
        z80.Registers.F.Should().Be(S | Y | X | C);
        z80.Registers.A.Should().Be(0xBF);
        z80.States.TotalStates.Should().Be(37);
    }

    [Theory]
    [InlineData(0x55, Z | H | N | C, 0xAA, S | Y | X | P | C)]
    [InlineData(0xAA, Z | H | N | C, 0xD5, S)]
    [InlineData(0x01, None, 0x00, Z | P | C)]
    [InlineData(0x80, C, 0xC0, S | P)]
    public void When_RR_L_InstructionIsExecuted_RegisterAndFlagsAreUpdated(
        byte value, Flags flags, byte expectedValue, Flags expectedFlags)
    {
        var z80 = new Z80Builder()
            .Flags(flags)
            .Code(
                $"LD L,{value}",
                "RR L")
            .Build();

        z80.Run(7 + 8);

        z80.Registers.L.Should().Be(expectedValue);
        z80.Registers.F.Should().Be(expectedFlags);
        z80.States.TotalStates.Should().Be(15);
    }

    [Theory]
    [InlineData("A")]
    [InlineData("B")]
    [InlineData("C")]
    [InlineData("D")]
    [InlineData("E")]
    [InlineData("H")]
    [InlineData("L")]
    public void When_RR_r_InstructionIsExecuted_RegisterAndFlagsAreUpdated(string register)
    {
        var z80 = new Z80Builder()
            .Flags(All)
            .Code(
                $"LD {register},0xAA",
                $"RR {register}")
            .Build();

        z80.Run(7 + 8);

        z80.Registers.ValueOf(register).Should().Be(0xD5);
        z80.Registers.F.Should().Be(S);
        z80.States.TotalStates.Should().Be(15);
    }

    [Fact]
    public void When_RR_HL_InstructionIsExecuted_RegisterAndFlagsAreUpdated()
    {
        var builder = new Z80Builder()
            .Flags(None)
            .Code(
                "LD HL,0x06",
                "RR (HL)",
                "db 0,0x81");
        var z80 = builder.Build();

        z80.Run(10 + 15);

        builder.Memory![0x06].Should().Be(0x40);
        z80.Registers.F.Should().Be(C);
        z80.States.TotalStates.Should().Be(25);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_RR_IXY_InstructionIsExecuted_RegisterAndFlagsAreUpdated(string register)
    {
        var builder = new Z80Builder()
            .Flags(All)
            .Code(
                $"LD {register},4",
                $"RR ({register}+5)",
                "db 0,0x99");
        var z80 = builder.Build();

        z80.Run(14 + 23);

        builder.Memory![0x09].Should().Be(0xCC);
        z80.Registers.F.Should().Be(S | X | P | C);
        z80.States.TotalStates.Should().Be(37);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_RR_IXY_r_InstructionIsExecuted_RegisterAndFlagsAreUpdated(string register)
    {
        var builder = new Z80Builder()
            .Flags(All)
            .Code(
                $"LD {register},4",
                $"RR ({register}+5),D",
                "db 0,0x99");
        var z80 = builder.Build();

        z80.Run(14 + 23);

        builder.Memory![0x09].Should().Be(0xCC);
        z80.Registers.F.Should().Be(S | X | P | C);
        z80.Registers.D.Should().Be(0xCC);
        z80.States.TotalStates.Should().Be(37);
    }

    [Theory]
    [InlineData(0x55, Z | H | N | C, 0xAA, S | Y | X | P)]
    [InlineData(0xAA, Z | H | N, 0x54, C)]
    [InlineData(0x99, All, 0x32, Y | C)]
    [InlineData(0xFF, C, 0xFE, S | Y | X | C)]
    public void When_SLA_B_InstructionIsExecuted_RegisterAndFlagsAreUpdated(
        byte value, Flags flags, byte expectedValue, Flags expectedFlags)
    {
        var z80 = new Z80Builder()
            .Flags(flags)
            .Code(
                $"LD B,{value}",
                "SLA B")
            .Build();

        z80.Run(7 + 8);

        z80.Registers.B.Should().Be(expectedValue);
        z80.Registers.F.Should().Be(expectedFlags);
        z80.States.TotalStates.Should().Be(15);
    }

    [Theory]
    [InlineData("A")]
    [InlineData("B")]
    [InlineData("C")]
    [InlineData("D")]
    [InlineData("E")]
    [InlineData("H")]
    [InlineData("L")]
    public void When_SLA_r_InstructionIsExecuted_RegisterAndFlagsAreUpdated(string register)
    {
        var z80 = new Z80Builder()
            .Flags(All)
            .Code(
                $"LD {register},0xAA",
                $"SLA {register}")
            .Build();

        z80.Run(7 + 8);

        z80.Registers.ValueOf(register).Should().Be(0x54);
        z80.Registers.F.Should().Be(C);
        z80.States.TotalStates.Should().Be(15);
    }

    [Fact]
    public void When_SLA_HL_InstructionIsExecuted_RegisterAndFlagsAreUpdated()
    {
        var builder = new Z80Builder()
            .Flags(None)
            .Code(
                "LD HL,0x06",
                "SLA (HL)",
                "db 0,0x81");
        var z80 = builder.Build();

        z80.Run(10 + 15);

        builder.Memory![0x06].Should().Be(0x02);
        z80.Registers.F.Should().Be(C);
        z80.States.TotalStates.Should().Be(25);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_SLA_IXY_InstructionIsExecuted_RegisterAndFlagsAreUpdated(string register)
    {
        var builder = new Z80Builder()
            .Flags(All)
            .Code(
                $"LD {register},4",
                $"SLA ({register}+5)",
                "db 0,0x99");
        var z80 = builder.Build();

        z80.Run(14 + 23);

        builder.Memory![0x09].Should().Be(0x32);
        z80.Registers.F.Should().Be(Y | C);
        z80.States.TotalStates.Should().Be(37);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_SLA_IXY_r_InstructionIsExecuted_RegisterAndFlagsAreUpdated(string register)
    {
        var builder = new Z80Builder()
            .Flags(All)
            .Code(
                $"LD {register},4",
                $"SLA ({register}+5),C",
                "db 0,0x99");
        var z80 = builder.Build();

        z80.Run(14 + 23);

        builder.Memory![0x09].Should().Be(0x32);
        z80.Registers.F.Should().Be(Y | C);
        z80.Registers.C.Should().Be(0x32);
        z80.States.TotalStates.Should().Be(37);
    }

    [Theory]
    [InlineData(0x85, Z | H | N, 0xC2, S | C)]
    [InlineData(0x7F, None, 0x3F, Y | X | P | C)]
    [InlineData(0x01, All, 0, Z | P | C)]
    public void When_SRA_D_InstructionIsExecuted_RegisterAndFlagsAreUpdated(
        byte value, Flags flags, byte expectedValue, Flags expectedFlags)
    {
        var z80 = new Z80Builder()
            .Flags(flags)
            .Code(
                $"LD D,{value}",
                "SRA D")
            .Build();

        z80.Run(7 + 8);

        z80.Registers.D.Should().Be(expectedValue);
        z80.Registers.F.Should().Be(expectedFlags);
        z80.States.TotalStates.Should().Be(15);
    }

    [Theory]
    [InlineData("A")]
    [InlineData("B")]
    [InlineData("C")]
    [InlineData("D")]
    [InlineData("E")]
    [InlineData("H")]
    [InlineData("L")]
    public void When_SRA_r_InstructionIsExecuted_RegisterAndFlagsAreUpdated(string register)
    {
        var z80 = new Z80Builder()
            .Flags(All)
            .Code(
                $"LD {register},0xAA",
                $"SRA {register}")
            .Build();

        z80.Run(7 + 8);

        z80.Registers.ValueOf(register).Should().Be(0xD5);
        z80.Registers.F.Should().Be(S);
        z80.States.TotalStates.Should().Be(15);
    }

    [Fact]
    public void When_SRA_HL_InstructionIsExecuted_RegisterAndFlagsAreUpdated()
    {
        var builder = new Z80Builder()
            .Flags(None)
            .Code(
                "LD HL,0x06",
                "SRA (HL)",
                "db 0,0x81");
        var z80 = builder.Build();

        z80.Run(10 + 15);

        builder.Memory![0x06].Should().Be(0xC0);
        z80.Registers.F.Should().Be(S | P | C);
        z80.States.TotalStates.Should().Be(25);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_SRA_IXY_InstructionIsExecuted_RegisterAndFlagsAreUpdated(string register)
    {
        var builder = new Z80Builder()
            .Flags(All)
            .Code(
                $"LD {register},4",
                $"SRA ({register}+5)",
                "db 0,0x99");
        var z80 = builder.Build();

        z80.Run(14 + 23);

        builder.Memory![0x09].Should().Be(0xCC);
        z80.Registers.F.Should().Be(S | X | P | C);
        z80.States.TotalStates.Should().Be(37);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_SRA_IXY_r_InstructionIsExecuted_RegisterAndFlagsAreUpdated(string register)
    {
        var builder = new Z80Builder()
            .Flags(All)
            .Code(
                $"LD {register},4",
                $"SRA ({register}+5),E",
                "db 0,0x99");
        var z80 = builder.Build();

        z80.Run(14 + 23);

        builder.Memory![0x09].Should().Be(0xCC);
        z80.Registers.F.Should().Be(S | X | P | C);
        z80.Registers.E.Should().Be(0xCC);
        z80.States.TotalStates.Should().Be(37);
    }

    [Theory]
    [InlineData(0x85, S | H | N, 0x42, P | C)]
    [InlineData(0xFE, All, 0x7F, Y | X)]
    [InlineData(0x01, None, 0, Z | P | C)]
    public void When_SRL_H_InstructionIsExecuted_RegisterAndFlagsAreUpdated(
        byte value, Flags flags, byte expectedValue, Flags expectedFlags)
    {
        var z80 = new Z80Builder()
            .Flags(flags)
            .Code(
                $"LD H,{value}",
                "SRL H")
            .Build();

        z80.Run(7 + 8);

        z80.Registers.H.Should().Be(expectedValue);
        z80.Registers.F.Should().Be(expectedFlags);
        z80.States.TotalStates.Should().Be(15);
    }

    [Theory]
    [InlineData("A")]
    [InlineData("B")]
    [InlineData("C")]
    [InlineData("D")]
    [InlineData("E")]
    [InlineData("H")]
    [InlineData("L")]
    public void When_SRL_r_InstructionIsExecuted_RegisterAndFlagsAreUpdated(string register)
    {
        var z80 = new Z80Builder()
            .Flags(All)
            .Code(
                $"LD {register},0xAA",
                $"SRL {register}")
            .Build();

        z80.Run(7 + 8);

        z80.Registers.ValueOf(register).Should().Be(0x55);
        z80.Registers.F.Should().Be(P);
        z80.States.TotalStates.Should().Be(15);
    }

    [Fact]
    public void When_SRL_HL_InstructionIsExecuted_RegisterAndFlagsAreUpdated()
    {
        var builder = new Z80Builder()
            .Flags(None)
            .Code(
                "LD HL,0x06",
                "SRL (HL)",
                "db 0,0x81");
        var z80 = builder.Build();

        z80.Run(10 + 15);

        builder.Memory![0x06].Should().Be(0x40);
        z80.Registers.F.Should().Be(C);
        z80.States.TotalStates.Should().Be(25);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_SRL_IXY_InstructionIsExecuted_RegisterAndFlagsAreUpdated(string register)
    {
        var builder = new Z80Builder()
            .Flags(All)
            .Code(
                $"LD {register},4",
                $"SRL ({register}+5)",
                "db 0,0x99");
        var z80 = builder.Build();

        z80.Run(14 + 23);

        builder.Memory![0x09].Should().Be(0x4C);
        z80.Registers.F.Should().Be(X | C);
        z80.States.TotalStates.Should().Be(37);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_SRL_IXY_r_InstructionIsExecuted_RegisterAndFlagsAreUpdated(string register)
    {
        var builder = new Z80Builder()
            .Flags(All)
            .Code(
                $"LD {register},4",
                $"SRL ({register}+5),E",
                "db 0,0x99");
        var z80 = builder.Build();

        z80.Run(14 + 23);

        builder.Memory![0x09].Should().Be(0x4C);
        z80.Registers.F.Should().Be(X | C);
        z80.Registers.E.Should().Be(0x4C);
        z80.States.TotalStates.Should().Be(37);
    }

    [Theory]
    [InlineData(0x55, Z | H | N | C, 0xAB, S | Y | X)]
    [InlineData(0xAA, Z | H | N, 0x55, P | C)]
    [InlineData(0x99, All, 0x33, Y | P | C)]
    [InlineData(0xFF, C, 0xFF, S | Y | X | P | C)]
    public void When_SLL_H_InstructionIsExecuted_RegisterAndFlagsAreUpdated(
        byte value, Flags flags, byte expectedValue, Flags expectedFlags)
    {
        var z80 = new Z80Builder()
            .Flags(flags)
            .Code(
                $"LD H,{value}",
                "SLL H")
            .Build();

        z80.Run(7 + 8);

        z80.Registers.H.Should().Be(expectedValue);
        z80.Registers.F.Should().Be(expectedFlags);
        z80.States.TotalStates.Should().Be(15);
    }

    [Theory]
    [InlineData("A")]
    [InlineData("B")]
    [InlineData("C")]
    [InlineData("D")]
    [InlineData("E")]
    [InlineData("H")]
    [InlineData("L")]
    public void When_SLL_r_InstructionIsExecuted_RegisterAndFlagsAreUpdated(string register)
    {
        var z80 = new Z80Builder()
            .Flags(All)
            .Code(
                $"LD {register},0xAA",
                $"SLL {register}")
            .Build();

        z80.Run(7 + 8);

        z80.Registers.ValueOf(register).Should().Be(0x55);
        z80.Registers.F.Should().Be(P | C);
        z80.States.TotalStates.Should().Be(15);
    }

    [Fact]
    public void When_SLL_HL_InstructionIsExecuted_RegisterAndFlagsAreUpdated()
    {
        var builder = new Z80Builder()
            .Flags(None)
            .Code(
                "LD HL,0x06",
                "SLL (HL)",
                "db 0,0x81");
        var z80 = builder.Build();

        z80.Run(10 + 15);

        builder.Memory![0x06].Should().Be(0x03);
        z80.Registers.F.Should().Be(P | C);
        z80.States.TotalStates.Should().Be(25);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_SLL_IXY_InstructionIsExecuted_RegisterAndFlagsAreUpdated(string register)
    {
        var builder = new Z80Builder()
            .Flags(All)
            .Code(
                $"LD {register},4",
                $"SLL ({register}+5)",
                "db 0,0x99");
        var z80 = builder.Build();

        z80.Run(14 + 23);

        builder.Memory![0x09].Should().Be(0x33);
        z80.Registers.F.Should().Be(Y | P | C);
        z80.States.TotalStates.Should().Be(37);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_SLL_IXY_r_InstructionIsExecuted_RegisterAndFlagsAreUpdated(string register)
    {
        var builder = new Z80Builder()
            .Flags(All)
            .Code(
                $"LD {register},4",
                $"SLL ({register}+5),B",
                "db 0,0x99");
        var z80 = builder.Build();

        z80.Run(14 + 23);

        builder.Memory![0x09].Should().Be(0x33);
        z80.Registers.F.Should().Be(Y | P | C);
        z80.Registers.B.Should().Be(0x33);
        z80.States.TotalStates.Should().Be(37);
    }

    [Theory]
    [InlineData(0x7A, 0x31, All, 0x73, 0x1A, Y | C)]
    [InlineData(0x0F, 0x0A, All, 0x00, 0xAF, Z | P | C)]
    [InlineData(0x55, 0xAA, None, 0x5A, 0xA5, X | P)]
    public void When_RLD_InstructionIsExecuted_RegisterAndFlagsAreUpdated(
        byte a, byte mem, Flags flags, byte expectedA, byte expectedMem, Flags expectedFlags)
    {
        var builder = new Z80Builder()
            .Flags(flags)
            .Code(
                $"LD A,{a}",
                "LD HL,8",
                "RLD",
                $"db 0,{mem}");
        var z80 = builder.Build();

        z80.Run(7 + 10 + 18);

        z80.Registers.A.Should().Be(expectedA);
        builder.Memory![0x08].Should().Be(expectedMem);
        z80.Registers.F.Should().Be(expectedFlags);
        z80.States.TotalStates.Should().Be(35);
    }

    [Theory]
    [InlineData(0x84, 0x20, All, 0x80, 0x42, S | C)]
    [InlineData(0x03, 0x60, All, 0x00, 0x36, Z | P | C)]
    [InlineData(0x55, 0xAA, None, 0x5A, 0x5A, X | P)]
    public void When_RRD_InstructionIsExecuted_RegisterAndFlagsAreUpdated(
        byte a, byte mem, Flags flags, byte expectedA, byte expectedMem, Flags expectedFlags)
    {
        var builder = new Z80Builder()
            .Flags(flags)
            .Code(
                $"LD A,{a}",
                "LD HL,8",
                "RRD",
                $"db 0,{mem}");
        var z80 = builder.Build();

        z80.Run(7 + 10 + 18);

        z80.Registers.A.Should().Be(expectedA);
        builder.Memory![0x08].Should().Be(expectedMem);
        z80.Registers.F.Should().Be(expectedFlags);
        z80.States.TotalStates.Should().Be(35);
    }
}
