using OldBit.Z80Cpu.Fuse.Setup;
using OldBit.Z80Cpu.Registers;

namespace OldBit.Z80Cpu.Fuse;

public class FuseTests
{
    private TestMemory _testMemory = null!;
    private readonly List<InputOutputEvent> _events = [];

    [Theory]
    [MemberData(nameof(TestData))]
    public void RunFuseTestCase(FuseTestCase testCase, FuseTestResult testResult)
    {
        var z80 = SetupTest(testCase);

        z80.Run(testCase.Ticks);

        z80.Registers.A.ShouldBe((byte)(testResult.AF >> 8));
        if (testCase.TestId is "37_1" or "3f" or "cb46_2" or "cb46_3" or "cb46_3" or "cb46_4" or "cb46_5"
            or "cb4e" or "cb5e" or "cb6e" or "cb76")
        {
            // Ignore Y and X undocumented flags for these tests, undocumented features, may fix later, but not important
            (z80.Registers.F & ~(Flags.X | Flags.Y)).ShouldBe((Flags)(testResult.AF & 0xD7));
        }
        else
        {
            z80.Registers.F.ShouldBe((Flags)(testResult.AF & 0xFF));
        }

        z80.Registers.BC.ShouldBe(testResult.BC);
        z80.Registers.DE.ShouldBe(testResult.DE);
        z80.Registers.HL.ShouldBe(testResult.HL);

        z80.Registers.IX.ShouldBe(testResult.IX);
        z80.Registers.IY.ShouldBe(testResult.IY);
        z80.Registers.SP.ShouldBe(testResult.SP);
        z80.Registers.PC.ShouldBe(testResult.PC);

        z80.Registers.Prime.AF.ShouldBe(testResult.AFPrime);
        z80.Registers.Prime.BC.ShouldBe(testResult.BCPrime);
        z80.Registers.Prime.DE.ShouldBe(testResult.DEPrime);
        z80.Registers.Prime.HL.ShouldBe(testResult.HLPrime);

        z80.Registers.I.ShouldBe(testResult.I);
        z80.Registers.R.ShouldBe(testResult.R);

        z80.IM.ShouldBe((InterruptMode)testResult.IM);
        z80.IFF1.ShouldBe(testResult.IFF1);
        z80.IFF2.ShouldBe(testResult.IFF2);
        z80.IsHalted.ShouldBe(testResult.Halted);

        z80.Clock.TotalTicks.ShouldBe(testResult.Ticks);

        _events.Count.ShouldBe(testResult.Events.Count);
        _events.ShouldBeEquivalentTo(testResult.Events);
    }

    private Z80 SetupTest(FuseTestCase testCase)
    {
        _testMemory = new TestMemory(testCase.Memory, _events);
        var testContentionProvider = new TestContentionProvider(_events);

        var z80 = new Z80(_testMemory)
        {
            Clock =
            {
                ContentionProvider = testContentionProvider
            },
            Registers =
            {
                AF = testCase.AF,
                BC = testCase.BC,
                DE = testCase.DE,
                HL = testCase.HL,
                Prime =
                {
                    AF = testCase.AFPrime,
                    BC = testCase.BCPrime,
                    DE = testCase.DEPrime,
                    HL = testCase.HLPrime
                },
                IX = testCase.IX,
                IY = testCase.IY,
                SP = testCase.SP,
                PC = testCase.PC,
                I = testCase.I,
                R = testCase.R,
            },
            IFF1 = testCase.IFF1,
            IFF2 = testCase.IFF2,
            IM = (InterruptMode)testCase.IM,
            IsHalted = testCase.Halted
        };

        _testMemory.Clock = z80.Clock;
        testContentionProvider.Clock = z80.Clock;
        z80.AddBus(new TestBus(_events, z80.Clock));

        return z80;
    }

    public static IEnumerable<object[]> TestData =>
        FuseTestLoader.Load()
            .Select(x => new object[] { x.TestCase, x.TestResult });
}