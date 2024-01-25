using Z80.Net.Helpers;
using Z80.Net.Registers;
using static Z80.Net.Instructions.OpCodes;
using static Z80.Net.Registers.Flags;

namespace Z80.Net;

partial class Z80
{
    private void Add8BitArithmeticInstructions()
    {
        _opCodes[ADD_A_A] = () => { ADD(Registers.A); };
        _opCodes[ADD_A_B] = () => { ADD(Registers.B); };
        _opCodes[ADD_A_C] = () => { ADD(Registers.C); };
        _opCodes[ADD_A_D] = () => { ADD(Registers.D); };
        _opCodes[ADD_A_E] = () => { ADD(Registers.E); };
        _opCodes[ADD_A_H] = () => { ADD(Registers.XH); };
        _opCodes[ADD_A_L] = () => { ADD(Registers.XL); };
        _opCodes[ADD_A_n] = () => { ADD(ReadNextByte()); };
        _opCodes[ADD_A_HL] = () => { ADD(ReadMemoryAtHL()); };

        _opCodes[ADC_A_A] = () => { ADC(Registers.A); };
        _opCodes[ADC_A_B] = () => { ADC(Registers.B); };
        _opCodes[ADC_A_C] = () => { ADC(Registers.C); };
        _opCodes[ADC_A_D] = () => { ADC(Registers.D); };
        _opCodes[ADC_A_E] = () => { ADC(Registers.E); };
        _opCodes[ADC_A_H] = () => { ADC(Registers.XH); };
        _opCodes[ADC_A_L] = () => { ADC(Registers.XL); };
        _opCodes[ADC_A_n] = () => { ADC(ReadNextByte()); };
        _opCodes[ADC_A_HL] = () => { ADC(ReadMemoryAtHL()); };

        _opCodes[SUB_A] = () => { Sub(Registers.A); };
        _opCodes[SUB_B] = () => { Sub(Registers.B); };
        _opCodes[SUB_C] = () => { Sub(Registers.C); };
        _opCodes[SUB_D] = () => { Sub(Registers.D); };
        _opCodes[SUB_E] = () => { Sub(Registers.E); };
        _opCodes[SUB_H] = () => { Sub(Registers.XH); };
        _opCodes[SUB_L] = () => { Sub(Registers.XL); };
        _opCodes[SUB_n] = () => { Sub(ReadNextByte()); };
        _opCodes[SUB_HL] = () => { Sub(ReadMemoryAtHL()); };

        _opCodes[SBC_A_A] = () => { SBC(Registers.A); };
        _opCodes[SBC_A_B] = () => { SBC(Registers.B); };
        _opCodes[SBC_A_C] = () => { SBC(Registers.C); };
        _opCodes[SBC_A_D] = () => { SBC(Registers.D); };
        _opCodes[SBC_A_E] = () => { SBC(Registers.E); };
        _opCodes[SBC_A_H] = () => { SBC(Registers.XH); };
        _opCodes[SBC_A_L] = () => { SBC(Registers.XL); };
        _opCodes[SBC_A_n] = () => { SBC(ReadNextByte()); };
        _opCodes[SBC_A_HL] = () => { SBC(ReadMemoryAtHL()); };

        _opCodes[AND_A] = () => { AND(Registers.A); };
        _opCodes[AND_B] = () => { AND(Registers.B); };
        _opCodes[AND_C] = () => { AND(Registers.C); };
        _opCodes[AND_D] = () => { AND(Registers.D); };
        _opCodes[AND_E] = () => { AND(Registers.E); };
        _opCodes[AND_H] = () => { AND(Registers.XH); };
        _opCodes[AND_L] = () => { AND(Registers.XL); };
        _opCodes[AND_n] = () => { AND(ReadNextByte()); };
        _opCodes[AND_HL] = () => { AND(ReadMemoryAtHL()); };

        _opCodes[OR_A] = () => { OR(Registers.A); };
        _opCodes[OR_B] = () => { OR(Registers.B); };
        _opCodes[OR_C] = () => { OR(Registers.C); };
        _opCodes[OR_D] = () => { OR(Registers.D); };
        _opCodes[OR_E] = () => { OR(Registers.E); };
        _opCodes[OR_H] = () => { OR(Registers.XH); };
        _opCodes[OR_L] = () => { OR(Registers.XL); };
        _opCodes[OR_n] = () => { OR(ReadNextByte()); };
        _opCodes[OR_HL] = () => { OR(ReadMemoryAtHL()); };

        _opCodes[XOR_A] = () => { XOR(Registers.A); };
        _opCodes[XOR_B] = () => { XOR(Registers.B); };
        _opCodes[XOR_C] = () => { XOR(Registers.C); };
        _opCodes[XOR_D] = () => { XOR(Registers.D); };
        _opCodes[XOR_E] = () => { XOR(Registers.E); };
        _opCodes[XOR_H] = () => { XOR(Registers.XH); };
        _opCodes[XOR_L] = () => { XOR(Registers.XL); };
        _opCodes[XOR_n] = () => { XOR(ReadNextByte()); };
        _opCodes[XOR_HL] = () => { XOR(ReadMemoryAtHL()); };

        _opCodes[CP_A] = () => { CP(Registers.A); };
        _opCodes[CP_B] = () => { CP(Registers.B); };
        _opCodes[CP_C] = () => { CP(Registers.C); };
        _opCodes[CP_D] = () => { CP(Registers.D); };
        _opCodes[CP_E] = () => { CP(Registers.E); };
        _opCodes[CP_H] = () => { CP(Registers.XH); };
        _opCodes[CP_L] = () => { CP(Registers.XL); };
        _opCodes[CP_n] = () => { CP(ReadNextByte()); };
        _opCodes[CP_HL] = () => { CP(ReadMemoryAtHL()); };

        _opCodes[INC_A] = () => { Registers.A = INC(Registers.A); };
        _opCodes[INC_B] = () => { Registers.B = INC(Registers.B); };
        _opCodes[INC_C] = () => { Registers.C = INC(Registers.C); };
        _opCodes[INC_D] = () => { Registers.D = INC(Registers.D); };
        _opCodes[INC_E] = () => { Registers.E = INC(Registers.E); };
        _opCodes[INC_H] = () => { Registers.XH = INC(Registers.XH); };
        _opCodes[INC_L] = () => { Registers.XL = INC(Registers.XL); };
        _opCodes[INC_MHL] = () =>
        {
            var address = CalculateHLAddress();
            var value = ReadByte(address);
            AddCycles(1);
            WriteByte(address, INC(value));
        };

        _opCodes[DEC_A] = () => { Registers.A = DEC(Registers.A); };
        _opCodes[DEC_B] = () => { Registers.B = DEC(Registers.B); };
        _opCodes[DEC_C] = () => { Registers.C = DEC(Registers.C); };
        _opCodes[DEC_D] = () => { Registers.D = DEC(Registers.D); };
        _opCodes[DEC_E] = () => { Registers.E = DEC(Registers.E); };
        _opCodes[DEC_H] = () => { Registers.XH = DEC(Registers.XH); };
        _opCodes[DEC_L] = () => { Registers.XL = DEC(Registers.XL); };
        _opCodes[DEC_MHL] = () =>
        {
            var address = CalculateHLAddress();
            var value = ReadByte(address);
            AddCycles(1);
            WriteByte(address, DEC(value));
        };
    }

    private void ADD(byte value, bool addCarry = false)
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

    private void ADC(byte value) => ADD(value, true);

    private void Sub(byte value, bool subCarry = false)
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

    private void SBC(byte value) => Sub(value, true);

    private void AND(byte value)
    {
        Registers.A &= value;
        Registers.F = (S | Y | X) & (Flags)Registers.A | H | Parity.Lookup[Registers.A];
        Registers.F |= Registers.A == 0 ? Z : 0;
    }

    private void OR(byte value)
    {
        Registers.A |= value;
        Registers.F = (S | Y | X) & (Flags)Registers.A | Parity.Lookup[Registers.A];
        Registers.F |= Registers.A == 0 ? Z : 0;
    }

    private void XOR(byte value)
    {
        Registers.A ^= value;
        Registers.F = (S | Y | X) & (Flags)Registers.A | Parity.Lookup[Registers.A];
        Registers.F |= Registers.A == 0 ? Z : 0;
    }

    private void CP(byte value)
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

    private byte INC(byte value)
    {
        var result = (byte)(value + 1);

        Registers.F &= C;
        Registers.F |= (S | Y | X) & (Flags)result;
        Registers.F |= value == 0x7F ? P : 0;
        Registers.F |= (value & 0x0F) == 0x0F ? H : 0;
        Registers.F |= result == 0 ? Z : 0;

        return result;
    }

    private byte DEC(byte value)
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