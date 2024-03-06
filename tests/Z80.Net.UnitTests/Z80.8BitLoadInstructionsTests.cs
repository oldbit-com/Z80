namespace Z80.Net.UnitTests;

public class Z808BitLoadInstructionsTests
{
    [Fact]
    public void When_LD_R_n_InstructionIsExecuted_RegisterValueIsUpdated()
    {
        var z80 = new CodeBuilder()
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
        z80.CycleCounter.TotalCycles.Should().Be(93);
    }

    [Theory]
    [InlineData("A", 1)]
    [InlineData("B", 2)]
    [InlineData("C", 3)]
    [InlineData("D", 4)]
    [InlineData("E", 5)]
    [InlineData("H", 6)]
    [InlineData("L", 7)]
    public void When_LD_R_R_InstructionIsExecuted_RegisterValueIsUpdated(string register, byte value)
    {
        var z80 = new CodeBuilder()
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
        z80.CycleCounter.TotalCycles.Should().Be(35);
    }

    [Fact]
    public void When_LD_R_HL_InstructionIsExecuted_RegisterValueIsUpdated()
    {
        // A,B,C,D,E
        var z80 = new CodeBuilder()
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
        z80.CycleCounter.TotalCycles.Should().Be(49);

        // H register
        z80 = new CodeBuilder()
            .Code(
                "LD H,0x00",
                "LD L,0x05",
                "LD H,(HL)",
                "db 0x78")
            .Build();

        z80.Run(21);

        z80.Registers.H.Should().Be(0x78);
        z80.CycleCounter.TotalCycles.Should().Be(21);

        // L register
        z80 = new CodeBuilder()
            .Code(
                "LD H,0x00",
                "LD L,0x05",
                "LD L,(HL)",
                "db 0x78")
            .Build();

        z80.Run(21);

        z80.Registers.L.Should().Be(0x78);
        z80.CycleCounter.TotalCycles.Should().Be(21);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_LD_R_IXY_InstructionIsExecuted_RegisterValueIsUpdated(string register)
    {
        var z80 = new CodeBuilder()
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
        z80.CycleCounter.TotalCycles.Should().Be(155);
    }

    [Fact]
    public void When_LD_MemoryHL_R_InstructionIsExecuted_MemoryValueIsUpdated()
    {
        var builder = new CodeBuilder()
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
        z80.CycleCounter.TotalCycles.Should().Be(24);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_LD_MemoryIXY_R_InstructionIsExecuted_MemoryValueIsUpdated(string register)
    {
        var builder = new CodeBuilder()
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
        z80.CycleCounter.TotalCycles.Should().Be(40);
    }
}