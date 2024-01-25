using Z80.Net.Helpers;
using Z80.Net.Registers;
using static Z80.Net.Instructions.OpCodes;
using static Z80.Net.Registers.Flags;

namespace Z80.Net;

partial class Z80
{
    private void Add8BitArithmeticInstructions()
    {
        _opCodes[ADD_A_A] = () => { Add(Registers.A); };
        _opCodes[ADD_A_B] = () => { Add(Registers.B); };
        _opCodes[ADD_A_C] = () => { Add(Registers.C); };
        _opCodes[ADD_A_D] = () => { Add(Registers.D); };
        _opCodes[ADD_A_E] = () => { Add(Registers.E); };
        _opCodes[ADD_A_H] = () => { Add(Registers.XH); };
        _opCodes[ADD_A_L] = () => { Add(Registers.XL); };
        _opCodes[ADD_A_n] = () => { Add(ReadNextByte()); };
        _opCodes[ADD_A_HL] = () => { Add(ReadMemoryAtHL()); };

        _opCodes[ADC_A_A] = () => { Adc(Registers.A); };
        _opCodes[ADC_A_B] = () => { Adc(Registers.B); };
        _opCodes[ADC_A_C] = () => { Adc(Registers.C); };
        _opCodes[ADC_A_D] = () => { Adc(Registers.D); };
        _opCodes[ADC_A_E] = () => { Adc(Registers.E); };
        _opCodes[ADC_A_H] = () => { Adc(Registers.XH); };
        _opCodes[ADC_A_L] = () => { Adc(Registers.XL); };
        _opCodes[ADC_A_n] = () => { Adc(ReadNextByte()); };
        _opCodes[ADC_A_HL] = () => { Adc(ReadMemoryAtHL()); };

        _opCodes[SUB_A] = () => { Sub(Registers.A); };
        _opCodes[SUB_B] = () => { Sub(Registers.B); };
        _opCodes[SUB_C] = () => { Sub(Registers.C); };
        _opCodes[SUB_D] = () => { Sub(Registers.D); };
        _opCodes[SUB_E] = () => { Sub(Registers.E); };
        _opCodes[SUB_H] = () => { Sub(Registers.XH); };
        _opCodes[SUB_L] = () => { Sub(Registers.XL); };
        _opCodes[SUB_n] = () => { Sub(ReadNextByte()); };
        _opCodes[SUB_HL] = () => { Sub(ReadMemoryAtHL()); };

        _opCodes[SBC_A_A] = () => { Sbc(Registers.A); };
        _opCodes[SBC_A_B] = () => { Sbc(Registers.B); };
        _opCodes[SBC_A_C] = () => { Sbc(Registers.C); };
        _opCodes[SBC_A_D] = () => { Sbc(Registers.D); };
        _opCodes[SBC_A_E] = () => { Sbc(Registers.E); };
        _opCodes[SBC_A_H] = () => { Sbc(Registers.XH); };
        _opCodes[SBC_A_L] = () => { Sbc(Registers.XL); };
        _opCodes[SBC_A_n] = () => { Sbc(ReadNextByte()); };
        _opCodes[SBC_A_HL] = () => { Sbc(ReadMemoryAtHL()); };

        _opCodes[AND_A] = () => { And(Registers.A); };
        _opCodes[AND_B] = () => { And(Registers.B); };
        _opCodes[AND_C] = () => { And(Registers.C); };
        _opCodes[AND_D] = () => { And(Registers.D); };
        _opCodes[AND_E] = () => { And(Registers.E); };
        _opCodes[AND_H] = () => { And(Registers.XH); };
        _opCodes[AND_L] = () => { And(Registers.XL); };
        _opCodes[AND_n] = () => { And(ReadNextByte()); };
        _opCodes[AND_HL] = () => { And(ReadMemoryAtHL()); };

        _opCodes[OR_A] = () => { Or(Registers.A); };
        _opCodes[OR_B] = () => { Or(Registers.B); };
        _opCodes[OR_C] = () => { Or(Registers.C); };
        _opCodes[OR_D] = () => { Or(Registers.D); };
        _opCodes[OR_E] = () => { Or(Registers.E); };
        _opCodes[OR_H] = () => { Or(Registers.XH); };
        _opCodes[OR_L] = () => { Or(Registers.XL); };
        _opCodes[OR_n] = () => { Or(ReadNextByte()); };
        _opCodes[OR_HL] = () => { Or(ReadMemoryAtHL()); };

        _opCodes[XOR_A] = () => { Xor(Registers.A); };
        _opCodes[XOR_B] = () => { Xor(Registers.B); };
        _opCodes[XOR_C] = () => { Xor(Registers.C); };
        _opCodes[XOR_D] = () => { Xor(Registers.D); };
        _opCodes[XOR_E] = () => { Xor(Registers.E); };
        _opCodes[XOR_H] = () => { Xor(Registers.XH); };
        _opCodes[XOR_L] = () => { Xor(Registers.XL); };
        _opCodes[XOR_n] = () => { Xor(ReadNextByte()); };
        _opCodes[XOR_HL] = () => { Xor(ReadMemoryAtHL()); };

        _opCodes[CP_A] = () => { Cp(Registers.A); };
        _opCodes[CP_B] = () => { Cp(Registers.B); };
        _opCodes[CP_C] = () => { Cp(Registers.C); };
        _opCodes[CP_D] = () => { Cp(Registers.D); };
        _opCodes[CP_E] = () => { Cp(Registers.E); };
        _opCodes[CP_H] = () => { Cp(Registers.XH); };
        _opCodes[CP_L] = () => { Cp(Registers.XL); };
        _opCodes[CP_n] = () => { Cp(ReadNextByte()); };
        _opCodes[CP_HL] = () => { Cp(ReadMemoryAtHL()); };

        _opCodes[INC_A] = () => { Registers.A = Inc(Registers.A); };
        _opCodes[INC_B] = () => { Registers.B = Inc(Registers.B); };
        _opCodes[INC_C] = () => { Registers.C = Inc(Registers.C); };
        _opCodes[INC_D] = () => { Registers.D = Inc(Registers.D); };
        _opCodes[INC_E] = () => { Registers.E = Inc(Registers.E); };
        _opCodes[INC_H] = () => { Registers.XH = Inc(Registers.XH); };
        _opCodes[INC_L] = () => { Registers.XL = Inc(Registers.XL); };
        _opCodes[INC_MHL] = () =>
        {
            var address = CalculateHLAddress();
            var value = ReadByte(address);
            AddCycles(1);
            WriteByte(address, Inc(value));
        };

        _opCodes[DEC_A] = () => { Registers.A = Dec(Registers.A); };
        _opCodes[DEC_B] = () => { Registers.B = Dec(Registers.B); };
        _opCodes[DEC_C] = () => { Registers.C = Dec(Registers.C); };
        _opCodes[DEC_D] = () => { Registers.D = Dec(Registers.D); };
        _opCodes[DEC_E] = () => { Registers.E = Dec(Registers.E); };
        _opCodes[DEC_H] = () => { Registers.XH = Dec(Registers.XH); };
        _opCodes[DEC_L] = () => { Registers.XL = Dec(Registers.XL); };
        _opCodes[DEC_MHL] = () =>
        {
            var address = CalculateHLAddress();
            var value = ReadByte(address);
            AddCycles(1);
            WriteByte(address, Dec(value));
        };
    }

    private void Add(byte value, bool addCarry = false)
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

    private void Adc(byte value) => Add(value, true);

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

    private void Sbc(byte value) => Sub(value, true);

    private void And(byte value)
    {
        Registers.A &= value;
        Registers.F = (S | Y | X) & (Flags)Registers.A | H | Parity.Lookup[Registers.A];
        Registers.F |= Registers.A == 0 ? Z : 0;
    }

    private void Or(byte value)
    {
        Registers.A |= value;
        Registers.F = (S | Y | X) & (Flags)Registers.A | Parity.Lookup[Registers.A];
        Registers.F |= Registers.A == 0 ? Z : 0;
    }

    private void Xor(byte value)
    {
        Registers.A ^= value;
        Registers.F = (S | Y | X) & (Flags)Registers.A | Parity.Lookup[Registers.A];
        Registers.F |= Registers.A == 0 ? Z : 0;
    }

    private void Cp(byte value)
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

    private byte Inc(byte value)
    {
        var result = (byte)(value + 1);

        Registers.F &= C;
        Registers.F |= (S | Y | X) & (Flags)result;
        Registers.F |= value == 0x7F ? P : 0;
        Registers.F |= (value & 0x0F) == 0x0F ? H : 0;
        Registers.F |= result == 0 ? Z : 0;

        return result;
    }

    private byte Dec(byte value)
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