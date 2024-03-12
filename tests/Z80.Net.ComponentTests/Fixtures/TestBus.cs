using Xunit.Abstractions;

namespace Z80.Net.ComponentTests.Fixtures;

public class TestBus : IBus
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly byte _textPort = 5;    // Port used by Bootstrap to output a character

    public TestBus(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    public byte Read(Word address) => 0xFF;

    public void Write(Word address, byte data)
    {
        if ((address & 0xFF) == _textPort)
        {

        }
    }
}