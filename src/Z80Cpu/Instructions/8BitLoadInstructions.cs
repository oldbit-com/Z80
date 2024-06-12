using OldBit.Z80Cpu.Registers;
using static OldBit.Z80Cpu.Registers.Flags;

namespace OldBit.Z80Cpu;

partial class Z80
{
    private void Add8BitLoadInstructions()
    {
        _opCodes["LD A,n"] = () => Registers.A = FetchByte();
        _opCodes["LD B,n"] = () => Registers.B = FetchByte();
        _opCodes["LD C,n"] = () => Registers.C = FetchByte();
        _opCodes["LD D,n"] = () => Registers.D = FetchByte();
        _opCodes["LD E,n"] = () => Registers.E = FetchByte();
        _opCodes["LD H,n"] = () => Registers.XH = FetchByte();
        _opCodes["LD L,n"] = () => Registers.XL = FetchByte();

        _opCodes["LD A,A"] = () => { };
        _opCodes["LD A,B"] = () => Registers.A = Registers.B;
        _opCodes["LD A,C"] = () => Registers.A = Registers.C;
        _opCodes["LD A,D"] = () => Registers.A = Registers.D;
        _opCodes["LD A,E"] = () => Registers.A = Registers.E;
        _opCodes["LD A,H"] = () => Registers.A = Registers.XH;
        _opCodes["LD A,L"] = () => Registers.A = Registers.XL;

        _opCodes["LD B,A"] = () => Registers.B = Registers.A;
        _opCodes["LD B,B"] = () => { };
        _opCodes["LD B,C"] = () => Registers.B = Registers.C;
        _opCodes["LD B,D"] = () => Registers.B = Registers.D;
        _opCodes["LD B,E"] = () => Registers.B = Registers.E;
        _opCodes["LD B,H"] = () => Registers.B = Registers.XH;
        _opCodes["LD B,L"] = () => Registers.B = Registers.XL;

        _opCodes["LD C,A"] = () => Registers.C = Registers.A;
        _opCodes["LD C,B"] = () => Registers.C = Registers.B;
        _opCodes["LD C,C"] = () => { };
        _opCodes["LD C,D"] = () => Registers.C = Registers.D;
        _opCodes["LD C,E"] = () => Registers.C = Registers.E;
        _opCodes["LD C,H"] = () => Registers.C = Registers.XH;
        _opCodes["LD C,L"] = () => Registers.C = Registers.XL;

        _opCodes["LD D,A"] = () => Registers.D = Registers.A;
        _opCodes["LD D,B"] = () => Registers.D = Registers.B;
        _opCodes["LD D,C"] = () => Registers.D = Registers.C;
        _opCodes["LD D,D"] = () => { };
        _opCodes["LD D,E"] = () => Registers.D = Registers.E;
        _opCodes["LD D,H"] = () => Registers.D = Registers.XH;
        _opCodes["LD D,L"] = () => Registers.D = Registers.XL;

        _opCodes["LD E,A"] = () => Registers.E = Registers.A;
        _opCodes["LD E,B"] = () => Registers.E = Registers.B;
        _opCodes["LD E,C"] = () => Registers.E = Registers.C;
        _opCodes["LD E,D"] = () => Registers.E = Registers.D;
        _opCodes["LD E,E"] = () => { };
        _opCodes["LD E,H"] = () => Registers.E = Registers.XH;
        _opCodes["LD E,L"] = () => Registers.E = Registers.XL;

        _opCodes["LD H,A"] = () => Registers.XH = Registers.A;
        _opCodes["LD H,B"] = () => Registers.XH = Registers.B;
        _opCodes["LD H,C"] = () => Registers.XH = Registers.C;
        _opCodes["LD H,D"] = () => Registers.XH = Registers.D;
        _opCodes["LD H,E"] = () => Registers.XH = Registers.E;
        _opCodes["LD H,H"] = () => { };
        _opCodes["LD H,L"] = () => Registers.XH = Registers.XL;

        _opCodes["LD L,A"] = () => Registers.XL = Registers.A;
        _opCodes["LD L,B"] = () => Registers.XL = Registers.B;
        _opCodes["LD L,C"] = () => Registers.XL = Registers.C;
        _opCodes["LD L,D"] = () => Registers.XL = Registers.D;
        _opCodes["LD L,E"] = () => Registers.XL = Registers.E;
        _opCodes["LD L,H"] = () => Registers.XL = Registers.XH;
        _opCodes["LD L,L"] = () => { };

        _opCodes["LD A,(HL)"] = () => Registers.A = ReadByteAtExtendedHL(extraIndexCycles: 5);
        _opCodes["LD B,(HL)"] = () => Registers.B = ReadByteAtExtendedHL(extraIndexCycles: 5);
        _opCodes["LD C,(HL)"] = () => Registers.C = ReadByteAtExtendedHL(extraIndexCycles: 5);
        _opCodes["LD D,(HL)"] = () => Registers.D = ReadByteAtExtendedHL(extraIndexCycles: 5);
        _opCodes["LD E,(HL)"] = () => Registers.E = ReadByteAtExtendedHL(extraIndexCycles: 5);
        _opCodes["LD H,(HL)"] = () => Registers.H = ReadByteAtExtendedHL(extraIndexCycles: 5);
        _opCodes["LD L,(HL)"] = () => Registers.L = ReadByteAtExtendedHL(extraIndexCycles: 5);

        _opCodes["LD (HL),A"] = () => { WriteByte(CalculateExtendedHL(extraIndexCycles: 5), Registers.A); };
        _opCodes["LD (HL),B"] = () => { WriteByte(CalculateExtendedHL(extraIndexCycles: 5), Registers.B); };
        _opCodes["LD (HL),C"] = () => { WriteByte(CalculateExtendedHL(extraIndexCycles: 5), Registers.C); };
        _opCodes["LD (HL),D"] = () => { WriteByte(CalculateExtendedHL(extraIndexCycles: 5), Registers.D); };
        _opCodes["LD (HL),E"] = () => { WriteByte(CalculateExtendedHL(extraIndexCycles: 5), Registers.E); };
        _opCodes["LD (HL),H"] = () => { WriteByte(CalculateExtendedHL(extraIndexCycles: 5), Registers.H); };
        _opCodes["LD (HL),L"] = () => { WriteByte(CalculateExtendedHL(extraIndexCycles: 5), Registers.L); };
        _opCodes["LD (HL),n"] = () => { WriteByte(CalculateExtendedHL(extraIndexCycles: 2), FetchByte()); };

        _opCodes["LD A,(BC)"] = () => Registers.A = ReadByte(Registers.BC);
        _opCodes["LD A,(DE)"] = () => Registers.A = ReadByte(Registers.DE);
        _opCodes["LD A,(nn)"] = () => Registers.A = ReadByte(FetchWord());

        _opCodes["LD (BC),A"] = () => WriteByte(Registers.BC, Registers.A);
        _opCodes["LD (DE),A"] = () => WriteByte(Registers.DE, Registers.A);
        _opCodes["LD (nn),A"] = () => WriteByte(FetchWord(), Registers.A);

        _opCodes["LD I,A"] = () =>
        {
            Cycles.Add(1);
            Registers.I = Registers.A;
        };
        _opCodes["LD A,I"] = () =>
        {
            Cycles.Add(1);
            Registers.A = Registers.I;
            Registers.F &= C;
            Registers.F |= Registers.A == 0 ? Z : 0;
            Registers.F |= IFF2 ? P : 0;
            Registers.F |= (Flags)Registers.A & S;
        };

        _opCodes["LD R,A"] = () =>
        {
            Cycles.Add(1);
            Registers.R = Registers.A;
        };
        _opCodes["LD A,R"] = () =>
        {
            Cycles.Add(1);
            Registers.A = Registers.R;
            Registers.F &= C | S;
            Registers.F |= Registers.A == 0 ? Z : 0;
            Registers.F |= IFF2 ? P : 0;
            Registers.F |= (Flags)(Registers.A & (byte)S);
        };
    }
}
