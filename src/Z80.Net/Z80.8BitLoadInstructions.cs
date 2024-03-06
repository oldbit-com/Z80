using Z80.Net.Registers;
using static Z80.Net.Instructions.OpCodes;

namespace Z80.Net;

partial class Z80
{
    private void Add8BitLoadInstructions()
    {
        _opCodes[LD_A_n] = () => Registers.A = ReadNextByte();
        _opCodes[LD_B_n] = () => Registers.B = ReadNextByte();
        _opCodes[LD_C_n] = () => Registers.C = ReadNextByte();
        _opCodes[LD_D_n] = () => Registers.D = ReadNextByte();
        _opCodes[LD_E_n] = () => Registers.E = ReadNextByte();
        _opCodes[LD_H_n] = () => Registers.XH = ReadNextByte();
        _opCodes[LD_L_n] = () => Registers.XL = ReadNextByte();

        _opCodes[LD_A_A] = () => { };
        _opCodes[LD_A_B] = () => Registers.A = Registers.B;
        _opCodes[LD_A_C] = () => Registers.A = Registers.C;
        _opCodes[LD_A_D] = () => Registers.A = Registers.D;
        _opCodes[LD_A_E] = () => Registers.A = Registers.E;
        _opCodes[LD_A_H] = () => Registers.A = Registers.H;
        _opCodes[LD_A_L] = () => Registers.A = Registers.L;

        _opCodes[LD_B_A] = () => Registers.B = Registers.A;
        _opCodes[LD_B_B] = () => { };
        _opCodes[LD_B_C] = () => Registers.B = Registers.C;
        _opCodes[LD_B_D] = () => Registers.B = Registers.D;
        _opCodes[LD_B_E] = () => Registers.B = Registers.E;
        _opCodes[LD_B_H] = () => Registers.B = Registers.H;
        _opCodes[LD_B_L] = () => Registers.B = Registers.L;

        _opCodes[LD_C_A] = () => Registers.C = Registers.A;
        _opCodes[LD_C_B] = () => Registers.C = Registers.B;
        _opCodes[LD_C_C] = () => { };
        _opCodes[LD_C_D] = () => Registers.C = Registers.D;
        _opCodes[LD_C_E] = () => Registers.C = Registers.E;
        _opCodes[LD_C_H] = () => Registers.C = Registers.H;
        _opCodes[LD_C_L] = () => Registers.C = Registers.L;

        _opCodes[LD_D_A] = () => Registers.D = Registers.A;
        _opCodes[LD_D_B] = () => Registers.D = Registers.B;
        _opCodes[LD_D_C] = () => Registers.D = Registers.C;
        _opCodes[LD_D_D] = () => { };
        _opCodes[LD_D_E] = () => Registers.D = Registers.E;
        _opCodes[LD_D_H] = () => Registers.D = Registers.H;
        _opCodes[LD_D_L] = () => Registers.D = Registers.L;

        _opCodes[LD_E_A] = () => Registers.E = Registers.A;
        _opCodes[LD_E_B] = () => Registers.E = Registers.B;
        _opCodes[LD_E_C] = () => Registers.E = Registers.C;
        _opCodes[LD_E_D] = () => Registers.E = Registers.D;
        _opCodes[LD_E_E] = () => { };
        _opCodes[LD_E_H] = () => Registers.E = Registers.H;
        _opCodes[LD_E_L] = () => Registers.E = Registers.L;

        _opCodes[LD_H_A] = () => Registers.H = Registers.A;
        _opCodes[LD_H_B] = () => Registers.H = Registers.B;
        _opCodes[LD_H_C] = () => Registers.H = Registers.C;
        _opCodes[LD_H_D] = () => Registers.H = Registers.D;
        _opCodes[LD_H_E] = () => Registers.H = Registers.E;
        _opCodes[LD_H_H] = () => { };
        _opCodes[LD_H_L] = () => Registers.H = Registers.L;

        _opCodes[LD_L_A] = () => Registers.L = Registers.A;
        _opCodes[LD_L_B] = () => Registers.L = Registers.B;
        _opCodes[LD_L_C] = () => Registers.L = Registers.C;
        _opCodes[LD_L_D] = () => Registers.L = Registers.D;
        _opCodes[LD_L_E] = () => Registers.L = Registers.E;
        _opCodes[LD_L_H] = () => Registers.L = Registers.H;
        _opCodes[LD_L_L] = () => { };

        _opCodes[LD_A_HL] = () => Registers.A = ReadByte(CalculateHLAddress());
        _opCodes[LD_B_HL] = () => Registers.B = ReadByte(CalculateHLAddress());
        _opCodes[LD_C_HL] = () => Registers.C = ReadByte(CalculateHLAddress());
        _opCodes[LD_D_HL] = () => Registers.D = ReadByte(CalculateHLAddress());
        _opCodes[LD_E_HL] = () => Registers.E = ReadByte(CalculateHLAddress());
        _opCodes[LD_H_HL] = () => Registers.H = ReadByte(CalculateHLAddress());
        _opCodes[LD_L_HL] = () => Registers.L = ReadByte(CalculateHLAddress());

        _opCodes[LD_HL_A] = () => { WriteByte(CalculateHLAddress(), Registers.A); };
        _opCodes[LD_HL_B] = () => { WriteByte(CalculateHLAddress(), Registers.B); };
        _opCodes[LD_HL_C] = () => { WriteByte(CalculateHLAddress(), Registers.C); };
        _opCodes[LD_HL_D] = () => { WriteByte(CalculateHLAddress(), Registers.D); };
        _opCodes[LD_HL_E] = () => { WriteByte(CalculateHLAddress(), Registers.E); };
        _opCodes[LD_HL_H] = () => { WriteByte(CalculateHLAddress(), Registers.H); };
        _opCodes[LD_HL_L] = () => { WriteByte(CalculateHLAddress(), Registers.L); };
    }
}