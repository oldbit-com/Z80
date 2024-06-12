# Z80 CPU Emulator

![Build] (https://github.com/oldbit-com/Z80Cpu/actions/workflows/build.yml/badge.svg)

## Introduction
This is a Z80 CPU emulator written in C#. There are many Z80 emulators out there, but this one is mine :)
I tried to write as clean and readable code as possible, so it can be used as a reference for anyone who 
wants to learn how to write a CPU emulator. This is just a fun project for me, so I don't have any plans for it
apart from using it in my other projects.

## Features
- Full Z80 instruction set
- Undocumented instructions

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

## References
- [Z80 CPU User Manual](http://www.zilog.com/docs/z80/um0080.pdf)
- [Z80 OpCodes Table](http://clrhome.org/table/)
- [The Undocumented Z80 Documented](http://www.z80.info/zip/z80-documented.pdf)
