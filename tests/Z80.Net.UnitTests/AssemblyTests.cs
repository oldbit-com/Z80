using Z80.Net.UnitTests.Support;

namespace Z80.Net.UnitTests;

public class AssemblyTests
{
    [Fact]
    public void TestInstruction()
    {
        var parser = new AssemblyParser();
        parser.Parse(
            "LD A,B",
            "LD A,0xFF");
    }
}