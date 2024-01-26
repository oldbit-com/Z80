using Z80.Net.Instructions;
using static Z80.Net.Instructions.OpCodes;
using static Z80.Net.Registers.Flags;

namespace Z80.Net;

partial class Z80
{
    private void AddJumpInstructions()
    {
        _opCodes[OpCodes.JP] = () => ExecuteJP();
        _opCodes[JP_C] = () => ExecuteJP( (Registers.F & C) != 0);
        _opCodes[JP_NC] = () => ExecuteJP( (Registers.F & C) == 0);
        _opCodes[JP_Z] = () => ExecuteJP( (Registers.F & Z) != 0);
        _opCodes[JP_NZ] = () => ExecuteJP( (Registers.F & Z) == 0);
        _opCodes[JP_PE] = () => ExecuteJP( (Registers.F & P) != 0);
        _opCodes[JP_PO] = () => ExecuteJP( (Registers.F & P) == 0);
        _opCodes[JP_M] = () => ExecuteJP( (Registers.F & S) != 0);
        _opCodes[JP_P] = () => ExecuteJP( (Registers.F & S) == 0);

        _opCodes[OpCodes.JR] = () => ExecuteJR();
        _opCodes[JR_C] = () => ExecuteJR( (Registers.F & C) != 0);
        _opCodes[JR_NC] = () => ExecuteJR( (Registers.F & C) == 0);
        _opCodes[JR_Z] = () => ExecuteJR( (Registers.F & Z) != 0);
        _opCodes[JR_NZ] = () => ExecuteJR( (Registers.F & Z) == 0);

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

    private void ExecuteJP(bool shouldJump = true)
    {
        var pc = ReadNextWord();
        if (!shouldJump)
        {
            return;
        }

        Registers.PC = pc;
    }

    private void ExecuteJR(bool shouldJump = true)
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