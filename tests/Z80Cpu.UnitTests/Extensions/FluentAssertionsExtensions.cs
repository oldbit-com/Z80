using FluentAssertions.Numeric;

namespace OldBit.Z80Cpu.UnitTests.Extensions;

public static class FluentAssertionsExtensions
{
    public static AndConstraint<NumericAssertions<byte>> Be(this NumericAssertions<byte> parent, int expected, string because = "",
        params object[] becauseArg)
    {
        parent.Subject.Should().Be((byte)expected);
        return new AndConstraint<NumericAssertions<byte>>(parent);
    }

    public static AndConstraint<NumericAssertions<Word>> Be(this NumericAssertions<Word> parent, int expected, string because = "",
        params object[] becauseArg)
    {
        parent.Subject.Should().Be((Word)expected);
        return new AndConstraint<NumericAssertions<Word>>(parent);
    }
}