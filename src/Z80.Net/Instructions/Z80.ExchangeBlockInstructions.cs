using Z80.Net.Registers;
using static Z80.Net.Registers.Flags;

namespace Z80.Net;

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
            var (h, l) = (HX: Registers.XH, LX: Registers.XL);

            States.Add(1);

            WriteByte(Registers.SP, l);
            WriteByte((Word)(Registers.SP + 1), h);

            States.Add(2);

            (Registers.XH, Registers.XL) = (y, x);
        };

        _opCodes["LDI"] = () => Execute_LDI_LDD(increment: true);
        _opCodes["LDIR"] = () => Execute_LDIR_LDDR(increment: true);
        _opCodes["LDD"] = () => Execute_LDI_LDD(increment: false);
        _opCodes["LDDR"] = () => Execute_LDIR_LDDR(increment: false);

        _opCodes["CPI"] = () => ExecuteBlockCompare("CPI");
        _opCodes["CPIR"] = () => ExecuteBlockCompare("CPIR");
        _opCodes["CPD"] = () => ExecuteBlockCompare("CPD");
        _opCodes["CPDR"] = () => ExecuteBlockCompare("CPDR");
    }

    private void Execute_LDI_LDD(bool increment)
    {
        var value = ReadByte(Registers.HL);

        WriteByte(Registers.DE, value);

        States.Add(2);

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
        Registers.F |= ((value & 0b0000010) != 0) ? Y : 0;
        Registers.F |= ((value & 0b0001000) != 0) ? X : 0;
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

        States.Add(5);
    }

    private void Execute_CPI_CPD(bool increment)
    {

    }

    private void ExecuteBlockCompare(string opCode)
    {
        var hl = Registers.XHL;
        var bc = Registers.BC - 1;

        var offset = opCode is "CPI" or "CPIR" ? 1 : -1;
        Registers.XHL = (Word)(hl + offset);
        Registers.BC = (Word)bc;

        var n = ReadByte(hl);

        States.Add(5);

        var test = Registers.A - n;
        Registers.F = Registers.F & C | N | (Flags)(test & (int)S);
        if (test == 0) Registers.F |= Z;

        Registers.F |= (Flags)(Registers.A ^ n ^ test) & H;
        if (bc != 0) Registers.F |= P;

        n = (byte)(test - (int)(Registers.F & H) >> 4);

        // TODO: Implement
    }
}