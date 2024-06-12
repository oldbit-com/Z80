using FluentAssertions;
using OldBit.Z80Cpu.Fuse.Setup;

namespace OldBit.Z80Cpu.Fuse;

public class FuseTests
{
    [Theory]
    [MemberData(nameof(TestData))]
    public void RunFuseTestCase(FuseTestCase testCase, FuseTestResult testResult)
    {
        var z80 = SetupTest(testCase);

        z80.Run(testCase.States);

        // Ignore Y and X undocumented flags for now
        (z80.Registers.AF & 0xD7).Should().Be(testResult.AF & 0xD7);
        z80.Registers.BC.Should().Be(testResult.BC);
        z80.Registers.DE.Should().Be(testResult.DE);
        z80.Registers.HL.Should().Be(testResult.HL);

        z80.Registers.IX.Should().Be(testResult.IX);
        z80.Registers.IY.Should().Be(testResult.IY);
        z80.Registers.SP.Should().Be(testResult.SP);
        z80.Registers.PC.Should().Be(testResult.PC);

        z80.Registers.Prime.AF.Should().Be(testResult.AFPrime);
        z80.Registers.Prime.BC.Should().Be(testResult.BCPrime);
        z80.Registers.Prime.DE.Should().Be(testResult.DEPrime);
        z80.Registers.Prime.HL.Should().Be(testResult.HLPrime);

        z80.Registers.I.Should().Be(testResult.I);
        z80.Registers.R.Should().Be(testResult.R);

        z80.IM.Should().Be((InterruptMode)testResult.IM);
        z80.IFF1.Should().Be(testResult.IFF1);
        z80.IFF2.Should().Be(testResult.IFF2);
        z80.IsHalted.Should().Be(testResult.Halted);

        z80.Cycles.TotalCycles.Should().Be(testResult.States);

    }

    private static Z80 SetupTest(FuseTestCase testCase)
    {
        var memory = new TestMemory(testCase.Memory);

        var z80 = new Z80(memory)
        {
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

        z80.AddBus(new TestBus());

        return z80;
    }

    public static IEnumerable<object[]> TestData =>
        FuseTestLoader.Load()
            //.Where(x => x.TestCase.TestId == "76")
            .Select(x => new object[] { x.TestCase, x.TestResult });
}