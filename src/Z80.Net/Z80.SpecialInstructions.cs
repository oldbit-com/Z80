using Z80.Net.Registers;
using static Z80.Net.Instructions.OpCodes;

namespace Z80.Net;

partial class Z80
{
    private void AddSpecialInstructions()
    {
        _opCodes[ED] = () => _opCodePrefix = ED;

        _opCodes[CB] = () => _opCodePrefix = CB;

        _opCodes[IX] = () => Registers.HLContext = HLContext.IX;

        _opCodes[IY] = () => Registers.HLContext = HLContext.IY;
    }
}