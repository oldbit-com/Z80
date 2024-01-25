using Z80.Net.Helpers;
using Z80.Net.Instructions;
using static Z80.Net.Instructions.OpCodes;
using static Z80.Net.Registers.Flags;

namespace Z80.Net;

partial class Z80
{
    private void AddCallAndReturnInstructions()
    {
        _opCodes[OpCodes.CALL] = () => CALL();
        _opCodes[CALL_C] = () => CALL( (Registers.F & C) != 0);
        _opCodes[CALL_NC] = () => CALL( (Registers.F & C) == 0);
        _opCodes[CALL_Z] = () => CALL( (Registers.F & Z) != 0);
        _opCodes[CALL_NZ] = () => CALL( (Registers.F & Z) == 0);
        _opCodes[CALL_PE] = () => CALL( (Registers.F & P) != 0);
        _opCodes[CALL_PO] = () => CALL( (Registers.F & P) == 0);
        _opCodes[CALL_M] = () => CALL( (Registers.F & S) != 0);
        _opCodes[CALL_P] = () => CALL( (Registers.F & S) == 0);

        _opCodes[OpCodes.RET] = RET;
        _opCodes[RET_C] = () => RET( (Registers.F & C) != 0);
        _opCodes[RET_NC] = () => RET( (Registers.F & C) == 0);
        _opCodes[RET_Z] = () => RET( (Registers.F & Z) != 0);
        _opCodes[RET_NZ] = () => RET( (Registers.F & Z) == 0);
        _opCodes[RET_PE] = () => RET( (Registers.F & P) != 0);
        _opCodes[RET_PO] = () => RET( (Registers.F & P) == 0);
        _opCodes[RET_M] = () => RET( (Registers.F & S) != 0);
        _opCodes[RET_P] = () => RET( (Registers.F & S) == 0);

        _opCodes[OpCodes.RETI] = RETI;
        _opCodes[RETN] = RETI;

        _opCodes[RST_00] = () => { RST(0x00); };
        _opCodes[RST_08] = () => { RST(0x08); };
        _opCodes[RST_10] = () => { RST(0x10); };
        _opCodes[RST_18] = () => { RST(0x18); };
        _opCodes[RST_20] = () => { RST(0x20); };
        _opCodes[RST_28] = () => { RST(0x28); };
        _opCodes[RST_30] = () => { RST(0x30); };
        _opCodes[RST_38] = () => { RST(0x38); };
    }

    private void CALL(bool shouldCall = true)
    {
        var pc = ReadNextWord();
        if (!shouldCall)
        {
            return;
        }

        AddCycles(1);
        Registers.SP -= 1;
        WriteByte(Registers.SP, TypeConverter.High(Registers.PC));
        Registers.SP -= 1;
        WriteByte(Registers.SP, TypeConverter.Low(Registers.PC));
        Registers.PC = pc;
    }

    private void RET()
    {
        Registers.PC = (ushort)(ReadByte(Registers.SP + 1) << 8 | ReadByte(Registers.SP));
        Registers.SP += 2;
    }

    private void RET(bool shouldReturn)
    {
        AddCycles(1);
        if (shouldReturn)
        {
            RET();
        }
    }

    private void RETI()
    {
        IFF1 = IFF2;
        RET();
    }

    private void RST(byte pc)
    {
        AddCycles(1);
        Registers.SP -= 1;
        WriteByte(Registers.SP, TypeConverter.High(Registers.PC));
        Registers.SP -= 1;
        WriteByte(Registers.SP, TypeConverter.Low(Registers.PC));
        Registers.PC = pc;
    }
}