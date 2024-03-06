using Z80.Net.Registers;
using static Z80.Net.Instructions.OpCodes;
using static Z80.Net.Registers.Flags;

namespace Z80.Net;

partial class Z80
{
    private void Add16BitArithmeticInstructions()
    {
        _opCodes[ADD_HL_BC] = () => { ExecuteADD(Registers.BC); };
        _opCodes[ADD_HL_DE] = () => { ExecuteADD(Registers.DE); };
        _opCodes[ADD_HL_HL] = () => { ExecuteADD(Registers.XHL); };
        _opCodes[ADD_HL_SP] = () => { ExecuteADD(Registers.SP); };

        _opCodes[ADC_HL_BC] = () => { ExecuteADC(Registers.BC); };
        _opCodes[ADC_HL_DE] = () => { ExecuteADC(Registers.DE); };
        _opCodes[ADC_HL_HL] = () => { ExecuteADC(Registers.HL); };
        _opCodes[ADC_HL_SP] = () => { ExecuteADC(Registers.SP); };

        _opCodes[SBC_HL_BC] = () => { ExecuteSBC(Registers.BC); };
        _opCodes[SBC_HL_DE] = () => { ExecuteSBC(Registers.DE); };
        _opCodes[SBC_HL_HL] = () => { ExecuteSBC(Registers.HL); };
        _opCodes[SBC_HL_SP] = () => { ExecuteSBC(Registers.SP); };

        _opCodes[INC_BC] = () => { Registers.BC = ExecuteINC(Registers.BC); };
        _opCodes[INC_DE] = () => { Registers.DE = ExecuteINC(Registers.DE); };
        _opCodes[INC_HL] = () => { Registers.XHL = ExecuteINC(Registers.XHL); };
        _opCodes[INC_SP] = () => { Registers.SP = ExecuteINC(Registers.SP); };

        _opCodes[DEC_BC] = () => { Registers.BC = ExecuteDEC(Registers.BC); };
        _opCodes[DEC_DE] = () => { Registers.DE = ExecuteDEC(Registers.DE); };
        _opCodes[DEC_HL] = () => { Registers.XHL = ExecuteDEC(Registers.XHL); };
        _opCodes[DEC_SP] = () => { Registers.SP = ExecuteDEC(Registers.SP); };
    }

    private void ExecuteADD(ushort value)
    {
        AddStates(7);

        var oldValue = Registers.XHL;
        var newValue = oldValue + value;
        var result = (ushort)newValue;

        Registers.F &= (S | Z | P);
        Registers.F |= (Flags)((oldValue ^ value ^ result) >> 8) & H;
        Registers.F |= (Flags)(result >> 8) & (Y | X);
        Registers.F |= newValue > 0xFFFF ? C : 0;

        Registers.XHL = result;
    }

    private void ExecuteADC(ushort value)
    {
        AddStates(7);

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

    private void ExecuteSBC(ushort value)
    {
        AddStates(7);

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

    private ushort ExecuteINC(ushort value)
    {
        AddStates(2);
        return (ushort)(value + 1);
    }

    private ushort ExecuteDEC(ushort value)
    {
        AddStates(2);
        return (ushort)(value - 1);
    }
}