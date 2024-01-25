using static Z80.Net.Instructions.OpCodes;
using static Z80.Net.Registers.Flags;

namespace Z80.Net;

partial class Z80
{
    private void AddJumpInstructions()
    {
        _opCodes[JP] = () => JumpAbsolute();
        _opCodes[JP_C] = () => JumpAbsolute( (Registers.F & C) != 0);
        _opCodes[JP_NC] = () => JumpAbsolute( (Registers.F & C) == 0);
        _opCodes[JP_Z] = () => JumpAbsolute( (Registers.F & Z) != 0);
        _opCodes[JP_NZ] = () => JumpAbsolute( (Registers.F & Z) == 0);
        _opCodes[JP_PE] = () => JumpAbsolute( (Registers.F & P) != 0);
        _opCodes[JP_PO] = () => JumpAbsolute( (Registers.F & P) == 0);
        _opCodes[JP_M] = () => JumpAbsolute( (Registers.F & S) != 0);
        _opCodes[JP_P] = () => JumpAbsolute( (Registers.F & S) == 0);

        _opCodes[JR] = () => JumpRelative();
        _opCodes[JR_C] = () => JumpRelative( (Registers.F & C) != 0);
        _opCodes[JR_NC] = () => JumpRelative( (Registers.F & C) == 0);
        _opCodes[JR_Z] = () => JumpRelative( (Registers.F & Z) != 0);
        _opCodes[JR_NZ] = () => JumpRelative( (Registers.F & Z) == 0);

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

    private void JumpAbsolute(bool shouldJump = true)
    {
        var pc = ReadNextWord();
        if (!shouldJump)
        {
            return;
        }

        Registers.PC = pc;
    }

    private void JumpRelative(bool shouldJump = true)
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