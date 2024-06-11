namespace OldBit.Z80Cpu.Fuse.Setup;

public class TestBus : IBus
{
    public byte Read(Word address) => (byte)(address >> 8);

    public void Write(Word address, byte data)
    {
    }
}