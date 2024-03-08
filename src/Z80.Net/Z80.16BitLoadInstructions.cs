using Z80.Net.Registers;

namespace Z80.Net;

partial class Z80
{
    private void Add16BitLoadInstructions()
    {
        _opCodes["LD BC,nn"] = () => Registers.BC = ReadWordAndMove();
        _opCodes["LD DE,nn"] = () => Registers.DE = ReadWordAndMove();
        _opCodes["LD HL,nn"] = () => Registers.XHL = ReadWordAndMove();
        _opCodes["LD SP,nn"] = () => Registers.SP = ReadWordAndMove();

        _opCodes["LD HL,(nn)"] = () => Registers.XHL = ReadWord(ReadWordAndMove());
        _opCodes["LD BC,(nn)"] = () => Registers.BC = ReadWord(ReadWordAndMove());
        _opCodes["LD DE,(nn)"] = () => Registers.DE = ReadWord(ReadWordAndMove());
        _opCodes["LD SP,(nn)"] = () => Registers.SP = ReadWord(ReadWordAndMove());

        _opCodes["LD (nn),HL"] = () => throw new NotImplementedException();
        _opCodes["LD (nn),BC"] = () => throw new NotImplementedException();
        _opCodes["LD (nn),DE"] = () => throw new NotImplementedException();
        _opCodes["LD (nn),SP"] = () => throw new NotImplementedException();

        _opCodes["LD SP,HL"] = () => throw new NotImplementedException();

        _opCodes["ED LD HL,(nn)"] = _opCodes["LD HL,(nn)"];
        _opCodes["ED LD (nn),HL"] = _opCodes["LD (nn),HL"];

        _opCodes["PUSH AF"] = () => ExecutePUSH(Registers.A, (byte)Registers.F);
        _opCodes["PUSH BC"] = () => ExecutePUSH(Registers.B, Registers.C);
        _opCodes["PUSH DE"] = () => ExecutePUSH(Registers.D, Registers.E);
        _opCodes["PUSH HL"] = () => ExecutePUSH(Registers.XH, Registers.XL);

        _opCodes["POP AF"] = () =>
        {
            var (a, f) = ExecutePOP();
            (Registers.A, Registers.F) = (a, (Flags)f);
        };
        _opCodes["POP BC"] = () => (Registers.B, Registers.C) = ExecutePOP();
        _opCodes["POP DE"] = () => (Registers.D, Registers.E) = ExecutePOP();
        _opCodes["POP HL"] = () => (Registers.XH, Registers.XL) = ExecutePOP();
    }

    private void ExecutePUSH(byte highByte, byte lowByte)
    {
        AddStates(1);
        Registers.SP -= 1;
        WriteByte(Registers.SP, highByte);
        Registers.SP -= 1;
        WriteByte(Registers.SP, lowByte);
    }

    private (byte highByte, byte lowByte) ExecutePOP()
    {
        var highByte = ReadByte(Registers.SP + 1);
        var lowByte = ReadByte(Registers.SP);
        Registers.SP += 2;
        return (highByte, lowByte);
    }
}