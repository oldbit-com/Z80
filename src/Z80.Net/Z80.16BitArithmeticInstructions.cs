using Z80.Net.Registers;
using static Z80.Net.Registers.Flags;

namespace Z80.Net;

partial class Z80
{
    private void Add16BitArithmeticInstructions()
    {
        _opCodes["ADD HL,BC"] = () => { Execute_ADD(Registers.BC); };
        _opCodes["ADD HL,DE"] = () => { Execute_ADD(Registers.DE); };
        _opCodes["ADD HL,HL"] = () => { Execute_ADD(Registers.XHL); };
        _opCodes["ADD HL,SP"] = () => { Execute_ADD(Registers.SP); };

        _opCodes["ADC HL,BC"] = () => { Execute_ADC(Registers.BC); };
        _opCodes["ADC HL,DE"] = () => { Execute_ADC(Registers.DE); };
        _opCodes["ADC HL,HL"] = () => { Execute_ADC(Registers.HL); };
        _opCodes["ADC HL,SP"] = () => { Execute_ADC(Registers.SP); };

        _opCodes["SBC HL,BC"] = () => { Execute_SBC(Registers.BC); };
        _opCodes["SBC HL,DE"] = () => { Execute_SBC(Registers.DE); };
        _opCodes["SBC HL,HL"] = () => { Execute_SBC(Registers.HL); };
        _opCodes["SBC HL,SP"] = () => { Execute_SBC(Registers.SP); };

        _opCodes["INC BC"] = () => { Registers.BC = Execute_INC(Registers.BC); };
        _opCodes["INC DE"] = () => { Registers.DE = Execute_INC(Registers.DE); };
        _opCodes["INC HL"] = () => { Registers.XHL = Execute_INC(Registers.XHL); };
        _opCodes["INC SP"] = () => { Registers.SP = Execute_INC(Registers.SP); };

        _opCodes["DEC BC"] = () => { Registers.BC = Execute_DEC(Registers.BC); };
        _opCodes["DEC DE"] = () => { Registers.DE = Execute_DEC(Registers.DE); };
        _opCodes["DEC HL"] = () => { Registers.XHL = Execute_DEC(Registers.XHL); };
        _opCodes["DEC SP"] = () => { Registers.SP = Execute_DEC(Registers.SP); };
    }

    private void Execute_ADD(Word value)
    {
        States.Add(7);

        var oldValue = Registers.XHL;
        var newValue = oldValue + value;
        var result = (Word)newValue;

        Registers.F &= (S | Z | P);
        Registers.F |= (Flags)((oldValue ^ value ^ result) >> 8) & H;
        Registers.F |= (Flags)(result >> 8) & (Y | X);
        Registers.F |= newValue > 0xFFFF ? C : 0;

        Registers.XHL = result;
    }

    private void Execute_ADC(Word value)
    {
        States.Add(7);

        var oldValue = Registers.HL;
        var newValue = oldValue + value + (byte)(Registers.F & C);
        var result = (Word)newValue;

        Registers.F = result > 0x7FFF ? S : 0;
        Registers.F |= result == 0 ? Z : 0;
        Registers.F |= (Flags)((oldValue ^ value ^ result) >> 8) & H;
        Registers.F |= newValue > 0xFFFF ? C : 0;
        Registers.F |= ((oldValue ^ value) & 0x8000) == 0 && ((oldValue ^ result) & 0x8000) != 0 ? P : 0;
        Registers.F |= (Flags)(result >> 8) & (Y | X);

        Registers.HL = result;
    }

    private void Execute_SBC(Word value)
    {
        States.Add(7);

        var oldValue = Registers.HL;
        var newValue = oldValue - value - (byte)(Registers.F & C);
        var result = (Word)newValue;

        Registers.F = N;
        Registers.F |= result > 0x7FFF ? S : 0;
        Registers.F |= result == 0 ? Z : 0;
        Registers.F |= (Flags)((oldValue ^ value ^ result) >> 8) & H;
        Registers.F |= newValue < 0 ? C : 0;
        Registers.F |= ((oldValue ^ value) & 0x8000) != 0 && ((oldValue ^ result) & 0x8000) != 0 ? P : 0;
        Registers.F |= (Flags)(result >> 8) & (Y | X);

        Registers.HL = result;
    }

    private Word Execute_INC(Word value)
    {
        States.Add(2);
        return (Word)(value + 1);
    }

    private Word Execute_DEC(Word value)
    {
        States.Add(2);
        return (Word)(value - 1);
    }
}