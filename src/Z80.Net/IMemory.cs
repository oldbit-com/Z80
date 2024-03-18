namespace OldBit.Z80.Net;

public interface IMemory
{
    byte Read(Word address);
    void Write(Word address, byte data);
}