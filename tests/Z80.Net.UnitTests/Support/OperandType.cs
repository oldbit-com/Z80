namespace Z80.Net.UnitTests.Support;

internal enum OperandType
{
    Unknown,

    RegisterA,
    RegisterB,
    RegisterC,
    RegisterD,
    RegisterE,
    RegisterH,
    RegisterL,
    RegisterF,
    RegisterIXH,
    RegisterIXL,
    RegisterIYH,
    RegisterIYL,

    RegisterAF,
    RegisterBC,
    RegisterDE,
    RegisterHL,
    RegisterSP,
    RegisterIX,
    RegisterIY,

    Value,
    Memory,
    MemoryHL,
    MemoryIXd,
    MemoryIYd,
    MemoryBC,
    MemoryDE,

    ConditionC,
    ConditionNC,
    ConditionZ,
    ConditionNZ,
    ConditionM,
    ConditionP,
    ConditionPE,
    ConditionPO,
}