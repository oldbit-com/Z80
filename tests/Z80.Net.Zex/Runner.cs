using System.Reflection;
using OldBit.Z80.Net.Zex.Setup;

namespace OldBit.Z80.Net.Zex;

public static class Runner
{
    public static void Run(string fileName)
    {
        var testFile =  File.ReadAllBytes(Path.Combine(BinFolder, "TestFiles", "binaries", fileName));
        var bootstrapFile =  File.ReadAllBytes(Path.Combine(BinFolder, "Bootstrap", "binaries", "bootstrap.bin"));

        var memory = new TestMemory();
        memory.CopyFrom(bootstrapFile, 0);
        memory.CopyFrom(testFile, 0x100);

        var bus = new TestBus();

        var z80 = new Z80(memory).AddBus(bus);
        z80.Run();
    }

    private static string BinFolder => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

}