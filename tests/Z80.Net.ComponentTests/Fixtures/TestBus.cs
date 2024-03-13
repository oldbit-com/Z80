using System.Text;
using Xunit.Abstractions;

namespace Z80.Net.ComponentTests.Fixtures;

internal class TestBus(ITestOutputHelper outputHelper) : IBus
{
    private readonly byte _textPort = 5;    // Port used by Bootstrap to output a character
    private readonly StringBuilder _text = new();

    public byte Read(Word address) => 0xFF;

    public void Write(Word address, byte data)
    {
        if ((address & 0xFF) != _textPort)
        {
            return;
        }

        var s = Encoding.ASCII.GetString(new[] { data });
        if (s == "$")
        {
            outputHelper.WriteLine(_text.ToString());
        }
        else
        {
            _text.Append(s);
        }
    }
}