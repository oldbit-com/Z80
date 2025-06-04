using System.Text;

namespace OldBit.Z80Cpu.Zex.Setup;

internal class TestBus(Action exitAction) : IBus
{
    private const byte TextOutPort = 5; // Port used by Bootstrap to output a character
    private const byte ExitPort = 6;    // Port used by Bootstrap to detect test completion

    public byte Read(Word port) => 0xFF;

    public void Write(Word port, byte data)
    {
        if ((port & 0xFF) == ExitPort && data == 0xA5)
        {
            exitAction.Invoke();
        }

        if ((port & 0xFF) != TextOutPort)
        {
            return;
        }

        var s = Encoding.ASCII.GetString(new[] { data });
        Console.Write(s);
    }
}
