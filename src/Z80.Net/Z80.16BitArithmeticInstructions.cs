using Z80.Net.Registers;
using static Z80.Net.Instructions.OpCodes;
using static Z80.Net.Registers.Flags;

namespace Z80.Net;

partial class Z80
{
    private void Add16BitArithmeticInstructions()
    {
        _opCodes[ADD_HL_BC] = () => { ADD(Registers.BC); };
        _opCodes[ADD_HL_DE] = () => { ADD(Registers.DE); };
        _opCodes[ADD_HL_HL] = () => { ADD(Registers.XHL); };
        _opCodes[ADD_HL_SP] = () => { ADD(Registers.SP); };

        _opCodes[ADC_HL_BC] = () => { ADC(Registers.BC); };
        _opCodes[ADC_HL_DE] = () => { ADC(Registers.DE); };
        _opCodes[ADC_HL_HL] = () => { ADC(Registers.HL); };
        _opCodes[ADC_HL_SP] = () => { ADC(Registers.SP); };

        _opCodes[SBC_HL_BC] = () => { SBC(Registers.BC); };
        _opCodes[SBC_HL_DE] = () => { SBC(Registers.DE); };
        _opCodes[SBC_HL_HL] = () => { SBC(Registers.HL); };
        _opCodes[SBC_HL_SP] = () => { SBC(Registers.SP); };

        _opCodes[INC_BC] = () => { Registers.BC = INC(Registers.BC); };
        _opCodes[INC_DE] = () => { Registers.DE = INC(Registers.DE); };
        _opCodes[INC_HL] = () => { Registers.XHL = INC(Registers.XHL); };
        _opCodes[INC_SP] = () => { Registers.SP = INC(Registers.SP); };

        _opCodes[DEC_BC] = () => { Registers.BC = DEC(Registers.BC); };
        _opCodes[DEC_DE] = () => { Registers.DE = DEC(Registers.DE); };
        _opCodes[DEC_HL] = () => { Registers.XHL = DEC(Registers.XHL); };
        _opCodes[DEC_SP] = () => { Registers.SP = DEC(Registers.SP); };
    }

    private void ADD(ushort value)
    {
        AddCycles(7);

        var oldValue = Registers.XHL;
        var newValue = oldValue + value;
        var result = (ushort)newValue;

        Registers.F &= (S | Z | P);
        Registers.F |= (Flags)((oldValue ^ value ^ result) >> 8) & H;
        Registers.F |= (Flags)(result >> 8) & (Y | X);
        Registers.F |= newValue > 0xFFFF ? C : 0;

        Registers.XHL = result;
    }

    private void ADC(ushort value)
    {
        AddCycles(7);

        var oldValue = Registers.HL;
        var newValue = oldValue + value + (byte)(Registers.F & C);
        var result = (ushort)newValue;

        Registers.F = result > 0x7FFF ? S : 0;
        Registers.F |= result == 0 ? Z : 0;
        Registers.F |= (Flags)((oldValue ^ value ^ result) >> 8) & H;
        Registers.F |= newValue > 0xFFFF ? C : 0;
        Registers.F |= ((oldValue ^ value) & 0x8000) == 0 && ((oldValue ^ result) & 0x8000) != 0 ? P : 0;
        Registers.F |= (Flags)(result >> 8) & (Y | X);

        Registers.HL = result;
    }

    private void SBC(ushort value)
    {
        AddCycles(7);

        var oldValue = Registers.HL;
        var newValue = oldValue - value - (byte)(Registers.F & C);
        var result = (ushort)newValue;

        Registers.F = N;
        Registers.F |= result > 0x7FFF ? S : 0;
        Registers.F |= result == 0 ? Z : 0;
        Registers.F |= (Flags)((oldValue ^ value ^ result) >> 8) & H;
        Registers.F |= newValue < 0 ? C : 0;
        Registers.F |= ((oldValue ^ value) & 0x8000) != 0 && ((oldValue ^ result) & 0x8000) != 0 ? P : 0;
        Registers.F |= (Flags)(result >> 8) & (Y | X);

        Registers.HL = result;
    }

    private ushort INC(ushort value)
    {
        AddCycles(2);
        return (ushort)(value + 1);
    }

    private ushort DEC(ushort value)
    {
        AddCycles(2);
        return (ushort)(value - 1);
    }
}