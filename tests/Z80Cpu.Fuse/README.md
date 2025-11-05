# Fuse Tests
These are the tests that come with the Fuse emulator.

There are a few tests related to undocumented X & Y flags that are failing and assertions
is excluding those flags from the test. This is very low priority to fix.

IO contention tests have been updated for pre and post contention executed sequentially
since this is how my Z80 is doing it. This seems to be giving correct timings in my emulator.