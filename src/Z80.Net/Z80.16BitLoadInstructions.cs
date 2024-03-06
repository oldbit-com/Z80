using Z80.Net.Registers;
using static Z80.Net.Instructions.OpCodes;

namespace Z80.Net;

partial class Z80
{
    private void Add16BitLoadInstructions()
    {
        _opCodes[LD_BC_nn] = () => Registers.BC = ReadWordAndMove();
        _opCodes[LD_DE_nn] = () => Registers.DE = ReadWordAndMove();
        _opCodes[LD_HL_nn] = () => Registers.XHL = ReadWordAndMove();
        _opCodes[LD_SP_nn] = () => Registers.SP = ReadWordAndMove();

        _opCodes[LD_HL_mm] = () => Registers.XHL = ReadWord(ReadWordAndMove());
        _opCodes[LD_BC_mm] = () => Registers.BC = ReadWord(ReadWordAndMove());
        _opCodes[LD_DE_mm] = () => Registers.DE = ReadWord(ReadWordAndMove());
        _opCodes[ED_LD_HL_mm] = _opCodes[LD_HL_mm];
        _opCodes[LD_SP_mm] = () => Registers.SP = ReadWord(ReadWordAndMove());

        _opCodes[LD_mm_HL] = () => { };
        _opCodes[LD_mm_BC] = () => { };

        _opCodes[PUSH_AF] = () => ExecutePUSH(Registers.A, (byte)Registers.F);
        _opCodes[PUSH_BC] = () => ExecutePUSH(Registers.B, Registers.C);
        _opCodes[PUSH_DE] = () => ExecutePUSH(Registers.D, Registers.E);
        _opCodes[PUSH_HL] = () => ExecutePUSH(Registers.XH, Registers.XL);

        _opCodes[POP_AF] = () =>
        {
            var (a, f) = ExecutePOP();
            (Registers.A, Registers.F) = (a, (Flags)f);
        };
        _opCodes[POP_BC] = () => (Registers.B, Registers.C) = ExecutePOP();
        _opCodes[POP_DE] = () => (Registers.D, Registers.E) = ExecutePOP();
        _opCodes[POP_HL] = () => (Registers.XH, Registers.XL) = ExecutePOP();
    }

    private void ExecutePUSH(byte highByte, byte lowByte)
    {
        AddStates(1);
        Registers.SP -= 1;
        WriteByte(Registers.SP, highByte);
        Registers.SP -= 1;
        WriteByte(Registers.SP, lowByte);
    }

    private (byte highByte, byte lowByte) ExecutePOP()
    {
        var highByte = ReadByte(Registers.SP + 1);
        var lowByte = ReadByte(Registers.SP);
        Registers.SP += 2;
        return (highByte, lowByte);
    }
}