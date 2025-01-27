namespace OldBit.Z80Cpu;

/// <summary>
/// Represents a memory device that can be read from and written to.
/// </summary>
public interface IMemory
{
    /// <summary>
    /// Reads a byte from the specified memory address.
    /// </summary>
    /// <param name="address">The memory address to read from.</param>
    /// <returns>The byte read from the specified memory address.</returns>
    byte Read(Word address);


    /// <summary>
    /// Writes a byte to the specified memory address.
    /// </summary>
    /// <param name="address">The memory address to write to.</param>
    /// <param name="data">The byte to write to the specified memory address.</param>
    void Write(Word address, byte data);
}