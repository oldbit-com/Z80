using System.Text;

namespace OldBit.Z80Cpu.Zex.Setup;

internal class TestBus() : IBus
{
    private readonly byte _textPort = 5;    // Port used by Bootstrap to output a character

    public byte Read(Word address) => 0xFF;

    public void Write(Word address, byte data)
    {
        if ((address & 0xFF) != _textPort)
        {
            return;
        }

        var s = Encoding.ASCII.GetString(new[] { data });
        Console.Write(s);
    }
}