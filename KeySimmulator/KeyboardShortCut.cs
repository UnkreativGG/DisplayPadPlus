using DisplayPadPlus.DeviceSystem.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeySimmulator;
internal class KeyboardShortCut : ASimpleAction
{
    private readonly User32_SendInput.Input[] Input;



    public KeyboardShortCut(User32_SendInput.Keyboard.KeyboardEventVK vK, bool strg = false, bool shift = false, bool alt = false, bool win = false)
    {
        List<User32_SendInput.Input> list = [];

        if (strg)
            list.Add(User32_SendInput.Keyboard.Inputbuilder(User32_SendInput.Keyboard.KeyboardEventVK.VK_CONTROL, true));
        if (shift)
            list.Add(User32_SendInput.Keyboard.Inputbuilder(User32_SendInput.Keyboard.KeyboardEventVK.VK_SHIFT, true));
        if (alt)
            list.Add(User32_SendInput.Keyboard.Inputbuilder(User32_SendInput.Keyboard.KeyboardEventVK.VK_MENU, true));
        if (win)
            list.Add(User32_SendInput.Keyboard.Inputbuilder(User32_SendInput.Keyboard.KeyboardEventVK.VK_LWIN, true));

        list.Add(User32_SendInput.Keyboard.Inputbuilder(vK, true));
        list.Add(User32_SendInput.Keyboard.Inputbuilder(vK, false));

        if (win)
            list.Add(User32_SendInput.Keyboard.Inputbuilder(User32_SendInput.Keyboard.KeyboardEventVK.VK_LWIN, false));
        if (alt)
            list.Add(User32_SendInput.Keyboard.Inputbuilder(User32_SendInput.Keyboard.KeyboardEventVK.VK_LMENU, false));
        if (shift)
            list.Add(User32_SendInput.Keyboard.Inputbuilder(User32_SendInput.Keyboard.KeyboardEventVK.VK_SHIFT, false));
        if (strg)
            list.Add(User32_SendInput.Keyboard.Inputbuilder(User32_SendInput.Keyboard.KeyboardEventVK.VK_CONTROL, false));

        Input = [.. list];
    }



    public override void InvokeAction()
    {
        User32_SendInput.SendInput(Input);
    }
}
