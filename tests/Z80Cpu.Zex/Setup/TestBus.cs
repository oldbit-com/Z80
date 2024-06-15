using System.Text;

namespace OldBit.Z80Cpu.Zex.Setup;

internal class TestBus : IBus
{
    private readonly byte _textPort = 5;    // Port used by Bootstrap to output a character

    public byte Read(Word port) => 0xFF;

    public void Write(Word port, byte data)
    {
        if ((port & 0xFF) != _textPort)
        {
            return;
        }

        var s = Encoding.ASCII.GetString(new[] { data });
        Console.Write(s);
    }
}
