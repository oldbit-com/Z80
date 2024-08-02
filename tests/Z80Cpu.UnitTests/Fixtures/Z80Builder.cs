using OldBit.Z80Cpu.Extensions;
using OldBit.Z80Cpu.Registers;
using OldBit.Z80Cpu.UnitTests.Support;

namespace OldBit.Z80Cpu.UnitTests.Fixtures;

internal sealed class Z80Builder
{
    private readonly List<byte> _code = [];
    private IBus? _bus;
    private Flags _flags;
    private Word _startAddress;
    private bool? _iff1;
    private bool? _iff2;
    private bool? _halt;
    private InterruptMode? _im;
    private byte? _i;
    private Word? _af;
    private Word? _afPrime;
    private Word? _bc;
    private Word? _bcPrime;
    private Word? _de;
    private Word? _dePrime;
    private Word? _hl;
    private Word? _hlPrime;
    private Word? _sp;

    internal TestMemory? Memory { get; private set; }

    internal Z80Builder Flags(Flags flags)
    {
        _flags = flags;

        return this;
    }

    internal Z80Builder Iff1(bool value)
    {
        _iff1 = value;

        return this;
    }

    internal Z80Builder Iff2(bool value)
    {
        _iff2 = value;

        return this;
    }

    internal Z80Builder Halt(bool value)
    {
        _halt = value;

        return this;
    }

    internal Z80Builder Im(InterruptMode mode)
    {
        _im = mode;

        return this;
    }

    internal Z80Builder StartAddress(Word startAddress)
    {
        _startAddress = startAddress;

        return this;
    }

    internal Z80Builder Code(params string[] lines)
    {
        var parser = new AssemblyParser();
        _code.AddRange(parser.Parse(lines));

        return this;
    }

    internal Z80Builder Bus(IBus? bus)
    {
        _bus = bus;

        return this;
    }

    internal Z80Builder SetAF(Word af, Word afPrime)
    {
        _af = af;
        _afPrime = afPrime;

        return this;
    }

    internal Z80Builder SetBC(Word bc, Word bcPrime)
    {
        _bc = bc;
        _bcPrime = bcPrime;

        return this;
    }

    internal Z80Builder SetDE(Word de, Word dePrime)
    {
        _de = de;
        _dePrime = dePrime;

        return this;
    }

    internal Z80Builder SetHL(Word hl, Word hlPrime)
    {
        _hl = hl;
        _hlPrime = hlPrime;

        return this;
    }

    internal Z80Builder SetSP(Word sp)
    {
        _sp = sp;

        return this;
    }

    internal Z80Builder SetI(byte i)
    {
        _i = i;

        return this;
    }

    internal Z80 Build()
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
        z80.AddBus(_bus);

        if (_af != null)
        {
            var (a, f) = _af.Value;
            z80.Registers.A = a;
            z80.Registers.F = (Flags)f;
        }

        if (_afPrime != null)
        {
            var (a, f) = _afPrime.Value;
            z80.Registers.Prime.A = a;
            z80.Registers.Prime.F = (Flags)f;
        }

        if (_bc != null)
        {
            z80.Registers.BC = _bc.Value;
        }

        if (_bcPrime != null)
        {
            z80.Registers.Prime.BC = _bcPrime.Value;
        }

        if (_de != null)
        {
            z80.Registers.DE = _de.Value;
        }

        if (_dePrime != null)
        {
            z80.Registers.Prime.DE = _dePrime.Value;
        }

        if (_hl != null)
        {
            z80.Registers.HL = _hl.Value;
        }

        if (_hlPrime != null)
        {
            z80.Registers.Prime.HL = _hlPrime.Value;
        }

        if (_sp != null)
        {
            z80.Registers.SP = _sp.Value;
        }

        if (_i != null)
        {
            z80.Registers.I = _i.Value;
        }

        if (_iff1 != null)
        {
            z80.IFF1 = _iff1.Value;
        }

        if (_iff2 != null)
        {
            z80.IFF2 = _iff2.Value;
        }

        if (_halt != null)
        {
            z80.IsHalted = _halt.Value;
        }

        if (_im != null)
        {
            z80.IM = _im.Value;
        }

        return z80;
    }
}