using Z80.Net.Instructions;
using static Z80.Net.Instructions.OpCodes;
using static Z80.Net.Registers.Flags;

namespace Z80.Net;

partial class Z80
{
    private void AddJumpInstructions()
    {
        _opCodes[OpCodes.JP] = () => JP();
        _opCodes[JP_C] = () => JP( (Registers.F & C) != 0);
        _opCodes[JP_NC] = () => JP( (Registers.F & C) == 0);
        _opCodes[JP_Z] = () => JP( (Registers.F & Z) != 0);
        _opCodes[JP_NZ] = () => JP( (Registers.F & Z) == 0);
        _opCodes[JP_PE] = () => JP( (Registers.F & P) != 0);
        _opCodes[JP_PO] = () => JP( (Registers.F & P) == 0);
        _opCodes[JP_M] = () => JP( (Registers.F & S) != 0);
        _opCodes[JP_P] = () => JP( (Registers.F & S) == 0);

        _opCodes[OpCodes.JR] = () => JR();
        _opCodes[JR_C] = () => JR( (Registers.F & C) != 0);
        _opCodes[JR_NC] = () => JR( (Registers.F & C) == 0);
        _opCodes[JR_Z] = () => JR( (Registers.F & Z) != 0);
        _opCodes[JR_NZ] = () => JR( (Registers.F & Z) == 0);

        _opCodes[JP_HL] = () => { Registers.PC = Registers.XHL; };

        _opCodes[DJNZ] = () =>
        {
            AddCycles(1);
            var offset = (sbyte)ReadNextByte();
            Registers.B -= 1;
            if (Registers.B != 0)
            {
                AddCycles(5);
                Registers.PC = (ushort)(Registers.PC + offset);
            }
        };
    }

    private void JP(bool shouldJump = true)
    {
        var pc = ReadNextWord();
        if (!shouldJump)
        {
            return;
        }

        Registers.PC = pc;
    }

    private void JR(bool shouldJump = true)
    {
        var offset = ReadNextByte();
        if (!shouldJump)
        {
            return;
        }

        AddCycles(5);
        Registers.PC += (ushort)(sbyte)offset;
    }
}