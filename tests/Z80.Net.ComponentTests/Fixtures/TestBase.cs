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

    protected Z80 CreateZ80(string fileName)
    {
        var testFile =  File.ReadAllBytes(Path.Combine(BinFolder, "TestFiles", "binaries", fileName));
        var bootstrapFile =  File.ReadAllBytes(Path.Combine(BinFolder, "Bootstrap", "binaries", "bootstrap.bin"));

        var memory = new TestMemory();
        memory.CopyFrom(bootstrapFile, 0);
        memory.CopyFrom(testFile, 0x100);

        var bus = new TestBus(_outputHelper);

        return new Z80(memory, bus);
    }

    private static string BinFolder => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
}