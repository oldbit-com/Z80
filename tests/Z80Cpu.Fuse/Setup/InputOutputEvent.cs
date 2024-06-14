namespace OldBit.Z80Cpu.Fuse.Setup;

public class InputOutputEvent
{
    public string Type { get; set; }
    public string Address { get; set; }
    public string Value { get; set; }

    public InputOutputEvent(Word time, string type, Word address, byte value)
    {
        Type = type;
        Address = address.ToString("X4");
        Value = value.ToString("X2");
    }
}