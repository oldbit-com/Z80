using System.Text.RegularExpressions;
using FluentAssertions;

namespace Z80.Net.UnitTests.Parser;

/// <summary>
/// This is very simple assembly parser for building test code. It only
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

    private IEnumerable<int> ParseLine(string code)
    {
        var instruction = new Instruction(code);

        List<int> result;

        // DB pseudo instruction (define byte)
        if (instruction.Mnemonic.Equals("DB"))
        {
            result = new List<int>();
            foreach (var number in instruction.Operands)
            {
                if (Operand.TryParseNumber(number, out var value))
                {
                    result.Add(value);
                }
            }

            return result;
        }

        // Single mnemonic instructions
        int[]? opCodes = instruction.Mnemonic switch
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
            "LDI" => [0xED, 0xA0],
            "LDD" => [0xED, 0xA8],
            "LDIR" => [0xED, 0xB0],
            "LDDR" => [0xED, 0xB8],
            _ => null,
        };

        if (opCodes != null)
        {
            return opCodes;
        }

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
                    return CodeWithOptionalPrefix(operand1.CodePrefix, 0b00001001 | RegisterCodes[operand2.OperandType] << 4);
                }

                if (operand1.OperandType != OperandType.RegisterA)
                {
                    break;
                }

                if (operand2.Is8BitRegister)
                {
                    return CodeWithOptionalPrefix(operand1.CodePrefix ?? operand2.CodePrefix, 0b10000000 | RegisterCodes[operand2.OperandType]);
                }

                switch (operand2.OperandType)
                {
                    case OperandType.Value:
                        return [0xC6, operand2.Value];

                    case OperandType.MemoryHL:
                        return [0x86];

                    case OperandType.MemoryIXd:
                    case OperandType.MemoryIYd:
                        return [operand2.CodePrefix!.Value, 0x86];
                }

                break;

            case "ADC":
                operand1 = Operand.Parse(instruction.Operands[0]);
                operand2 = Operand.Parse(instruction.Operands[1]);

                if (operand1.IsHLorIXorIYRegister && operand2.Is16BitRegister)
                {
                    return CodeWithOptionalPrefix(operand1.CodePrefix, 0xED, 0b01001010 | RegisterCodes[operand2.OperandType] << 4);
                }

                if (operand1.OperandType != OperandType.RegisterA)
                {
                    break;
                }

                if (operand2.Is8BitRegister)
                {
                    return CodeWithOptionalPrefix(operand1.CodePrefix ?? operand2.CodePrefix, 0b10001000 | RegisterCodes[operand2.OperandType]);
                }

                switch (operand2.OperandType)
                {
                    case OperandType.Value:
                        return [0xCE, operand2.Value];

                    case OperandType.MemoryHL:
                        return [0x8E];

                    case OperandType.MemoryIXd:
                    case OperandType.MemoryIYd:
                        return [operand2.CodePrefix!.Value, 0x8E];
                }

                break;

            case "SUB":
                operand1 = Operand.Parse(instruction.Operands[0]);

                if (operand1.Is8BitRegister)
                {
                    return CodeWithOptionalPrefix(operand1.CodePrefix, 0b10010000 | RegisterCodes[operand1.OperandType]);
                }

                switch (operand1.OperandType)
                {
                    case OperandType.Value:
                        return [0xD6, operand1.Value];

                    case OperandType.MemoryHL:
                        return [0x96];

                    case OperandType.MemoryIXd:
                    case OperandType.MemoryIYd:
                        return [operand1.CodePrefix!.Value, 0x96];
                }

                break;

            case "SBC":
                operand1 = Operand.Parse(instruction.Operands[0]);

                if (operand1.Is8BitRegister)
                {
                    return CodeWithOptionalPrefix(operand1.CodePrefix, 0b10011000 | RegisterCodes[operand1.OperandType]);
                }

                switch (operand1.OperandType)
                {
                    case OperandType.Value:
                        return [0xDE, operand1.Value];

                    case OperandType.MemoryHL:
                        return [0x9E];

                    case OperandType.MemoryIXd:
                    case OperandType.MemoryIYd:
                        return [operand1.CodePrefix!.Value, 0x9E];

                    case OperandType.RegisterHL:
                        operand2 = Operand.Parse(instruction.Operands[1]);
                        return [0xED, 0b01000010 | RegisterCodes[operand2.OperandType] << 4];
                }

                break;

            case "INC":
                operand1 = Operand.Parse(instruction.Operands[0]);

                if (operand1.Is8BitRegister)
                {
                    return CodeWithOptionalPrefix(operand1.CodePrefix, 0b00000100 | RegisterCodes[operand1.OperandType] << 3);
                }

                if (operand1.Is16BitRegister)
                {
                    return CodeWithOptionalPrefix(operand1.CodePrefix, 0b00000011 | RegisterCodes[operand1.OperandType] << 4);
                }

                switch (operand1.OperandType)
                {
                    case OperandType.MemoryHL:
                        return [0x34];

                    case OperandType.MemoryIXd:
                    case OperandType.MemoryIYd:
                        return [operand1.CodePrefix!.Value, 0x34];
                }

                break;

            case "DEC":
                operand1 = Operand.Parse(instruction.Operands[0]);

                if (operand1.Is16BitRegister)
                {
                    return CodeWithOptionalPrefix(operand1.CodePrefix, 0b00001011 | RegisterCodes[operand1.OperandType] << 4);
                }

                break;

            case "RLC":
                operand1 = Operand.Parse(instruction.Operands[0]);

                if (operand1.Is8BitRegister)
                {
                    return CodeWithOptionalPrefix(operand1.CodePrefix, 0xCB, RegisterCodes[operand1.OperandType]);
                }

                switch (operand1.OperandType)
                {
                    case OperandType.MemoryHL:
                        return [0xCB, 0x06];

                    case OperandType.MemoryIXd:
                    case OperandType.MemoryIYd:
                        return [operand1.CodePrefix!.Value, 0xCB, operand1.Offset, 0x06];
                }

                break;

            case "CALL":
                if (instruction.Operands.Length > 1)
                {
                    operand1 = Operand.Parse(instruction.Operands[0], instruction.Operands[1]);
                    (hi, lo) = Helpers.TypeConverter.ToBytes((ushort)operand1.Value);

                    return [0b11000100 | ConditionCodes[operand1.OperandType] << 3, lo, hi];
                }

                operand1 = Operand.Parse(instruction.Operands[0]);
                (hi, lo) = Helpers.TypeConverter.ToBytes((ushort)operand1.Value);

                return [0xCD, lo, hi];

            case "JP":
                if (instruction.Operands.Length > 1)
                {
                    operand1 = Operand.Parse(instruction.Operands[0], instruction.Operands[1]);
                    (hi, lo) = Helpers.TypeConverter.ToBytes((ushort)operand1.Value);

                    return [0b11000010 | ConditionCodes[operand1.OperandType] << 3, lo, hi];
                }

                operand1 = Operand.Parse(instruction.Operands[0]);
                switch (operand1.OperandType)
                {
                    case OperandType.MemoryHL:
                        return [0xE9];

                    case OperandType.MemoryIXd:
                    case OperandType.MemoryIYd:
                        return [operand1.CodePrefix!.Value, 0xE9];
                }

                (hi, lo) = Helpers.TypeConverter.ToBytes((ushort)operand1.Value);
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

            case "LD":
            {
                operand1 = Operand.Parse(instruction.Operands[0]);
                operand2 = Operand.Parse(instruction.Operands[1]);

                switch (operand1.Is8BitRegister)
                {
                    // LD r,r'
                    case true when operand2.Is8BitRegister:
                        return CodeWithOptionalPrefix(operand1.CodePrefix ?? operand2.CodePrefix, 0b01000000 | RegisterCodes[operand1.OperandType] << 3 | RegisterCodes[operand2.OperandType]);

                    // LD r,n
                    case true when operand2.OperandType == OperandType.Value:
                        return CodeWithOptionalPrefix(operand1.CodePrefix, 0b00000110 | RegisterCodes[operand1.OperandType] << 3, operand2.Value);

                    // LD r,(HL)
                    case true when operand2.OperandType == OperandType.MemoryHL:
                        return [0b01000110 | RegisterCodes[operand1.OperandType] << 3];

                    // LD r,(IX+d) / LD r,(IY+d)
                    case true when operand2.OperandType is OperandType.MemoryIXd or OperandType.MemoryIYd:
                        return [operand2.CodePrefix!.Value, 0b01000110 | RegisterCodes[operand1.OperandType] << 3, operand2.Offset];
                }

                // LD rr,nn
                if (operand1.Is16BitRegister && operand2.OperandType == OperandType.Value)
                {
                    (hi, lo) = Helpers.TypeConverter.ToBytes((ushort)operand2.Value);
                    return CodeWithOptionalPrefix(operand1.CodePrefix, 0b00000001 | RegisterCodes[operand1.OperandType] << 4, lo, hi);
                }

                // LD HL,(nn) / LD IX,(nn) / LD IY,(nn)
                if (operand1.IsHLorIXorIYRegister && operand2.OperandType == OperandType.Memory)
                {
                    (hi, lo) = Helpers.TypeConverter.ToBytes((ushort)operand2.Value);
                    return CodeWithOptionalPrefix(operand1.CodePrefix, 0x2A | RegisterCodes[operand1.OperandType], lo, hi);
                }

                // LD rr,(nn)
                if (operand1.Is16BitRegister && operand2.OperandType == OperandType.Memory)
                {
                    (hi, lo) = Helpers.TypeConverter.ToBytes((ushort)operand2.Value);
                    return [0xED, 0b01001011 | RegisterCodes[operand1.OperandType] << 4, lo, hi];
                }

                break;
            }

            case "PUSH":
            case "POP":
                operand1 = Operand.Parse(instruction.Operands[0]);
                opCode = (instruction.Mnemonic == "PUSH" ? 0b11000101 : 0b11000001) | RegisterCodes[operand1.OperandType] << 4;
                return CodeWithOptionalPrefix(operand1.CodePrefix, opCode);

            case "RET":
                if (instruction.Operands[0].Length > 0)
                {
                    operand1 = Operand.Parse(instruction.Operands[0], string.Empty);
                    return [0b11000000 | ConditionCodes[operand1.OperandType] << 3];
                }

                return [0xC9];

            case "RST":
                if (Operand.TryParseNumber(instruction.Operands[0], out var rst))
                {
                    return [0b11000111 | RstCodes[rst] << 3];
                }
                break;
        }

        throw new ArgumentException($"Instruction not supported: {code}");
    }

    private static readonly Dictionary<OperandType, int> RegisterCodes = new()
    {
        { OperandType.RegisterA, 0b111 },
        { OperandType.RegisterB, 0b000 },
        { OperandType.RegisterC, 0b001 },
        { OperandType.RegisterD, 0b010 },
        { OperandType.RegisterE, 0b011 },
        { OperandType.RegisterH, 0b100 },
        { OperandType.RegisterL, 0b101 },
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

    private static readonly Dictionary<OperandType, int> ConditionCodes = new()
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

    private static readonly Dictionary<int, int> RstCodes = new()
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