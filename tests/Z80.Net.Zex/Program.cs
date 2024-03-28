using OldBit.Z80.Net.Zex;

Console.WriteLine("Specify which test to run:");
Console.WriteLine("1. Prelim");
Console.WriteLine("2. ZexDoc");
Console.WriteLine("3. ZexAll");

var key = Console.ReadKey();
Console.WriteLine();

switch (key.KeyChar)
{
    case '1':
        Console.WriteLine("Running Prelim...");
        Runner.Run("prelim.bin");
        break;

    case '2':
        Console.WriteLine("Running ZexDoc...");
        Runner.Run("zexdoc.bin");
        break;

    case '3':
        Console.WriteLine("Running ZexAll...");
        Runner.Run("zexall.bin");
        break;

    default:
        Console.WriteLine("Invalid option.");
        return;
}
