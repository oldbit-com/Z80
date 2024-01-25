using Z80.Net.Registers;
using static Z80.Net.Instructions.OpCodes;
using static Z80.Net.Registers.Flags;

namespace Z80.Net;

partial class Z80
{
    private void Add16BitArithmeticInstructions()
    {
        _opCodes[ADD_HL_BC] = () => { Add(Registers.BC); };
        _opCodes[ADD_HL_DE] = () => { Add(Registers.DE); };
        _opCodes[ADD_HL_HL] = () => { Add(Registers.XHL); };
        _opCodes[ADD_HL_SP] = () => { Add(Registers.SP); };

        _opCodes[ADC_HL_BC] = () => { Adc(Registers.BC); };
        _opCodes[ADC_HL_DE] = () => { Adc(Registers.DE); };
        _opCodes[ADC_HL_HL] = () => { Adc(Registers.HL); };
        _opCodes[ADC_HL_SP] = () => { Adc(Registers.SP); };

        _opCodes[SBC_HL_BC] = () => { Sbc(Registers.BC); };
        _opCodes[SBC_HL_DE] = () => { Sbc(Registers.DE); };
        _opCodes[SBC_HL_HL] = () => { Sbc(Registers.HL); };
        _opCodes[SBC_HL_SP] = () => { Sbc(Registers.SP); };

        _opCodes[INC_BC] = () => { Registers.BC = Inc(Registers.BC); };
        _opCodes[INC_DE] = () => { Registers.DE = Inc(Registers.DE); };
        _opCodes[INC_HL] = () => { Registers.XHL = Inc(Registers.XHL); };
        _opCodes[INC_SP] = () => { Registers.SP = Inc(Registers.SP); };

        _opCodes[DEC_BC] = () => { Registers.BC = Dec(Registers.BC); };
        _opCodes[DEC_DE] = () => { Registers.DE = Dec(Registers.DE); };
        _opCodes[DEC_HL] = () => { Registers.XHL = Dec(Registers.XHL); };
        _opCodes[DEC_SP] = () => { Registers.SP = Dec(Registers.SP); };
    }

    private void Add(ushort value)
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

    private void Adc(ushort value)
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

    private void Sbc(ushort value)
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

    private ushort Inc(ushort value)
    {
        AddCycles(2);
        return (ushort)(value + 1);
    }

    private ushort Dec(ushort value)
    {
        AddCycles(2);
        return (ushort)(value - 1);
    }
}