using OldBit.Z80Cpu.Extensions;
using static OldBit.Z80Cpu.Registers.Flags;

namespace OldBit.Z80Cpu;

partial class Z80
{
    private void AddCallAndReturnInstructions()
    {
        _opCodes["CALL nn"] = () => Execute_CALL();
        _opCodes["CALL C,nn"] = () => Execute_CALL((Registers.F & C) != 0);
        _opCodes["CALL NC,nn"] = () => Execute_CALL((Registers.F & C) == 0);
        _opCodes["CALL Z,nn"] = () => Execute_CALL((Registers.F & Z) != 0);
        _opCodes["CALL NZ,nn"] = () => Execute_CALL((Registers.F & Z) == 0);
        _opCodes["CALL PE,nn"] = () => Execute_CALL((Registers.F & P) != 0);
        _opCodes["CALL PO,nn"] = () => Execute_CALL((Registers.F & P) == 0);
        _opCodes["CALL M,nn"] = () => Execute_CALL((Registers.F & S) != 0);
        _opCodes["CALL P,nn"] = () => Execute_CALL((Registers.F & S) == 0);

        _opCodes["RET"] = Execute_RET;
        _opCodes["RET C"] = () => Execute_RET((Registers.F & C) != 0);
        _opCodes["RET NC"] = () => Execute_RET((Registers.F & C) == 0);
        _opCodes["RET Z"] = () => Execute_RET((Registers.F & Z) != 0);
        _opCodes["RET NZ"] = () => Execute_RET((Registers.F & Z) == 0);
        _opCodes["RET PE"] = () => Execute_RET((Registers.F & P) != 0);
        _opCodes["RET PO"] = () => Execute_RET((Registers.F & P) == 0);
        _opCodes["RET M"] = () => Execute_RET((Registers.F & S) != 0);
        _opCodes["RET P"] = () => Execute_RET((Registers.F & S) == 0);

        _opCodes["RETI"] = Execute_RETI;
        _opCodes["RETN"] = Execute_RETI;

        _opCodes["RST 00h"] = () => { Execute_RST(0x00); };
        _opCodes["RST 08h"] = () => { Execute_RST(0x08); };
        _opCodes["RST 10h"] = () => { Execute_RST(0x10); };
        _opCodes["RST 18h"] = () => { Execute_RST(0x18); };
        _opCodes["RST 20h"] = () => { Execute_RST(0x20); };
        _opCodes["RST 28h"] = () => { Execute_RST(0x28); };
        _opCodes["RST 30h"] = () => { Execute_RST(0x30); };
        _opCodes["RST 38h"] = () => { Execute_RST(0x38); };
    }

    private void Execute_CALL(bool shouldCall = true)
    {
        var pc = FetchWord();

        if (!shouldCall)
        {
            return;
        }

        var (hiPC, loPC) = Registers.PC;

        Clock.AddMemoryContention((Word)(Registers.PC - 1), 1);

        Registers.SP -= 1;
        WriteByte(Registers.SP, hiPC);
        Registers.SP -= 1;
        WriteByte(Registers.SP, loPC);
        Registers.PC = pc;
    }

    private void Execute_RET()
    {
        Registers.PC = (Word)(ReadByte(Registers.SP) | ReadByte((Word)(Registers.SP + 1)) << 8);
        Registers.SP += 2;
    }

    private void Execute_RET(bool shouldReturn)
    {
        Clock.AddMemoryContention(Registers.IR, 1);

        if (shouldReturn)
        {
            Execute_RET();
        }
    }

    private void Execute_RETI()
    {
        IFF1 = IFF2;
        Execute_RET();
    }

    private void Execute_RST(byte pc)
    {
        var (hiPC, loPC) = Registers.PC;

        Clock.AddMemoryContention(Registers.IR, 1);

        Registers.SP -= 1;
        WriteByte(Registers.SP, hiPC);
        Registers.SP -= 1;
        WriteByte(Registers.SP, loPC);
        Registers.PC = pc;
    }
}