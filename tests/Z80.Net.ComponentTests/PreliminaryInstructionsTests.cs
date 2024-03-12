using Xunit.Abstractions;
using Z80.Net.ComponentTests.Fixtures;

namespace Z80.Net.ComponentTests;

public class PreliminaryInstructionsTests : TestBase
{
    public PreliminaryInstructionsTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [Fact]
    public void RunPreliminaryInstructionsTests()
    {
        Load("prelim.bin");
    }
}