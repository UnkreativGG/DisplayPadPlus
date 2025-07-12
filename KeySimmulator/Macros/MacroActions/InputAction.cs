using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeySimmulator.Macros.MacroActions;
internal class InputAction : IMacroAction
{
    internal readonly User32_SendInput.Input[] Input;



    public InputAction(User32_SendInput.Input input)
        => Input = [input];

    public InputAction(User32_SendInput.Input[] input)
        => Input = input;



    public void Execute()
    {
        User32_SendInput.SendInput(Input);
    }
}
