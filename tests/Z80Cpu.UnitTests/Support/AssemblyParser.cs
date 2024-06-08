using OldBit.Z80Cpu.Extensions;

namespace OldBit.Z80Cpu.UnitTests.Support;

#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).

/// <summary>
/// This is a very simple assembly parser for compiling test code. It only
/// provides basic functionality needed, nothing else.
/// </summary>
internal class AssemblyParser
{
    public List<byte> Parse(params string[] lines)
    {
        var code = new List<byte>();
        foreach (var line in lines)
        {
            code.AddRange(ParseLine(line.Trim()).Select(x => (byte)x));
        }

        return code;
    }

    private static IEnumerable<int> ParseLine(string code)
    {
        var instruction = new Instruction(code);

        if (instruction.Mnemonic.Equals("DB"))
        {
            var result = new List<int>();
            foreach (var number in instruction.Operands)
            {
                if (Operand.TryParseNumber(number, out var value))
                {
                    result.Add(value);
                }
            }

            return result;
        }

        var opCodes = GetSimpleOpCode(instruction.Mnemonic);

        if (opCodes != null)
        {
            return opCodes;
        }

        opCodes = GetComplexOpCodes(instruction);
        if (opCodes != null)
        {
            return opCodes;
        }

        throw new ArgumentException($"Instruction not supported: {code}");
    }

    private static IEnumerable<int>? GetSimpleOpCode(string mnemonic)
    {
        return mnemonic switch
        {
            "NOP" => [0x00],
            "RLCA" => [0x07],
            "RRCA" => [0x0F],
            "RLA" => [0x17],
            "RRA" => [0x1F],
            "DAA" => [0x27],
            "CPL" => [0x2F],
            "SCF" => [0x37],
            "CCF" => [0x3F],
            "HALT" => [0x76],
            "DI" => [0xF3],
            "EI" => [0xFB],
            "EXX" => [0xD9],
            "NEG" => [0xED, 0x44],
            "RETI" => [0xED, 0x4D],
            "RETN" => [0xED, 0x45],
            "RLD" => [0xED, 0x6F],
            "RRD" => [0xED, 0x67],
            "LDI" => [0xED, 0xA0],
            "CPI" => [0xED, 0xA1],
            "INI" => [0xED, 0xA2],
            "OUTI" => [0xED, 0xA3],
            "LDD" => [0xED, 0xA8],
            "CPD" => [0xED, 0xA9],
            "IND" => [0xED, 0xAA],
            "OUTD" => [0xED, 0xAB],
            "LDIR" => [0xED, 0xB0],
            "CPIR" => [0xED, 0xB1],
            "INIR" => [0xED, 0xB2],
            "OTIR" => [0xED, 0xB3],
            "LDDR" => [0xED, 0xB8],
            "CPDR" => [0xED, 0xB9],
            "INDR" => [0xED, 0xBA],
            "OTDR" => [0xED, 0xBB],
            _ => null,
        };
    }

    private static IEnumerable<int>? GetComplexOpCodes(Instruction instruction)
    {
        Operand operand1;
        Operand operand2;
        byte hi;
        byte lo;
        int opCode;

        switch (instruction.Mnemonic)
        {
            case "ADD":
                operand1 = Operand.Parse(instruction.Operands[0]);
                operand2 = Operand.Parse(instruction.Operands[1]);

                if (operand1.IsHLorIXorIYRegister && operand2.Is16BitRegister)
                {
                    return CodeWithOptionalPrefix(operand1.CodePrefix, 0b00001001 | RegisterCodeMap[operand2.OperandType] << 4);
                }

                if (operand1.OperandType != OperandType.RegisterA)
                {
                    break;
                }

                if (operand2.Is8BitRegister)
                {
                    return CodeWithOptionalPrefix(operand1.CodePrefix ?? operand2.CodePrefix, 0b10000000 | RegisterCodeMap[operand2.OperandType]);
                }

                switch (operand2.OperandType)
                {
                    case OperandType.Value:
                        return [0xC6, operand2.Value];

                    case OperandType.AddressHL:
                        return [0x86];

                    case OperandType.AddressIXd:
                    case OperandType.AddressIYd:
                        return [operand2.CodePrefix!.Value, 0x86, operand2.Offset];
                }

                break;

            case "ADC":
                operand1 = Operand.Parse(instruction.Operands[0]);
                operand2 = Operand.Parse(instruction.Operands[1]);

                if (operand1.IsHLorIXorIYRegister && operand2.Is16BitRegister)
                {
                    return CodeWithOptionalPrefix(operand1.CodePrefix, 0xED, 0b01001010 | RegisterCodeMap[operand2.OperandType] << 4);
                }

                if (operand1.OperandType != OperandType.RegisterA)
                {
                    break;
                }

                if (operand2.Is8BitRegister)
                {
                    return CodeWithOptionalPrefix(operand1.CodePrefix ?? operand2.CodePrefix, 0b10001000 | RegisterCodeMap[operand2.OperandType]);
                }

                switch (operand2.OperandType)
                {
                    case OperandType.Value:
                        return [0xCE, operand2.Value];

                    case OperandType.AddressHL:
                        return [0x8E];

                    case OperandType.AddressIXd:
                    case OperandType.AddressIYd:
                        return [operand2.CodePrefix!.Value, 0x8E, operand2.Offset];
                }

                break;

            case "SUB":
            case "CP":
                operand1 = Operand.Parse(instruction.Operands[0]);

                if (operand1.Is8BitRegister)
                {
                    opCode = instruction.Mnemonic switch
                    {
                        "SUB" => 0b10010000,
                        "CP" => 0b10111000
                    };
                    return CodeWithOptionalPrefix(operand1.CodePrefix, opCode | RegisterCodeMap[operand1.OperandType]);
                }

                switch (operand1.OperandType)
                {
                    case OperandType.Value:
                        opCode = instruction.Mnemonic switch
                        {
                            "SUB" => 0xD6,
                            "CP" => 0xFE
                        };
                        return [opCode, operand1.Value];

                    case OperandType.AddressHL:
                        opCode = instruction.Mnemonic switch
                        {
                            "SUB" => 0x96,
                            "CP" => 0xBE
                        };
                        return [opCode];

                    case OperandType.AddressIXd:
                    case OperandType.AddressIYd:
                        opCode = instruction.Mnemonic switch
                        {
                            "SUB" => 0x96,
                            "CP" => 0xBE
                        };
                        return [operand1.CodePrefix!.Value, opCode, operand1.Offset];
                }

                break;

            case "SBC":
                operand1 = Operand.Parse(instruction.Operands[0]);
                operand2 = Operand.Parse(instruction.Operands[1]);

                if (operand1.OperandType == OperandType.RegisterHL)
                {
                    return [0xED, 0b01000010 | RegisterCodeMap[operand2.OperandType] << 4];
                }

                if (operand2.Is8BitRegister)
                {
                    return CodeWithOptionalPrefix(operand2.CodePrefix, 0b10011000 | RegisterCodeMap[operand2.OperandType]);
                }

                switch (operand2.OperandType)
                {
                    case OperandType.Value:
                        return [0xDE, operand2.Value];

                    case OperandType.AddressHL:
                        return [0x9E];

                    case OperandType.AddressIXd:
                    case OperandType.AddressIYd:
                        return [operand2.CodePrefix!.Value, 0x9E, operand2.Offset];
                }

                break;

            case "INC":
            case "DEC":
                operand1 = Operand.Parse(instruction.Operands[0]);

                if (operand1.Is8BitRegister)
                {
                    opCode = instruction.Mnemonic switch
                    {
                        "INC" => 0b00000100,
                        "DEC" => 0b00000101
                    };
                    return CodeWithOptionalPrefix(operand1.CodePrefix, opCode | RegisterCodeMap[operand1.OperandType] << 3);
                }

                if (operand1.Is16BitRegister)
                {
                    opCode = instruction.Mnemonic switch
                    {
                        "INC" => 0b00000011,
                        "DEC" => 0b00001011
                    };
                    return CodeWithOptionalPrefix(operand1.CodePrefix, opCode | RegisterCodeMap[operand1.OperandType] << 4);
                }

                switch (operand1.OperandType)
                {
                    case OperandType.AddressHL:
                        opCode = instruction.Mnemonic switch
                        {
                            "INC" => 0x34,
                            "DEC" => 0x35
                        };
                        return [opCode];

                    case OperandType.AddressIXd:
                    case OperandType.AddressIYd:
                        opCode = instruction.Mnemonic switch
                        {
                            "INC" => 0x34,
                            "DEC" => 0x35
                        };
                        return [operand1.CodePrefix!.Value, opCode, operand1.Offset];
                }

                break;

            case "RLC":
            case "RL":
            case "RRC":
            case "RR":
            case "SLA":
            case "SRA":
            case "SRL":
            case "SLL":
                operand1 = Operand.Parse(instruction.Operands[0]);

                if (operand1.Is8BitRegister)
                {
                    opCode = instruction.Mnemonic switch
                    {
                        "RLC" => 0b00000000,
                        "RL" => 0b00010000,
                        "RRC" => 0b00001000,
                        "RR" => 0b00011000,
                        "SLA" => 0b00100000,
                        "SRA" => 0b00101000,
                        "SRL" => 0b00111000,
                        "SLL" => 0b00110000
                    };
                    return CodeWithOptionalPrefix(operand1.CodePrefix, 0xCB, opCode | RegisterCodeMap[operand1.OperandType]);
                }

                opCode = instruction.Mnemonic switch
                {
                    "RLC" => 0x06,
                    "RL" => 0x16,
                    "RRC" => 0x0E,
                    "RR" => 0x1E,
                    "SLA" => 0x26,
                    "SRA" => 0x2E,
                    "SRL" => 0x3E,
                    "SLL" => 0x36
                };

                switch (operand1.OperandType)
                {
                    case OperandType.AddressHL:
                        return [0xCB, opCode];

                    case OperandType.AddressIXd:
                    case OperandType.AddressIYd:
                        return [operand1.CodePrefix!.Value, 0xCB, operand1.Offset, opCode];
                }

                break;

            case "AND":
            case "OR":
            case "XOR":
                operand1 = Operand.Parse(instruction.Operands[0]);

                if (operand1.Is8BitRegister)
                {
                    opCode = instruction.Mnemonic switch
                    {
                        "AND" => 0b10100000,
                        "OR" => 0b10110000,
                        "XOR" => 0b10101000
                    };
                    return CodeWithOptionalPrefix(operand1.CodePrefix, opCode | RegisterCodeMap[operand1.OperandType]);
                }

                switch (operand1.OperandType)
                {
                    case OperandType.Value:
                        opCode = instruction.Mnemonic switch
                        {
                            "AND" => 0xE6,
                            "OR" => 0xF6,
                            "XOR" => 0xEE
                        };
                        return [opCode, operand1.Value];

                    case OperandType.AddressHL:
                        opCode = instruction.Mnemonic switch
                        {
                            "AND" => 0xA6,
                            "OR" => 0xB6,
                            "XOR" => 0xAE
                        };
                        return [opCode];

                    case OperandType.AddressIXd:
                    case OperandType.AddressIYd:
                        opCode = instruction.Mnemonic switch
                        {
                            "AND" => 0xA6,
                            "OR" => 0xB6,
                            "XOR" => 0xAE
                        };
                        return [operand1.CodePrefix!.Value, opCode, operand1.Offset];
                }

                break;

            case "CALL":
                if (instruction.Operands.Length > 1)
                {
                    operand1 = Operand.Parse(instruction.Operands[0], instruction.Operands[1]);
                    (hi, lo) = (Word)operand1.Value;

                    return [0b11000100 | ConditionCodeMap[operand1.OperandType] << 3, lo, hi];
                }

                operand1 = Operand.Parse(instruction.Operands[0]);
                (hi, lo) = (Word)operand1.Value;

                return [0xCD, lo, hi];

            case "JP":
                if (instruction.Operands.Length > 1)
                {
                    operand1 = Operand.Parse(instruction.Operands[0], instruction.Operands[1]);
                    (hi, lo) = (Word)operand1.Value;

                    return [0b11000010 | ConditionCodeMap[operand1.OperandType] << 3, lo, hi];
                }

                operand1 = Operand.Parse(instruction.Operands[0]);
                switch (operand1.OperandType)
                {
                    case OperandType.AddressHL:
                        return [0xE9];

                    case OperandType.AddressIXd:
                    case OperandType.AddressIYd:
                        return [operand1.CodePrefix!.Value, 0xE9];
                }

                (hi, lo) = (Word)operand1.Value;
                return [0xC3, lo, hi];

            case "JR":
                if (instruction.Operands.Length > 1)
                {
                    operand1 = Operand.Parse(instruction.Operands[0], instruction.Operands[1]);
                    switch (operand1.OperandType)
                    {
                        case OperandType.ConditionC:
                            return [0x38, operand1.Value];

                        case OperandType.ConditionNC:
                            return [0x30, operand1.Value];

                        case OperandType.ConditionZ:
                            return [0x28, operand1.Value];

                        case OperandType.ConditionNZ:
                            return [0x20, operand1.Value];
                    }
                    break;
                }

                operand1 = Operand.Parse(instruction.Operands[0]);
                return [0x18, operand1.Value];

            case "DJNZ":
                operand1 = Operand.Parse(instruction.Operands[0]);
                return [0x10, operand1.Value];

            case "EX":
                operand1 = Operand.Parse(instruction.Operands[0]);
                operand2 = Operand.Parse(instruction.Operands[1]);

                switch (operand1.OperandType)
                {
                    case OperandType.RegisterDE when operand2.OperandType == OperandType.RegisterHL:
                        return [0xEB];

                    case OperandType.RegisterAF when operand2.OperandType == OperandType.RegisterAF:
                        return [0x08];

                    case OperandType.RegisterSP when operand2.IsHLorIXorIYRegister:
                        return CodeWithOptionalPrefix(operand2.CodePrefix, 0xE3);
                }

                break;

            case "IM":
            {
                var mode = instruction.Operands[0] switch
                {
                    "0" => 0x46,
                    "1" => 0x56,
                    "2" => 0x5E,
                    _ => throw new ArgumentException($"Invalid IM mode: {instruction.Operands[0]}")
                };

                return new[] { 0xED, mode };
            }

            case "IN":
                operand1 = Operand.Parse(instruction.Operands[0]);
                operand2 = Operand.Parse(instruction.Operands[1]);

                if (operand1.OperandType == OperandType.RegisterA && operand2.OperandType == OperandType.Value)
                {
                    return [0xDB, operand2.Value];
                }

                if (operand1.Is8BitRegister && operand2.OperandType == OperandType.RegisterC)
                {
                    return [0xED, 0b01000000 | RegisterCodeMap[operand1.OperandType] << 3];
                }

                break;

            case "OUT":
                operand1 = Operand.Parse(instruction.Operands[0]);
                operand2 = Operand.Parse(instruction.Operands[1]);

                if (operand1.OperandType == OperandType.Address && operand2.OperandType == OperandType.RegisterA)
                {
                    return [0xD3, operand1.Value];
                }

                if (operand1.OperandType == OperandType.RegisterC && operand2.Is8BitRegister)
                {
                    return [0xED, 0b01000001 | RegisterCodeMap[operand2.OperandType] << 3];
                }

                break;

            case "LD":
            {
                operand1 = Operand.Parse(instruction.Operands[0]);
                operand2 = Operand.Parse(instruction.Operands[1]);

                switch (operand1.Is8BitRegister)
                {
                    // LD r,r
                    case true when operand2.Is8BitRegister:
                        return operand1.OperandType switch
                        {
                            OperandType.RegisterA when operand2.OperandType == OperandType.RegisterI => [0xED, 0x57],
                            OperandType.RegisterI when operand2.OperandType == OperandType.RegisterA => [0xED, 0x47],
                            OperandType.RegisterA when operand2.OperandType == OperandType.RegisterR => [0xED, 0x5F],
                            OperandType.RegisterR when operand2.OperandType == OperandType.RegisterA => [0xED, 0x4F],
                            _ => CodeWithOptionalPrefix(operand1.CodePrefix ?? operand2.CodePrefix,
                                0b01000000 | RegisterCodeMap[operand1.OperandType] << 3 | RegisterCodeMap[operand2.OperandType])
                        };

                    // LD r,n
                    case true when operand2.OperandType == OperandType.Value:
                        return CodeWithOptionalPrefix(operand1.CodePrefix, 0b00000110 | RegisterCodeMap[operand1.OperandType] << 3, operand2.Value);

                    // LD r,(HL)
                    case true when operand2.OperandType == OperandType.AddressHL:
                        return [0b01000110 | RegisterCodeMap[operand1.OperandType] << 3];

                    // LD r,(IX+d) / LD r,(IY+d)
                    case true when operand2.OperandType is OperandType.AddressIXd or OperandType.AddressIYd:
                        return [operand2.CodePrefix!.Value, 0b01000110 | RegisterCodeMap[operand1.OperandType] << 3, operand2.Offset];

                    // LD A,(BC)
                    case true when operand2.OperandType == OperandType.AddressBC:
                        return [0x0A];

                    // LD A,(DE)
                    case true when operand2.OperandType == OperandType.AddressDE:
                        return [0x1A];

                    // LD A,(nn)
                    case true when operand2.OperandType == OperandType.Address:
                        (hi, lo) = (Word)operand2.Value;
                        return [0x3A, lo, hi];

                    // LD A,I
                    case true when operand2.OperandType == OperandType.RegisterI:
                        return [0xED, 0x57];

                    // LD A,R
                    case true when operand2.OperandType == OperandType.RegisterR:
                        return [0xED, 0x5F];
                }

                // LD rr,nn
                if (operand1.Is16BitRegister && operand2.OperandType == OperandType.Value)
                {
                    (hi, lo) = (Word)operand2.Value;
                    return CodeWithOptionalPrefix(operand1.CodePrefix, 0b00000001 | RegisterCodeMap[operand1.OperandType] << 4, lo, hi);
                }

                // LD HL,(nn) / LD IX,(nn) / LD IY,(nn)
                if (operand1.IsHLorIXorIYRegister && operand2.OperandType == OperandType.Address)
                {
                    (hi, lo) = (Word)operand2.Value;
                    return CodeWithOptionalPrefix(operand1.CodePrefix, 0x2A | RegisterCodeMap[operand1.OperandType], lo, hi);
                }

                // LD rr,(nn)
                if (operand1.Is16BitRegister && operand2.OperandType == OperandType.Address)
                {
                    (hi, lo) = (Word)operand2.Value;
                    return [0xED, 0b01001011 | RegisterCodeMap[operand1.OperandType] << 4, lo, hi];
                }

                if (operand1.OperandType == OperandType.AddressHL)
                {
                    // LD (HL),r
                    if (operand2.Is8BitRegister)
                    {
                        return [0b01110000 | RegisterCodeMap[operand2.OperandType]];
                    }

                    // LD (HL),n
                    if (operand2.OperandType == OperandType.Value)
                    {
                        return [0x36, operand2.Value];
                    }
                }

                if (operand1.OperandType is OperandType.AddressIXd or OperandType.AddressIYd)
                {
                    // LD (IX+d),r / LD (IY+d),r
                    if (operand2.Is8BitRegister)
                    {
                        return [operand1.CodePrefix!.Value, 0b01110000 | RegisterCodeMap[operand2.OperandType], operand2.Offset];
                    }

                    // LD (IX+d),n / LD (IY+d),n
                    if (operand2.OperandType == OperandType.Value)
                    {
                        return [operand1.CodePrefix!.Value, 0x36, operand1.Offset, operand2.Value];
                    }
                }

                // LD SP,HL / LD SP,IX / LD SP,IY
                if (operand1.OperandType == OperandType.RegisterSP && operand2.IsHLorIXorIYRegister)
                {
                    return CodeWithOptionalPrefix(operand2.CodePrefix, 0xF9);
                }

                if (operand1.OperandType == OperandType.Address)
                {
                    // LD (nn),HL / LD (nn),IX / LD (nn),IY
                    if (operand2.IsHLorIXorIYRegister)
                    {
                        (hi, lo) = (Word)operand1.Value;
                        return CodeWithOptionalPrefix(operand2.CodePrefix, 0x22, lo, hi);
                    }

                    // LD (nn),rr
                    if (operand2.Is16BitRegister)
                    {
                        (hi, lo) = (Word)operand1.Value;
                        return [0xED, 0b01000011 | RegisterCodeMap[operand2.OperandType] << 4, lo, hi];
                    }
                }

                // LD A,(BC)
                if (operand1.OperandType == OperandType.AddressBC && operand2.OperandType == OperandType.RegisterA)
                {
                    return [0x02];
                }

                // LD A,(DE)
                if (operand1.OperandType == OperandType.AddressDE && operand2.OperandType == OperandType.RegisterA)
                {
                    return [0x12];
                }

                // LD A,(nn)
                if (operand1.OperandType == OperandType.Address && operand2.OperandType == OperandType.RegisterA)
                {
                    (hi, lo) = (Word)operand1.Value;
                    return [0x32, lo, hi];
                }

                break;
            }

            case "PUSH":
            case "POP":
                operand1 = Operand.Parse(instruction.Operands[0]);
                opCode = (instruction.Mnemonic == "PUSH" ? 0b11000101 : 0b11000001) | RegisterCodeMap[operand1.OperandType] << 4;
                return CodeWithOptionalPrefix(operand1.CodePrefix, opCode);

            case "RET":
                if (instruction.Operands[0].Length > 0)
                {
                    operand1 = Operand.Parse(instruction.Operands[0], string.Empty);
                    return [0b11000000 | ConditionCodeMap[operand1.OperandType] << 3];
                }

                return [0xC9];

            case "RST":
                if (Operand.TryParseNumber(instruction.Operands[0], out var rst))
                {
                    return [0b11000111 | RstCodeMap[rst] << 3];
                }
                break;

            case "BIT":
            case "SET":
            case "RES":
                operand1 = Operand.Parse(instruction.Operands[0]);
                operand2 = Operand.Parse(instruction.Operands[1]);

                var prefix = instruction.Mnemonic switch
                {
                    "BIT" => 0b01000000,
                    "SET" => 0b11000000,
                    "RES" => 0b10000000
                };

                switch (operand2.OperandType)
                {
                    case OperandType.AddressHL:
                        return [0xCB, prefix | 0b00000110 | operand1.Value << 3];

                    case OperandType.AddressIXd:
                    case OperandType.AddressIYd:
                        return [operand2.CodePrefix!.Value, 0xCB, operand2.Offset, prefix | 0b00000110 | operand1.Value << 3];
                }

                return [0xCB, prefix | operand1.Value << 3 | RegisterCodeMap[operand2.OperandType]];
        }

        return null;
    }

    private static readonly Dictionary<OperandType, int> RegisterCodeMap = new()
    {
        { OperandType.RegisterA, 0b111 },
        { OperandType.RegisterB, 0b000 },
        { OperandType.RegisterC, 0b001 },
        { OperandType.RegisterD, 0b010 },
        { OperandType.RegisterE, 0b011 },
        { OperandType.RegisterH, 0b100 },
        { OperandType.RegisterL, 0b101 },
        { OperandType.RegisterF, 0b110 },
        { OperandType.RegisterIXH, 0b100 },
        { OperandType.RegisterIXL, 0b101 },
        { OperandType.RegisterIYH, 0b100 },
        { OperandType.RegisterIYL, 0b101 },
        { OperandType.RegisterAF, 0b11 },
        { OperandType.RegisterBC, 0b00 },
        { OperandType.RegisterDE, 0b01 },
        { OperandType.RegisterHL, 0b10 },
        { OperandType.RegisterSP, 0b11 },
        { OperandType.RegisterIX, 0b10 },
        { OperandType.RegisterIY, 0b10 }
    };

    private static readonly Dictionary<OperandType, int> ConditionCodeMap = new()
    {
        { OperandType.ConditionNZ, 0b000 },
        { OperandType.ConditionZ, 0b001 },
        { OperandType.ConditionNC, 0b010 },
        { OperandType.ConditionC, 0b011 },
        { OperandType.ConditionPO, 0b100 },
        { OperandType.ConditionPE, 0b101 },
        { OperandType.ConditionP, 0b110 },
        { OperandType.ConditionM, 0b111 }
    };

    private static readonly Dictionary<int, int> RstCodeMap = new()
    {
        { 0x00, 0b000 },
        { 0x08, 0b001 },
        { 0x10, 0b010 },
        { 0x18, 0b011 },
        { 0x20, 0b100 },
        { 0x28, 0b101 },
        { 0x30, 0b110 },
        { 0x38, 0b111 },
    };

    private static IEnumerable<int> CodeWithOptionalPrefix(byte? prefix, params int[] values)
    {
        if (prefix != null)
        {
            yield return prefix.Value;
        }
        foreach (var value in values)
        {
            yield return value;
        }
    }
}
