# Z80 CPU Emulator

![Build](https://github.com/oldbit-com/Z80Cpu/actions/workflows/build.yml/badge.svg)

## Introduction
This is a Z80 CPU emulator written in C#. I have created it as a fun project. It is quite generic and possibly can be 
used in any project that requires a Z80 CPU emulator.

There is also my own fork of this project that adds some features specific to the ZX Spectrum, 
like memory contention handling. I simply wanted to keep this project as generic as possible.

## Features
- Full Z80 instruction set
- Undocumented instructions (at least the most common ones)

## Implementation
- The emulator is written in C# 12 and .NET 8
- Source files are kept small and clean using partial classes
  - The main class is `Z80.cs`
  - Instructions are implemented in a separate files based on logical groups:
    - `8BitArithmeticInstructions`
    - `8BitLoadInstructions`
    - `16BitArithmeticInstructions`
    - `16BitLoadInstructions`
    - `BitSetResetTestInstructions`
    - `CallReturnRestartInstructions`
    - `ControlInstructions`
    - `ExchangeBlockInstructions`
    - `GeneralPurposeArithmeticInstructions`
    - `InputOutputInstructions`
    - `JumpInstructions`
    - `RotateShiftInstructions`
    - `UndocumentedInstructions`
- There are 3 types of tests:
  - Unit tests (I have written a very basic Z80 assembly parser to help me write these)
  - Fuse tests (tests that come with the Fuse source code)
  - Zex tests (command line runner of the Z80 instruction exerciser)

## Testing
The emulator has been tested using the following test suites:
- Fuse tests
- Zex tests
- My own unit tests

## Usage
There are two interfaces that can be used to interact with the emulator:
- `IMemory`
- `IBus`

The `IMemory` interface is used to read and write memory, while the `IBus` interface is used to read and write I/O ports.

As a minimum you need to implement `IMemory` to be able to run the emulator. The `IBus` interface is optional.

The following is an example of a simple memory implementation:
```csharp
public class Memory : IMemory
{
    private readonly byte[] _memory = new byte[65535];

    public byte Read(ushort address) => _memory[address];

    public void Write(ushort address, byte data) => _memory[address] = data;
}
```
Then you can create an instance of the `Z80` class:
```csharp
var memory = new Memory();

memory.Write(0, 0x3E); // LD A, 0x12
memory.Write(1, 0x12);

var cpu = new Z80(memory);
cpu.Run(7); // Run for 7 cycles
```

## References
- [Z80 CPU User Manual](http://www.zilog.com/docs/z80/um0080.pdf)
- [Z80 OpCodes Table](http://clrhome.org/table/)
- [The Undocumented Z80 Documented](http://www.z80.info/zip/z80-documented.pdf)
