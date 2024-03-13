using Xunit.Abstractions;
using Z80.Net.ComponentTests.Fixtures;

namespace Z80.Net.ComponentTests;

public class PreliminaryInstructionsTests(ITestOutputHelper outputHelper) : TestBase(outputHelper)
{
    [Fact]
    public void RunPreliminaryInstructionsTests()
    {
        var z80  = CreateZ80("prelim.bin");
        z80.Run();

        z80.Registers.PC.Should().Be(0xF001);
        z80.States.TotalStates.Should().Be(10086);
    }

    [Fact]
    public void RunDocumentedInstructionsTests()
    {
        var z80  = CreateZ80("zexdoc.bin");
        z80.Run();

        z80.Registers.PC.Should().Be(0xF001);
        z80.States.TotalStates.Should().Be(10086);
    }
}