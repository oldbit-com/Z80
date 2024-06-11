# Zex Tests
This is a command line runner of the Z80 instruction exerciser. 

It allows running the Z80 instruction exerciser on the Z80 emulator.

The test is implemented as a command line application since it can run for a relatively long time and it is not practical to execute them as a part of the normal unit tests run.

When running you can choose to run:
- Prelim tests (prelim.bin)
- ZexDoc tests (zexdoc.bin)
- ZexAll tests (zexall.bin)

It is best to run it in the Release configuration.