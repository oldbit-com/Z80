using System.Text.RegularExpressions;

namespace Z80.Net.UnitTests.Support;

internal class Operand
{
    public static Operand Parse(string s)
    {
        var operand = new Operand();
        operand.ParsePrivate(s);
        return operand;
    }

    public static Operand Parse(string condition, string value)
    {
        var operand = new Operand();
        operand.ParsePrivate(condition, value);
        return operand;
    }

    private void ParsePrivate(string s)
    {
        OperandType = s.ToUpperInvariant() switch
        {
            "A" => OperandType.RegisterA,
            "B" => OperandType.RegisterB,
            "C" => OperandType.RegisterC,
            "D" => OperandType.RegisterD,
            "E" => OperandType.RegisterE,
            "H" => OperandType.RegisterH,
            "L" => OperandType.RegisterL,
            "IXH" => OperandType.RegisterIXH,
            "IXL" => OperandType.RegisterIXL,
            "IYH" => OperandType.RegisterIYH,
            "IYL" => OperandType.RegisterIYL,

            "AF" => OperandType.RegisterAF,
            "BC" => OperandType.RegisterBC,
            "DE" => OperandType.RegisterDE,
            "HL" => OperandType.RegisterHL,
            "SP" => OperandType.RegisterSP,
            "IX" => OperandType.RegisterIX,
            "IY" => OperandType.RegisterIY,

            _ => OperandType.Unknown
        };

        if (OperandType != OperandType.Unknown)
        {
            return;
        }

        if (TryParseNumber(s, out var value))
        {
            OperandType = OperandType.Value;
            Value = value;
            return;
        }

        if (s.StartsWith("(") && s.EndsWith(")") && TryParseNumber(s[1..^1], out value))
        {
            OperandType = OperandType.Memory;
            Value = value;
            return;
        }

        if (s.Equals("(HL)", StringComparison.OrdinalIgnoreCase))
        {
            OperandType = OperandType.MemoryHL;
            return;
        }

        if (s.StartsWith("(IX", StringComparison.OrdinalIgnoreCase))
        {
            OperandType = OperandType.MemoryIXd;
            Offset = ParseIndexRegisterOffset(s);
            return;
        }

        if (s.StartsWith("(IY", StringComparison.OrdinalIgnoreCase))
        {
            OperandType = OperandType.MemoryIYd;
            Offset = ParseIndexRegisterOffset(s);
            return;
        }
    }

    private void ParsePrivate(string condition, string value)
    {
        OperandType = condition.ToUpperInvariant() switch
        {
            "C" => OperandType.ConditionC,
            "NC" => OperandType.ConditionNC,
            "Z" => OperandType.ConditionZ,
            "NZ" => OperandType.ConditionNZ,
            "M" => OperandType.ConditionM,
            "P" => OperandType.ConditionP,
            "PE" => OperandType.ConditionPE,
            "PO" => OperandType.ConditionPO,
            _ => OperandType.Unknown
        };

        var address = Parse(value);
        Value = address.Value;
    }

    internal static bool TryParseNumber(string s, out int value)
    {
        if (s.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        {
            value = Convert.ToUInt16(s, 16);
            return true;
        }

        if (s.EndsWith("h", StringComparison.OrdinalIgnoreCase))
        {
            value = Convert.ToUInt16(s[..^1], 16);
            return true;
        }

        if (int.TryParse(s, out var parsedValue))
        {
            value = parsedValue;
            return true;
        }

        value = 0;
        return false;
    }

    private static int ParseIndexRegisterOffset(string regAndOffset)
    {
        var regex = new Regex(@"\(I[X|Y]([+-].*?)\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        var match = regex.Match(regAndOffset);

        if (match.Success)
        {
            var sign = match.Groups[1].Value[0];
            if (TryParseNumber(match.Groups[1].Value[1..], out var value))
            {
                return sign switch
                {
                    '-' => -value,
                    _ => value,
                };
            }
        }

        return 0;
    }

    public bool Is8BitRegister => OperandType is >= OperandType.RegisterA and <= OperandType.RegisterIYL;

    public bool Is16BitRegister => OperandType is >= OperandType.RegisterAF and <= OperandType.RegisterIY;

    public bool IsHLorIXorIYRegister => OperandType is OperandType.RegisterHL or OperandType.RegisterIX or OperandType.RegisterIY;

    public byte? CodePrefix => OperandType switch
    {
        OperandType.RegisterIXH
            or OperandType.RegisterIXL
            or OperandType.MemoryIXd
            or OperandType.RegisterIX => 0xDD,
        OperandType.RegisterIYH
            or OperandType.RegisterIYL
            or OperandType.MemoryIYd
            or OperandType.RegisterIY => 0xFD,
        _ => null
    };

    public OperandType OperandType { get; private set; } = OperandType.Unknown;

    public int Value { get; private set; }

    public int Offset { get; private set; }
}