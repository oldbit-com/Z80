namespace OldBit.Z80Cpu.Fuse.Setup;

public class InputOutputEvent
{
    public string Time { get; set; }
    public string Type { get; set; }
    public string Address { get; set; }
    public string Value { get; set; }

    public InputOutputEvent(int time, string type, Word address, byte value)
    {
        Time = time.ToString();
        Type = type;
        Address = address.ToString("X4");
        Value = value.ToString("X2");
    }
}