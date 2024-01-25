using Z80.Net.Helpers;
using static Z80.Net.Instructions.OpCodes;
using static Z80.Net.Registers.Flags;

namespace Z80.Net;

partial class Z80
{
    private void AddCallAndReturnInstructions()
    {
        _opCodes[CALL] = () => Call();
        _opCodes[CALL_C] = () => Call( (Registers.F & C) != 0);
        _opCodes[CALL_NC] = () => Call( (Registers.F & C) == 0);
        _opCodes[CALL_Z] = () => Call( (Registers.F & Z) != 0);
        _opCodes[CALL_NZ] = () => Call( (Registers.F & Z) == 0);
        _opCodes[CALL_PE] = () => Call( (Registers.F & P) != 0);
        _opCodes[CALL_PO] = () => Call( (Registers.F & P) == 0);
        _opCodes[CALL_M] = () => Call( (Registers.F & S) != 0);
        _opCodes[CALL_P] = () => Call( (Registers.F & S) == 0);

        _opCodes[RET] = Return;
        _opCodes[RET_C] = () => Return( (Registers.F & C) != 0);
        _opCodes[RET_NC] = () => Return( (Registers.F & C) == 0);
        _opCodes[RET_Z] = () => Return( (Registers.F & Z) != 0);
        _opCodes[RET_NZ] = () => Return( (Registers.F & Z) == 0);
        _opCodes[RET_PE] = () => Return( (Registers.F & P) != 0);
        _opCodes[RET_PO] = () => Return( (Registers.F & P) == 0);
        _opCodes[RET_M] = () => Return( (Registers.F & S) != 0);
        _opCodes[RET_P] = () => Return( (Registers.F & S) == 0);

        _opCodes[RETI] = ReturnFromInterrupt;
        _opCodes[RETN] = ReturnFromInterrupt;

        _opCodes[RST_00] = () => { Rst(0x00); };
        _opCodes[RST_08] = () => { Rst(0x08); };
        _opCodes[RST_10] = () => { Rst(0x10); };
        _opCodes[RST_18] = () => { Rst(0x18); };
        _opCodes[RST_20] = () => { Rst(0x20); };
        _opCodes[RST_28] = () => { Rst(0x28); };
        _opCodes[RST_30] = () => { Rst(0x30); };
        _opCodes[RST_38] = () => { Rst(0x38); };
    }

    private void Call(bool shouldCall = true)
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

    private void Return()
    {
        Registers.PC = (ushort)(ReadByte(Registers.SP + 1) << 8 | ReadByte(Registers.SP));
        Registers.SP += 2;
    }

    private void Return(bool shouldReturn)
    {
        AddCycles(1);
        if (shouldReturn)
        {
            Return();
        }
    }

    private void ReturnFromInterrupt()
    {
        IFF1 = IFF2;
        Return();
    }

    private void Rst(byte pc)
    {
        AddCycles(1);
        Registers.SP -= 1;
        WriteByte(Registers.SP, TypeConverter.High(Registers.PC));
        Registers.SP -= 1;
        WriteByte(Registers.SP, TypeConverter.Low(Registers.PC));
        Registers.PC = pc;
    }
}