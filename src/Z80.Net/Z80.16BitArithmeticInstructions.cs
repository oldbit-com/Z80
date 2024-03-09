using Z80.Net.Registers;
using static Z80.Net.Registers.Flags;

namespace Z80.Net;

partial class Z80
{
    private void Add16BitArithmeticInstructions()
    {
        _opCodes["ADD HL,BC"] = () => { ExecuteADD(Registers.BC); };
        _opCodes["ADD HL,DE"] = () => { ExecuteADD(Registers.DE); };
        _opCodes["ADD HL,HL"] = () => { ExecuteADD(Registers.XHL); };
        _opCodes["ADD HL,SP"] = () => { ExecuteADD(Registers.SP); };

        _opCodes["ADC HL,BC"] = () => { ExecuteADC(Registers.BC); };
        _opCodes["ADC HL,DE"] = () => { ExecuteADC(Registers.DE); };
        _opCodes["ADC HL,HL"] = () => { ExecuteADC(Registers.HL); };
        _opCodes["ADC HL,SP"] = () => { ExecuteADC(Registers.SP); };

        _opCodes["SBC HL,BC"] = () => { ExecuteSBC(Registers.BC); };
        _opCodes["SBC HL,DE"] = () => { ExecuteSBC(Registers.DE); };
        _opCodes["SBC HL,HL"] = () => { ExecuteSBC(Registers.HL); };
        _opCodes["SBC HL,SP"] = () => { ExecuteSBC(Registers.SP); };

        _opCodes["INC BC"] = () => { Registers.BC = ExecuteINC(Registers.BC); };
        _opCodes["INC DE"] = () => { Registers.DE = ExecuteINC(Registers.DE); };
        _opCodes["INC HL"] = () => { Registers.XHL = ExecuteINC(Registers.XHL); };
        _opCodes["INC SP"] = () => { Registers.SP = ExecuteINC(Registers.SP); };

        _opCodes["DEC BC"] = () => { Registers.BC = ExecuteDEC(Registers.BC); };
        _opCodes["DEC DE"] = () => { Registers.DE = ExecuteDEC(Registers.DE); };
        _opCodes["DEC HL"] = () => { Registers.XHL = ExecuteDEC(Registers.XHL); };
        _opCodes["DEC SP"] = () => { Registers.SP = ExecuteDEC(Registers.SP); };
    }

    private void ExecuteADD(Word value)
    {
        AddStates(7);

        var oldValue = Registers.XHL;
        var newValue = oldValue + value;
        var result = (Word)newValue;

        Registers.F &= (S | Z | P);
        Registers.F |= (Flags)((oldValue ^ value ^ result) >> 8) & H;
        Registers.F |= (Flags)(result >> 8) & (Y | X);
        Registers.F |= newValue > 0xFFFF ? C : 0;

        Registers.XHL = result;
    }

    private void ExecuteADC(Word value)
    {
        AddStates(7);

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

    private void ExecuteSBC(Word value)
    {
        AddStates(7);

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

    private Word ExecuteINC(Word value)
    {
        AddStates(2);
        return (Word)(value + 1);
    }

    private Word ExecuteDEC(Word value)
    {
        AddStates(2);
        return (Word)(value - 1);
    }
}