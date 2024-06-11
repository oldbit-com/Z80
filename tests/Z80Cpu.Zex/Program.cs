using OldBit.Z80Cpu.Zex;

Console.WriteLine("Specify which test to run:");
Console.WriteLine("1. Prelim");
Console.WriteLine("2. ZexDoc");
Console.WriteLine("3. ZexAll");
Console.WriteLine("4. All above (1-3)");

var key = Console.ReadKey();
Console.WriteLine();

switch (key.KeyChar)
{
    case '1':
        Console.WriteLine("Running Prelim...");
        Runner.Run("prelim.bin");
        Console.WriteLine();
        break;

    case '2':
        Console.WriteLine("Running ZexDoc...");
        Runner.Run("zexdoc.bin");
        Console.WriteLine();
        break;

    case '3':
        Console.WriteLine("Running ZexAll...");
        Runner.Run("zexall.bin");
        Console.WriteLine();
        break;

    case '4':
        Console.WriteLine("[1/3] Running Prelim...");
        Runner.Run("prelim.bin");
        Console.WriteLine();
        Console.WriteLine();

        Console.WriteLine("[2/3] Running ZexDoc...");
        Runner.Run("zexdoc.bin");
        Console.WriteLine();
        Console.WriteLine();

        Console.WriteLine("[3/3] Running ZexAll...");
        Runner.Run("zexall.bin");
        Console.WriteLine();
        Console.WriteLine();
        break;

    default:
        Console.WriteLine("Invalid option.");
        return;
}

