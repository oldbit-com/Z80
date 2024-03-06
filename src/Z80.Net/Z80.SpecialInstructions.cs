using Z80.Net.Registers;
using static Z80.Net.Instructions.OpCodes;

namespace Z80.Net;

partial class Z80
{
    private void AddSpecialInstructions()
    {
        _opCodes[ED] = () => _opCodePrefix = ED;

        _opCodes[CB] = () => _opCodePrefix = CB;

        _opCodes[IX] = () => Registers.Context = RegisterContext.IX;

        _opCodes[IY] = () => Registers.Context = RegisterContext.IY;
    }
}