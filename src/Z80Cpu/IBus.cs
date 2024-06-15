namespace OldBit.Z80Cpu;

/// <summary>
/// Represents the I/O bus. It is accessed using IN and OUT instructions.
/// </summary>
public interface IBus
{
    /// <summary>
    /// Reads an 8-bit value from the I/O device.
    /// </summary>
    /// <param name="port">The address of the input port.</param>
    /// <returns>A value read from the I/O device.</returns>
    byte Read(Word port);

    /// <summary>
    /// Writes an 8-bit value to the I/O devices.
    /// </summary>
    /// <param name="port">The address of the output port.</param>
    /// <param name="data">The data to be written to the I/O device.</param>
    void Write(Word port, byte data);
}