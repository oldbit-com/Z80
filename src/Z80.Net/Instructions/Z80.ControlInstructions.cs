namespace OldBit.Z80.Net;

partial class Z80
{
    private void AddControlInstructions()
    {
        _opCodes["NOP"] = () => { };

        _opCodes["HALT"] = () => IsHalted = true;

        _opCodes["DI"] = () =>
        {
            IFF1 = false;
            IFF2 = false;
        };

        _opCodes["EI"] = () =>
        {
            IFF1 = true;
            IFF2 = true;
        };

        _opCodes["IM 0"] = () => InterruptMode = InterruptMode.Mode0;

        _opCodes["IM 1"] = () => InterruptMode = InterruptMode.Mode1;

        _opCodes["IM 2"] = () => InterruptMode = InterruptMode.Mode2;
    }
}