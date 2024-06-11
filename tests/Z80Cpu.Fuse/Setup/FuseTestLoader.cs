using System.Reflection;

namespace OldBit.Z80Cpu.Fuse.Setup;

public static class FuseTestLoader
{
    public static IEnumerable<(FuseTestCase TestCase, FuseTestResult TestResult)> Load()
    {
        var tests = LoadTests().ToList();
        var results = LoadResults().ToDictionary(x => x.TestId);

        foreach (var test in tests)
        {
            yield return (test, results[test.TestId]);
        }
    }

    private static IEnumerable<FuseTestCase> LoadTests()
    {
        var allLines = File.ReadAllLines(Path.Combine(BinFolder, "TestFiles", "tests.in"));

        var testLines = new List<string>();
        foreach (var line in allLines)
        {
            if (IsEmptyLine(line))
            {
                continue;
            }

            if (IsEndOfTest(line))
            {
                yield return FuseTestCase.Parse(testLines);
                testLines.Clear();

                continue;
            }

            testLines.Add(line);
        }
    }

    private static IEnumerable<FuseTestResult> LoadResults()
    {
        var allLines = File.ReadAllLines(Path.Combine(BinFolder, "TestFiles", "tests.expected"));

        var testLines = new List<string>();
        foreach (var line in allLines)
        {
            if (IsEmptyLine(line) && testLines.Count > 0)
            {
                yield return FuseTestResult.Parse(testLines);
                testLines.Clear();

                continue;
            }

            testLines.Add(line);
        }
    }

    private static bool IsEndOfTest(string line) => line.Trim().Equals("-1");
    private static bool IsEmptyLine(string line) => string.IsNullOrWhiteSpace(line);
    private static string BinFolder => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
}