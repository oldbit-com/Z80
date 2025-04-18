using static OldBit.Z80Cpu.Registers.Flags;

namespace OldBit.Z80Cpu;

partial class Z80
{
    private void AddJumpInstructions()
    {
        _opCodes["JP nn"] = () => Execute_JP();
        _opCodes["JP C,nn"] = () => Execute_JP((Registers.F & C) != 0);
        _opCodes["JP NC,nn"] = () => Execute_JP((Registers.F & C) == 0);
        _opCodes["JP Z,nn"] = () => Execute_JP((Registers.F & Z) != 0);
        _opCodes["JP NZ,nn"] = () => Execute_JP((Registers.F & Z) == 0);
        _opCodes["JP PE,nn"] = () => Execute_JP((Registers.F & P) != 0);
        _opCodes["JP PO,nn"] = () => Execute_JP((Registers.F & P) == 0);
        _opCodes["JP M,nn"] = () => Execute_JP((Registers.F & S) != 0);
        _opCodes["JP P,nn"] = () => Execute_JP((Registers.F & S) == 0);

        _opCodes["JR e"] = () => Execute_JR();
        _opCodes["JR C,e"] = () => Execute_JR((Registers.F & C) != 0);
        _opCodes["JR NC,e"] = () => Execute_JR((Registers.F & C) == 0);
        _opCodes["JR Z,e"] = () => Execute_JR((Registers.F & Z) != 0);
        _opCodes["JR NZ,e"] = () => Execute_JR((Registers.F & Z) == 0);

        _opCodes["JP (HL)"] = () => { Registers.PC = Registers.XHL; };

        _opCodes["DJNZ"] = Execute_DJNZ;
    }

    private void Execute_JP(bool shouldJump = true)
    {
        var pc = FetchWord();
        if (!shouldJump)
        {
            return;
        }

        Registers.PC = pc;
    }

    private void Execute_JR(bool shouldJump = true)
    {
        if (!shouldJump)
        {
            Clock.AddTicks(3);
            Registers.PC += 1;

            return;
        }

        var offset = FetchByte();

        Clock.AddTicks(5);

        Registers.PC += (Word)(sbyte)offset;
    }

    private void Execute_DJNZ()
    {
        Clock.AddTicks(1);

        Registers.B -= 1;

        Execute_JR(Registers.B != 0);
    }
}