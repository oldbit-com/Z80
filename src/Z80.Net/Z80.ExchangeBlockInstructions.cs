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

        _opCodes["EX AF,AF"] = () => (Registers.A, Registers.Alternative.A, Registers.F, Registers.Alternative.F) =
            (Registers.Alternative.A, Registers.A, Registers.Alternative.F, Registers.F);

        _opCodes["EXX"] = () =>
        {
            (Registers.B, Registers.Alternative.B, Registers.C, Registers.Alternative.C) =
                (Registers.Alternative.B, Registers.B, Registers.Alternative.C, Registers.C);

            (Registers.D, Registers.Alternative.D, Registers.E, Registers.Alternative.E) =
                (Registers.Alternative.D, Registers.D, Registers.Alternative.E, Registers.E);

            (Registers.H, Registers.Alternative.H, Registers.L, Registers.Alternative.L) =
                (Registers.Alternative.H, Registers.H, Registers.Alternative.L, Registers.L);
        };

        _opCodes["EX (SP),HL"] = () =>
        {
            var (x, y) = (ReadByte(Registers.SP), ReadByte(Registers.SP + 1));
            var (h, l) = (HX: Registers.XH, LX: Registers.XL);
            AddStates(1);
            WriteByte(Registers.SP, l);
            WriteByte(Registers.SP + 1, h);
            AddStates(2);
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
        var hl = Registers.XHL;
        var de = Registers.DE;
        var bc = Registers.BC - 1;
        var n = ReadByte(hl);

        WriteByte(de, n);
        AddStates(2);

        if (increment)
        {
            Registers.XHL = (Word)(hl + 1);
            Registers.DE = (Word)(de + 1);
        }
        else
        {
            Registers.XHL = (Word)(hl - 1);
            Registers.DE = (Word)(de - 1);
        }
        Registers.BC = (Word)bc;

        Registers.F &= S | Z | C;
        n += Registers.A;
        if ((n & 0b0000010) != 0) Registers.F |= Y;
        if ((n & 0b0001000) != 0) Registers.F |= X;
        Registers.F |= bc != 0 ? P : 0;
    }

    private void Execute_LDIR_LDDR(bool increment)
    {
        Execute_LDI_LDD(increment);

        if (Registers.BC == 0)
        {
            return;
        }

        Registers.PC -= 2;
        AddStates(5);
    }

    private void ExecuteBlockCompare(string opCode)
    {
        var hl = Registers.XHL;
        var bc = Registers.BC - 1;

        var offset = opCode is "CPI" or "CPIR" ? 1 : -1;
        Registers.XHL = (Word)(hl + offset);
        Registers.BC = (Word)bc;

        var n = ReadByte(hl);
        AddStates(5);
        var test = Registers.A - n;
        Registers.F = Registers.F & C | N | (Flags)(test & (int)S);
        if (test == 0) Registers.F |= Z;

        Registers.F |= (Flags)(Registers.A ^ n ^ test) & H;
        if (bc != 0) Registers.F |= P;

        n = (byte)(test - (int)(Registers.F & H) >> 4);

        // TODO: Implement
    }
}