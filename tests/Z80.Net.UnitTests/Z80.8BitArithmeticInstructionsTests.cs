using Z80.Net.Registers;

namespace Z80.Net.UnitTests;

public class Z808BitArithmeticInstructionsTests
{
    [Fact]
    public void When_ADD_A_A_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var z80 = new Z80Builder()
            .Flags(All ^ Z)
            .Code(
                "LD A,0x00",
                "ADD A,A")
            .Build();

        z80.Run(7 + 4);

        z80.Registers.A.Should().Be(0x00);
        z80.Registers.F.Should().Be(Z);
        z80.States.TotalStates.Should().Be(11);
    }


    [Theory]
    [InlineData(0xFF, 0x01, All ^ Z, 0x00, Z | H | C)]
    [InlineData(0x70, 0x70, All, 0xE0, S | Y | P)]
    [InlineData(0xF0, 0xB0, All, 0xA0, S | Y | C)]
    [InlineData(0x8F, 0x81, All, 0x10, H | P | C)]
    [InlineData(0x81, 0x8F, All, 0x10, H | P | C)]
    public void When_ADD_A_r_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet(
        byte a, byte b, Flags flags, byte expectedResult, Flags expectedFlags)
    {
        var z80 = new Z80Builder()
            .Flags(flags)
            .Code(
                $"LD A,{a}",
                $"LD B,{b}",
                "ADD A,B")
            .Build();

        z80.Run(7 + 7 + 4);

        z80.Registers.A.Should().Be(expectedResult);
        z80.Registers.F.Should().Be(expectedFlags);
        z80.States.TotalStates.Should().Be(18);
    }

    [Fact]
    public void When_ADD_A_n_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var z80 = new Z80Builder()
            .Flags(All)
            .Code(
                "LD A,0x10",
                "ADD A,0x20")
            .Build();

        z80.Run(7 + 7);

        z80.Registers.A.Should().Be(0x30);
        z80.Registers.F.Should().Be(Y);
        z80.States.TotalStates.Should().Be(14);
    }

    [Fact]
    public void When_ADD_A_HL_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var z80 = new Z80Builder()
            .Flags(All ^ Z)
            .Code(
                "LD A,0xFF",
                "LD H,0x00",
                "LD L,0x08",
                "ADD A,(HL)",
                "NOP",
                "db 0x01")
            .Build();

        z80.Run(7 + 7 + 7 + 7);

        z80.Registers.A.Should().Be(0x00);
        z80.Registers.F.Should().Be(Z | H | C);
        z80.States.TotalStates.Should().Be(28);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_ADD_A_IXY_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet(string register)
    {
        // IX or IY
        var z80 = new Z80Builder()
            .Flags(All ^ Z)
            .Code(
                "LD A,0x22",
                $"LD {register},0x04",
                $"ADD A,({register}+0x06)",
                "NOP",
                "db 0x13")
            .Build();

        z80.Run(7 + 14 + 19);

        z80.Registers.A.Should().Be(0x35);
        z80.Registers.F.Should().Be(Y);
        z80.States.TotalStates.Should().Be(40);

        // IXL or IYL
        z80 = new Z80Builder()
            .Flags(All ^ Z)
            .Code(
                "LD A,0x22",
                $"LD {register},0x1213",
                $"ADD A,{register}L")
            .Build();

        z80.Run(7 + 14 + 8);

        z80.Registers.A.Should().Be(0x35);
        z80.Registers.F.Should().Be(Y);
        z80.States.TotalStates.Should().Be(29);

        // IXH or IYH
        z80 = new Z80Builder()
            .Flags(All ^ Z)
            .Code(
                "LD A,0x22",
                $"LD {register},0x1213",
                $"ADD A,{register}H")
            .Build();

        z80.Run(7 + 14 + 8);

        z80.Registers.A.Should().Be(0x34);
        z80.Registers.F.Should().Be(Y);
        z80.States.TotalStates.Should().Be(29);
    }

    [Fact]
    public void When_ADC_A_A_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var z80 = new Z80Builder()
            .Flags(C)
            .Code(
                "LD A,0x7F",
                "ADC A,A")
            .Build();

        z80.Run(7 + 4);

        z80.Registers.A.Should().Be(0xFF);
        z80.Registers.F.Should().Be(P | X | H | Y | S);
        z80.States.TotalStates.Should().Be(11);
    }

    [Theory]
    [InlineData(0x00, 0xFF, N | C, 0x00, Z | H | C)]
    [InlineData(0x0F, 0x00, C, 0x10, H)]
    [InlineData(0x0F, 0x69, C, 0x79, Y | H | X)]
    [InlineData(0x0E, 0x01, C, 0x10, H)]
    [InlineData(0x0E, 0x01, None, 0x0F, X)]
    public void When_ADC_A_r_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet(
        byte a, byte b, Flags flags, byte expectedResult, Flags expectedFlags)
    {
        var z80 = new Z80Builder()
            .Flags(flags)
            .Code(
                $"LD A,{a}",
                $"LD B,{b}",
                "ADC A,B")
            .Build();

        z80.Run(7 + 7 + 4);

        z80.Registers.A.Should().Be(expectedResult);
        z80.Registers.F.Should().Be(expectedFlags);
        z80.States.TotalStates.Should().Be(18);
    }

    [Fact]
    public void When_ADC_A_n_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var z80 = new Z80Builder()
            .Flags(C)
            .Code(
                "LD A,0x20",
                "ADC A,0x20")
            .Build();

        z80.Run(7 + 7);

        z80.Registers.A.Should().Be(0x41);
        z80.Registers.F.Should().Be(None);
        z80.States.TotalStates.Should().Be(14);
    }

    [Fact]
    public void When_ADC_A_HL_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var z80 = new Z80Builder()
            .Flags(C)
            .Code(
                "LD A,0x0F",
                "LD H,0x00",
                "LD L,0x08",
                "ADC A,(HL)",
                "NOP",
                "db 0x70")
            .Build();

        z80.Run(7 + 7 + 7 + 7);

        z80.Registers.A.Should().Be(0x80);
        z80.Registers.F.Should().Be(S | H | P);
        z80.States.TotalStates.Should().Be(28);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_ADC_A_IXY_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet(string register)
    {
        // IX or IY
        var z80 = new Z80Builder()
            .Flags(C)
            .Code(
                "LD A,0x22",
                $"LD {register},0x06",
                $"ADC A,({register}+4)",
                "NOP",
                "db 0x13")
            .Build();

        z80.Run(7 + 14 + 19);

        z80.Registers.A.Should().Be(0x36);
        z80.Registers.F.Should().Be(Y);
        z80.States.TotalStates.Should().Be(40);

        // IXL or IYL
        z80 = new Z80Builder()
            .Flags(C)
            .Code(
                "LD A,0x22",
                $"LD {register},0x1213",
                $"ADC A,{register}L",
                "NOP",
                "db 0x70")
            .Build();

        z80.Run(7 + 14 + 8);

        z80.Registers.A.Should().Be(0x36);
        z80.Registers.F.Should().Be(Y);
        z80.States.TotalStates.Should().Be(29);

        // IXH or IYH
        z80 = new Z80Builder()
            .Flags(C)
            .Code(
                "LD A,0x22",
                $"LD {register},0x1213",
                $"ADC A,{register}H",
                "NOP",
                "db 0x70")
            .Build();

        z80.Run(7 + 14 + 8);

        z80.Registers.A.Should().Be(0x35);
        z80.Registers.F.Should().Be(Y);
        z80.States.TotalStates.Should().Be(29);
    }

    [Fact]
    public void When_SUB_r_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD A,0x20",
                "SUB A")
            .Build();

        z80.Run(7 + 4);

        z80.Registers.A.Should().Be(0x00);
        z80.Registers.F.Should().Be(Z | N);
        z80.States.TotalStates.Should().Be(11);

        z80 = new Z80Builder()
            .Flags(Z)
            .Code(
                "LD A,0x90",
                "LD H,0x20",
                "SUB H")
            .Build();

        z80.Run(7 + 7 + 4);

        z80.Registers.A.Should().Be(0x70);
        z80.Registers.F.Should().Be(Y | P | N);
        z80.States.TotalStates.Should().Be(18);
    }

    [Fact]
    public void When_SUB_n_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var z80 = new Z80Builder()
            .Flags(Z)
            .Code(
                "LD A,0x00",
                "SUB 1")
            .Build();

        z80.Run(7 + 7);

        z80.Registers.A.Should().Be(0xFF);
        z80.Registers.F.Should().Be(S | Y | H | X | N | C);
        z80.States.TotalStates.Should().Be(14);
    }

    [Fact]
    public void When_SUB_HL_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD A,0x7F",
                "LD L,0x08",
                "LD H,0x00",
                "SUB (HL)",
                "NOP",
                "db 0x80")
            .Build();

        z80.Run(7 + 7 + 7 + 7);

        z80.Registers.A.Should().Be(0xFF);
        z80.Registers.F.Should().Be(S | Y | X | P | N | C);
        z80.States.TotalStates.Should().Be(28);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_SUB_IXY_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet(string register)
    {
        // IX or IY
        var z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD A,0x12",
                $"LD {register},4",
                $"SUB ({register}+6)",
                "NOP",
                "db 0x02")
            .Build();

        z80.Run(7 + 14 + 19);

        z80.Registers.A.Should().Be(0x10);
        z80.Registers.F.Should().Be(N);
        z80.States.TotalStates.Should().Be(40);

        // IXL or IYL
        z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD A,0x12",
                $"LD {register},0x1202",
                $"SUB {register}L")
            .Build();

        z80.Run(7 + 14 + 8);

        z80.Registers.A.Should().Be(0x10);
        z80.Registers.F.Should().Be(N);
        z80.States.TotalStates.Should().Be(29);

        // IXH or IYH
        z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD A,0x12",
                $"LD {register},0x0212",
                $"SUB {register}H")
            .Build();

        z80.Run(7 + 14 + 8);

        z80.Registers.A.Should().Be(0x10);
        z80.Registers.F.Should().Be(N);
        z80.States.TotalStates.Should().Be(29);
    }

    [Fact]
    public void When_SBC_A_r_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var z80 = new Z80Builder()
            .Flags(C)
            .Code(
                "LD A,0x01",
                "LD B,0x01",
                "SBC A,B")
            .Build();

        z80.Run(7 + 7 + 4);

        z80.Registers.A.Should().Be(0xFF);
        z80.Registers.F.Should().Be(S | Y | H | X | N | C);
        z80.States.TotalStates.Should().Be(18);

        z80 = new Z80Builder()
            .Flags(C)
            .Code(
                "LD A,0x7F",
                "LD L,0x80",
                "SBC A,L")
            .Build();

        z80.Run(7 + 7 + 4);

        z80.Registers.A.Should().Be(0xFE);
        z80.Registers.F.Should().Be(S | Y | X | P | N | C);
        z80.States.TotalStates.Should().Be(18);

        z80 = new Z80Builder()
            .Flags(C)
            .Code(
                "LD A,0xFF",
                "SBC A,A")
            .Build();

        z80.Run(7 + 4);

        z80.Registers.A.Should().Be(0xFF);
        z80.Registers.F.Should().Be(S | Y | H | X | N | C);
        z80.States.TotalStates.Should().Be(11);
    }

    [Fact]
    public void When_SBC_A_n_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var z80 = new Z80Builder()
            .Flags(C)
            .Code(
                "LD A,0x02",
                "SBC A,0x01")
            .Build();

        z80.Run(7 + 7);

        z80.Registers.A.Should().Be(0x00);
        z80.Registers.F.Should().Be(Z | N);
        z80.States.TotalStates.Should().Be(14);
    }

    [Fact]
    public void When_SBC_A_HL_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var z80 = new Z80Builder()
            .Flags(C)
            .Code(
                "LD A,0x81",
                "LD H,0x00",
                "LD L,0x08",
                "SBC A,(HL)",
                "NOP",
                "db 0x01")
            .Build();

        z80.Run(7 + 7 + 7 + 7);

        z80.Registers.A.Should().Be(0x7F);
        z80.Registers.F.Should().Be(Y | H | X | P | N);
        z80.States.TotalStates.Should().Be(28);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_SBC_A_IXY_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet(string register)
    {
        var z80 = new Z80Builder()
            .Flags(C)
            .Code(
                "LD A,0x12",
                $"LD {register},0x02",
                $"SBC A,({register}+8)",
                "NOP",
                "db 0x01")
            .Build();

        z80.Run(7 + 14 + 19);

        z80.Registers.A.Should().Be(0x10);
        z80.Registers.F.Should().Be(N);
        z80.States.TotalStates.Should().Be(40);

        // IXL or IYL
        z80 = new Z80Builder()
            .Flags(C)
            .Code(
                "LD A,0x12",
                $"LD {register},0x0002",
                $"SBC A,{register}L")
            .Build();

        z80.Run(7 + 14 + 8);

        z80.Registers.A.Should().Be(0x0F);
        z80.Registers.F.Should().Be(H | X | N);
        z80.States.TotalStates.Should().Be(29);

        // IXH or IYH
        z80 = new Z80Builder()
            .Flags(C)
            .Code(
                "LD A,0x12",
                $"LD {register},0x0200",
                $"SBC A,{register}H")
            .Build();

        z80.Run(7 + 14 + 8);

        z80.Registers.A.Should().Be(0x0F);
        z80.Registers.F.Should().Be(H | X | N);
        z80.States.TotalStates.Should().Be(29);
    }

    [Fact]
    public void When_AND_r_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var z80 = new Z80Builder()
            .Flags(N | C)
            .Code(
                "LD A,0x0F",
                "LD B,0xF0",
                "AND B")
            .Build();

        z80.Run(7 + 7 + 4);

        z80.Registers.A.Should().Be(0x00);
        z80.Registers.F.Should().Be(Z | H | P);
        z80.States.TotalStates.Should().Be(18);
    }

    [Fact]
    public void When_AND_n_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD A,0x8F",
                "AND 0xF3")
            .Build();

        z80.Run(7 + 7);

        z80.Registers.A.Should().Be(0x83);
        z80.Registers.F.Should().Be(S | H);
        z80.States.TotalStates.Should().Be(14);
    }

    [Fact]
    public void When_AND_HL_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD A,0xFF",
                "LD H,0x00",
                "LD L,0x08",
                "AND (HL)",
                "NOP",
                "db 0x81")
            .Build();

        z80.Run(7 + 7 + 7 + 7);

        z80.Registers.A.Should().Be(0x81);
        z80.Registers.F.Should().Be(S | H | P);
        z80.States.TotalStates.Should().Be(28);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_AND_A_IXY_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet(string register)
    {
        // IX or IY
        var z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD A,0x88",
                $"LD {register},0x01",
                $"AND ({register}+0x09)",
                "NOP",
                "db 0x08")
            .Build();

        z80.Run(7 + 14 + 19);

        z80.Registers.A.Should().Be(0x08);
        z80.Registers.F.Should().Be(H | X);
        z80.States.TotalStates.Should().Be(40);

        // IXL or IYL
        z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD A,0x01",
                $"LD {register},0x0003",
                $"AND {register}L")
            .Build();

        z80.Run(7 + 14 + 8);

        z80.Registers.A.Should().Be(0x01);
        z80.Registers.F.Should().Be(H);
        z80.States.TotalStates.Should().Be(29);

        // IXH or IYH
        z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD A,0x01",
                $"LD {register},0x0300",
                $"AND {register}H")
            .Build();

        z80.Run(7 + 14 + 8);

        z80.Registers.A.Should().Be(0x01);
        z80.Registers.F.Should().Be(H);
        z80.States.TotalStates.Should().Be(29);
    }

    [Fact]
    public void When_OR_r_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var z80 = new Z80Builder()
            .Flags(S | H | N | C)
            .Code(
                "LD A,0x00",
                "LD B,0x00",
                "OR B")
            .Build();

        z80.Run(7 + 7 + 4);

        z80.Registers.A.Should().Be(0x00);
        z80.Registers.F.Should().Be(Z | P);
        z80.States.TotalStates.Should().Be(18);
    }

    [Fact]
    public void When_OR_n_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var z80 = new Z80Builder()
            .Flags(S | H | N | C)
            .Code(
                "LD A,0x11",
                "OR 0x20")
            .Build();

        z80.Run(7 + 7);

        z80.Registers.A.Should().Be(0x31);
        z80.Registers.F.Should().Be(Y);
        z80.States.TotalStates.Should().Be(14);
    }

    [Fact]
    public void When_OR_HL_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var z80 = new Z80Builder()
            .Flags(S | H | N | C)
            .Code(
                "LD A,0x8A",
                "LD H,0x00",
                "LD L,0x08",
                "OR (HL)",
                "NOP",
                "db 0x85")
            .Build();

        z80.Run(7 + 7 + 7 + 7);

        z80.Registers.A.Should().Be(0x8F);
        z80.Registers.F.Should().Be(S | X);
        z80.States.TotalStates.Should().Be(28);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_OR_A_IXY_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet(string register)
    {
        // IX or IY
        var z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD A,0x80",
                $"LD {register},0x01",
                $"OR ({register}+0x09)",
                "NOP",
                "db 0x08")
            .Build();

        z80.Run(7 + 14 + 19);

        z80.Registers.A.Should().Be(0x88);
        z80.Registers.F.Should().Be(S | X | P);
        z80.States.TotalStates.Should().Be(40);

        // IXL or IYL
        z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD A,0x01",
                $"LD {register},0x0012",
                $"OR {register}L")
            .Build();

        z80.Run(7 + 14 + 8);

        z80.Registers.A.Should().Be(0x13);
        z80.Registers.F.Should().Be(None);
        z80.States.TotalStates.Should().Be(29);

        // IXH or IYH
        z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD A,0x01",
                $"LD {register},0x1200",
                $"OR {register}H")
            .Build();

        z80.Run(7 + 14 + 8);

        z80.Registers.A.Should().Be(0x13);
        z80.Registers.F.Should().Be(None);
        z80.States.TotalStates.Should().Be(29);
    }

    [Fact]
    public void When_XOR_r_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var z80 = new Z80Builder()
            .Flags(N | H | C)
            .Code(
                "LD A,0x1F",
                "LD B,0x1F",
                "XOR B")
            .Build();

        z80.Run(7 + 7 + 4);

        z80.Registers.A.Should().Be(0x00);
        z80.Registers.F.Should().Be(Z | P);
        z80.States.TotalStates.Should().Be(18);
    }

    [Fact]
    public void When_XOR_n_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD A,0x1F",
                "XOR 0x0F")
            .Build();

        z80.Run(7 + 7);

        z80.Registers.A.Should().Be(0x10);
        z80.Registers.F.Should().Be(None);
        z80.States.TotalStates.Should().Be(14);
    }

    [Fact]
    public void When_XOR_HL_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD A,0x1F",
                "LD H,0x00",
                "LD L,0x08",
                "XOR (HL)",
                "NOP",
                "db 0x8F")
            .Build();

        z80.Run(7 + 7 + 7 + 7);

        z80.Registers.A.Should().Be(0x90);
        z80.Registers.F.Should().Be(S | P);
        z80.States.TotalStates.Should().Be(28);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_XOR_A_IXY_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet(string register)
    {
        // IX or IY
        var z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD A,0x88",
                $"LD {register},0x01",
                $"XOR ({register}+9)",
                "NOP",
                "db 0x08")
            .Build();

        z80.Run(7 + 14 + 19);

        z80.Registers.A.Should().Be(0x80);
        z80.Registers.F.Should().Be(S);
        z80.States.TotalStates.Should().Be(40);

        // IXL or IYL
        z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD A,0x01",
                $"LD {register},0x0003",
                $"XOR {register}L")
            .Build();

        z80.Run(7 + 14 + 8);

        z80.Registers.A.Should().Be(0x02);
        z80.Registers.F.Should().Be(None);
        z80.States.TotalStates.Should().Be(29);

        // IXH or IYH
        z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD A,0x01",
                $"LD {register},0x0300",
                $"XOR {register}H")
            .Build();

        z80.Run(7 + 14 + 8);

        z80.Registers.A.Should().Be(0x02);
        z80.Registers.F.Should().Be(None);
        z80.States.TotalStates.Should().Be(29);
    }

    [Fact]
    public void When_CP_r_InstructionIsExecuted_FlagsAreSet()
    {
        var z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD A,0x20",
                "CP A")
            .Build();

        z80.Run(7 + 4);

        z80.Registers.A.Should().Be(0x20);
        z80.Registers.F.Should().Be(Z | Y | N);
        z80.States.TotalStates.Should().Be(11);

        z80 = new Z80Builder()
            .Flags(Z)
            .Code(
                "LD A,0x00",
                "LD B,0x01",
                "CP B")
            .Build();

        z80.Run(7 + 7 + 4);

        z80.Registers.A.Should().Be(0x00);
        z80.Registers.F.Should().Be(S | H | N | C);
        z80.States.TotalStates.Should().Be(18);
    }

    [Fact]
    public void When_CP_n_InstructionIsExecuted_FlagsAreSet()
    {
        var z80 = new Z80Builder()
            .Flags(Z)
            .Code(
                "LD A,0x90",
                "CP 0x20")
            .Build();

        z80.Run(7 + 7);

        z80.Registers.A.Should().Be(0x90);
        z80.Registers.F.Should().Be(Y | P | N);
        z80.States.TotalStates.Should().Be(14);
    }

    [Fact]
    public void When_CP_HL_InstructionIsExecuted_FlagsAreSet()
    {
        var z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD A,0x7F",
                "LD H,0x00",
                "LD L,0x08",
                "CP (HL)",
                "NOP",
                "db 0x80")
            .Build();

        z80.Run(7 + 7 + 7 + 7);

        z80.Registers.A.Should().Be(0x7F);
        z80.Registers.F.Should().Be(S | P | N | C);
        z80.States.TotalStates.Should().Be(28);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_CP_A_IndexRegister_InstructionIsExecuted_FlagsAreSet(string register)
    {
        // IX or IY
        var z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD A,0x7F",
                $"LD {register},0x04",
                $"CP ({register}+0x06)",
                "NOP",
                "db 0x80")
            .Build();

        z80.Run(7 + 14 + 19);

        z80.Registers.A.Should().Be(0x7F);
        z80.Registers.F.Should().Be(S | P | N | C);
        z80.States.TotalStates.Should().Be(40);

        // IXL or IYL
        z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD A,0x7F",
                $"LD {register},0x0080",
                $"CP {register}L")
            .Build();

        z80.Run(7 + 14 + 8);

        z80.Registers.A.Should().Be(0x7F);
        z80.Registers.F.Should().Be(S | P | N | C);
        z80.States.TotalStates.Should().Be(29);

        // IXH or IYH
        z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD A,0x7F",
                $"LD {register},0x8000",
                $"CP {register}H")
            .Build();

        z80.Run(7 + 14 + 8);

        z80.Registers.A.Should().Be(0x7F);
        z80.Registers.F.Should().Be(S | P | N | C);
        z80.States.TotalStates.Should().Be(29);
    }

    [Fact]
    public void When_INC_r_InstructionIsExecuted_RegisterIsUpdatedAndFlagsSet()
    {
        var z80 = new Z80Builder()
            .Flags(All)
            .Code(
                "LD A,0x00",
                "INC A")
            .Build();

        z80.Run(7 + 4);

        z80.Registers.A.Should().Be(0x01);
        z80.Registers.F.Should().Be(C);
        z80.States.TotalStates.Should().Be(11);

        z80 = new Z80Builder()
            .Flags(All ^ Z)
            .Code(
                "LD C,0xFF",
                "INC C")
            .Build();

        z80.Run(7 + 4);

        z80.Registers.C.Should().Be(0x00);
        z80.Registers.F.Should().Be(Z | H | C);
        z80.States.TotalStates.Should().Be(11);

        z80 = new Z80Builder()
            .Flags(N)
            .Code(
                "LD D,0x7F",
                "INC D")
            .Build();

        z80.Run(7 + 4);

        z80.Registers.D.Should().Be(0x80);
        z80.Registers.F.Should().Be(S | H | P);
        z80.States.TotalStates.Should().Be(11);

        z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD B,0x92",
                "INC B")
            .Build();

        z80.Run(7 + 4);

        z80.Registers.B.Should().Be(0x93);
        z80.Registers.F.Should().Be(S);
        z80.States.TotalStates.Should().Be(11);

        z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD E,0x10",
                "INC E")
            .Build();

        z80.Run(7 + 4);

        z80.Registers.E.Should().Be(0x11);
        z80.Registers.F.Should().Be(None);
        z80.States.TotalStates.Should().Be(11);
    }

    [Fact]
    public void When_INC_IXH_InstructionIsExecuted_RegisterIsUpdatedAndFlagsSet()
    {
        var z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD IXH,0x99",
                "INC IXH")
            .Build();

        z80.Run(11 + 8);

        z80.Registers.IXH.Should().Be(0x9A);
        z80.Registers.F.Should().Be(S | X);
        z80.States.TotalStates.Should().Be(19);
    }

    [Fact]
    public void When_INC_IYL_InstructionIsExecuted_RegisterIsUpdatedAndFlagsSet()
    {
        var z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD IYL,0xAB",
                "INC IYL")
            .Build();

        z80.Run(11 + 8);

        z80.Registers.IYL.Should().Be(0xAC);
        z80.Registers.F.Should().Be(S | Y | X);
        z80.States.TotalStates.Should().Be(19);
    }

    [Theory]
    [InlineData(0xFF, S | P | N | C, 0x00, Z | H | C)]
    [InlineData(0x7F, None, 0x80, S | H | P)]
    [InlineData(0xFE, None, 0xFF, S | Y | X)]
    [InlineData(0x20, None, 0x21, Y)]
    public void When_INC_Memory_HL_InstructionIsExecuted_MemoryIsUpdatedAndFlagsSet(
        byte value, Flags flags, byte expectedValue, Flags expectedFlags)
    {
        var builder = new Z80Builder()
            .Flags(flags)
            .Code(
                "LD HL,0x05",
                "INC (HL)",
                "NOP",
                $"db {value}");
        var z80 = builder.Build();
        var memory = builder.Memory!;

        z80.Run(10 + 11);

        memory[0x05].Should().Be(expectedValue);
        z80.Registers.F.Should().Be(expectedFlags);
        z80.States.TotalStates.Should().Be(21);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_INC_Memory_IXY_InstructionIsExecuted_MemoryIsUpdatedAndFlagsSet(string register)
    {
        var builder = new Z80Builder()
            .Flags(None)
            .Code(
                $"LD {register},0x05",
                $"INC ({register}+3)",
                "NOP",
                "db 0x3F");
        var z80 = builder.Build();
        var memory = builder.Memory!;

        z80.Run(14 + 23);

        memory[0x08].Should().Be(0x40);
        z80.Registers.F.Should().Be(H);
        z80.States.TotalStates.Should().Be(37);

        builder = new Z80Builder()
            .Flags(None)
            .Code(
                $"LD {register},0x69",
                $"INC ({register}-97)",
                "NOP",
                "db 0xFE");
        z80 = builder.Build();
        memory = builder.Memory!;

        z80.Run(14 + 23);

        memory[0x08].Should().Be(0xFF);
        z80.Registers.F.Should().Be(S | Y | X);
        z80.States.TotalStates.Should().Be(37);
    }

    [Fact]
    public void When_DEC_r_InstructionIsExecuted_RegisterIsUpdatedAndFlagsSet()
    {
        var z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD A,0x01",
                "DEC A")
            .Build();

        z80.Run(7 + 4);

        z80.Registers.A.Should().Be(0x00);
        z80.Registers.F.Should().Be(Z | N);
        z80.States.TotalStates.Should().Be(11);

        z80 = new Z80Builder()
            .Flags(All ^ (Z | H | N))
            .Code(
                "LD C,0",
                "DEC C")
            .Build();

        z80.Run(7 + 4);

        z80.Registers.C.Should().Be(0xFF);
        z80.Registers.F.Should().Be(S | H | Y | X | N | C);
        z80.States.TotalStates.Should().Be(11);

        z80 = new Z80Builder()
            .Flags(Z | S)
            .Code(
                "LD D,0x80",
                "DEC D")
            .Build();

        z80.Run(7 + 4);

        z80.Registers.D.Should().Be(0x7F);
        z80.Registers.F.Should().Be(Y | H | X | P | N);
        z80.States.TotalStates.Should().Be(11);

        z80 = new Z80Builder()
            .Flags(All)
            .Code(
                "LD B,0xAB",
                "DEC B")
            .Build();

        z80.Run(7 + 4);

        z80.Registers.B.Should().Be(0xAA);
        z80.Registers.F.Should().Be(S | Y | X | N | C);
        z80.States.TotalStates.Should().Be(11);
    }

    [Fact]
    public void When_DEC_IXH_InstructionIsExecuted_RegisterIsUpdatedAndFlagsSet()
    {
        var z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD IXH,0x99",
                "DEC IXH")
            .Build();

        z80.Run(11 + 8);

        z80.Registers.IXH.Should().Be(0x98);
        z80.Registers.F.Should().Be(S | X | N);
        z80.States.TotalStates.Should().Be(19);
    }

    [Fact]
    public void When_DEC_IYL_InstructionIsExecuted_RegisterIsUpdatedAndFlagsSet()
    {
        var z80 = new Z80Builder()
            .Flags(None)
            .Code(
                "LD IYL,0xAB",
                "DEC IYL")
            .Build();

        z80.Run(11 + 8);

        z80.Registers.IYL.Should().Be(0xAA);
        z80.Registers.F.Should().Be(S | Y | X | N);
        z80.States.TotalStates.Should().Be(19);
    }

    [Theory]
    [InlineData(0x00, C, 0xFF, S | Y | H | X | N | C)]
    [InlineData(0x01, None, 0x00, Z | N)]
    [InlineData(0x80, None, 0x7F, P | Y | H | X | N)]
    public void When_DEC_Memory_HL_InstructionIsExecuted_MemoryIsUpdatedAndFlagsSet(
        byte value, Flags flags, byte expectedValue, Flags expectedFlags)
    {
        var builder = new Z80Builder()
            .Flags(flags)
            .Code(
                "LD HL,0x05",
                "DEC (HL)",
                "NOP",
                $"db {value}");
        var z80 = builder.Build();
        var memory = builder.Memory!;

        z80.Run(10 + 11);

        memory[0x05].Should().Be(expectedValue);
        z80.Registers.F.Should().Be(expectedFlags);
        z80.States.TotalStates.Should().Be(21);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_DEC_Memory_IXY_InstructionIsExecuted_MemoryIsUpdatedAndFlagsSet(string register)
    {
        var builder = new Z80Builder()
            .Flags(None)
            .Code(
                $"LD {register},0x05",
                $"DEC ({register}+3)",
                "NOP",
                "db 0x3F");
        var z80 = builder.Build();
        var memory = builder.Memory!;

        z80.Run(14 + 23);

        memory![0x08].Should().Be(0x3E);
        z80.Registers.F.Should().Be(Y | X | N);
        z80.States.TotalStates.Should().Be(37);

        builder = new Z80Builder()
            .Flags(None)
            .Code(
                $"LD {register},0x69",
                $"DEC ({register}-97)",
                "NOP",
                "db 0x00");
        z80 = builder.Build();
        memory = builder.Memory!;

        z80.Run(14 + 23);

        memory![0x08].Should().Be(0xFF);
        z80.Registers.F.Should().Be(S | Y | H | X | N);
        z80.States.TotalStates.Should().Be(37);
    }
}