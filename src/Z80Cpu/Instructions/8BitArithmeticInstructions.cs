using OldBit.Z80Cpu.Helpers;
using OldBit.Z80Cpu.Registers;
using static OldBit.Z80Cpu.Registers.Flags;

namespace OldBit.Z80Cpu;

partial class Z80
{
    private void Add8BitArithmeticInstructions()
    {
        _opCodes["ADD A,A"] = () => { Execute_ADD(Registers.A); };
        _opCodes["ADD A,B"] = () => { Execute_ADD(Registers.B); };
        _opCodes["ADD A,C"] = () => { Execute_ADD(Registers.C); };
        _opCodes["ADD A,D"] = () => { Execute_ADD(Registers.D); };
        _opCodes["ADD A,E"] = () => { Execute_ADD(Registers.E); };
        _opCodes["ADD A,H"] = () => { Execute_ADD(Registers.XH); };
        _opCodes["ADD A,L"] = () => { Execute_ADD(Registers.XL); };
        _opCodes["ADD A,n"] = () => { Execute_ADD(FetchByte()); };
        _opCodes["ADD A,(HL)"] = () => { Execute_ADD(ReadByteAtExtendedHL(extraIndexStates: 5)); };

        _opCodes["ADC A,A"] = () => { Execute_ADC(Registers.A); };
        _opCodes["ADC A,B"] = () => { Execute_ADC(Registers.B); };
        _opCodes["ADC A,C"] = () => { Execute_ADC(Registers.C); };
        _opCodes["ADC A,D"] = () => { Execute_ADC(Registers.D); };
        _opCodes["ADC A,E"] = () => { Execute_ADC(Registers.E); };
        _opCodes["ADC A,H"] = () => { Execute_ADC(Registers.XH); };
        _opCodes["ADC A,L"] = () => { Execute_ADC(Registers.XL); };
        _opCodes["ADC A,n"] = () => { Execute_ADC(FetchByte()); };
        _opCodes["ADC A,(HL)"] = () => { Execute_ADC(ReadByteAtExtendedHL(extraIndexStates: 5)); };

        _opCodes["SUB A"] = () => { Execute_SUB(Registers.A); };
        _opCodes["SUB B"] = () => { Execute_SUB(Registers.B); };
        _opCodes["SUB C"] = () => { Execute_SUB(Registers.C); };
        _opCodes["SUB D"] = () => { Execute_SUB(Registers.D); };
        _opCodes["SUB E"] = () => { Execute_SUB(Registers.E); };
        _opCodes["SUB H"] = () => { Execute_SUB(Registers.XH); };
        _opCodes["SUB L"] = () => { Execute_SUB(Registers.XL); };
        _opCodes["SUB n"] = () => { Execute_SUB(FetchByte()); };
        _opCodes["SUB (HL)"] = () => { Execute_SUB(ReadByteAtExtendedHL(extraIndexStates: 5)); };

        _opCodes["SBC A,A"] = () => { Execute_SBC(Registers.A); };
        _opCodes["SBC A,B"] = () => { Execute_SBC(Registers.B); };
        _opCodes["SBC A,C"] = () => { Execute_SBC(Registers.C); };
        _opCodes["SBC A,D"] = () => { Execute_SBC(Registers.D); };
        _opCodes["SBC A,E"] = () => { Execute_SBC(Registers.E); };
        _opCodes["SBC A,H"] = () => { Execute_SBC(Registers.XH); };
        _opCodes["SBC A,L"] = () => { Execute_SBC(Registers.XL); };
        _opCodes["SBC A,n"] = () => { Execute_SBC(FetchByte()); };
        _opCodes["SBC A,(HL)"] = () => { Execute_SBC(ReadByteAtExtendedHL(extraIndexStates: 5)); };

        _opCodes["AND A"] = () => { Execute_AND(Registers.A); };
        _opCodes["AND B"] = () => { Execute_AND(Registers.B); };
        _opCodes["AND C"] = () => { Execute_AND(Registers.C); };
        _opCodes["AND D"] = () => { Execute_AND(Registers.D); };
        _opCodes["AND E"] = () => { Execute_AND(Registers.E); };
        _opCodes["AND H"] = () => { Execute_AND(Registers.XH); };
        _opCodes["AND L"] = () => { Execute_AND(Registers.XL); };
        _opCodes["AND n"] = () => { Execute_AND(FetchByte()); };
        _opCodes["AND (HL)"] = () => { Execute_AND(ReadByteAtExtendedHL(extraIndexStates: 5)); };

        _opCodes["OR A"] = () => { Execute_OR(Registers.A); };
        _opCodes["OR B"] = () => { Execute_OR(Registers.B); };
        _opCodes["OR C"] = () => { Execute_OR(Registers.C); };
        _opCodes["OR D"] = () => { Execute_OR(Registers.D); };
        _opCodes["OR E"] = () => { Execute_OR(Registers.E); };
        _opCodes["OR H"] = () => { Execute_OR(Registers.XH); };
        _opCodes["OR L"] = () => { Execute_OR(Registers.XL); };
        _opCodes["OR n"] = () => { Execute_OR(FetchByte()); };
        _opCodes["OR (HL)"] = () => { Execute_OR(ReadByteAtExtendedHL(extraIndexStates: 5)); };

        _opCodes["XOR A"] = () => { Execute_XOR(Registers.A); };
        _opCodes["XOR B"] = () => { Execute_XOR(Registers.B); };
        _opCodes["XOR C"] = () => { Execute_XOR(Registers.C); };
        _opCodes["XOR D"] = () => { Execute_XOR(Registers.D); };
        _opCodes["XOR E"] = () => { Execute_XOR(Registers.E); };
        _opCodes["XOR H"] = () => { Execute_XOR(Registers.XH); };
        _opCodes["XOR L"] = () => { Execute_XOR(Registers.XL); };
        _opCodes["XOR n"] = () => { Execute_XOR(FetchByte()); };
        _opCodes["XOR (HL)"] = () => { Execute_XOR(ReadByteAtExtendedHL(extraIndexStates: 5)); };

        _opCodes["CP A"] = () => { Execute_CP(Registers.A); };
        _opCodes["CP B"] = () => { Execute_CP(Registers.B); };
        _opCodes["CP C"] = () => { Execute_CP(Registers.C); };
        _opCodes["CP D"] = () => { Execute_CP(Registers.D); };
        _opCodes["CP E"] = () => { Execute_CP(Registers.E); };
        _opCodes["CP H"] = () => { Execute_CP(Registers.XH); };
        _opCodes["CP L"] = () => { Execute_CP(Registers.XL); };
        _opCodes["CP n"] = () => { Execute_CP(FetchByte()); };
        _opCodes["CP (HL)"] = () => { Execute_CP(ReadByteAtExtendedHL(extraIndexStates: 5)); };

        _opCodes["INC A"] = () => { Registers.A = Execute_INC(Registers.A); };
        _opCodes["INC B"] = () => { Registers.B = Execute_INC(Registers.B); };
        _opCodes["INC C"] = () => { Registers.C = Execute_INC(Registers.C); };
        _opCodes["INC D"] = () => { Registers.D = Execute_INC(Registers.D); };
        _opCodes["INC E"] = () => { Registers.E = Execute_INC(Registers.E); };
        _opCodes["INC H"] = () => { Registers.XH = Execute_INC(Registers.XH); };
        _opCodes["INC L"] = () => { Registers.XL = Execute_INC(Registers.XL); };
        _opCodes["INC (HL)"] = () =>
        {
            var address = CalculateExtendedHL(extraIndexStates: 5);
            var value = ReadByte(address);

            Clock.MemoryContention(Registers.Context == RegisterContext.HL ? Registers.HL: address, 1);

            WriteByte(address, Execute_INC(value));
        };

        _opCodes["DEC A"] = () => { Registers.A = Execute_DEC(Registers.A); };
        _opCodes["DEC B"] = () => { Registers.B = Execute_DEC(Registers.B); };
        _opCodes["DEC C"] = () => { Registers.C = Execute_DEC(Registers.C); };
        _opCodes["DEC D"] = () => { Registers.D = Execute_DEC(Registers.D); };
        _opCodes["DEC E"] = () => { Registers.E = Execute_DEC(Registers.E); };
        _opCodes["DEC H"] = () => { Registers.XH = Execute_DEC(Registers.XH); };
        _opCodes["DEC L"] = () => { Registers.XL = Execute_DEC(Registers.XL); };
        _opCodes["DEC (HL)"] = () =>
        {
            var address = CalculateExtendedHL(extraIndexStates: 5);
            var value = ReadByte(address);

            Clock.MemoryContention(Registers.Context == RegisterContext.HL ? Registers.HL: address, 1);

            WriteByte(address, Execute_DEC(value));
        };
    }

    private void Execute_ADD(byte value, bool addCarry = false)
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

    private void Execute_ADC(byte value) => Execute_ADD(value, true);

    private void Execute_SUB(byte value, bool subCarry = false)
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

    private void Execute_SBC(byte value) => Execute_SUB(value, true);

    private void Execute_AND(byte value)
    {
        Registers.A &= value;
        Registers.F = (S | Y | X) & (Flags)Registers.A | H | Parity.Lookup[Registers.A];
        Registers.F |= Registers.A == 0 ? Z : 0;
    }

    private void Execute_OR(byte value)
    {
        Registers.A |= value;
        Registers.F = (S | Y | X) & (Flags)Registers.A | Parity.Lookup[Registers.A];
        Registers.F |= Registers.A == 0 ? Z : 0;
    }

    private void Execute_XOR(byte value)
    {
        Registers.A ^= value;
        Registers.F = (S | Y | X) & (Flags)Registers.A | Parity.Lookup[Registers.A];
        Registers.F |= Registers.A == 0 ? Z : 0;
    }

    private void Execute_CP(byte value)
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

    private byte Execute_INC(byte value)
    {
        var result = (byte)(value + 1);

        Registers.F &= C;
        Registers.F |= (S | Y | X) & (Flags)result;
        Registers.F |= value == 0x7F ? P : 0;
        Registers.F |= (value & 0x0F) == 0x0F ? H : 0;
        Registers.F |= result == 0 ? Z : 0;

        return result;
    }

    private byte Execute_DEC(byte value)
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
