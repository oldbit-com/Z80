namespace Z80.Net;

public interface IMemory
{
    byte Read(int address);
    void Write(int address, byte value);
}