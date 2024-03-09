using static Z80.Net.Registers.Flags;

namespace Z80.Net;

partial class Z80
{
    private void AddJumpInstructions()
    {
        _opCodes["JP nn"] = () => ExecuteJP();
        _opCodes["JP C,nn"] = () => ExecuteJP( (Registers.F & C) != 0);
        _opCodes["JP NC,nn"] = () => ExecuteJP( (Registers.F & C) == 0);
        _opCodes["JP Z,nn"] = () => ExecuteJP( (Registers.F & Z) != 0);
        _opCodes["JP NZ,nn"] = () => ExecuteJP( (Registers.F & Z) == 0);
        _opCodes["JP PE,nn"] = () => ExecuteJP( (Registers.F & P) != 0);
        _opCodes["JP PO,nn"] = () => ExecuteJP( (Registers.F & P) == 0);
        _opCodes["JP M,nn"] = () => ExecuteJP( (Registers.F & S) != 0);
        _opCodes["JP P,nn"] = () => ExecuteJP( (Registers.F & S) == 0);

        _opCodes["JR e"] = () => ExecuteJR();
        _opCodes["JR C,e"] = () => ExecuteJR( (Registers.F & C) != 0);
        _opCodes["JR NC,e"] = () => ExecuteJR( (Registers.F & C) == 0);
        _opCodes["JR Z,e"] = () => ExecuteJR( (Registers.F & Z) != 0);
        _opCodes["JR NZ,e"] = () => ExecuteJR( (Registers.F & Z) == 0);

        _opCodes["JP (HL)"] = () => { Registers.PC = Registers.XHL; };

        _opCodes["DJNZ"] = () =>
        {
            AddStates(1);
            var offset = (sbyte)FetchByte();
            Registers.B -= 1;
            if (Registers.B != 0)
            {
                AddStates(5);
                Registers.PC = (Word)(Registers.PC + offset);
            }
        };
    }

    private void ExecuteJP(bool shouldJump = true)
    {
        var pc = FetchWord();
        if (!shouldJump)
        {
            return;
        }

        Registers.PC = pc;
    }

    private void ExecuteJR(bool shouldJump = true)
    {
        var offset = FetchByte();
        if (!shouldJump)
        {
            return;
        }

        AddStates(5);
        Registers.PC += (Word)(sbyte)offset;
    }
}