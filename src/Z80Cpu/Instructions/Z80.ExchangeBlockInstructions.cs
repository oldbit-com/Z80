using OldBit.Z80Cpu.Registers;
using static OldBit.Z80Cpu.Registers.Flags;

namespace OldBit.Z80Cpu;

partial class Z80
{
    private void AddExchangeBlockInstructions()
    {
        _opCodes["EX DE,HL"] = () =>
        {
            (Registers.HL, Registers.DE) =
                (Registers.DE, Registers.HL);
        };

        _opCodes["EX AF,AF"] = () => (Registers.A, Registers.Prime.A, Registers.F, Registers.Prime.F) =
            (Registers.Prime.A, Registers.A, Registers.Prime.F, Registers.F);

        _opCodes["EXX"] = () =>
        {
            (Registers.B, Registers.Prime.B, Registers.C, Registers.Prime.C) =
                (Registers.Prime.B, Registers.B, Registers.Prime.C, Registers.C);

            (Registers.D, Registers.Prime.D, Registers.E, Registers.Prime.E) =
                (Registers.Prime.D, Registers.D, Registers.Prime.E, Registers.E);

            (Registers.H, Registers.Prime.H, Registers.L, Registers.Prime.L) =
                (Registers.Prime.H, Registers.H, Registers.Prime.L, Registers.L);
        };

        _opCodes["EX (SP),HL"] = () =>
        {
            var (x, y) = (ReadByte(Registers.SP), ReadByte((Word)(Registers.SP + 1)));
            var (h, l) = (Registers.XH, Registers.XL);

            Cycles.Add(1);

            WriteByte(Registers.SP, l);
            WriteByte((Word)(Registers.SP + 1), h);

            Cycles.Add(2);

            (Registers.XH, Registers.XL) = (y, x);
        };

        _opCodes["LDI"] = () => Execute_LDI_LDD(increment: true);
        _opCodes["LDIR"] = () => Execute_LDIR_LDDR(increment: true);
        _opCodes["LDD"] = () => Execute_LDI_LDD(increment: false);
        _opCodes["LDDR"] = () => Execute_LDIR_LDDR(increment: false);

        _opCodes["CPI"] = () => Execute_CPI_CPD(increment: true);
        _opCodes["CPIR"] = () => Execute_CPIR_CPDR(increment: true);
        _opCodes["CPD"] = () => Execute_CPI_CPD(increment: false);
        _opCodes["CPDR"] = () => Execute_CPIR_CPDR(increment: false);
    }

    private void Execute_LDI_LDD(bool increment)
    {
        var value = ReadByte(Registers.HL);

        WriteByte(Registers.DE, value);

        Cycles.Add(2);

        if (increment)
        {
            Registers.HL += 1;
            Registers.DE += 1;
        }
        else
        {
            Registers.HL -= 1;
            Registers.DE -= 1;
        }

        Registers.BC -= 1;

        Registers.F &= S | Z | C;
        value += Registers.A;
        Registers.F |= (value & 0b0000010) != 0 ? Y : 0;
        Registers.F |= (value & 0b0001000) != 0 ? X : 0;
        Registers.F |= Registers.BC != 0 ? P : 0;
    }

    private void Execute_LDIR_LDDR(bool increment)
    {
        Execute_LDI_LDD(increment);

        if (Registers.BC == 0)
        {
            return;
        }

        Registers.PC -= 2;

        Cycles.Add(5);
    }

    private int Execute_CPI_CPD(bool increment)
    {
        var value = ReadByte(Registers.HL);

        Cycles.Add(5);

        if (increment)
        {
            Registers.HL += 1;
        }
        else
        {
            Registers.HL -= 1;
        }

        Registers.BC -= 1;

        var result = Registers.A - value;

        Registers.F &= C;
        Registers.F |= N | (Flags)(result & (byte)S);
        Registers.F |= result == 0 ? Z : 0;
        Registers.F |= (Flags)(Registers.A ^ value ^ result) & H;
        var n = result - ((Registers.F & H) == H ? 1 : 0);
        Registers.F |= ((n & 0b0000010) != 0) ? Y : 0;
        Registers.F |= ((n & 0b0001000) != 0) ? X : 0;
        Registers.F |= Registers.BC != 0 ? P : 0;

        return result;
    }

    private void Execute_CPIR_CPDR(bool increment)
    {
        var result = Execute_CPI_CPD(increment);

        if (Registers.BC == 0 || result == 0)
        {
            return;
        }

        Registers.PC -= 2;

        Cycles.Add(5);
    }
}
