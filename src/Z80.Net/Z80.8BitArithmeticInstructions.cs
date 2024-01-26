using Z80.Net.Helpers;
using Z80.Net.Registers;
using static Z80.Net.Instructions.OpCodes;
using static Z80.Net.Registers.Flags;

namespace Z80.Net;

partial class Z80
{
    private void Add8BitArithmeticInstructions()
    {
        _opCodes[ADD_A_A] = () => { ExecuteADD(Registers.A); };
        _opCodes[ADD_A_B] = () => { ExecuteADD(Registers.B); };
        _opCodes[ADD_A_C] = () => { ExecuteADD(Registers.C); };
        _opCodes[ADD_A_D] = () => { ExecuteADD(Registers.D); };
        _opCodes[ADD_A_E] = () => { ExecuteADD(Registers.E); };
        _opCodes[ADD_A_H] = () => { ExecuteADD(Registers.XH); };
        _opCodes[ADD_A_L] = () => { ExecuteADD(Registers.XL); };
        _opCodes[ADD_A_n] = () => { ExecuteADD(ReadNextByte()); };
        _opCodes[ADD_A_HL] = () => { ExecuteADD(ReadMemoryAtHL()); };

        _opCodes[ADC_A_A] = () => { ExecuteADC(Registers.A); };
        _opCodes[ADC_A_B] = () => { ExecuteADC(Registers.B); };
        _opCodes[ADC_A_C] = () => { ExecuteADC(Registers.C); };
        _opCodes[ADC_A_D] = () => { ExecuteADC(Registers.D); };
        _opCodes[ADC_A_E] = () => { ExecuteADC(Registers.E); };
        _opCodes[ADC_A_H] = () => { ExecuteADC(Registers.XH); };
        _opCodes[ADC_A_L] = () => { ExecuteADC(Registers.XL); };
        _opCodes[ADC_A_n] = () => { ExecuteADC(ReadNextByte()); };
        _opCodes[ADC_A_HL] = () => { ExecuteADC(ReadMemoryAtHL()); };

        _opCodes[SUB_A] = () => { ExecuteSUB(Registers.A); };
        _opCodes[SUB_B] = () => { ExecuteSUB(Registers.B); };
        _opCodes[SUB_C] = () => { ExecuteSUB(Registers.C); };
        _opCodes[SUB_D] = () => { ExecuteSUB(Registers.D); };
        _opCodes[SUB_E] = () => { ExecuteSUB(Registers.E); };
        _opCodes[SUB_H] = () => { ExecuteSUB(Registers.XH); };
        _opCodes[SUB_L] = () => { ExecuteSUB(Registers.XL); };
        _opCodes[SUB_n] = () => { ExecuteSUB(ReadNextByte()); };
        _opCodes[SUB_HL] = () => { ExecuteSUB(ReadMemoryAtHL()); };

        _opCodes[SBC_A_A] = () => { ExecuteSBC(Registers.A); };
        _opCodes[SBC_A_B] = () => { ExecuteSBC(Registers.B); };
        _opCodes[SBC_A_C] = () => { ExecuteSBC(Registers.C); };
        _opCodes[SBC_A_D] = () => { ExecuteSBC(Registers.D); };
        _opCodes[SBC_A_E] = () => { ExecuteSBC(Registers.E); };
        _opCodes[SBC_A_H] = () => { ExecuteSBC(Registers.XH); };
        _opCodes[SBC_A_L] = () => { ExecuteSBC(Registers.XL); };
        _opCodes[SBC_A_n] = () => { ExecuteSBC(ReadNextByte()); };
        _opCodes[SBC_A_HL] = () => { ExecuteSBC(ReadMemoryAtHL()); };

        _opCodes[AND_A] = () => { ExecuteAND(Registers.A); };
        _opCodes[AND_B] = () => { ExecuteAND(Registers.B); };
        _opCodes[AND_C] = () => { ExecuteAND(Registers.C); };
        _opCodes[AND_D] = () => { ExecuteAND(Registers.D); };
        _opCodes[AND_E] = () => { ExecuteAND(Registers.E); };
        _opCodes[AND_H] = () => { ExecuteAND(Registers.XH); };
        _opCodes[AND_L] = () => { ExecuteAND(Registers.XL); };
        _opCodes[AND_n] = () => { ExecuteAND(ReadNextByte()); };
        _opCodes[AND_HL] = () => { ExecuteAND(ReadMemoryAtHL()); };

        _opCodes[OR_A] = () => { ExecuteOR(Registers.A); };
        _opCodes[OR_B] = () => { ExecuteOR(Registers.B); };
        _opCodes[OR_C] = () => { ExecuteOR(Registers.C); };
        _opCodes[OR_D] = () => { ExecuteOR(Registers.D); };
        _opCodes[OR_E] = () => { ExecuteOR(Registers.E); };
        _opCodes[OR_H] = () => { ExecuteOR(Registers.XH); };
        _opCodes[OR_L] = () => { ExecuteOR(Registers.XL); };
        _opCodes[OR_n] = () => { ExecuteOR(ReadNextByte()); };
        _opCodes[OR_HL] = () => { ExecuteOR(ReadMemoryAtHL()); };

        _opCodes[XOR_A] = () => { ExecuteXOR(Registers.A); };
        _opCodes[XOR_B] = () => { ExecuteXOR(Registers.B); };
        _opCodes[XOR_C] = () => { ExecuteXOR(Registers.C); };
        _opCodes[XOR_D] = () => { ExecuteXOR(Registers.D); };
        _opCodes[XOR_E] = () => { ExecuteXOR(Registers.E); };
        _opCodes[XOR_H] = () => { ExecuteXOR(Registers.XH); };
        _opCodes[XOR_L] = () => { ExecuteXOR(Registers.XL); };
        _opCodes[XOR_n] = () => { ExecuteXOR(ReadNextByte()); };
        _opCodes[XOR_HL] = () => { ExecuteXOR(ReadMemoryAtHL()); };

        _opCodes[CP_A] = () => { ExecuteCP(Registers.A); };
        _opCodes[CP_B] = () => { ExecuteCP(Registers.B); };
        _opCodes[CP_C] = () => { ExecuteCP(Registers.C); };
        _opCodes[CP_D] = () => { ExecuteCP(Registers.D); };
        _opCodes[CP_E] = () => { ExecuteCP(Registers.E); };
        _opCodes[CP_H] = () => { ExecuteCP(Registers.XH); };
        _opCodes[CP_L] = () => { ExecuteCP(Registers.XL); };
        _opCodes[CP_n] = () => { ExecuteCP(ReadNextByte()); };
        _opCodes[CP_HL] = () => { ExecuteCP(ReadMemoryAtHL()); };

        _opCodes[INC_A] = () => { Registers.A = ExecuteINC(Registers.A); };
        _opCodes[INC_B] = () => { Registers.B = ExecuteINC(Registers.B); };
        _opCodes[INC_C] = () => { Registers.C = ExecuteINC(Registers.C); };
        _opCodes[INC_D] = () => { Registers.D = ExecuteINC(Registers.D); };
        _opCodes[INC_E] = () => { Registers.E = ExecuteINC(Registers.E); };
        _opCodes[INC_H] = () => { Registers.XH = ExecuteINC(Registers.XH); };
        _opCodes[INC_L] = () => { Registers.XL = ExecuteINC(Registers.XL); };
        _opCodes[INC_MHL] = () =>
        {
            var address = CalculateHLAddress();
            var value = ReadByte(address);
            AddCycles(1);
            WriteByte(address, ExecuteINC(value));
        };

        _opCodes[DEC_A] = () => { Registers.A = ExecuteDEC(Registers.A); };
        _opCodes[DEC_B] = () => { Registers.B = ExecuteDEC(Registers.B); };
        _opCodes[DEC_C] = () => { Registers.C = ExecuteDEC(Registers.C); };
        _opCodes[DEC_D] = () => { Registers.D = ExecuteDEC(Registers.D); };
        _opCodes[DEC_E] = () => { Registers.E = ExecuteDEC(Registers.E); };
        _opCodes[DEC_H] = () => { Registers.XH = ExecuteDEC(Registers.XH); };
        _opCodes[DEC_L] = () => { Registers.XL = ExecuteDEC(Registers.XL); };
        _opCodes[DEC_MHL] = () =>
        {
            var address = CalculateHLAddress();
            var value = ReadByte(address);
            AddCycles(1);
            WriteByte(address, ExecuteDEC(value));
        };
    }

    private void ExecuteADD(byte value, bool addCarry = false)
    {
        var oldValue = Registers.A;
        var newValue = Registers.A + value  + (addCarry ? (byte)(Registers.F & C) : 0);
        var result = (byte)newValue;

        Registers.F = (S | Y | X) & (Flags)result;
        Registers.F |= result == 0 ? Z : 0;
        Registers.F |= (Flags)(oldValue ^ value ^ result) & H;
        Registers.F |= newValue > 0xFF ? C : 0;
        Registers.F |= ((oldValue ^ value) & 0x80) == 0 && ((oldValue ^ result) & 0x80) != 0 ? P : 0;

        Registers.A = result;
    }

    private void ExecuteADC(byte value) => ExecuteADD(value, true);

    private void ExecuteSUB(byte value, bool subCarry = false)
    {
        var oldValue = Registers.A;
        var newValue = Registers.A - value - (subCarry ? (byte)(Registers.F & C) : 0);
        var result = (byte)newValue;

        Registers.F = (S | Y | X) & (Flags)result | N;
        Registers.F |= result == 0 ? Z : 0;
        Registers.F |= (Flags)(oldValue ^ value ^ result) & H;
        Registers.F |= newValue < 0 ? C : 0;
        Registers.F |= ((oldValue ^ value) & 0x80) != 0 && ((oldValue ^ result) & 0x80) != 0 ? P : 0;

        Registers.A = result;
    }

    private void ExecuteSBC(byte value) => ExecuteSUB(value, true);

    private void ExecuteAND(byte value)
    {
        Registers.A &= value;
        Registers.F = (S | Y | X) & (Flags)Registers.A | H | Parity.Lookup[Registers.A];
        Registers.F |= Registers.A == 0 ? Z : 0;
    }

    private void ExecuteOR(byte value)
    {
        Registers.A |= value;
        Registers.F = (S | Y | X) & (Flags)Registers.A | Parity.Lookup[Registers.A];
        Registers.F |= Registers.A == 0 ? Z : 0;
    }

    private void ExecuteXOR(byte value)
    {
        Registers.A ^= value;
        Registers.F = (S | Y | X) & (Flags)Registers.A | Parity.Lookup[Registers.A];
        Registers.F |= Registers.A == 0 ? Z : 0;
    }

    private void ExecuteCP(byte value)
    {
        var result = Registers.A - value;

        Registers.F = N;
        Registers.F |= S & (Flags)result;
        Registers.F |= (Y | X) & (Flags)value;
        Registers.F |= (Flags)(Registers.A ^ value ^ result) & H;
        Registers.F |= result == 0 ? Z : 0;
        Registers.F |= ((Registers.A ^ value) & 0x80) != 0 && ((Registers.A ^ result) & 0x80) != 0 ? P : 0;
        Registers.F |= result < 0 ? C : 0;
    }

    private byte ExecuteINC(byte value)
    {
        var result = (byte)(value + 1);

        Registers.F &= C;
        Registers.F |= (S | Y | X) & (Flags)result;
        Registers.F |= value == 0x7F ? P : 0;
        Registers.F |= (value & 0x0F) == 0x0F ? H : 0;
        Registers.F |= result == 0 ? Z : 0;

        return result;
    }

    private byte ExecuteDEC(byte value)
    {
        var result = (byte)(value - 1);

        Registers.F &= C;
        Registers.F |= N;
        Registers.F |= (S | Y | X) & (Flags)result;
        Registers.F |= value == 0x80 ? P : 0;
        Registers.F |= (value & 0x0F) == 0 ? H : 0;
        Registers.F |= result == 0 ? Z : 0;

        return result;
    }
}