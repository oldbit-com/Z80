using FluentAssertions;
using FluentAssertions.Numeric;

namespace Z80.Net.UnitTests.Extensions;

public static class FluentAssertionsExtensions
{
    public static AndConstraint<NumericAssertions<byte>> Be(this NumericAssertions<byte> parent, int expected, string because = "",
        params object[] becauseArg)
    {
        parent.Subject.Should().Be((byte)expected);
        return new AndConstraint<NumericAssertions<byte>>(parent);
    }

    public static AndConstraint<NumericAssertions<ushort>> Be(this NumericAssertions<ushort> parent, int expected, string because = "",
        params object[] becauseArg)
    {
        parent.Subject.Should().Be((ushort)expected);
        return new AndConstraint<NumericAssertions<ushort>>(parent);
    }
}