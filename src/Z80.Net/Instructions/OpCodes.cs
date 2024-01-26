namespace Z80.Net.Instructions;

public static class OpCodes
{
	static OpCodes()
	{
		foreach (var field in typeof(OpCodes).GetFields().Where(f => f.FieldType == typeof(OpCode)))
		{
			var fieldInfo = typeof(OpCodes).GetField(field.Name);
			var fieldValue = fieldInfo?.GetValue(null);
			if (fieldValue is OpCode opCode)
			{
				AllOpCodes.Add(opCode, opCode);
			}
		}
	}

	public static readonly Dictionary<int, OpCode> AllOpCodes = new();

    // Primary op codes
    public static readonly OpCode NOP         = new(0x00, "NOP");
	public static readonly OpCode LD_BC_nn    = new(0x01, "LD BC,NN", 2);
	public static readonly OpCode LD_BC_A     = new(0x02, "LD (BC),A");
	public static readonly OpCode INC_BC      = new(0x03, "INC BC");
	public static readonly OpCode INC_B       = new(0x04, "INC B");
	public static readonly OpCode DEC_B       = new(0x05, "DEC B");
	public static readonly OpCode LD_B_n      = new(0x06, "LD B,N", 1);
	public static readonly OpCode RLCA        = new(0x07, "RLCA");
	public static readonly OpCode EX_AF_AF    = new(0x08, "EX AF,AF'");
	public static readonly OpCode ADD_HL_BC   = new(0x09, "ADD HL,BC");
	public static readonly OpCode LD_A_BC     = new(0x0A, "LD A,(BC)");
	public static readonly OpCode DEC_BC      = new(0x0B, "DEC BC");
	public static readonly OpCode INC_C       = new(0x0C, "INC C");
	public static readonly OpCode DEC_C       = new(0x0D, "DEC C");
	public static readonly OpCode LD_C_n      = new(0x0E, "LD C,N", 1);
	public static readonly OpCode RRCA        = new(0x0F, "RRCA");
	public static readonly OpCode DJNZ        = new(0x10, "DJNZ O");
	public static readonly OpCode LD_DE_nn    = new(0x11, "LD DE,NN", 2);
	public static readonly OpCode LD_DE_A     = new(0x12, "LD (DE),A");
	public static readonly OpCode INC_DE      = new(0x13, "INC DE");
	public static readonly OpCode INC_D       = new(0x14, "INC D");
	public static readonly OpCode DEC_D       = new(0x15, "DEC D");
	public static readonly OpCode LD_D_n      = new(0x16, "LD D,N", 1);
	public static readonly OpCode RLA         = new(0x17, "RLA");
	public static readonly OpCode JR          = new(0x18, "JR", 1);
	public static readonly OpCode ADD_HL_DE   = new(0x19, "ADD HL,DE");
	public static readonly OpCode LD_A_DE     = new(0x1A, "LD A,(DE)");
	public static readonly OpCode DEC_DE      = new(0x1B, "DEC DE");
	public static readonly OpCode INC_E       = new(0x1C, "INC E");
	public static readonly OpCode DEC_E       = new(0x1D, "DEC E");
	public static readonly OpCode LD_E_n      = new(0x1E, "LD E,N");
	public static readonly OpCode RRA         = new(0x1F, "RRA");
	public static readonly OpCode JR_NZ       = new(0x20, "JR NZ", 1);
	public static readonly OpCode LD_HL_nn    = new(0x21, "LD HL,NN", 2);
	public static readonly OpCode LD_mm_HL    = new(0x22, "LD (NN),HL");
	public static readonly OpCode INC_HL      = new(0x23, "INC HL");
	public static readonly OpCode INC_H       = new(0x24, "INC H");
	public static readonly OpCode DEC_H       = new(0x25, "DEC H");
	public static readonly OpCode LD_H_n      = new(0x26, "LD H,N");
	public static readonly OpCode DAA         = new(0x27, "DAA");
	public static readonly OpCode JR_Z        = new(0x28, "JR Z", 1);
	public static readonly OpCode ADD_HL_HL   = new(0x29, "ADD HL,HL");
	public static readonly OpCode LD_HL_mm    = new(0x2A, "LD HL,(NN)");
	public static readonly OpCode DEC_HL      = new(0x2B, "DEC HL");
	public static readonly OpCode INC_L       = new(0x2C, "INC L");
	public static readonly OpCode DEC_L       = new(0x2D, "DEC L");
	public static readonly OpCode LD_L_n      = new(0x2E, "LD L,N");
	public static readonly OpCode CPL         = new(0x2F, "CPL");
	public static readonly OpCode JR_NC       = new(0x30, "JR NC", 1);
	public static readonly OpCode LD_SP_nn    = new(0x31, "LD SP,NN", 2);
	public static readonly OpCode LD_mm_A     = new(0x32, "LD (NN),A");
	public static readonly OpCode INC_SP      = new(0x33, "INC SP");
	public static readonly OpCode INC_MHL     = new(0x34, "INC (HL)");
	public static readonly OpCode DEC_MHL     = new(0x35, "DEC (HL)");
	public static readonly OpCode LD_MHL_n    = new(0x36, "LD (HL),N");
	public static readonly OpCode SCF         = new(0x37, "SCF");
	public static readonly OpCode JR_C        = new(0x38, "JR C,O", 1);
	public static readonly OpCode ADD_HL_SP   = new(0x39, "ADD HL,SP");
	public static readonly OpCode LD_A_mm     = new(0x3A, "LD A,(NN)");
	public static readonly OpCode DEC_SP      = new(0x3B, "DEC SP");
	public static readonly OpCode INC_A       = new(0x3C, "INC A");
	public static readonly OpCode DEC_A       = new(0x3D, "DEC A");
	public static readonly OpCode LD_A_n      = new(0x3E, "LD A,N", 1);
	public static readonly OpCode CCF         = new(0x3F, "CCF");
	public static readonly OpCode LD_B_B      = new(0x40, "LD B,B");
	public static readonly OpCode LD_B_C      = new(0x41, "LD B,C");
	public static readonly OpCode LD_B_D      = new(0x42, "LD B,D");
	public static readonly OpCode LD_B_E      = new(0x43, "LD B,E");
	public static readonly OpCode LD_B_H      = new(0x44, "LD B,H");
	public static readonly OpCode LD_B_L      = new(0x45, "LD B,L");
	public static readonly OpCode LD_B_HL     = new(0x46, "LD B,(HL)");
	public static readonly OpCode LD_B_A      = new(0x47, "LD B,A");
	public static readonly OpCode LD_C_B      = new(0x48, "LD C,B");
	public static readonly OpCode LD_C_C      = new(0x49, "LD C,C");
	public static readonly OpCode LD_C_D      = new(0x4A, "LD C,D");
	public static readonly OpCode LD_C_E      = new(0x4B, "LD C,E");
	public static readonly OpCode LD_C_H      = new(0x4C, "LD C,H");
	public static readonly OpCode LD_C_L      = new(0x4D, "LD C,L");
	public static readonly OpCode LD_C_HL     = new(0x4E, "LD C,(HL)");
	public static readonly OpCode LD_C_A      = new(0x4F, "LD C,A");
	public static readonly OpCode LD_D_B      = new(0x50, "LD D,B");
	public static readonly OpCode LD_D_C      = new(0x51, "LD D,C");
	public static readonly OpCode LD_D_D      = new(0x52, "LD D,D");
	public static readonly OpCode LD_D_E      = new(0x53, "LD D,E");
	public static readonly OpCode LD_D_H      = new(0x54, "LD D,H");
	public static readonly OpCode LD_D_L      = new(0x55, "LD D,L");
	public static readonly OpCode LD_D_HL     = new(0x56, "LD D,(HL)");
	public static readonly OpCode LD_D_A      = new(0x57, "LD D,A√");
	public static readonly OpCode LD_E_B      = new(0x58, "LD E,B√");
	public static readonly OpCode LD_E_C      = new(0x59, "LD E,C√");
	public static readonly OpCode LD_E_D      = new(0x5A, "LD E,D√");
	public static readonly OpCode LD_E_E      = new(0x5B, "LD E,E√");
	public static readonly OpCode LD_E_H      = new(0x5C, "LD E,H√");
	public static readonly OpCode LD_E_L      = new(0x5D, "LD E,L√");
	public static readonly OpCode LD_E_HL     = new(0x5E, "LD E,(HL)");
	public static readonly OpCode LD_E_A      = new(0x5F, "LD E,A");
	public static readonly OpCode LD_H_B      = new(0x60, "LD H,B");
	public static readonly OpCode LD_H_C      = new(0x61, "LD H,C");
	public static readonly OpCode LD_H_D      = new(0x62, "LD H,D");
	public static readonly OpCode LD_H_E      = new(0x63, "LD H,E");
	public static readonly OpCode LD_H_H      = new(0x64, "LD H,H");
	public static readonly OpCode LD_H_L      = new(0x65, "LD H,L");
	public static readonly OpCode LD_H_HL     = new(0x66, "LD H,(HL)");
	public static readonly OpCode LD_H_A      = new(0x67, "LD H,A");
	public static readonly OpCode LD_L_B      = new(0x68, "LD L,B");
	public static readonly OpCode LD_L_C      = new(0x69, "LD L,C");
	public static readonly OpCode LD_L_D      = new(0x6A, "LD L,D");
	public static readonly OpCode LD_L_E      = new(0x6B, "LD L,E");
	public static readonly OpCode LD_L_H      = new(0x6C, "LD L,H");
	public static readonly OpCode LD_L_L      = new(0x6D, "LD L,L");
	public static readonly OpCode LD_L_HL     = new(0x6E, "LD L,(HL)");
	public static readonly OpCode LD_L_A      = new(0x6F, "LD L,A");
	public static readonly OpCode LD_HL_B     = new(0x70, "LD (HL),B");
	public static readonly OpCode LD_HL_C     = new(0x71, "LD (HL),C");
	public static readonly OpCode LD_HL_D     = new(0x72, "LD (HL),D");
	public static readonly OpCode LD_HL_E     = new(0x73, "LD (HL),E");
	public static readonly OpCode LD_HL_H     = new(0x74, "LD (HL),H");
	public static readonly OpCode LD_HL_L     = new(0x75, "LD (HL),L");
	public static readonly OpCode HALT        = new(0x76, "HALT");
	public static readonly OpCode LD_HL_A     = new(0x77, "LD (HL),A");
	public static readonly OpCode LD_A_B      = new(0x78, "LD A,B");
	public static readonly OpCode LD_A_C      = new(0x79, "LD A,C");
	public static readonly OpCode LD_A_D      = new(0x7A, "LD A,D");
	public static readonly OpCode LD_A_E      = new(0x7B, "LD A,E");
	public static readonly OpCode LD_A_H      = new(0x7C, "LD A,H");
	public static readonly OpCode LD_A_L      = new(0x7D, "LD A,L");
	public static readonly OpCode LD_A_HL     = new(0x7E, "LD A,(HL)");
	public static readonly OpCode LD_A_A      = new(0x7F, "LD A,A");
	public static readonly OpCode ADD_A_B     = new(0x80, "ADD A,B");
	public static readonly OpCode ADD_A_C     = new(0x81, "ADD A,C");
	public static readonly OpCode ADD_A_D     = new(0x82, "ADD A,D");
	public static readonly OpCode ADD_A_E     = new(0x83, "ADD A,E");
	public static readonly OpCode ADD_A_H     = new(0x84, "ADD A,H");
	public static readonly OpCode ADD_A_L     = new(0x85, "ADD A,L");
	public static readonly OpCode ADD_A_HL    = new(0x86, "ADD A,(HL)");
	public static readonly OpCode ADD_A_A     = new(0x87, "ADD A,A");
	public static readonly OpCode ADC_A_B     = new(0x88, "ADC A,B");
	public static readonly OpCode ADC_A_C     = new(0x89, "ADC A,C");
	public static readonly OpCode ADC_A_D     = new(0x8A, "ADC A,D");
	public static readonly OpCode ADC_A_E     = new(0x8B, "ADC A,E");
	public static readonly OpCode ADC_A_H     = new(0x8C, "ADC A,H");
	public static readonly OpCode ADC_A_L     = new(0x8D, "ADC A,L");
	public static readonly OpCode ADC_A_HL    = new(0x8E, "ADC A,(HL)");
	public static readonly OpCode ADC_A_A     = new(0x8F, "ADC A,A");
	public static readonly OpCode SUB_B       = new(0x90, "SUB B");
	public static readonly OpCode SUB_C       = new(0x91, "SUB C");
	public static readonly OpCode SUB_D       = new(0x92, "SUB D");
	public static readonly OpCode SUB_E       = new(0x93, "SUB E");
	public static readonly OpCode SUB_H       = new(0x94, "SUB H");
	public static readonly OpCode SUB_L       = new(0x95, "SUB L");
	public static readonly OpCode SUB_HL      = new(0x96, "SUB (HL)");
	public static readonly OpCode SUB_A       = new(0x97, "SUB A");
	public static readonly OpCode SBC_A_B     = new(0x98, "SBC A,B");
	public static readonly OpCode SBC_A_C     = new(0x99, "SBC A,C");
	public static readonly OpCode SBC_A_D     = new(0x9A, "SBC A,D");
	public static readonly OpCode SBC_A_E     = new(0x9B, "SBC A,E");
	public static readonly OpCode SBC_A_H     = new(0x9C, "SBC A,H");
	public static readonly OpCode SBC_A_L     = new(0x9D, "SBC A,L");
	public static readonly OpCode SBC_A_HL    = new(0x9E, "SBC A,(HL)");
	public static readonly OpCode SBC_A_A     = new(0x9F, "SBC A,A");
	public static readonly OpCode AND_B       = new(0xA0, "AND B");
	public static readonly OpCode AND_C       = new(0xA1, "AND C");
	public static readonly OpCode AND_D       = new(0xA2, "AND D");
	public static readonly OpCode AND_E       = new(0xA3, "AND E");
	public static readonly OpCode AND_H       = new(0xA4, "AND H");
	public static readonly OpCode AND_L       = new(0xA5, "AND L");
	public static readonly OpCode AND_HL      = new(0xA6, "AND (HL)");
	public static readonly OpCode AND_A       = new(0xA7, "AND A");
	public static readonly OpCode XOR_B       = new(0xA8, "XOR B");
	public static readonly OpCode XOR_C       = new(0xA9, "XOR C");
	public static readonly OpCode XOR_D       = new(0xAA, "XOR D");
	public static readonly OpCode XOR_E       = new(0xAB, "XOR E");
	public static readonly OpCode XOR_H       = new(0xAC, "XOR H");
	public static readonly OpCode XOR_L       = new(0xAD, "XOR L");
	public static readonly OpCode XOR_HL      = new(0xAE, "XOR (HL)");
	public static readonly OpCode XOR_A       = new(0xAF, "XOR A");
	public static readonly OpCode OR_B        = new(0xB0, "OR B");
	public static readonly OpCode OR_C        = new(0xB1, "OR C");
	public static readonly OpCode OR_D        = new(0xB2, "OR D");
	public static readonly OpCode OR_E        = new(0xB3, "OR E");
	public static readonly OpCode OR_H        = new(0xB4, "OR H");
	public static readonly OpCode OR_L        = new(0xB5, "OR L");
	public static readonly OpCode OR_HL       = new(0xB6, "OR (HL)");
	public static readonly OpCode OR_A        = new(0xB7, "OR A");
	public static readonly OpCode CP_B        = new(0xB8, "CP B");
	public static readonly OpCode CP_C        = new(0xB9, "CP C");
	public static readonly OpCode CP_D        = new(0xBA, "CP D");
	public static readonly OpCode CP_E        = new(0xBB, "CP E");
	public static readonly OpCode CP_H        = new(0xBC, "CP H");
	public static readonly OpCode CP_L        = new(0xBD, "CP L");
	public static readonly OpCode CP_HL       = new(0xBE, "CP (HL)");
	public static readonly OpCode CP_A        = new(0xBF, "CP A");
	public static readonly OpCode RET_NZ      = new(0xC0, "RET NZ");
	public static readonly OpCode POP_BC      = new(0xC1, "POP BC");
	public static readonly OpCode JP_NZ       = new(0xC2, "JP NZ,NN", 2);
	public static readonly OpCode JP          = new(0xC3, "JP NN", 2);
	public static readonly OpCode CALL_NZ     = new(0xC4, "CALL NZ,NN");
	public static readonly OpCode PUSH_BC     = new(0xC5, "PUSH BC");
	public static readonly OpCode ADD_A_n     = new(0xC6, "ADD A.N");
	public static readonly OpCode RST_00      = new(0xC7, "RST 00H");
	public static readonly OpCode RET_Z       = new(0xC8, "RET Z");
	public static readonly OpCode RET         = new(0xC9, "RET");
	public static readonly OpCode JP_Z        = new(0xCA, "JP Z,NN", 2);
	public static readonly OpCode CB          = new(0xCB, "BIT OPERATIONS PREFIX");
	public static readonly OpCode CALL_Z      = new(0xCC, "CALL Z,NN");
	public static readonly OpCode CALL        = new(0xCD, "CALL NN");
	public static readonly OpCode ADC_A_n     = new(0xCE, "ADC A,N");
	public static readonly OpCode RST_08      = new(0xCF, "RST 08H");
	public static readonly OpCode RET_NC      = new(0xD0, "RET NC");
	public static readonly OpCode POP_DE      = new(0xD1, "POP DE");
	public static readonly OpCode JP_NC       = new(0xD2, "JP NC,NN", 2);
	public static readonly OpCode OUT_N_A     = new(0xD3, "OUT (N),A");
	public static readonly OpCode CALL_NC     = new(0xD4, "CALL NC,NN");
	public static readonly OpCode PUSH_DE     = new(0xD5, "PUSH DE");
	public static readonly OpCode SUB_n       = new(0xD6, "SUB N");
	public static readonly OpCode RST_10      = new(0xD7, "RST 10H");
	public static readonly OpCode RET_C       = new(0xD8, "RET C");
	public static readonly OpCode EXX         = new(0xD9, "EXX");
	public static readonly OpCode JP_C        = new(0xDA, "JP C,NN", 2);
	public static readonly OpCode IN_A_n      = new(0xDB, "IN A,(N)");
	public static readonly OpCode CALL_C      = new(0xDC, "CALL C,NN");
	public static readonly OpCode IX          = new(0xDD, "IX INSTRUCTION PREFIX");
	public static readonly OpCode SBC_A_n     = new(0xDE, "SBC A,N");
	public static readonly OpCode RST_18      = new(0xDF, "RST 18H");
	public static readonly OpCode RET_PO      = new(0xE0, "RET PO");
	public static readonly OpCode POP_HL      = new(0xE1, "POP HL");
	public static readonly OpCode JP_PO       = new(0xE2, "JP PO,NN", 2);
	public static readonly OpCode EX_SP_HL    = new(0xE3, "EX (SP),HL");
	public static readonly OpCode CALL_PO     = new(0xE4, "CALL PO,NN");
	public static readonly OpCode PUSH_HL     = new(0xE5, "PUSH HL");
	public static readonly OpCode AND_n       = new(0xE6, "AND N");
	public static readonly OpCode RST_20      = new(0xE7, "RST 20H");
	public static readonly OpCode RET_PE      = new(0xE8, "RET PE");
	public static readonly OpCode JP_HL       = new(0xE9, "JP (HL)");
	public static readonly OpCode JP_PE       = new(0xEA, "JP PE,NN", 2);
	public static readonly OpCode EX_DE_HL    = new(0xEB, "EX DE,HL");
	public static readonly OpCode CALL_PE     = new(0xEC, "CALL PE,NN");
	public static readonly OpCode ED          = new(0xED, "ED PREFIX");
	public static readonly OpCode XOR_n       = new(0xEE, "XOR N");
	public static readonly OpCode RST_28      = new(0xEF, "RST 28H");
	public static readonly OpCode RET_P       = new(0xF0, "RET P");
	public static readonly OpCode POP_AF      = new(0xF1, "POP AF");
	public static readonly OpCode JP_P        = new(0xF2, "JP P,NN", 2);
	public static readonly OpCode DI          = new(0xF3, "DI");
	public static readonly OpCode CALL_P      = new(0xF4, "CALL P,NN");
	public static readonly OpCode PUSH_AF     = new(0xF5, "PUSH AF");
	public static readonly OpCode OR_n        = new(0xF6, "OR N");
	public static readonly OpCode RST_30      = new(0xF7, "RST 30H");
	public static readonly OpCode RET_M       = new(0xF8, "RET M");
	public static readonly OpCode LD_SP_HL    = new(0xF9, "LD SP,HL");
	public static readonly OpCode JP_M        = new(0xFA, "JP M,NN", 2);
	public static readonly OpCode EI          = new(0xFB, "EI");
	public static readonly OpCode CALL_M      = new(0xFC, "CALL M,NN");
	public static readonly OpCode IY          = new(0xFD, "IY INSTRUCTION PREFIX");
	public static readonly OpCode CP_n        = new(0xFE, "CP N");
	public static readonly OpCode RST_38      = new(0xFF, "RST 38H");

	// Extended op codes with ED prefix
    public static readonly OpCode IN_B_C      = new(0x40, 0xED, "IN B,(C)");
	public static readonly OpCode OUT_C_B     = new(0x41, 0xED, "OUT (C),B");
	public static readonly OpCode SBC_HL_BC   = new(0x42, 0xED, "SBC HL,BC");
	public static readonly OpCode LD_mm_BC    = new(0x43, 0xED, "LD (NN),BC");
	public static readonly OpCode NEG         = new(0x44, 0xED, "NEG");
	public static readonly OpCode RETN        = new(0x45, 0xED, "RETN");
	public static readonly OpCode IM0         = new(0x46, 0xED, "IM 0");
	public static readonly OpCode LD_I_A      = new(0x47, 0xED, "LD I,A");
	public static readonly OpCode IN_C_C      = new(0x48, 0xED, "IN C,(C)");
	public static readonly OpCode OUT_C_C     = new(0x49, 0xED, "OUT (C),C");
	public static readonly OpCode ADC_HL_BC   = new(0x4A, 0xED, "ADC HL,BC");
	public static readonly OpCode LD_BC_mm    = new(0x4B, 0xED, "LD BC,(NN)");
	public static readonly OpCode RETI        = new(0x4D, 0xED, "RETI");
	public static readonly OpCode LD_R_A      = new(0x4F, 0xED, "LD R,A");
	public static readonly OpCode IN_D_C      = new(0x50, 0xED, "IN D,(C)");
	public static readonly OpCode OUT_C_D     = new(0x51, 0xED, "OUT (C),D");
	public static readonly OpCode SBC_HL_DE   = new(0x52, 0xED, "SBC HL,DE");
	public static readonly OpCode LD_mm_DE    = new(0x53, 0xED, "LD (NN),DE");
	public static readonly OpCode IM1         = new(0x56, 0xED, "IM 1");
	public static readonly OpCode LD_A_I      = new(0x57, 0xED, "LD A,I");
	public static readonly OpCode IN_E_C      = new(0x58, 0xED, "IN E,(C)");
	public static readonly OpCode OUT_C_E     = new(0x59, 0xED, "OUT (C),E");
	public static readonly OpCode ADC_HL_DE   = new(0x5A, 0xED, "ADC HL,DE");
	public static readonly OpCode LD_DE_mm    = new(0x5B, 0xED, "LD DE,(NN)");
	public static readonly OpCode IM2         = new(0x5E, 0xED, "IM 2");
	public static readonly OpCode LD_A_R      = new(0x5F, 0xED, "LD A,R");
	public static readonly OpCode IN_H_C      = new(0x60, 0xED, "IN H,(C)");
	public static readonly OpCode OUT_C_H     = new(0x61, 0xED, "OUT (C),H");
	public static readonly OpCode SBC_HL_HL   = new(0x62, 0xED, "SBC HL,HL");
	public static readonly OpCode ED_LD_mm_HL = new(0x63, 0xED, "LD (NN),HL");
	public static readonly OpCode RRD         = new(0x67, 0xED, "RRD");
	public static readonly OpCode IN_L_C      = new(0x68, 0xED, "IN L,(C)");
	public static readonly OpCode OUT_C_L     = new(0x69, 0xED, "OUT (C),L");
	public static readonly OpCode ADC_HL_HL   = new(0x6A, 0xED, "ADC HL,HL");
	public static readonly OpCode ED_LD_HL_mm = new(0x6B, 0xED, "LD HL,(NN)");
	public static readonly OpCode RLD         = new(0x6F, 0xED, "RLD");
	public static readonly OpCode IN_F_C      = new(0x70, 0xED, "IN F,(C)");
	public static readonly OpCode OUT_C_F     = new(0x71, 0xED, "OUT (C),F");
	public static readonly OpCode SBC_HL_SP   = new(0x72, 0xED, "SBC HL,SP");
	public static readonly OpCode LD_mm_SP    = new(0x73, 0xED, "LD (NN),SP");
	public static readonly OpCode IN_A_C      = new(0x78, 0xED, "IN A,(C)");
	public static readonly OpCode OUT_C_A     = new(0x79, 0xED, "OUT (C),A");
	public static readonly OpCode ADC_HL_SP   = new(0x7A, 0xED, "ADC HL,SP");
	public static readonly OpCode LD_SP_mm    = new(0x7B, 0xED, "LD SP,(NN)");
	public static readonly OpCode LDI         = new(0xA0, 0xED, "LDI");
	public static readonly OpCode CPI         = new(0xA1, 0xED, "CPI");
	public static readonly OpCode INI         = new(0xA2, 0xED, "INI");
	public static readonly OpCode OUTI        = new(0xA3, 0xED, "OUTI");
	public static readonly OpCode LDD         = new(0xA8, 0xED, "LDD");
	public static readonly OpCode CPD         = new(0xA9, 0xED, "CPD");
	public static readonly OpCode IND         = new(0xAA, 0xED, "IND");
	public static readonly OpCode OUTD        = new(0xAB, 0xED, "OUTD");
	public static readonly OpCode LDIR        = new(0xB0, 0xED, "LDIR");
	public static readonly OpCode CPIR        = new(0xB1, 0xED, "CPIR");
	public static readonly OpCode INIR        = new(0xB2, 0xED, "INIR");
	public static readonly OpCode OTIR        = new(0xB3, 0xED, "OTIR");
	public static readonly OpCode LDDR        = new(0xB8, 0xED, "LDDR");
	public static readonly OpCode CPDR        = new(0xB9, 0xED, "CPDR");
	public static readonly OpCode INDR        = new(0xBA, 0xED, "INDR");
	public static readonly OpCode OTDR        = new(0xBB, 0xED, "OTDR");

    // Bit op codes with CB prefix
    public static readonly OpCode RLC_B       = new(0x00, 0xCB, "RLC B");
    public static readonly OpCode RLC_C       = new(0x01, 0xCB, "RLC C");
    public static readonly OpCode RLC_D       = new(0x02, 0xCB, "RLC D");
    public static readonly OpCode RLC_E       = new(0x03, 0xCB, "RLC E");
    public static readonly OpCode RLC_H       = new(0x04, 0xCB, "RLC H");
    public static readonly OpCode RLC_L       = new(0x05, 0xCB, "RLC L");
    public static readonly OpCode RLC_HL      = new(0x06, 0xCB, "RLC (HL)");
    public static readonly OpCode RLC_A       = new(0x07, 0xCB, "RLC A");
    public static readonly OpCode RRC_B       = new(0x08, 0xCB, "RRC B");
    public static readonly OpCode RRC_C       = new(0x09, 0xCB, "RRC C");
    public static readonly OpCode RRC_D       = new(0x0A, 0xCB, "RRC D");
    public static readonly OpCode RRC_E       = new(0x0B, 0xCB, "RRC E");
    public static readonly OpCode RRC_H       = new(0x0C, 0xCB, "RRC H");
    public static readonly OpCode RRC_L       = new(0x0D, 0xCB, "RRC L");
    public static readonly OpCode RRC_HL      = new(0x0E, 0xCB, "RRC (HL)");
    public static readonly OpCode RRC_A       = new(0x0F, 0xCB, "RRC A");

    public static readonly OpCode RL_B        = new(0x10, 0xCB, "RL B");
    public static readonly OpCode RL_C        = new(0x11, 0xCB, "RL C");
    public static readonly OpCode RL_D        = new(0x12, 0xCB, "RL D");
    public static readonly OpCode RL_E        = new(0x13, 0xCB, "RL E");
    public static readonly OpCode RL_H        = new(0x14, 0xCB, "RL H");
    public static readonly OpCode RL_L        = new(0x15, 0xCB, "RL L");
    public static readonly OpCode RL_HL       = new(0x16, 0xCB, "RL (HL)");
    public static readonly OpCode RL_A        = new(0x17, 0xCB, "RL A");
    public static readonly OpCode RR_B        = new(0x18, 0xCB, "RR B");
    public static readonly OpCode RR_C        = new(0x19, 0xCB, "RR C");
    public static readonly OpCode RR_D        = new(0x1A, 0xCB, "RR D");
    public static readonly OpCode RR_E        = new(0x1B, 0xCB, "RR E");
    public static readonly OpCode RR_H        = new(0x1C, 0xCB, "RR H");
    public static readonly OpCode RR_L        = new(0x1D, 0xCB, "RR L");
    public static readonly OpCode RR_HL       = new(0x1E, 0xCB, "RR (HL)");
    public static readonly OpCode RR_A        = new(0x1F, 0xCB, "RR A");

    public static readonly OpCode SLA_B       = new(0x20, 0xCB, "SLA B");
    public static readonly OpCode SLA_C       = new(0x21, 0xCB, "SLA C");
    public static readonly OpCode SLA_D       = new(0x22, 0xCB, "SLA D");
    public static readonly OpCode SLA_E       = new(0x23, 0xCB, "SLA E");
    public static readonly OpCode SLA_H       = new(0x24, 0xCB, "SLA H");
    public static readonly OpCode SLA_L       = new(0x25, 0xCB, "SLA L");
    public static readonly OpCode SLA_HL      = new(0x26, 0xCB, "SLA (HL)");
    public static readonly OpCode SLA_A       = new(0x27, 0xCB, "SLA A");
    public static readonly OpCode SRA_B       = new(0x28, 0xCB, "SRA B");
    public static readonly OpCode SRA_C       = new(0x29, 0xCB, "SRA C");
    public static readonly OpCode SRA_D       = new(0x2A, 0xCB, "SRA D");
    public static readonly OpCode SRA_E       = new(0x2B, 0xCB, "SRA E");
    public static readonly OpCode SRA_H       = new(0x2C, 0xCB, "SRA H");
    public static readonly OpCode SRA_L       = new(0x2D, 0xCB, "SRA L");
    public static readonly OpCode SRA_HL      = new(0x2E, 0xCB, "SRA (HL)");
    public static readonly OpCode SRA_A       = new(0x2F, 0xCB, "SRA A");

    public static readonly OpCode SLL_B       = new(0x30, 0xCB, "SLL B");
    public static readonly OpCode SLL_C       = new(0x31, 0xCB, "SLL C");
    public static readonly OpCode SLL_D       = new(0x32, 0xCB, "SLL D");
    public static readonly OpCode SLL_E       = new(0x33, 0xCB, "SLL E");
    public static readonly OpCode SLL_H       = new(0x34, 0xCB, "SLL H");
    public static readonly OpCode SLL_L       = new(0x35, 0xCB, "SLL L");
    public static readonly OpCode SLL_HL      = new(0x36, 0xCB, "SLL (HL)");
    public static readonly OpCode SLL_A       = new(0x37, 0xCB, "SLL A");
    public static readonly OpCode SRL_B       = new(0x38, 0xCB, "SRL B");
    public static readonly OpCode SRL_C       = new(0x39, 0xCB, "SRL C");
    public static readonly OpCode SRL_D       = new(0x3A, 0xCB, "SRL D");
    public static readonly OpCode SRL_E       = new(0x3B, 0xCB, "SRL E");
    public static readonly OpCode SRL_H       = new(0x3C, 0xCB, "SRL H");
    public static readonly OpCode SRL_L       = new(0x3D, 0xCB, "SRL L");
    public static readonly OpCode SRL_HL      = new(0x3E, 0xCB, "SRL (HL)");
    public static readonly OpCode SRL_A       = new(0x3F, 0xCB, "SRL A");

    public static readonly OpCode BIT_b_B     = new(0x40, 0xCB, "BIT {bit},B");
    public static readonly OpCode BIT_b_C     = new(0x41, 0xCB, "BIT {bit},C");
    public static readonly OpCode BIT_b_D     = new(0x42, 0xCB, "BIT {bit},D");
    public static readonly OpCode BIT_b_E     = new(0x43, 0xCB, "BIT {bit},E");
    public static readonly OpCode BIT_b_H     = new(0x44, 0xCB, "BIT {bit},H");
    public static readonly OpCode BIT_b_L     = new(0x45, 0xCB, "BIT {bit},L");
    public static readonly OpCode BIT_b_HL    = new(0x46, 0xCB, "BIT {bit},(HL)");
    public static readonly OpCode BIT_b_A     = new(0x47, 0xCB, "BIT {bit},A");

    public static readonly OpCode RES_b_B     = new(0x80, 0xCB, "RES {bit},B");
    public static readonly OpCode RES_b_C     = new(0x81, 0xCB, "RES {bit},C");
    public static readonly OpCode RES_b_D     = new(0x82, 0xCB, "RES {bit},D");
    public static readonly OpCode RES_b_E     = new(0x83, 0xCB, "RES {bit},E");
    public static readonly OpCode RES_b_H     = new(0x84, 0xCB, "RES {bit},H");
    public static readonly OpCode RES_b_L     = new(0x85, 0xCB, "RES {bit},L");
    public static readonly OpCode RES_b_HL    = new(0x86, 0xCB, "RES {bit},(HL)");
    public static readonly OpCode RES_b_A     = new(0x87, 0xCB, "RES {bit},A");

    public static readonly OpCode SET_b_B     = new(0xC0, 0xCB, "SET {bit},B");
    public static readonly OpCode SET_b_C     = new(0xC1, 0xCB, "SET {bit},C");
    public static readonly OpCode SET_b_D     = new(0xC2, 0xCB, "SET {bit},D");
    public static readonly OpCode SET_b_E     = new(0xC3, 0xCB, "SET {bit},E");
    public static readonly OpCode SET_b_H     = new(0xC4, 0xCB, "SET {bit},H");
    public static readonly OpCode SET_b_L     = new(0xC5, 0xCB, "SET {bit},L");
    public static readonly OpCode SET_b_HL    = new(0xC6, 0xCB, "SET {bit},(HL)");
    public static readonly OpCode SET_b_A     = new(0xC7, 0xCB, "SET {bit},A");

}