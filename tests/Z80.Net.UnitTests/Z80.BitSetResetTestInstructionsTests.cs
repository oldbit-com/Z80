using OldBit.Z80.Net.Registers;
using OldBit.Z80.Net.UnitTests.Extensions;
using OldBit.Z80.Net.UnitTests.Fixtures;

namespace OldBit.Z80.Net.UnitTests;

public class Z80BitSetResetTestInstructionsTests
{
    [Theory]
    [InlineData(6, 0x40, Z | N | C, H | C)]
    [InlineData(0, 0xFE, None, Z | H | P)]
    [InlineData(5, 0x20, All, Y | H | C)]
    public void When_BIT_n_A_InstructionIsExecuted_FlagsAreSet(
        int bit, byte value, Flags flags, Flags expectedFlags)
    {
        var z80 = new Z80Builder()
            .Flags(flags)
            .Code(
                $"LD A,{value}",
                $"BIT {bit},A")
            .Build();

        z80.Run(7 + 8);

        z80.Registers.F.Should().Be(expectedFlags);
        z80.Cycles.TotalCycles.Should().Be(15);
    }

    [Theory]
    [MemberData(nameof(BitTestData))]
    public void When_BIT_n_R_InstructionIsExecuted_FlagsAreSet(string register, int bit, Flags expectedFlags)
    {
        var z80 = new Z80Builder()
            .Flags(All)
            .Code(
                $"LD {register},0xFF",
                $"BIT {bit},{register}")
            .Build();

        z80.Run(7 + 8);

        z80.Registers.F.Should().Be(expectedFlags);
        z80.Cycles.TotalCycles.Should().Be(15);
    }

    [Theory]
    [InlineData(0, H)]
    [InlineData(1, H | Z | P)]
    [InlineData(2, H)]
    [InlineData(3, H)]
    [InlineData(4, H)]
    [InlineData(5, H)]
    [InlineData(6, H)]
    [InlineData(7, H | S)]
    public void When_BIT_n_HL_InstructionIsExecuted_FlagsAreSet(int bit, Flags expectedFlags)
    {
        var z80 = new Z80Builder()
            .Flags(Z | N)
            .Code(
                "LD HL,0x06",
                $"BIT {bit},(HL)",
                "NOP",
                "db 0xFD")
            .Build();

        z80.Run(10 + 12);

        z80.Registers.F.Should().Be(expectedFlags);
        z80.Cycles.TotalCycles.Should().Be(22);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_BIT_n_IXY_InstructionIsExecuted_FlagsAreSet(string register)
    {
        var z80 = new Z80Builder()
            .Flags(Z | N)
            .Code(
                $"LD {register},0x07",
                $"BIT 2,({register}+2)",
                "NOP",
                "db 0xFD")
            .Build();

        z80.Run(14 + 20);

        z80.Registers.F.Should().Be(H);
        z80.Cycles.TotalCycles.Should().Be(34);
    }

    [Theory]
    [MemberData(nameof(SetResTestData))]
    public void When_SET_n_R_InstructionIsExecuted_BitIsSet(string register, int bit)
    {
        var z80 = new Z80Builder()
            .Flags(All)
            .Code(
                $"LD {register},0x00",
                $"SET {bit},{register}")
            .Build();

        z80.Run(7 + 8);

        z80.Registers.ValueOf(register).Should().Be((byte)(1 << bit));
        z80.Cycles.TotalCycles.Should().Be(15);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    public void When_SET_n_HL_InstructionIsExecuted_BitIsSet(int bit)
    {
        var builder = new Z80Builder()
            .Code(
                "LD HL,5",
                $"SET {bit},(HL)",
                "db 0");

        var z80 = builder.Build();
        var memory = builder.Memory!;

        z80.Run(10 + 15);

        memory[5].Should().Be((byte)(1 << bit));
        z80.Cycles.TotalCycles.Should().Be(25);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_SET_n_IXY_InstructionIsExecuted_BitIsSet(string register)
    {
        var builder = new Z80Builder()
            .Code(
                $"LD {register},0x07",
                $"SET 2,({register}+2)",
                "NOP",
                "db 0");

        var z80 = builder.Build();
        var memory = builder.Memory!;

        z80.Run(14 + 23);

        memory[9].Should().Be(0b00000100);
        z80.Cycles.TotalCycles.Should().Be(37);
    }

    [Theory]
    [MemberData(nameof(SetResTestData))]
    public void When_RES_n_R_InstructionIsExecuted_BitIsReset(string register, int bit)
    {
        var z80 = new Z80Builder()
            .Flags(All)
            .Code(
                $"LD {register},0xFF",
                $"RES {bit},{register}")
            .Build();

        z80.Run(7 + 8);

        z80.Registers.ValueOf(register).Should().Be((byte)(0xFF & ~(1 << bit)));
        z80.Cycles.TotalCycles.Should().Be(15);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    public void When_RES_n_HL_InstructionIsExecuted_BitIsReset(int bit)
    {
        var builder = new Z80Builder()
            .Code(
                "LD HL,5",
                $"RES {bit},(HL)",
                "db 0xFF");

        var z80 = builder.Build();
        var memory = builder.Memory!;

        z80.Run(10 + 15);

        memory[5].Should().Be((byte)(0xFF & ~(1 << bit)));
        z80.Cycles.TotalCycles.Should().Be(25);
    }

    [Theory]
    [InlineData("IX")]
    [InlineData("IY")]
    public void When_RES_n_IXY_InstructionIsExecuted_BitReset(string register)
    {
        var builder = new Z80Builder()
            .Code(
                $"LD {register},0x07",
                $"RES 2,({register}+2)",
                "NOP",
                "db 0xFF");

        var z80 = builder.Build();
        var memory = builder.Memory!;

        z80.Run(14 + 23);

        memory[9].Should().Be(0b11111011);
        z80.Cycles.TotalCycles.Should().Be(37);
    }

    public static IEnumerable<object[]> BitTestData()
    {
        var bits = Enumerable.Range(0, 8);
        var registers = new[] { "A", "B", "C", "D", "E", "H", "L" };
        foreach (var bit in bits)
        {
            foreach (var register in registers)
            {
                var flags = H | C;
                flags |= bit == 7 ? S : 0;
                flags |= bit == 3 ? X : 0;
                flags |= bit == 5 ? Y : 0;

                yield return [register, bit, flags];
            }
        }
    }

    public static IEnumerable<object[]> SetResTestData()
    {
        var bits = Enumerable.Range(0, 8);
        var registers = new[] { "A", "B", "C", "D", "E", "H", "L" };
        foreach (var bit in bits)
        {
            foreach (var register in registers)
            {
                yield return [register, bit];
            }
        }
    }
}
