namespace OldBit.Z80Cpu;

public interface IMemory
{
    byte Read(Word address);
    void Write(Word address, byte data);
}