using Xunit.Abstractions;

namespace Z80.Net.ComponentTests.Fixtures;

public class TestFixture
{
    private readonly ITestOutputHelper _outputHelper;

    public TestFixture(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;

    }
}