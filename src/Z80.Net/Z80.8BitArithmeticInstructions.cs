using Z80.Net.Helpers;
using Z80.Net.Registers;
using static Z80.Net.Registers.Flags;

namespace Z80.Net;

partial class Z80
{
    private void Add8BitArithmeticInstructions()
    {
        _opCodes["ADD A,A"] = () => { ExecuteADD(Registers.A); };
        _opCodes["ADD A,B"] = () => { ExecuteADD(Registers.B); };
        _opCodes["ADD A,C"] = () => { ExecuteADD(Registers.C); };
        _opCodes["ADD A,D"] = () => { ExecuteADD(Registers.D); };
        _opCodes["ADD A,E"] = () => { ExecuteADD(Registers.E); };
        _opCodes["ADD A,H"] = () => { ExecuteADD(Registers.XH); };
        _opCodes["ADD A,L"] = () => { ExecuteADD(Registers.XL); };
        _opCodes["ADD A,n"] = () => { ExecuteADD(FetchByte()); };
        _opCodes["ADD A,(HL)"] = () => { ExecuteADD(ReadMemoryAtHL(extraIndexStates: 5)); };

        _opCodes["ADC A,A"] = () => { ExecuteADC(Registers.A); };
        _opCodes["ADC A,B"] = () => { ExecuteADC(Registers.B); };
        _opCodes["ADC A,C"] = () => { ExecuteADC(Registers.C); };
        _opCodes["ADC A,D"] = () => { ExecuteADC(Registers.D); };
        _opCodes["ADC A,E"] = () => { ExecuteADC(Registers.E); };
        _opCodes["ADC A,H"] = () => { ExecuteADC(Registers.XH); };
        _opCodes["ADC A,L"] = () => { ExecuteADC(Registers.XL); };
        _opCodes["ADC A,n"] = () => { ExecuteADC(FetchByte()); };
        _opCodes["ADC A,(HL)"] = () => { ExecuteADC(ReadMemoryAtHL(extraIndexStates: 5)); };

        _opCodes["SUB A"] = () => { ExecuteSUB(Registers.A); };
        _opCodes["SUB B"] = () => { ExecuteSUB(Registers.B); };
        _opCodes["SUB C"] = () => { ExecuteSUB(Registers.C); };
        _opCodes["SUB D"] = () => { ExecuteSUB(Registers.D); };
        _opCodes["SUB E"] = () => { ExecuteSUB(Registers.E); };
        _opCodes["SUB H"] = () => { ExecuteSUB(Registers.XH); };
        _opCodes["SUB L"] = () => { ExecuteSUB(Registers.XL); };
        _opCodes["SUB n"] = () => { ExecuteSUB(FetchByte()); };
        _opCodes["SUB (HL)"] = () => { ExecuteSUB(ReadMemoryAtHL(extraIndexStates: 5)); };

        _opCodes["SBC A,A"] = () => { ExecuteSBC(Registers.A); };
        _opCodes["SBC A,B"] = () => { ExecuteSBC(Registers.B); };
        _opCodes["SBC A,C"] = () => { ExecuteSBC(Registers.C); };
        _opCodes["SBC A,D"] = () => { ExecuteSBC(Registers.D); };
        _opCodes["SBC A,E"] = () => { ExecuteSBC(Registers.E); };
        _opCodes["SBC A,H"] = () => { ExecuteSBC(Registers.XH); };
        _opCodes["SBC A,L"] = () => { ExecuteSBC(Registers.XL); };
        _opCodes["SBC A,n"] = () => { ExecuteSBC(FetchByte()); };
        _opCodes["SBC A,(HL)"] = () => { ExecuteSBC(ReadMemoryAtHL(extraIndexStates: 5)); };

        _opCodes["AND A"] = () => { ExecuteAND(Registers.A); };
        _opCodes["AND B"] = () => { ExecuteAND(Registers.B); };
        _opCodes["AND C"] = () => { ExecuteAND(Registers.C); };
        _opCodes["AND D"] = () => { ExecuteAND(Registers.D); };
        _opCodes["AND E"] = () => { ExecuteAND(Registers.E); };
        _opCodes["AND H"] = () => { ExecuteAND(Registers.XH); };
        _opCodes["AND L"] = () => { ExecuteAND(Registers.XL); };
        _opCodes["AND n"] = () => { ExecuteAND(FetchByte()); };
        _opCodes["AND (HL)"] = () => { ExecuteAND(ReadMemoryAtHL(extraIndexStates: 5)); };

        _opCodes["OR A"] = () => { ExecuteOR(Registers.A); };
        _opCodes["OR B"] = () => { ExecuteOR(Registers.B); };
        _opCodes["OR C"] = () => { ExecuteOR(Registers.C); };
        _opCodes["OR D"] = () => { ExecuteOR(Registers.D); };
        _opCodes["OR E"] = () => { ExecuteOR(Registers.E); };
        _opCodes["OR H"] = () => { ExecuteOR(Registers.XH); };
        _opCodes["OR L"] = () => { ExecuteOR(Registers.XL); };
        _opCodes["OR n"] = () => { ExecuteOR(FetchByte()); };
        _opCodes["OR (HL)"] = () => { ExecuteOR(ReadMemoryAtHL(extraIndexStates: 5)); };

        _opCodes["XOR A"] = () => { ExecuteXOR(Registers.A); };
        _opCodes["XOR B"] = () => { ExecuteXOR(Registers.B); };
        _opCodes["XOR C"] = () => { ExecuteXOR(Registers.C); };
        _opCodes["XOR D"] = () => { ExecuteXOR(Registers.D); };
        _opCodes["XOR E"] = () => { ExecuteXOR(Registers.E); };
        _opCodes["XOR H"] = () => { ExecuteXOR(Registers.XH); };
        _opCodes["XOR L"] = () => { ExecuteXOR(Registers.XL); };
        _opCodes["XOR n"] = () => { ExecuteXOR(FetchByte()); };
        _opCodes["XOR (HL)"] = () => { ExecuteXOR(ReadMemoryAtHL(extraIndexStates: 5)); };

        _opCodes["CP A"] = () => { ExecuteCP(Registers.A); };
        _opCodes["CP B"] = () => { ExecuteCP(Registers.B); };
        _opCodes["CP C"] = () => { ExecuteCP(Registers.C); };
        _opCodes["CP D"] = () => { ExecuteCP(Registers.D); };
        _opCodes["CP E"] = () => { ExecuteCP(Registers.E); };
        _opCodes["CP H"] = () => { ExecuteCP(Registers.XH); };
        _opCodes["CP L"] = () => { ExecuteCP(Registers.XL); };
        _opCodes["CP n"] = () => { ExecuteCP(FetchByte()); };
        _opCodes["CP (HL)"] = () => { ExecuteCP(ReadMemoryAtHL(extraIndexStates: 5)); };

        _opCodes["INC A"] = () => { Registers.A = ExecuteINC(Registers.A); };
        _opCodes["INC B"] = () => { Registers.B = ExecuteINC(Registers.B); };
        _opCodes["INC C"] = () => { Registers.C = ExecuteINC(Registers.C); };
        _opCodes["INC D"] = () => { Registers.D = ExecuteINC(Registers.D); };
        _opCodes["INC E"] = () => { Registers.E = ExecuteINC(Registers.E); };
        _opCodes["INC H"] = () => { Registers.XH = ExecuteINC(Registers.XH); };
        _opCodes["INC L"] = () => { Registers.XL = ExecuteINC(Registers.XL); };
        _opCodes["INC (HL)"] = () =>
        {
            var address = CalculateHLAddress(extraIndexStates: 5);
            var value = ReadByte(address);
            AddStates(1);
            WriteByte(address, ExecuteINC(value));
        };

        _opCodes["DEC A"] = () => { Registers.A = ExecuteDEC(Registers.A); };
        _opCodes["DEC B"] = () => { Registers.B = ExecuteDEC(Registers.B); };
        _opCodes["DEC C"] = () => { Registers.C = ExecuteDEC(Registers.C); };
        _opCodes["DEC D"] = () => { Registers.D = ExecuteDEC(Registers.D); };
        _opCodes["DEC E"] = () => { Registers.E = ExecuteDEC(Registers.E); };
        _opCodes["DEC H"] = () => { Registers.XH = ExecuteDEC(Registers.XH); };
        _opCodes["DEC L"] = () => { Registers.XL = ExecuteDEC(Registers.XL); };
        _opCodes["DEC (HL)"] = () =>
        {
            var address = CalculateHLAddress(extraIndexStates: 5);
            var value = ReadByte(address);
            AddStates(1);
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