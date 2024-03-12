using System.Reflection;
using Xunit.Abstractions;

namespace Z80.Net.ComponentTests.Fixtures;

public class TestBase
{
    private readonly ITestOutputHelper _outputHelper;

    protected TestBase(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    protected void Load(string fileName)
    {
        var file =  File.ReadAllBytes(Path.Combine(BinFolder, "TestFiles", "binaries", fileName));

    }

    private void LoadBootstrap()
    {
        var file =  File.ReadAllBytes(Path.Combine(BinFolder, "Bootstrap", "binaries", "bootstrap.bin"));
    }

    private static string BinFolder => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
}