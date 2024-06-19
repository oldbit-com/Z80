using OldBit.Z80Cpu.Registers;
using OldBit.Z80Cpu.UnitTests.Fixtures;

namespace OldBit.Z80Cpu.UnitTests;

public class Z808BitLoadInstructionsTests
{
    [Fact]
    public void When_LD_R_n_InstructionIsExecuted_RegisterIsUpdated()
    {
        var z80 = new Z80Builder()
            .Code(
                "LD A,1",
                "LD B,2",
                "LD C,3",
                "LD D,4",
                "LD E,5",
                "LD H,6",
                "LD L,7",
                "LD IXH,8",
                "LD IXL,9",
                "LD IYH,10",
                "LD IYL,11")
            .Build();

        z80.Run(7 * 7 + 4 * 11);

        z80.Registers.A.Should().Be(0x01);
        z80.Registers.B.Should().Be(0x02);
        z80.Registers.C.Should().Be(0x03);
        z80.Registers.D.Should().Be(0x04);
        z80.Registers.E.Should().Be(0x05);
        z80.Registers.H.Should().Be(0x06);
        z80.Registers.L.Should().Be(0x07);
        z80.Registers.IXH.Should().Be(0x08);
        z80.Registers.IXL.Should().Be(0x09);
        z80.Registers.IYH.Should().Be(0x0A);
        z80.Registers.IYL.Should().Be(0x0B);
        z80.Clock.TotalTicks.Should().Be(93);
    }

    [Theory]
    [InlineData("A", 1)]
    [InlineData("B", 2)]
    [InlineData("C", 3)]
    [InlineData("D", 4)]
    [InlineData("E", 5)]
    [InlineData("H", 6)]
    [InlineData("L", 7)]
    public void When_LD_R_R_InstructionIsExecuted_RegisterIsUpdated(string register, byte value)
    {
        var z80 = new Z80Builder()
            .Code(
                $"LD {register},{value}",
                $"LD A,{register}",
                $"LD B,{register}",
                $"LD C,{register}",
                $"LD D,{register}",
                $"LD E,{register}",
                $"LD H,{register}",
                $"LD L,{register}")
            .Build();

        z80.Run(7 + 7 * 4);

        z80.Registers.A.Should().Be(value);
        z80.Registers.B.Should().Be(value);
        z80.Registers.C.Should().Be(value);
        z80.Registers.D.Should().Be(value);
        z80.Registers.E.Should().Be(value);
        z80.Registers.H.Should().Be(value);
        z80.Registers.L.Should().Be(value);
        z80.Clock.TotalTicks.Should().Be(35);
    }

    [Theory]
    [InlineData("A", 1)]
    [InlineData("B", 2)]
    [InlineData("C", 3)]
    [InlineData("D", 4)]
    [InlineData("E", 5)]
    public void When_LD_IXY_R_InstructionIsExecuted_RegisterIsUpdated(string register, byte value)
    {
        var z80 = new Z80Builder()
            .Code(
                $"LD {register},{value}",
                $"LD IXH,{register}",
                $"LD IXL,{register}",
                $"LD IYH,{register}",
                $"LD IYL,{register}")
            .Build();

        z80.Run(7 + 4 * 8);

        z80.Registers.IXH.Should().Be(value);
        z80.Registers.IXL.Should().Be(value);
        z80.Registers.IYH.Should().Be(value);
        z80.Registers.IYL.Should().Be(value);
        z80.Clock.TotalTicks.Should().Be(39);
    }

    [Fact]
    public void When_LD_IXL_IXH_InstructionIsExecuted_RegisterIsUpdated()
    {
        var z80 = new Z80Builder()
            .Code(
                "LD IXH,1",
                "LD IXL,IXH")
            .Build();

        z80.Run(11 + 8);

        z80.Registers.IXH.Should().Be(1);
        z80.Registers.IXL.Should().Be(1);
        z80.Clock.TotalTicks.Should().Be(19);
    }

    [Fact]
    public void When_LD_IXH_IXL_InstructionIsExecuted_RegisterIsUpdated()
    {
        var z80 = new Z80Builder()
            .Code(
                "LD IXL,1",
                "LD IXH,IXL")
            .Build();

        z80.Run(11 + 8);

        z80.Registers.IXH.Should().Be(1);
        z80.Registers.IXL.Should().Be(1);
        z80.Clock.TotalTicks.Should().Be(19);
    }

    [Fact]
    public void When_LD_IYL_IYH_InstructionIsExecuted_RegisterIsUpdated()
    {
        var z80 = new Z80Builder()
            .Code(
                "LD IYH,1",
                "LD IYL,IXH")
            .Build();

        z80.Run(11 + 8);

        z80.Registers.IYH.Should().Be(1);
        z80.Registers.IYL.Should().Be(1);
        z80.Clock.TotalTicks.Should().Be(19);
    }

    [Fact]
    public void When_LD_IYH_IYL_InstructionIsExecuted_RegisterIsUpdated()
    {
        var z80 = new Z80Builder()
            .Code(
                "LD IYL,1",
                "LD IYH,IXL")
            .Build();

        z80.Run(11 + 8);

        z80.Registers.IYH.Should().Be(1);
        z80.Registers.IYL.Should().Be(1);
        z80.Clock.TotalTicks.Should().Be(19);
    }

    [Fact]
    public void When_LD_R_HL_InstructionIsExecuted_RegisterIsUpdated()
    {
        // A,B,C,D,E
        var z80 = new Z80Builder()
            .Code(
                "LD H,0x00",
                "LD L,0x09",
                "LD A,(HL)",
                "LD B,(HL)",
                "LD C,(HL)",
                "LD D,(HL)",
                "LD E,(HL)",
                "db 0x78")
            .Build();

        z80.Run(49);

        z80.Registers.A.Should().Be(0x78);
        z80.Registers.B.Should().Be(0x78);
        z80.Registers.C.Should().Be(0x78);
        z80.Registers.D.Should().Be(0x78);
        z80.Registers.E.Should().Be(0x78);
        z80.Clock.TotalTicks.Should().Be(49);

        // H register
        z80 = new Z80Builder()
            .Code(
                "LD H,0x00",
                "LD L,0x05",
                "LD H,(HL)",
                "db 0x78")
            .Build();

        z80.Run(21);

        z80.Registers.H.Should().Be(0x78);
        z80.Clock.TotalTicks.Should().Be(21);

        // L register
        z80 = new Z80Builder()
            .Code(
                "LD H,0x00",
                "LD L,0x05",
                "LD L,(HL)",
                "db 0x78")
            .Build();

        z80.Run(21);

        z80.Registers.L.Should().Be(0x78);
        z80.Clock.TotalTicks.Should().Be(21);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_LD_R_IXY_InstructionIsExecuted_RegisterIsUpdated(string register)
    {
        var z80 = new Z80Builder()
            .Code(
                $"LD {register}H,0x00",
                $"LD {register}L,0x06",
                $"LD A,({register}+21)",
                $"LD B,({register}+21)",
                $"LD C,({register}+21)",
                $"LD D,({register}+21)",
                $"LD E,({register}+21)",
                $"LD H,({register}+21)",
                $"LD L,({register}+21)",
                "db 0x78")
            .Build();

        z80.Run(2 * 11 + 7 * 19);

        z80.Registers.A.Should().Be(0x78);
        z80.Registers.B.Should().Be(0x78);
        z80.Registers.C.Should().Be(0x78);
        z80.Registers.D.Should().Be(0x78);
        z80.Registers.E.Should().Be(0x78);
        z80.Registers.H.Should().Be(0x78);
        z80.Registers.L.Should().Be(0x78);
        z80.Clock.TotalTicks.Should().Be(155);
    }

    [Fact]
    public void When_LD_MemoryHL_R_InstructionIsExecuted_MemoryIsUpdated()
    {
        var builder = new Z80Builder()
            .Code(
                "LD A,0x99",
                "LD HL,0x07",
                "LD (HL),A",
                "NOP",
                "db 0");

        var z80 = builder.Build();
        var memory = builder.Memory!;

        z80.Run(7 + 10 + 7);
        memory[0x07].Should().Be(0x99);
        z80.Clock.TotalTicks.Should().Be(24);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_LD_MemoryIXY_R_InstructionIsExecuted_MemoryIsUpdated(string register)
    {
        var builder = new Z80Builder()
            .Code(
                "LD A,0x99",
                $"LD {register},7",
                $"LD ({register}+3),A",
                "NOP",
                "db 0");

        var z80 = builder.Build();
        var memory = builder.Memory!;

        z80.Run(7 + 14 + 19);
        memory[0x07].Should().Be(0x99);
        z80.Clock.TotalTicks.Should().Be(40);
    }

    [Fact]
    public void When_LD_MemoryHL_n_InstructionIsExecuted_MemoryIsUpdated()
    {
        var builder = new Z80Builder()
            .Code(
                "LD HL,6",
                "LD (HL),0x5A",
                "NOP",
                "db 0");

        var z80 = builder.Build();
        var memory = builder.Memory!;

        z80.Run(10 + 10);
        memory[0x06].Should().Be(0x5A);
        z80.Clock.TotalTicks.Should().Be(20);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_LD_MemoryIXY_n_InstructionIsExecuted_MemoryIsUpdated(string register)
    {
        var builder = new Z80Builder()
            .Code(
                $"LD {register},6",
                $"LD ({register}+3),0x5A",
                "NOP",
                "db 0");

        var z80 = builder.Build();
        var memory = builder.Memory!;

        z80.Run(14 + 19);
        memory[0x09].Should().Be(0x5A);
        z80.Clock.TotalTicks.Should().Be(33);
    }

    [Theory]
    [InlineData("BC")]
    [InlineData("DE")]
    public void When_LD_A_MemoryRR_InstructionIsExecuted_AccumulatorIsUpdated(string register)
    {
        var z80 = new Z80Builder()
            .Code(
                $"LD {register},0x05",
                $"LD A,({register})",
                "NOP",
                "db 0x78")
            .Build();

        z80.Run(10 + 7);
        z80.Registers.A.Should().Be(0x78);
        z80.Clock.TotalTicks.Should().Be(17);
    }

    [Fact]
    public void When_LD_A_MemoryAddress_InstructionIsExecuted_AccumulatorIsUpdated()
    {
        var z80 = new Z80Builder()
            .Code(
                "LD A,(4)",
                "NOP",
                "db 0x78")
            .Build();

        z80.Run(13);
        z80.Registers.A.Should().Be(0x78);
        z80.Clock.TotalTicks.Should().Be(13);
    }

    [Theory]
    [InlineData("BC")]
    [InlineData("DE")]
    public void When_LD_MemoryRR_A_InstructionIsExecuted_AccumulatorIsUpdated(string register)
    {
        var builder = new Z80Builder()
            .Code(
                "LD A,0x99",
                $"LD {register},0x07",
                $"LD ({register}),A",
                "NOP",
                "db 0");

        var z80 = builder.Build();
        var memory = builder.Memory!;

        z80.Run(7 + 10 + 7);
        memory[0x07].Should().Be(0x99);
        z80.Clock.TotalTicks.Should().Be(24);
    }

    [Fact]
    public void When_LD_Memory_A_InstructionIsExecuted_AccumulatorIsUpdated()
    {
        var builder = new Z80Builder()
            .Code(
                "LD A,0x99",
                "LD (6),A",
                "NOP",
                "db 0");

        var z80 = builder.Build();
        var memory = builder.Memory!;

        z80.Run(7 + 13);
        memory[0x06].Should().Be(0x99);
        z80.Clock.TotalTicks.Should().Be(20);
    }

    [Fact]
    public void When_LD_I_A_InstructionIsExecuted_InterruptRegisterIsUpdated()
    {
        var z80 = new Z80Builder()
            .Code(
                "LD A,0x99",
                "LD I,A")
            .Build();

        z80.Run(7 + 9);

        z80.Registers.I.Should().Be(0x99);
        z80.Clock.TotalTicks.Should().Be(16);
    }

    [Theory]
    [InlineData(0, H | N | C, Z | P | C)]
    [InlineData(0xFF, None, S | P | Y | X)]
    public void When_LD_A_I_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet(byte value, Flags flags, Flags expectedFlags)
    {
        var z80 = new Z80Builder()
            .Flags(flags)
            .Iff2(true)
            .Code(
                $"LD A,{value}",
                "LD I,A",
                "LD A,I")
            .Build();

        z80.Run(7 + 9 + 9);

        z80.Registers.A.Should().Be(value);
        z80.Registers.F.Should().Be(expectedFlags);
        z80.Clock.TotalTicks.Should().Be(25);
    }

    [Fact]
    public void When_LD_R_A_InstructionIsExecuted_RefreshRegisterIsUpdated()
    {
        var z80 = new Z80Builder()
            .Code(
                "LD A,0x99",
                "LD R,A")
            .Build();

        z80.Run(7 + 9);

        z80.Registers.R.Should().Be(0x99);
        z80.Clock.TotalTicks.Should().Be(16);
    }


    [Fact]
    public void When_LD_A_R_InstructionIsExecuted_AccumulatorIsUpdatedAndFlagsSet()
    {
        var z80 = new Z80Builder()
            .Flags(All)
            .Iff2(true)
            .Code("LD A,R")
            .Build();

        z80.Run(9);

        z80.Registers.A.Should().Be(2);
        z80.Registers.F.Should().Be(C | P | S);
        z80.Clock.TotalTicks.Should().Be(9);
    }
}
