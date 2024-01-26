using Z80.Net.Extensions;
using Z80.Net.Helpers;
using Z80.Net.Instructions;
using static Z80.Net.Instructions.OpCodes;
using static Z80.Net.Registers.Flags;

namespace Z80.Net;

partial class Z80
{
    private void AddCallAndReturnInstructions()
    {
        _opCodes[CALL] = () => ExecuteCALL();
        _opCodes[CALL_C] = () => ExecuteCALL( (Registers.F & C) != 0);
        _opCodes[CALL_NC] = () => ExecuteCALL( (Registers.F & C) == 0);
        _opCodes[CALL_Z] = () => ExecuteCALL( (Registers.F & Z) != 0);
        _opCodes[CALL_NZ] = () => ExecuteCALL( (Registers.F & Z) == 0);
        _opCodes[CALL_PE] = () => ExecuteCALL( (Registers.F & P) != 0);
        _opCodes[CALL_PO] = () => ExecuteCALL( (Registers.F & P) == 0);
        _opCodes[CALL_M] = () => ExecuteCALL( (Registers.F & S) != 0);
        _opCodes[CALL_P] = () => ExecuteCALL( (Registers.F & S) == 0);

        _opCodes[RET] = ExecuteRET;
        _opCodes[RET_C] = () => ExecuteRET( (Registers.F & C) != 0);
        _opCodes[RET_NC] = () => ExecuteRET( (Registers.F & C) == 0);
        _opCodes[RET_Z] = () => ExecuteRET( (Registers.F & Z) != 0);
        _opCodes[RET_NZ] = () => ExecuteRET( (Registers.F & Z) == 0);
        _opCodes[RET_PE] = () => ExecuteRET( (Registers.F & P) != 0);
        _opCodes[RET_PO] = () => ExecuteRET( (Registers.F & P) == 0);
        _opCodes[RET_M] = () => ExecuteRET( (Registers.F & S) != 0);
        _opCodes[RET_P] = () => ExecuteRET( (Registers.F & S) == 0);

        _opCodes[RETI] = ExecuteRETI;
        _opCodes[RETN] = ExecuteRETI;

        _opCodes[RST_00] = () => { ExecuteRST(0x00); };
        _opCodes[RST_08] = () => { ExecuteRST(0x08); };
        _opCodes[RST_10] = () => { ExecuteRST(0x10); };
        _opCodes[RST_18] = () => { ExecuteRST(0x18); };
        _opCodes[RST_20] = () => { ExecuteRST(0x20); };
        _opCodes[RST_28] = () => { ExecuteRST(0x28); };
        _opCodes[RST_30] = () => { ExecuteRST(0x30); };
        _opCodes[RST_38] = () => { ExecuteRST(0x38); };
    }

    private void ExecuteCALL(bool shouldCall = true)
    {
        var pc = ReadNextWord();
        if (!shouldCall)
        {
            return;
        }

        var (hiPC, loPC) = Registers.PC;

        AddCycles(1);
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
        AddCycles(1);
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

        AddCycles(1);
        Registers.SP -= 1;
        WriteByte(Registers.SP, hiPC);
        Registers.SP -= 1;
        WriteByte(Registers.SP, loPC);
        Registers.PC = pc;
    }
}