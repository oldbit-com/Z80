namespace OldBit.Z80Cpu.UnitTests.Extensions;

public static class ShouldlyExtensions
{
    public static void ShouldBe(this Word actual, int expected, string? message = null) =>
        actual.ShouldBe<Word>((Word)expected, message);

    public static void ShouldBe(this byte actual, int expected, string? message = null) =>
        actual.ShouldBe<byte>((byte)expected, message);
}