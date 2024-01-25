using Z80.Net.Registers;
using static Z80.Net.Instructions.OpCodes;

namespace Z80.Net;

partial class Z80
{
    private void Add16BitLoadInstructions()
    {
        _opCodes[LD_BC_nn] = () => Registers.BC = ReadNextWord();
        _opCodes[LD_DE_nn] = () => Registers.DE = ReadNextWord();
        _opCodes[LD_HL_nn] = () => Registers.XHL = ReadNextWord();
        _opCodes[LD_SP_nn] = () => Registers.SP = ReadNextWord();

        _opCodes[LD_HL_mm] = () => Registers.XHL = PeekWord(ReadNextWord());
        _opCodes[LD_BC_mm] = () => Registers.BC = PeekWord(ReadNextWord());
        _opCodes[LD_DE_mm] = () => Registers.DE = PeekWord(ReadNextWord());
        _opCodes[ED_LD_HL_mm] = _opCodes[LD_HL_mm];
        _opCodes[LD_SP_mm] = () => Registers.SP = PeekWord(ReadNextWord());

        _opCodes[LD_mm_HL] = () => { };
        _opCodes[LD_mm_BC] = () => { };

        _opCodes[PUSH_AF] = () => Push(Registers.A, (byte)Registers.F);
        _opCodes[PUSH_BC] = () => Push(Registers.B, Registers.C);
        _opCodes[PUSH_DE] = () => Push(Registers.D, Registers.E);
        _opCodes[PUSH_HL] = () => Push(Registers.XH, Registers.XL);

        _opCodes[POP_AF] = () =>
        {
            var (a, f) = Pop();
            (Registers.A, Registers.F) = (a, (Flags)f);
        };
        _opCodes[POP_BC] = () => (Registers.B, Registers.C) = Pop();
        _opCodes[POP_DE] = () => (Registers.D, Registers.E) = Pop();
        _opCodes[POP_HL] = () => (Registers.XH, Registers.XL) = Pop();
    }

    private void Push(byte highByte, byte lowByte)
    {
        AddCycles(1);
        Registers.SP -= 1;
        WriteByte(Registers.SP, highByte);
        Registers.SP -= 1;
        WriteByte(Registers.SP, lowByte);
    }

    private (byte highByte, byte lowByte) Pop()
    {
        var highByte = ReadByte(Registers.SP + 1);
        var lowByte = ReadByte(Registers.SP);
        Registers.SP += 2;
        return (highByte, lowByte);
    }
}