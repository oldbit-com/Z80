using Z80.Net.Registers;
using static Z80.Net.Registers.Flags;
using static Z80.Net.Instructions.OpCodes;

namespace Z80.Net;

partial class Z80
{
    private void AddExchangeBlockInstructions()
    {
        _opCodes[EX_DE_HL] = () =>
        {
            (Registers.HL, Registers.DE) = (Registers.DE, Registers.HL);
        };

        _opCodes[EX_AF_AF] = () => (Registers.A, Registers.Alternative.A, Registers.F, Registers.Alternative.F) = (Registers.Alternative.A, Registers.A, Registers.Alternative.F, Registers.F);

        _opCodes[EXX] = () =>
        {
            (Registers.B, Registers.Alternative.B, Registers.C, Registers.Alternative.C) = (Registers.Alternative.B, Registers.B, Registers.Alternative.C, Registers.C);
            (Registers.D, Registers.Alternative.D, Registers.E, Registers.Alternative.E) = (Registers.Alternative.D, Registers.D, Registers.Alternative.E, Registers.E);
            (Registers.H, Registers.Alternative.H, Registers.L, Registers.Alternative.L) = (Registers.Alternative.H, Registers.H, Registers.Alternative.L, Registers.L);
        };

        _opCodes[EX_SP_HL] = () =>
        {
            var (x, y) = (ReadByte(Registers.SP), ReadByte(Registers.SP + 1));
            var (h, l) = (HX: Registers.XH, LX: Registers.XL);
            AddCycles(1);
            WriteByte(Registers.SP, l);
            WriteByte(Registers.SP + 1, h);
            AddCycles(2);
            (Registers.XH, Registers.XL) = (y, x);
        };

        _opCodes[LDI] = () => BlockLoadInstruction(LDI);
        _opCodes[LDIR] = () => BlockLoadInstruction(LDIR);
        _opCodes[LDD] = () => BlockLoadInstruction(LDD);
        _opCodes[LDDR] = () => BlockLoadInstruction(LDDR);

        _opCodes[CPI] = () => BlockCompareInstruction(CPI);
        _opCodes[CPIR] = () => BlockCompareInstruction(CPIR);
        _opCodes[CPD] = () => BlockCompareInstruction(CPD);
        _opCodes[CPDR] = () => BlockCompareInstruction(CPDR);
    }

    private void BlockLoadInstruction(int opCode)
    {
        var hl = Registers.XHL;
        var de = Registers.DE;
        var bc = Registers.BC - 1;
        var n = ReadByte(hl);

        WriteByte(de, n);
        AddCycles(2);

        var offset = opCode == LDI || opCode == LDIR ? 1 : -1;
        Registers.XHL = (ushort)(hl + offset);
        Registers.DE = (ushort)(de + offset);
        Registers.BC = (ushort)bc;
        Registers.F &= S | Z | C;
        n += Registers.A;
        if ((n & 0b0000010) != 0) Registers.F |= Y;
        if ((n & 0b0001000) != 0) Registers.F |= X;
        if (bc == 0) return;
        Registers.F |= P;
        if (opCode == LDIR || opCode == LDDR) {
            Registers.PC -= 2;
            AddCycles(5);
        }
    }

    private void BlockCompareInstruction(int opCode)
    {
        var hl = Registers.XHL;
        var bc = Registers.BC - 1;

        var offset = opCode == CPI || opCode == CPIR ? 1 : -1;
        Registers.XHL = (ushort)(hl + offset);
        Registers.BC = (ushort)bc;

        var n = ReadByte(hl);
        AddCycles(5);
        var test = Registers.A - n;
        Registers.F = Registers.F & C | N | (Flags)(test & (int)S);
        if (test == 0) Registers.F |= Z;

        Registers.F |= (Flags)(Registers.A ^ n ^ test) & H;
        if (bc != 0) Registers.F |= P;

        n = (byte)(test - (int)(Registers.F & H) >> 4);
    }
}