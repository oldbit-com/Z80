namespace Z80.Net;

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
        _opCodes["LD A,H"] = () => Registers.A = Registers.H;
        _opCodes["LD A,L"] = () => Registers.A = Registers.L;

        _opCodes["LD B,A"] = () => Registers.B = Registers.A;
        _opCodes["LD B,B"] = () => { };
        _opCodes["LD B,C"] = () => Registers.B = Registers.C;
        _opCodes["LD B,D"] = () => Registers.B = Registers.D;
        _opCodes["LD B,E"] = () => Registers.B = Registers.E;
        _opCodes["LD B,H"] = () => Registers.B = Registers.H;
        _opCodes["LD B,L"] = () => Registers.B = Registers.L;

        _opCodes["LD C,A"] = () => Registers.C = Registers.A;
        _opCodes["LD C,B"] = () => Registers.C = Registers.B;
        _opCodes["LD C,C"] = () => { };
        _opCodes["LD C,D"] = () => Registers.C = Registers.D;
        _opCodes["LD C,E"] = () => Registers.C = Registers.E;
        _opCodes["LD C,H"] = () => Registers.C = Registers.H;
        _opCodes["LD C,L"] = () => Registers.C = Registers.L;

        _opCodes["LD D,A"] = () => Registers.D = Registers.A;
        _opCodes["LD D,B"] = () => Registers.D = Registers.B;
        _opCodes["LD D,C"] = () => Registers.D = Registers.C;
        _opCodes["LD D,D"] = () => { };
        _opCodes["LD D,E"] = () => Registers.D = Registers.E;
        _opCodes["LD D,H"] = () => Registers.D = Registers.H;
        _opCodes["LD D,L"] = () => Registers.D = Registers.L;

        _opCodes["LD E,A"] = () => Registers.E = Registers.A;
        _opCodes["LD E,B"] = () => Registers.E = Registers.B;
        _opCodes["LD E,C"] = () => Registers.E = Registers.C;
        _opCodes["LD E,D"] = () => Registers.E = Registers.D;
        _opCodes["LD E,E"] = () => { };
        _opCodes["LD E,H"] = () => Registers.E = Registers.H;
        _opCodes["LD E,L"] = () => Registers.E = Registers.L;

        _opCodes["LD H,A"] = () => Registers.H = Registers.A;
        _opCodes["LD H,B"] = () => Registers.H = Registers.B;
        _opCodes["LD H,C"] = () => Registers.H = Registers.C;
        _opCodes["LD H,D"] = () => Registers.H = Registers.D;
        _opCodes["LD H,E"] = () => Registers.H = Registers.E;
        _opCodes["LD H,H"] = () => { };
        _opCodes["LD H,L"] = () => Registers.H = Registers.L;

        _opCodes["LD L,A"] = () => Registers.L = Registers.A;
        _opCodes["LD L,B"] = () => Registers.L = Registers.B;
        _opCodes["LD L,C"] = () => Registers.L = Registers.C;
        _opCodes["LD L,D"] = () => Registers.L = Registers.D;
        _opCodes["LD L,E"] = () => Registers.L = Registers.E;
        _opCodes["LD L,H"] = () => Registers.L = Registers.H;
        _opCodes["LD L,L"] = () => { };

        _opCodes["LD A,(HL)"] = () => Registers.A = ReadByteAtExtendedHL(extraIndexStates: 5);
        _opCodes["LD B,(HL)"] = () => Registers.B = ReadByteAtExtendedHL(extraIndexStates: 5);
        _opCodes["LD C,(HL)"] = () => Registers.C = ReadByteAtExtendedHL(extraIndexStates: 5);
        _opCodes["LD D,(HL)"] = () => Registers.D = ReadByteAtExtendedHL(extraIndexStates: 5);
        _opCodes["LD E,(HL)"] = () => Registers.E = ReadByteAtExtendedHL(extraIndexStates: 5);
        _opCodes["LD H,(HL)"] = () => Registers.H = ReadByteAtExtendedHL(extraIndexStates: 5);
        _opCodes["LD L,(HL)"] = () => Registers.L = ReadByteAtExtendedHL(extraIndexStates: 5);

        _opCodes["LD (HL),A"] = () => { WriteByte(CalculateExtendedHL(extraIndexStates: 5), Registers.A); };
        _opCodes["LD (HL),B"] = () => { WriteByte(CalculateExtendedHL(extraIndexStates: 5), Registers.B); };
        _opCodes["LD (HL),C"] = () => { WriteByte(CalculateExtendedHL(extraIndexStates: 5), Registers.C); };
        _opCodes["LD (HL),D"] = () => { WriteByte(CalculateExtendedHL(extraIndexStates: 5), Registers.D); };
        _opCodes["LD (HL),E"] = () => { WriteByte(CalculateExtendedHL(extraIndexStates: 5), Registers.E); };
        _opCodes["LD (HL),H"] = () => { WriteByte(CalculateExtendedHL(extraIndexStates: 5), Registers.H); };
        _opCodes["LD (HL),L"] = () => { WriteByte(CalculateExtendedHL(extraIndexStates: 5), Registers.L); };
        _opCodes["LD (HL),n"] = () => { WriteByte(CalculateExtendedHL(extraIndexStates: 2), FetchByte()); };

        _opCodes["LD A,(BC)"] = () => Registers.A = ReadByte(Registers.BC);
        _opCodes["LD A,(DE)"] = () => Registers.A = ReadByte(Registers.DE);
        _opCodes["LD A,(nn)"] = () => Registers.A = ReadByte(FetchWord());

        _opCodes["LD (BC),A"] = () => WriteByte(Registers.BC, Registers.A);
        _opCodes["LD (DE),A"] = () => WriteByte(Registers.DE, Registers.A);
        _opCodes["LD (nn),A"] = () => WriteByte(FetchWord(), Registers.A);

        _opCodes["LD I,A"] = () => throw new NotImplementedException();
        _opCodes["LD A,I"] = () => throw new NotImplementedException();

        _opCodes["LD R,A"] = () => throw new NotImplementedException();
        _opCodes["LD A,R"] = () => throw new NotImplementedException();
    }
}