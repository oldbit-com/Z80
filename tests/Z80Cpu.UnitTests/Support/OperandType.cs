namespace OldBit.Z80Cpu.UnitTests.Support;

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
    RegisterI,
    RegisterR,
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
    Address,
    AddressHL,
    AddressIXd,
    AddressIYd,
    AddressBC,
    AddressDE,

    ConditionC,
    ConditionNC,
    ConditionZ,
    ConditionNZ,
    ConditionM,
    ConditionP,
    ConditionPE,
    ConditionPO,
}