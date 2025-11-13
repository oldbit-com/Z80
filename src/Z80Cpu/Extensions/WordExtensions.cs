namespace OldBit.Z80Cpu.Extensions;

/// <summary>
/// Extension methods for the Word type.
/// </summary>
public static class WordExtensions
{
    /// <summary>
    /// Deconstructs the Word value into its high and low bytes.
    /// </summary>
    /// <param name="value">The Word value to deconstruct.</param>
    /// <param name="highByte">The high byte of the Word value.</param>
    /// <param name="lowByte">The low byte of the Word value.</param>
    public static void Deconstruct(this Word value, out byte highByte, out byte lowByte)
    {
        highByte = (byte)(value >> 8);
        lowByte = (byte)(value & 0xFF);
    }
}