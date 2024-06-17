using OldBit.Z80Cpu.Registers;

namespace OldBit.Z80Cpu;

partial class Z80
{
    private void Add16BitLoadInstructions()
    {
        _opCodes["LD BC,nn"] = () => Registers.BC = FetchWord();
        _opCodes["LD DE,nn"] = () => Registers.DE = FetchWord();
        _opCodes["LD HL,nn"] = () => Registers.XHL = FetchWord();
        _opCodes["LD SP,nn"] = () => Registers.SP = FetchWord();

        _opCodes["LD HL,(nn)"] = () => Registers.XHL = ReadWord(FetchWord());
        _opCodes["LD BC,(nn)"] = () => Registers.BC = ReadWord(FetchWord());
        _opCodes["LD DE,(nn)"] = () => Registers.DE = ReadWord(FetchWord());
        _opCodes["LD SP,(nn)"] = () => Registers.SP = ReadWord(FetchWord());

        _opCodes["LD (nn),HL"] = () => WriteWord(FetchWord(), Registers.XHL);
        _opCodes["LD (nn),BC"] = () => WriteWord(FetchWord(), Registers.BC);
        _opCodes["LD (nn),DE"] = () => WriteWord(FetchWord(), Registers.DE);
        _opCodes["LD (nn),SP"] = () => WriteWord(FetchWord(), Registers.SP);

        _opCodes["LD SP,HL"] = () =>
        {
            States.MemoryContention(Registers.IR, 2);

            Registers.SP = Registers.XHL;
        };

        _opCodes["ED LD HL,(nn)"] = _opCodes["LD HL,(nn)"];
        _opCodes["ED LD (nn),HL"] = _opCodes["LD (nn),HL"];

        _opCodes["PUSH AF"] = () => Execute_PUSH(Registers.A, (byte)Registers.F);
        _opCodes["PUSH BC"] = () => Execute_PUSH(Registers.B, Registers.C);
        _opCodes["PUSH DE"] = () => Execute_PUSH(Registers.D, Registers.E);
        _opCodes["PUSH HL"] = () => Execute_PUSH(Registers.XH, Registers.XL);

        _opCodes["POP AF"] = () =>
        {
            var (a, f) = Execute_POP();
            (Registers.A, Registers.F) = (a, (Flags)f);
        };
        _opCodes["POP BC"] = () => (Registers.B, Registers.C) = Execute_POP();
        _opCodes["POP DE"] = () => (Registers.D, Registers.E) = Execute_POP();
        _opCodes["POP HL"] = () => (Registers.XH, Registers.XL) = Execute_POP();
    }

    private void Execute_PUSH(byte highByte, byte lowByte)
    {
        States.MemoryContention(Registers.IR, 1);

        Registers.SP -= 1;
        WriteByte(Registers.SP, highByte);
        Registers.SP -= 1;
        WriteByte(Registers.SP, lowByte);
    }

    private (byte highByte, byte lowByte) Execute_POP()
    {
        var lowByte = ReadByte(Registers.SP);
        var highByte = ReadByte((Word)(Registers.SP + 1));
        Registers.SP += 2;

        return (highByte, lowByte);
    }
}