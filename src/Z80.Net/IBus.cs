namespace Z80.Net;

/// <summary>
/// Represents the I/O bus. It is accessed using IN and OUT instructions.
/// </summary>
public interface IBus
{
    /// <summary>
    /// Reads an 8-bit value from the I/O device.
    /// </summary>
    /// <param name="top">The top half of the address (A8-A15).</param>
    /// <param name="bottom">The bottom half of the address (A0-A7).</param>
    /// <returns>A value read from the I/O device.</returns>
    byte Read(byte top, byte bottom);

    /// <summary>
    /// Writes an 8-bit value to the I/O devices.
    /// </summary>
    /// <param name="top">The top half of the address (A8-A15).</param>
    /// <param name="bottom">The bottom half of the address (A0-A7).</param>
    /// <param name="data">The data to be written to the I/O device.</param>
    void Write(byte top, byte bottom, byte data);
}