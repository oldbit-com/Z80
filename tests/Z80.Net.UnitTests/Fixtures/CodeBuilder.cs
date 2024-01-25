using Z80.Net.Extensions;
using Z80.Net.Helpers;
using Z80.Net.Instructions;
using Z80.Net.Registers;
using Z80.Net.UnitTests.Support;
using static Z80.Net.Instructions.OpCodes;

namespace Z80.Net.UnitTests.Fixtures;

public class CodeBuilder
{
    private readonly List<byte> _code = [];
    private Flags _flags;
    private ushort _startAddress;
    private bool? _iff1;
    private bool? _iff2;
    private ushort? _af;
    private ushort? _afPrime;
    private ushort? _bc;
    private ushort? _bcPrime;
    private ushort? _de;
    private ushort? _dePrime;
    private ushort? _hl;
    private ushort? _hlPrime;

    public TestMemory? Memory { get; private set; }

    public CodeBuilder Flags(Flags flags)
    {
        _flags = flags;
        return this;
    }

    public CodeBuilder Iff1(bool value)
    {
        _iff1 = value;
        return this;
    }

    public CodeBuilder Iff2(bool value)
    {
        _iff2 = value;
        return this;
    }

    public CodeBuilder StartAddress(ushort startAddress)
    {
        _startAddress = startAddress;
        return this;
    }

    public CodeBuilder Code(string code)
    {
        var parser = new AssemblyParser();
        var lines = code.Split(Environment.NewLine);
        _code.AddRange(parser.Parse(lines));

        return this;
    }

    public CodeBuilder Code(params string[] lines)
    {
        var parser = new AssemblyParser();
        _code.AddRange(parser.Parse(lines));

        return this;
    }

    private CodeBuilder OpCode(OpCode opCode, int? arg, bool argFirst)
    {
        if (opCode > 0xFF)
        {
            _code.Add((byte)(opCode >> 8));
        }

        if (!argFirst)
        {
            _code.Add(opCode);
        }

        if (arg != null)
        {
            switch (opCode.ArgSize)
            {
                case 1:
                    _code.Add((byte)arg);
                    break;
                case 2:
                {
                    var (highByte, lowByte) = (ushort)arg.Value;
                    _code.Add(lowByte);
                    _code.Add(highByte);
                    break;
                }
            }
        }

        if (argFirst)
        {
            _code.Add(opCode);
        }

        return this;
    }

    public CodeBuilder OpCode(OpCode opCode, int? arg = null) => OpCode(opCode, arg, false);

    public CodeBuilder OpCode(OpCode prefix, OpCode opCode, int? arg = null)
    {
        _code.Add(prefix);
        OpCode(opCode, arg, IsCBPrefixed(opCode) && IsIndexRegister(prefix));

        return this;
    }

    public CodeBuilder Data(byte value)
    {
        _code.Add(value);
        return this;
    }

    public CodeBuilder SetAF(ushort af, ushort afPrime)
    {
        _af = af;
        _afPrime = afPrime;
        return this;
    }

    public CodeBuilder SetBC(ushort bc, ushort bcPrime)
    {
        _bc = bc;
        _bcPrime = bcPrime;
        return this;
    }

    public CodeBuilder SetDE(ushort de, ushort dePrime)
    {
        _de = de;
        _dePrime = dePrime;
        return this;
    }

    public CodeBuilder SetHL(ushort hl, ushort hlPrime)
    {
        _hl = hl;
        _hlPrime = hlPrime;
        return this;
    }

    public Z80 Build()
    {
        Memory = new TestMemory(_code.ToArray());
        var z80 = new Z80(Memory)
        {
            Registers =
            {
                F = _flags,
                PC = _startAddress,
            }
        };

        if (_af != null)
        {
            var (a, f) = _af.Value;
            z80.Registers.A = a;
            z80.Registers.F = (Flags)f;
        }

        if (_afPrime != null)
        {
            var (a, f) = _afPrime.Value;
            z80.Registers.Alternative.A = a;
            z80.Registers.Alternative.F = (Flags)f;
        }

        if (_bc != null)
        {
            z80.Registers.BC = _bc.Value;
        }

        if (_bcPrime != null)
        {
            z80.Registers.Alternative.BC = _bcPrime.Value;
        }

        if (_de != null)
        {
            z80.Registers.DE = _de.Value;
        }

        if (_dePrime != null)
        {
            z80.Registers.Alternative.DE = _dePrime.Value;
        }

        if (_hl != null)
        {
            z80.Registers.HL = _hl.Value;
        }

        if (_hlPrime != null)
        {
            z80.Registers.Alternative.HL = _hlPrime.Value;
        }

        if (_iff1 != null)
        {
            z80.IFF1 = _iff1.Value;
        }

        if (_iff2 != null)
        {
            z80.IFF2 = _iff2.Value;
        }

        return z80;
    }

    private bool IsCBPrefixed(OpCode opCode) => (opCode & 0xFF00) >> 8 == CB;
    private bool IsIndexRegister(OpCode opCode) => opCode == IX || opCode == IY;
}