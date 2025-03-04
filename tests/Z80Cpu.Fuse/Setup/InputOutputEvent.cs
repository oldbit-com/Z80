namespace OldBit.Z80Cpu.Fuse.Setup;

public class InputOutputEvent(int time, string type, Word address, byte value)
{
    public string Time { get; set; } = time.ToString();
    public string Type { get; set; } = type;
    public string Address { get; set; } = address.ToString("X4");
    public string Value { get; set; } = value.ToString("X2");
}