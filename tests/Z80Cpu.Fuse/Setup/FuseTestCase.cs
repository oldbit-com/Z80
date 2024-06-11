namespace OldBit.Z80Cpu.Fuse.Setup;

public class FuseTestCase
{
    public string TestId { get; set; } = string.Empty;

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

    public List<(Word, byte)> Memory { get; } = [];

    public static FuseTestCase Parse(List<string> testLines)
    {
        var registers1 = testLines[1]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(o => Convert.ToUInt16(o, 16))
            .ToArray();

        var registers2 = testLines[2]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(o => Convert.ToUInt16(o, 16))
            .ToArray();

        var testCase = new FuseTestCase
        {
            TestId = testLines[0],
            AF = registers1[0],
            BC = registers1[1],
            DE = registers1[2],
            HL = registers1[3],
            AFPrime = registers1[4],
            BCPrime = registers1[5],
            DEPrime = registers1[6],
            HLPrime = registers1[7],
            IX = registers1[8],
            IY = registers1[9],
            SP = registers1[10],
            PC = registers1[11],
            MemPtr = registers1[12],

            I = (byte)registers2[0],
            R = (byte)registers2[1],
            IFF1 = registers2[2] != 0,
            IFF2 = registers2[3] != 0,
            IM = (byte)registers2[4],
            Halted = registers2[5] != 0,
            States = registers2[6]
        };

        foreach (var memoryLine in testLines.Skip(3))
        {
            var items = memoryLine
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .TakeWhile(o => o != "-1")
                .Select(o => Convert.ToUInt16(o, 16))
                .ToArray();

            var address = items[0];
            foreach (var value in items.Skip(1))
            {
                testCase.Memory.Add((address++, (byte)value));
            }
        }

        return testCase;
    }
}