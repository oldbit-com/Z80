using OldBit.Z80.Net.Helpers;
using OldBit.Z80.Net.Registers;
using static OldBit.Z80.Net.Registers.Flags;

namespace OldBit.Z80.Net;

partial class Z80
{
    private void AddGeneralPurposeArithmeticInstructions()
    {
        _opCodes["DAA"] = () =>
        {
            var cf = Registers.F & C;
            var hf = Registers.F & H;
            var nf = Registers.F & N;
            var low4Bits = Registers.A & 0x0F;
            byte diff = 0;

            Registers.F &= N;
            if (cf != 0 || Registers.A > 0x99)
            {
                diff = 0x60;
                Registers.F |= C;
            }
            if (hf != 0 || low4Bits > 0x09)
            {
                diff += 0x06;
            }
            Registers.A = nf == 0 ? Registers.A += diff : Registers.A -= diff;
            if (Parity.Lookup[Registers.A] != 0) Registers.F |= P;
            Registers.F |= (S | Y | X) & (Flags)Registers.A;
            if (Registers.A == 0) Registers.F |= Z;
            if (nf == 0 && low4Bits > 0x09 || nf != 0 && hf != 0 && low4Bits < 0x06) Registers.F |= H;
        };

        _opCodes["CPL"] = () =>
        {
            Registers.A = (byte)~Registers.A;
            Registers.F &= S | Z | P | C;
            Registers.F |= H | N | (Y | X) & (Flags)Registers.A;
        };

        _opCodes["NEG"] = () =>
        {
            var a = Registers.A;
            Registers.A = (byte)(~a + 1);
            Registers.F = N;
            Registers.F |= (S | Y | X) & (Flags)Registers.A | H & (Flags)(a ^ Registers.A);
            Registers.F |= Registers.A == 0 ? Z : 0;
            Registers.F |= Registers.A == 0x80 ? P : 0;
            Registers.F |= (a != 0) ? C : 0;
        };

        _opCodes["CCF"] = () =>
        {
            var oldCarry = (Registers.F & C) == C;
            Registers.F = (Registers.F & (S | Z | P | C) | (Y | X) & (Flags)Registers.A) ^ C;
            Registers.F |= oldCarry ? H : 0;
        };

        _opCodes["SCF"] = () => Registers.F = Registers.F & (S | Z | P) | C | (Y | X) & (Flags)Registers.A;
    }
}