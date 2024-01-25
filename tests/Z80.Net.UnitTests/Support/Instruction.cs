using System.Collections.Immutable;

namespace Z80.Net.UnitTests.Support;

internal class Instruction
{
    public Instruction(string code)
    {
        var (mnemonic, operand) = GetMnemonic(code);
        Mnemonic = mnemonic.ToUpperInvariant();
        Operands = operand
            .Split(",")
            .Select(x => x.Trim())
            .ToImmutableArray();
    }

    private (string Mnemonic, string Operand) GetMnemonic(string code)
    {
        var spacePos = code.Trim().IndexOf(' ');
        if (spacePos == -1)
        {
            return (code, string.Empty);
        }

        return (code[0..spacePos], code[spacePos..].Trim());
    }

    public string Mnemonic { get; }

    public ImmutableArray<string> Operands { get; }
}