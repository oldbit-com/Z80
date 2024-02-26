using Z80.Net.Instructions;
using Z80.Net.Registers;
using static Z80.Net.Instructions.OpCodes;
using static Z80.Net.Registers.Flags;

namespace Z80.Net.UnitTests;

public class Z808BitArithmeticInstructionsTests
{
    [Fact]
    public void When_ADD_A_A_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var z80 = new CodeBuilder()
            .Flags(All ^ Z)
            .Code(
                "LD A,0x00",
                "ADD A,A")
            .Build();

        z80.Run(7 + 4);

        z80.Registers.A.Should().Be(0x00);
        z80.Registers.F.Should().Be(Z);
        z80.CycleCounter.TotalCycles.Should().Be(11);
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
        var z80 = new CodeBuilder()
            .Flags(flags)
            .Code(
                $"LD A,{a}",
                $"LD B,{b}",
                "ADD A,B")
            .Build();

        z80.Run(7 + 7 + 4);

        z80.Registers.A.Should().Be(expectedResult);
        z80.Registers.F.Should().Be(expectedFlags);
        z80.CycleCounter.TotalCycles.Should().Be(18);
    }

    [Fact]
    public void When_ADD_A_n_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var z80 = new CodeBuilder()
            .Flags(All)
            .Code(
                "LD A,0x10",
                "ADD A,0x20")
            .Build();

        z80.Run(7 + 7);

        z80.Registers.A.Should().Be(0x30);
        z80.Registers.F.Should().Be(Y);
        z80.CycleCounter.TotalCycles.Should().Be(14);
    }

    [Fact]
    public void When_ADD_A_HL_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var z80 = new CodeBuilder()
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
        z80.CycleCounter.TotalCycles.Should().Be(28);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_ADD_A_IndexRegister_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet(string register)
    {
        // IX or IY
        var z80 = new CodeBuilder()
            .Flags(All ^ Z)
            .Code(
                "LD A,0x22",
                $"LD {register},0x09",
                $"ADD A,({register}+0x01)",
                "NOP",
                "db 0x13")
            .Build();

        z80.Run(7 + 14 + 19);

        z80.Registers.A.Should().Be(0x35);
        z80.Registers.F.Should().Be(Y);
        z80.CycleCounter.TotalCycles.Should().Be(40);

        // IXL or IYL
        z80 = new CodeBuilder()
            .Flags(All ^ Z)
            .Code(
                "LD A,0x22",
                $"LD {register},0x1213",
                $"ADD A,{register}L")
            .Build();

        z80.Run(7 + 14 + 8);

        z80.Registers.A.Should().Be(0x35);
        z80.Registers.F.Should().Be(Y);
        z80.CycleCounter.TotalCycles.Should().Be(29);

        // IXH or IYH
        z80 = new CodeBuilder()
            .Flags(All ^ Z)
            .Code(
                "LD A,0x22",
                $"LD {register},0x1213",
                $"ADD A,{register}H")
            .Build();

        z80.Run(7 + 14 + 8);

        z80.Registers.A.Should().Be(0x34);
        z80.Registers.F.Should().Be(Y);
        z80.CycleCounter.TotalCycles.Should().Be(29);
    }

    [Fact]
    public void When_ADC_A_A_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var z80 = new CodeBuilder()
            .Flags(C)
            .Code(
                "LD A,0x7F",
                "ADC A,A")
            .Build();

        z80.Run(7 + 4);

        z80.Registers.A.Should().Be(0xFF);
        z80.Registers.F.Should().Be(P | X | H | Y | S);
        z80.CycleCounter.TotalCycles.Should().Be(11);
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
        var z80 = new CodeBuilder()
            .Flags(flags)
            .Code(
                $"LD A,{a}",
                $"LD B,{b}",
                "ADC A,B")
            .Build();

        z80.Run(7 + 7 + 4);

        z80.Registers.A.Should().Be(expectedResult);
        z80.Registers.F.Should().Be(expectedFlags);
        z80.CycleCounter.TotalCycles.Should().Be(18);
    }

    [Fact]
    public void When_ADC_A_n_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var z80 = new CodeBuilder()
            .Flags(C)
            .Code(
                "LD A,0x20",
                "ADC A,0x20")
            .Build();

        z80.Run(7 + 7);

        z80.Registers.A.Should().Be(0x41);
        z80.Registers.F.Should().Be(None);
        z80.CycleCounter.TotalCycles.Should().Be(14);
    }

    [Fact]
    public void When_ADC_A_HL_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var z80 = new CodeBuilder()
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
        z80.CycleCounter.TotalCycles.Should().Be(28);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_ADC_A_IndexRegister_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet(string  register)
    {
        // IX or IY
        var z80 = new CodeBuilder()
            .Flags(C)
            .Code(
                "LD A,0x22",
                $"LD {register},0x09",
                $"ADC A,({register}+1)",
                "NOP",
                "db 0x13")
            .Build();

        z80.Run(7 + 14 + 19);

        z80.Registers.A.Should().Be(0x36);
        z80.Registers.F.Should().Be(Y);
        z80.CycleCounter.TotalCycles.Should().Be(40);

        // IXL or IYL
        z80 = new CodeBuilder()
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
        z80.CycleCounter.TotalCycles.Should().Be(29);

        // IXH or IYH
        z80 = new CodeBuilder()
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
        z80.CycleCounter.TotalCycles.Should().Be(29);
    }

    [Fact]
    public void When_SUB_r_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var z80 = new CodeBuilder()
            .Flags(None)
            .Code(
                "LD A,0x20",
                "SUB A")
            .Build();

        z80.Run(7 + 4);

        z80.Registers.A.Should().Be(0x00);
        z80.Registers.F.Should().Be(Z | N);
        z80.CycleCounter.TotalCycles.Should().Be(11);

        z80 = new CodeBuilder()
            .Flags(Z)
            .Code(
                "LD A,0x90",
                "LD H,0x20",
                "SUB H")
            .Build();

        z80.Run(7 + 7 + 4);

        z80.Registers.A.Should().Be(0x70);
        z80.Registers.F.Should().Be(Y | P | N);
        z80.CycleCounter.TotalCycles.Should().Be(18);
    }

    [Fact]
    public void When_SUB_n_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var z80 = new CodeBuilder()
            .Flags(Z)
            .Code(
                "LD A,0x00",
                "SUB 1")
            .Build();

        z80.Run(7 + 7);

        z80.Registers.A.Should().Be(0xFF);
        z80.Registers.F.Should().Be(S | Y | H | X | N | C);
        z80.CycleCounter.TotalCycles.Should().Be(14);
    }

    [Fact]
    public void When_SUB_HL_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var z80 = new CodeBuilder()
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
        z80.CycleCounter.TotalCycles.Should().Be(28);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_SUB_IndexRegister_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet(string register)
    {
        // IX or IY
        var z80 = new CodeBuilder()
            .Flags(None)
            .Code(
                "LD A,0x12",
                $"LD {register},0x09",
                $"SUB ({register})",
                "NOP",
                "db 0x02")
            .Build();

        z80.Run(7 + 14 + 19);

        z80.Registers.A.Should().Be(0x10);
        z80.Registers.F.Should().Be(N);
        z80.CycleCounter.TotalCycles.Should().Be(40);

        // IXL or IYL
        z80 = new CodeBuilder()
            .Flags(None)
            .Code(
                "LD A,0x12",
                $"LD {register},0x1202",
                $"SUB {register}L")
            .Build();

        z80.Run(7 + 14 + 8);

        z80.Registers.A.Should().Be(0x10);
        z80.Registers.F.Should().Be(N);
        z80.CycleCounter.TotalCycles.Should().Be(29);

        // IXH or IYH
        z80 = new CodeBuilder()
            .Flags(None)
            .Code(
                "LD A,0x12",
                $"LD {register},0x0212",
                $"SUB {register}H")
            .Build();

        z80.Run(7 + 14 + 8);

        z80.Registers.A.Should().Be(0x10);
        z80.Registers.F.Should().Be(N);
        z80.CycleCounter.TotalCycles.Should().Be(29);
    }

    [Fact]
    public void When_SBC_A_r_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var memory = new TestMemory(LD_A_n, 0x01, LD_B_n, 0x01, SBC_A_B);
        var z80 = new Z80(memory) { Registers = { F = C } };
        z80.Run(7 + 7 + 4);

        z80.Registers.A.Should().Be(0xFF);
        z80.Registers.F.Should().Be(S | Y | H | X | N | C);
        z80.CycleCounter.TotalCycles.Should().Be(18);

        memory = new TestMemory(LD_A_n, 0x7F, LD_L_n, 0x80, SBC_A_L);
        z80 = new Z80(memory) { Registers = { F = C } };
        z80.Run(7 + 7 + 4);

        z80.Registers.A.Should().Be(0xFE);
        z80.Registers.F.Should().Be(S | Y | X | P | N | C);
        z80.CycleCounter.TotalCycles.Should().Be(18);

        memory = new TestMemory(LD_A_n, 0xFF, SBC_A_A);
        z80 = new Z80(memory) { Registers = { F = C } };
        z80.Run(7 + 4);

        z80.Registers.A.Should().Be(0xFF);
        z80.Registers.F.Should().Be(S | Y | H | X | N | C);
        z80.CycleCounter.TotalCycles.Should().Be(11);
    }

    [Fact]
    public void When_SBC_A_n_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var memory = new TestMemory(LD_A_n, 0x02, SBC_A_n, 0x01);
        var z80 = new Z80(memory) { Registers = { F = C } };
        z80.Run(7 + 7);

        z80.Registers.A.Should().Be(0x00);
        z80.Registers.F.Should().Be(Z | N);
        z80.CycleCounter.TotalCycles.Should().Be(14);
    }

    [Fact]
    public void When_SBC_A_HL_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var memory = new TestMemory(LD_A_n, 0x81, LD_H_n, 0x00, LD_L_n, 0x08, SBC_A_HL, NOP, 0x01);
        var z80 = new Z80(memory) { Registers = { F = C } };
        z80.Run(7 + 7 + 7 + 7);

        z80.Registers.A.Should().Be(0x7F);
        z80.Registers.F.Should().Be(Y | H | X | P | N);
        z80.CycleCounter.TotalCycles.Should().Be(28);
    }

    [Theory]
    [MemberData(nameof(IndexRegisterPrefix))]
    public void When_SBC_A_IndexRegister_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet(OpCode prefix)
    {
        // IX or IY
        var memory = new TestMemory(LD_A_n, 0x12, prefix, LD_HL_nn, 0x02, 0x00, prefix, SBC_A_HL, 0x08, NOP, 0x01);
        var z80 = new Z80(memory);
        z80.Run(7 + 14 + 19);

        z80.Registers.A.Should().Be(0x10);
        z80.Registers.F.Should().Be(N);
        z80.CycleCounter.TotalCycles.Should().Be(40);

        // IXL or IYL
        memory = new TestMemory(LD_A_n, 0x12, prefix, LD_HL_nn, 0x02, 0x00, prefix, SBC_A_L);
        z80 = new Z80(memory) { Registers = { F = C }};
        z80.Run(7 + 14 + 8);

        z80.Registers.A.Should().Be(0x0F);
        z80.Registers.F.Should().Be(H | X | N);
        z80.CycleCounter.TotalCycles.Should().Be(29);

        // IXH or IYH
        memory = new TestMemory(LD_A_n, 0x12, prefix, LD_HL_nn, 0x00, 0x02, prefix, SBC_A_H);
        z80 = new Z80(memory) { Registers = { F = C }};
        z80.Run(7 + 14 + 8);

        z80.Registers.A.Should().Be(0x0F);
        z80.Registers.F.Should().Be(H | X | N);
        z80.CycleCounter.TotalCycles.Should().Be(29);
    }

    [Fact]
    public void When_AND_r_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var memory = new TestMemory(LD_A_n, 0x0F, LD_B_n, 0xF0, AND_B);
        var z80 = new Z80(memory) { Registers = { F = N | C } };
        z80.Run(7 + 7 + 4);

        z80.Registers.A.Should().Be(0x00);
        z80.Registers.F.Should().Be(Z | H | P);
        z80.CycleCounter.TotalCycles.Should().Be(18);
    }

    [Fact]
    public void When_AND_n_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var memory = new TestMemory(LD_A_n, 0x8F, AND_n, 0xF3);
        var z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(7 + 7);

        z80.Registers.A.Should().Be(0x83);
        z80.Registers.F.Should().Be(S | H);
        z80.CycleCounter.TotalCycles.Should().Be(14);
    }

    [Fact]
    public void When_AND_HL_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var memory = new TestMemory(LD_A_n, 0xFF, LD_H_n, 0x00, LD_L_n, 0x08, AND_HL, NOP, 0x81);
        var z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(7 + 7 + 7 + 7);

        z80.Registers.A.Should().Be(0x81);
        z80.Registers.F.Should().Be(S | H | P);
        z80.CycleCounter.TotalCycles.Should().Be(28);
    }

    [Theory]
    [MemberData(nameof(IndexRegisterPrefix))]
    public void When_AND_A_IndexRegister_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet(OpCode prefix)
    {
        // IX or IY
        var memory = new TestMemory(LD_A_n, 0x88, prefix, LD_HL_nn, 0x00, 0x00, prefix, AND_HL, 0x0A, NOP, 0x08);
        var z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(7 + 14 + 19);

        z80.Registers.A.Should().Be(0x08);
        z80.Registers.F.Should().Be(H | X);
        z80.CycleCounter.TotalCycles.Should().Be(40);

        // IXL or IYL
        memory = new TestMemory(LD_A_n, 0x01, prefix, LD_HL_nn, 0x03, 0x00, prefix, AND_L);
        z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(7 + 14 + 8);

        z80.Registers.A.Should().Be(0x01);
        z80.Registers.F.Should().Be(H);
        z80.CycleCounter.TotalCycles.Should().Be(29);

        // IXH or IYH
        memory = new TestMemory(LD_A_n, 0x01, prefix, LD_HL_nn, 0x00, 0x03, prefix, AND_H);
        z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(7 + 14 + 8);

        z80.Registers.A.Should().Be(0x01);
        z80.Registers.F.Should().Be(H);
        z80.CycleCounter.TotalCycles.Should().Be(29);
    }

    [Fact]
    public void When_OR_r_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var memory = new TestMemory(LD_A_n, 0x00, LD_B_n, 0x00, OR_B);
        var z80 = new Z80(memory) { Registers = { F = S | H | N | C } };
        z80.Run(7 + 7 + 4);

        z80.Registers.A.Should().Be(0x00);
        z80.Registers.F.Should().Be(Z | P);
        z80.CycleCounter.TotalCycles.Should().Be(18);
    }

    [Fact]
    public void When_OR_n_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var memory = new TestMemory(LD_A_n, 0x11, OR_n, 0x20);
        var z80 = new Z80(memory) { Registers = { F = S | H | N | C } };
        z80.Run(7 + 7);

        z80.Registers.A.Should().Be(0x31);
        z80.Registers.F.Should().Be(Y);
        z80.CycleCounter.TotalCycles.Should().Be(14);
    }

    [Fact]
    public void When_OR_HL_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var memory = new TestMemory(LD_A_n, 0x8A, LD_H_n, 0x00, LD_L_n, 0x08, OR_HL, NOP, 0x85);
        var z80 = new Z80(memory) { Registers = { F = S | H | N | C } };
        z80.Run(7 + 7 + 7 + 7);

        z80.Registers.A.Should().Be(0x8F);
        z80.Registers.F.Should().Be(S | X);
        z80.CycleCounter.TotalCycles.Should().Be(28);
    }

    [Theory]
    [MemberData(nameof(IndexRegisterPrefix))]
    public void When_OR_A_IndexRegister_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet(OpCode prefix)
    {
        // IX or IY
        var memory = new TestMemory(LD_A_n, 0x80, prefix, LD_HL_nn, 0x00, 0x00, prefix, OR_HL, 0x0A, NOP, 0x08);
        var z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(7 + 14 + 19);

        z80.Registers.A.Should().Be(0x88);
        z80.Registers.F.Should().Be(S | X | P);
        z80.CycleCounter.TotalCycles.Should().Be(40);

        // IXL or IYL
        memory = new TestMemory(LD_A_n, 0x01, prefix, LD_HL_nn, 0x12, 0x00, prefix, OR_L);
        z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(7 + 14 + 8);

        z80.Registers.A.Should().Be(0x13);
        z80.Registers.F.Should().Be(None);
        z80.CycleCounter.TotalCycles.Should().Be(29);

        // IXH or IYH
        memory = new TestMemory(LD_A_n, 0x01, prefix, LD_HL_nn, 0x00, 0x12, prefix, OR_H);
        z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(7 + 14 + 8);

        z80.Registers.A.Should().Be(0x13);
        z80.Registers.F.Should().Be(None);
        z80.CycleCounter.TotalCycles.Should().Be(29);
    }

    [Fact]
    public void When_XOR_r_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var memory = new TestMemory(LD_A_n, 0x1F, LD_B_n, 0x1F, XOR_B);
        var z80 = new Z80(memory) { Registers = { F = N | H | C } };
        z80.Run(7 + 7 + 4);

        z80.Registers.A.Should().Be(0x00);
        z80.Registers.F.Should().Be(Z | P);
        z80.CycleCounter.TotalCycles.Should().Be(18);
    }

    [Fact]
    public void When_XOR_n_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var memory = new TestMemory(LD_A_n, 0x1F, XOR_n, 0x0F);
        var z80 = new Z80(memory) { Registers = { F = None} };
        z80.Run(7 + 7);

        z80.Registers.A.Should().Be(0x10);
        z80.Registers.F.Should().Be(None);
        z80.CycleCounter.TotalCycles.Should().Be(14);
    }

    [Fact]
    public void When_XOR_HL_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var memory = new TestMemory(LD_A_n, 0x1F, LD_H_n, 0x00, LD_L_n, 0x08, XOR_HL, NOP, 0x8F);
        var z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(7 + 7 + 7 + 7);

        z80.Registers.A.Should().Be(0x90);
        z80.Registers.F.Should().Be(S | P);
        z80.CycleCounter.TotalCycles.Should().Be(28);
    }

    [Theory]
    [MemberData(nameof(IndexRegisterPrefix))]
    public void When_XOR_A_IndexRegister_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet(OpCode prefix)
    {
        // IX or IY
        var memory = new TestMemory(LD_A_n, 0x88, prefix, LD_HL_nn, 0x00, 0x00, prefix, XOR_HL, 0x0A, NOP, 0x08);
        var z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(7 + 14 + 19);

        z80.Registers.A.Should().Be(0x80);
        z80.Registers.F.Should().Be(S);
        z80.CycleCounter.TotalCycles.Should().Be(40);

        // IXL or IYL
        memory = new TestMemory(LD_A_n, 0x01, prefix, LD_HL_nn, 0x03, 0x00, prefix, XOR_L);
        z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(7 + 14 + 8);

        z80.Registers.A.Should().Be(0x02);
        z80.Registers.F.Should().Be(None);
        z80.CycleCounter.TotalCycles.Should().Be(29);

        // IXH or IYH
        memory = new TestMemory(LD_A_n, 0x01, prefix, LD_HL_nn, 0x00, 0x03, prefix, XOR_H);
        z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(7 + 14 + 8);

        z80.Registers.A.Should().Be(0x02);
        z80.Registers.F.Should().Be(None);
        z80.CycleCounter.TotalCycles.Should().Be(29);
    }

    [Fact]
    public void When_CP_r_InstructionIsExecuted_FlagsAreSet()
    {
        var memory = new TestMemory(LD_A_n, 0x20, CP_A);
        var z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(7 + 4);

        z80.Registers.A.Should().Be(0x20);
        z80.Registers.F.Should().Be(Z | Y | N);
        z80.CycleCounter.TotalCycles.Should().Be(11);

        memory = new TestMemory(LD_A_n, 0x00, LD_B_n, 0x01, CP_B);
        z80 = new Z80(memory) { Registers = { F = Z } };
        z80.Run(7 + 7 + 4);

        z80.Registers.A.Should().Be(0x00);
        z80.Registers.F.Should().Be(S | H | N | C);
        z80.CycleCounter.TotalCycles.Should().Be(18);
    }

    [Fact]
    public void When_CP_n_InstructionIsExecuted_FlagsAreSet()
    {
        var memory = new TestMemory(LD_A_n, 0x90, CP_n, 0x20);
        var z80 = new Z80(memory) { Registers = { F = Z} };
        z80.Run(7 + 7);

        z80.Registers.A.Should().Be(0x90);
        z80.Registers.F.Should().Be(Y | P | N);
        z80.CycleCounter.TotalCycles.Should().Be(14);
    }

    [Fact]
    public void When_CP_HL_InstructionIsExecuted_FlagsAreSet()
    {
        var memory = new TestMemory(LD_A_n, 0x7F, LD_H_n, 0x00, LD_L_n, 0x08, CP_HL, NOP, 0x80);
        var z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(7 + 7 + 7 + 7);

        z80.Registers.A.Should().Be(0x7F);
        z80.Registers.F.Should().Be(S | P | N | C);
        z80.CycleCounter.TotalCycles.Should().Be(28);
    }

    [Theory]
    [MemberData(nameof(IndexRegisterPrefix))]
    public void When_CP_A_IndexRegister_InstructionIsExecuted_FlagsAreSet(OpCode prefix)
    {
        // IX or IY
        var memory = new TestMemory(LD_A_n, 0x7F, prefix, LD_HL_nn, 0x00, 0x00, prefix, CP_HL, 0x0A, NOP, 0x80);
        var z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(7 + 14 + 19);

        z80.Registers.A.Should().Be(0x7F);
        z80.Registers.F.Should().Be(S | P | N | C);
        z80.CycleCounter.TotalCycles.Should().Be(40);

        // IXL or IYL
        memory = new TestMemory(LD_A_n, 0x7F, prefix, LD_HL_nn, 0x80, 0x00, prefix, CP_L);
        z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(7 + 14 + 8);

        z80.Registers.A.Should().Be(0x7F);
        z80.Registers.F.Should().Be(S | P | N | C);
        z80.CycleCounter.TotalCycles.Should().Be(29);

        // IXH or IYH
        memory = new TestMemory(LD_A_n, 0x7F, prefix, LD_HL_nn, 0x00, 0x80, prefix, CP_H);
        z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(7 + 14 + 8);

        z80.Registers.A.Should().Be(0x7F);
        z80.Registers.F.Should().Be(S | P | N | C);
        z80.CycleCounter.TotalCycles.Should().Be(29);
    }

    [Fact]
    public void When_INC_r_InstructionIsExecuted_RegisterIsUpdatedAndFlagsSet()
    {
        var memory = new TestMemory(LD_A_n, 0x00, INC_A);
        var z80 = new Z80(memory) { Registers = { F = All } };
        z80.Run(7 + 4);

        z80.Registers.A.Should().Be(0x01);
        z80.Registers.F.Should().Be(C);
        z80.CycleCounter.TotalCycles.Should().Be(11);

        memory = new TestMemory(LD_C_n, 0xFF, INC_C);
        z80 = new Z80(memory) { Registers = { F = All ^ Z } };
        z80.Run(7 + 4);

        z80.Registers.C.Should().Be(0x00);
        z80.Registers.F.Should().Be(Z | H | C);
        z80.CycleCounter.TotalCycles.Should().Be(11);

        memory = new TestMemory(LD_D_n, 0x7F, INC_D);
        z80 = new Z80(memory) { Registers = { F = N } };
        z80.Run(7 + 4);

        z80.Registers.D.Should().Be(0x80);
        z80.Registers.F.Should().Be(S | H | P);
        z80.CycleCounter.TotalCycles.Should().Be(11);

        memory = new TestMemory(LD_B_n, 0x92, INC_B);
        z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(7 + 4);

        z80.Registers.B.Should().Be(0x93);
        z80.Registers.F.Should().Be(S);
        z80.CycleCounter.TotalCycles.Should().Be(11);

        memory = new TestMemory(LD_E_n, 0x10, INC_E);
        z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(7 + 4);

        z80.Registers.E.Should().Be(0x11);
        z80.Registers.F.Should().Be(None);
        z80.CycleCounter.TotalCycles.Should().Be(11);
    }

    [Fact]
    public void When_INC_rXY_InstructionIsExecuted_RegisterIsUpdatedAndFlagsSet()
    {
        var memory = new TestMemory(IX, LD_H_n, 0x99, IX, INC_H);
        var z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(11 + 8);

        z80.Registers.IXH.Should().Be(0x9A);
        z80.Registers.F.Should().Be(S | X);
        z80.CycleCounter.TotalCycles.Should().Be(19);

        memory = new TestMemory(IY, LD_L_n, 0xAB, IY, INC_L);
        z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(11 + 8);

        z80.Registers.IYL.Should().Be(0xAC);
        z80.Registers.F.Should().Be(S | Y | X);
        z80.CycleCounter.TotalCycles.Should().Be(19);
    }

    [Fact]
    public void When_INC_mHL_InstructionIsExecuted_MemoryIsUpdatedAndFlagsSet()
    {
        var memory = new TestMemory(LD_HL_nn, 0x05, 0x00, INC_MHL, NOP, 0xFF);
        var z80 = new Z80(memory) { Registers = { F = S | P | N | C } };
        z80.Run(10 + 11);

        memory[0x05].Should().Be(0x00);
        z80.Registers.F.Should().Be(Z | H | C);
        z80.CycleCounter.TotalCycles.Should().Be(21);

        memory = new TestMemory(LD_HL_nn, 0x05, 0x00, INC_MHL, NOP, 0x7F);
        z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(10 + 11);

        memory[0x05].Should().Be(0x80);
        z80.Registers.F.Should().Be(S | H | P);
        z80.CycleCounter.TotalCycles.Should().Be(21);

        memory = new TestMemory(LD_HL_nn, 0x05, 0x00, INC_MHL, NOP, 0x20);
        z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(10 + 11);

        memory[0x05].Should().Be(0x21);
        z80.Registers.F.Should().Be(Y);
        z80.CycleCounter.TotalCycles.Should().Be(21);

        memory = new TestMemory(LD_HL_nn, 0x05, 0x00, INC_MHL, NOP, 0xFE);
        z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(10 + 11);

        memory[0x05].Should().Be(0xFF);
        z80.Registers.F.Should().Be(S | Y | X);
        z80.CycleCounter.TotalCycles.Should().Be(21);
    }

    [Theory]
    [MemberData(nameof(IndexRegisterPrefix))]
    public void When_INC_mIndexRegister_InstructionIsExecuted_MemoryIsUpdatedAndFlagsSet(OpCode prefix)
    {
        var memory = new TestMemory(prefix, LD_HL_nn, 0x05, 0x00, prefix, INC_MHL, 0x03, NOP, 0x3F);
        var z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(14 + 23);

        memory[0x08].Should().Be(0x40);
        z80.Registers.F.Should().Be(H);
        z80.CycleCounter.TotalCycles.Should().Be(37);

        memory = new TestMemory(prefix, LD_HL_nn, 0x69, 0x00, prefix, INC_MHL, 0x9F, NOP, 0xFE);
        z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(14 + 23);

        memory[0x08].Should().Be(0xFF);
        z80.Registers.F.Should().Be(S | Y | X);
        z80.CycleCounter.TotalCycles.Should().Be(37);
    }

    [Fact]
    public void When_DEC_r_InstructionIsExecuted_RegisterIsUpdatedAndFlagsSet()
    {
        var memory = new TestMemory(LD_A_n, 0x01, DEC_A);
        var z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(7 + 4);

        z80.Registers.A.Should().Be(0x00);
        z80.Registers.F.Should().Be(Z | N);
        z80.CycleCounter.TotalCycles.Should().Be(11);

        memory = new TestMemory(LD_C_n, 0x00, DEC_C);
        z80 = new Z80(memory) { Registers = { F = All ^ (Z | H | N) } };
        z80.Run(7 + 4);

        z80.Registers.C.Should().Be(0xFF);
        z80.Registers.F.Should().Be(S | H | Y | X | N | C);
        z80.CycleCounter.TotalCycles.Should().Be(11);

        memory = new TestMemory(LD_D_n, 0x80, DEC_D);
        z80 = new Z80(memory) { Registers = { F = Z | S } };
        z80.Run(7 + 4);

        z80.Registers.D.Should().Be(0x7F);
        z80.Registers.F.Should().Be(Y | H | X | P | N);
        z80.CycleCounter.TotalCycles.Should().Be(11);

        memory = new TestMemory(LD_B_n, 0xAB, DEC_B);
        z80 = new Z80(memory) { Registers = { F = All } };
        z80.Run(7 + 4);

        z80.Registers.B.Should().Be(0xAA);
        z80.Registers.F.Should().Be(S | Y | X | N | C);
        z80.CycleCounter.TotalCycles.Should().Be(11);
    }

    [Fact]
    public void When_DEC_rXY_InstructionIsExecuted_RegisterIsUpdatedAndFlagsSet()
    {
        var memory = new TestMemory(IX, LD_H_n, 0x99, IX, DEC_H);
        var z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(11 + 8);

        z80.Registers.IXH.Should().Be(0x98);
        z80.Registers.F.Should().Be(S | X | N);
        z80.CycleCounter.TotalCycles.Should().Be(19);

        memory = new TestMemory(IY, LD_L_n, 0xAB, IY, DEC_L);
        z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(11 + 8);

        z80.Registers.IYL.Should().Be(0xAA);
        z80.Registers.F.Should().Be(S | Y | X | N);
        z80.CycleCounter.TotalCycles.Should().Be(19);
    }

    [Fact]
    public void When_DEC_mHL_InstructionIsExecuted_MemoryIsUpdatedAndFlagsSet()
    {
        var memory = new TestMemory(LD_HL_nn, 0x05, 0x00, DEC_MHL, NOP, 0x00);
        var z80 = new Z80(memory) { Registers = { F = C } };
        z80.Run(10 + 11);

        memory[0x05].Should().Be(0xFF);
        z80.Registers.F.Should().Be(S | Y | H | X | N | C);
        z80.CycleCounter.TotalCycles.Should().Be(21);

        memory = new TestMemory(LD_HL_nn, 0x05, 0x00, DEC_MHL, NOP, 0x01);
        z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(10 + 11);

        memory[0x05].Should().Be(0x00);
        z80.Registers.F.Should().Be(Z | N);
        z80.CycleCounter.TotalCycles.Should().Be(21);

        memory = new TestMemory(LD_HL_nn, 0x05, 0x00, DEC_MHL, NOP, 0x80);
        z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(10 + 11);

        memory[0x05].Should().Be(0x7F);
        z80.Registers.F.Should().Be(P | Y | H | X | N);
        z80.CycleCounter.TotalCycles.Should().Be(21);
    }

    [Theory]
    [MemberData(nameof(IndexRegisterPrefix))]
    public void When_DEC_mIndexRegister_InstructionIsExecuted_MemoryIsUpdatedAndFlagsSet(OpCode prefix)
    {
        var memory = new TestMemory(prefix, LD_HL_nn, 0x05, 0x00, prefix, DEC_MHL, 0x03, NOP, 0x3F);
        var z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(14 + 23);

        memory[0x08].Should().Be(0x3E);
        z80.Registers.F.Should().Be(Y | X | N);
        z80.CycleCounter.TotalCycles.Should().Be(37);

        memory = new TestMemory(prefix, LD_HL_nn, 0x69, 0x00, prefix, DEC_MHL, 0x9F, NOP, 0x00);
        z80 = new Z80(memory) { Registers = { F = None } };
        z80.Run(14 + 23);

        memory[0x08].Should().Be(0xFF);
        z80.Registers.F.Should().Be(S | Y | H | X | N);
        z80.CycleCounter.TotalCycles.Should().Be(37);
    }

    public static IEnumerable<object[]> IndexRegisterPrefix => new List<object[]>
    {
        new object[] { IX },
        new object[] { IY },
    };
}