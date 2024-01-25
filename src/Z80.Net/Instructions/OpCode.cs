namespace Z80.Net.Instructions;

public readonly struct OpCode
{
    private readonly int _prefixedCode;
    private readonly byte _code;

    public OpCode(byte code, string mnemonic, int argSize = 0)
    {
        _code = code;
        Mnemonic = mnemonic;
        _prefixedCode =  code;
        ArgSize = argSize;
    }

    public OpCode(byte code, byte prefix, string mnemonic, int argSize = 0) : this(code, mnemonic, argSize)
    {
        _prefixedCode = prefix << 8 | code;
    }

    public string Mnemonic { get; }

    public int ArgSize { get; }

    public static implicit operator int(OpCode opCode) => opCode._prefixedCode;

    public static implicit operator byte(OpCode opCode) => opCode._code;

    public static bool operator ==(OpCode left, OpCode right)
    {
        return left._prefixedCode == right._prefixedCode;
    }

    public static bool operator !=(OpCode left, OpCode right)
    {
        return !(left == right);
    }

    public override int GetHashCode() => _prefixedCode;

    private bool Equals(OpCode other)
    {
        return _prefixedCode == other._prefixedCode;
    }

    public override bool Equals(object? obj)
    {
        return obj is OpCode other && Equals(other);
    }
}