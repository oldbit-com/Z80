using Z80.Net.Extensions;
using static Z80.Net.Registers.Flags;

namespace Z80.Net;

partial class Z80
{
    private void AddCallAndReturnInstructions()
    {
        _opCodes["CALL nn"] = () => ExecuteCALL();
        _opCodes["CALL C,nn"] = () => ExecuteCALL( (Registers.F & C) != 0);
        _opCodes["CALL NC,nn"] = () => ExecuteCALL( (Registers.F & C) == 0);
        _opCodes["CALL Z,nn"] = () => ExecuteCALL( (Registers.F & Z) != 0);
        _opCodes["CALL NZ,nn"] = () => ExecuteCALL( (Registers.F & Z) == 0);
        _opCodes["CALL PE,nn"] = () => ExecuteCALL( (Registers.F & P) != 0);
        _opCodes["CALL PO,nn"] = () => ExecuteCALL( (Registers.F & P) == 0);
        _opCodes["CALL M,nn"] = () => ExecuteCALL( (Registers.F & S) != 0);
        _opCodes["CALL P,nn"] = () => ExecuteCALL( (Registers.F & S) == 0);

        _opCodes["RET"] = ExecuteRET;
        _opCodes["RET C"] = () => ExecuteRET( (Registers.F & C) != 0);
        _opCodes["RET NC"] = () => ExecuteRET( (Registers.F & C) == 0);
        _opCodes["RET Z"] = () => ExecuteRET( (Registers.F & Z) != 0);
        _opCodes["RET NZ"] = () => ExecuteRET( (Registers.F & Z) == 0);
        _opCodes["RET PE"] = () => ExecuteRET( (Registers.F & P) != 0);
        _opCodes["RET PO"] = () => ExecuteRET( (Registers.F & P) == 0);
        _opCodes["RET M"] = () => ExecuteRET( (Registers.F & S) != 0);
        _opCodes["RET P"] = () => ExecuteRET( (Registers.F & S) == 0);

        _opCodes["RETI"] = ExecuteRETI;
        _opCodes["RETN"] = ExecuteRETI;

        _opCodes["RST 00h"] = () => { ExecuteRST(0x00); };
        _opCodes["RST 08h"] = () => { ExecuteRST(0x08); };
        _opCodes["RST 10h"] = () => { ExecuteRST(0x10); };
        _opCodes["RST 18h"] = () => { ExecuteRST(0x18); };
        _opCodes["RST 20h"] = () => { ExecuteRST(0x20); };
        _opCodes["RST 28h"] = () => { ExecuteRST(0x28); };
        _opCodes["RST 30h"] = () => { ExecuteRST(0x30); };
        _opCodes["RST 38h"] = () => { ExecuteRST(0x38); };
    }

    private void ExecuteCALL(bool shouldCall = true)
    {
        var pc = ReadWordAndMove();
        if (!shouldCall)
        {
            return;
        }

        var (hiPC, loPC) = Registers.PC;

        AddStates(1);
        Registers.SP -= 1;
        WriteByte(Registers.SP, hiPC);
        Registers.SP -= 1;
        WriteByte(Registers.SP, loPC);
        Registers.PC = pc;
    }

    private void ExecuteRET()
    {
        Registers.PC = (ushort)(ReadByte(Registers.SP + 1) << 8 | ReadByte(Registers.SP));
        Registers.SP += 2;
    }

    private void ExecuteRET(bool shouldReturn)
    {
        AddStates(1);
        if (shouldReturn)
        {
            ExecuteRET();
        }
    }

    private void ExecuteRETI()
    {
        IFF1 = IFF2;
        ExecuteRET();
    }

    private void ExecuteRST(byte pc)
    {
        var (hiPC, loPC) = Registers.PC;

        AddStates(1);
        Registers.SP -= 1;
        WriteByte(Registers.SP, hiPC);
        Registers.SP -= 1;
        WriteByte(Registers.SP, loPC);
        Registers.PC = pc;
    }
}