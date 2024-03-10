namespace Z80.Net;

public interface IMemory
{
    // TODO: change int to Word
    byte Read(int address);
    void Write(int address, byte value);
}