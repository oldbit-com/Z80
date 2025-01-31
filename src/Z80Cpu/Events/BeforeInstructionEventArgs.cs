namespace OldBit.Z80Cpu.Events;

/// <summary>
/// Represents the event arguments for the BeforeInstruction event.
/// </summary>
public class BeforeInstructionEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the program counter (PC) value.
    /// </summary>
    public Word PC { get; internal set; }

    /// <summary>
    /// Gets or sets a value indicating whether to break the execution.
    /// </summary>
    public bool IsBreakpoint { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BeforeInstructionEventArgs"/>
    /// class with the specified program counter value.
    /// </summary>
    /// <param name="pc">The program counter value.</param>
    private BeforeInstructionEventArgs(Word pc) => PC = pc;

    /// <summary>
    /// Gets the singleton instance of the <see cref="BeforeInstructionEventArgs"/> class.
    /// Shared instance is used to avoid unnecessary object creation.
    /// </summary>
    public static BeforeInstructionEventArgs Instance { get; } = new(0);
}