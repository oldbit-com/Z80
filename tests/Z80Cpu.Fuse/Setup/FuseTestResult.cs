namespace OldBit.Z80Cpu.Fuse.Setup;

public record Event(int Time, string Type, Word Address, byte Data);

public class FuseTestResult
{
    public string TestId { get; set; } = string.Empty;

    public List<Event> Events { get; } = [];

    public Word AF { get; set; }
    public Word BC { get; set; }
    public Word DE { get; set; }
    public Word HL { get; set; }
    public Word AFPrime { get; set; }
    public Word BCPrime { get; set; }
    public Word DEPrime { get; set; }
    public Word HLPrime { get; set; }
    public Word IX { get; set; }
    public Word IY { get; set; }
    public Word SP { get; set; }
    public Word PC { get; set; }
    public Word MemPtr { get; set; }

    public byte I { get; set; }
    public byte R { get; set; }
    public bool IFF1 { get; set; }
    public bool IFF2 { get; set; }
    public byte IM { get; set; }
    public bool Halted { get; set; }
    public int States { get; set; }

    public static FuseTestResult Parse(List<string> testLines)
    {
        var testResult = new FuseTestResult
        {
            TestId = testLines[0],
        };

        var events = testLines.Skip(1).Where(x =>
            x.Contains(" MR ") ||
            x.Contains(" MW ") ||
            x.Contains(" MC ") ||
            x.Contains(" PR ") ||
            x.Contains(" PW ") ||
            x.Contains(" PC ")).ToList();

        foreach (var parts in events.Select(@event => @event.Split(' ', StringSplitOptions.RemoveEmptyEntries)))
        {
            testResult.Events.Add(new Event(
                Convert.ToUInt16(parts[0]),
                parts[1],
                Convert.ToUInt16(parts[2], 16),
                parts.Length > 3 ? Convert.ToByte(parts[3], 16) : (byte)0));
        }

        var registers1 = testLines.Skip(1 + events.Count).First()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(o => Convert.ToUInt16(o, 16))
            .ToArray();

        testResult.AF = registers1[0];
        testResult.BC = registers1[1];
        testResult.DE = registers1[2];
        testResult.HL = registers1[3];
        testResult.AFPrime = registers1[4];
        testResult.BCPrime = registers1[5];
        testResult.DEPrime = registers1[6];
        testResult.HLPrime = registers1[7];
        testResult.IX = registers1[8];
        testResult.IY = registers1[9];
        testResult.SP = registers1[10];
        testResult.PC = registers1[11];
        testResult.MemPtr = registers1[12];

        var registers2 = testLines.Skip(2 + events.Count).First()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .ToArray();

        testResult.I = (byte)Convert.ToUInt16(registers2[0], 16);
        testResult.R = (byte)Convert.ToUInt16(registers2[1], 16);
        testResult.IFF1 = registers2[2] != "0";
        testResult.IFF2 = registers2[3] != "0";
        testResult.IM = (byte)Convert.ToUInt16(registers2[4], 16);
        testResult.Halted = registers2[5] != "0";
        testResult.States = Convert.ToUInt16(registers2[6]);

        return testResult;
    }
}