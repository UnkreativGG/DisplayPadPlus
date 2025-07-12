using KeySimmulator.Macros.MacroActions;

namespace KeySimmulator.Macros;
public class MacroBuilder
{
    /// <summary>
    /// The delay between each action. Can be overridden with Delay(int delay).
    /// </summary>
    public int DefaultDelay = 0;
    /// <summary>
    /// If the activity state of the macro should be a toggle if the button needs to be held down.
    /// </summary>
    public bool Toggle = true;
    /// <summary>
    /// How often the macro should be played. -1 for infinited.
    /// </summary>
    public int TimesToPlay = 1;
    /// <summary>
    /// Weather or not the macro should be finished, when deactivated.
    /// Still not implemented.
    /// </summary>
    public bool FinishWhenDeactivated = false;







    internal List<IMacroAction> MacroActions = [];




    public MacroBuilder Keyboard_Click(User32_SendInput.Keyboard.KeyboardEventVK vk)
        => Keyboard_Down(vk).Keyboard_UP(vk);




    public MacroBuilder Keyboard_Down(User32_SendInput.Keyboard.KeyboardEventVK vk)
    {
        MacroActions.Add(new InputAction(User32_SendInput.Keyboard.Inputbuilder(vk, true)));
        return this;
    }



    public MacroBuilder Keyboard_UP(User32_SendInput.Keyboard.KeyboardEventVK vk)
    {
        MacroActions.Add(new InputAction(User32_SendInput.Keyboard.Inputbuilder(vk, false)));
        return this;
    }




    public MacroBuilder Delay(int deleay)
    {
        MacroActions.Add(new DelayAction(deleay));
        return this;
    }




    public MacroBuilder Keyboard_PrintString(object obj)
        => Keyboard_PrintString(obj.ToString() ?? "macro -> null object");


    public MacroBuilder Keyboard_PrintString(string str)
    {
        for (int i = 0; i < str.Length; i++)
        {
            char c = str[i];
            c = c.ToString().ToUpper()[0];
            User32_SendInput.Keyboard.KeyboardEventVK vk;

            switch (c)
            {
                case ' ': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_SPACE; break;

                case '0': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_0; break;
                case '1': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_1; break;
                case '2': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_2; break;
                case '3': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_3; break;
                case '4': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_4; break;
                case '5': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_5; break;
                case '6': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_6; break;
                case '7': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_7; break;
                case '8': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_8; break;
                case '9': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_9; break;

                case 'A': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_A; break;
                case 'B': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_B; break;
                case 'C': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_C; break;
                case 'D': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_D; break;
                case 'E': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_E; break;
                case 'F': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_F; break;
                case 'G': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_G; break;
                case 'H': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_H; break;
                case 'I': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_I; break;
                case 'J': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_J; break;
                case 'K': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_K; break;
                case 'L': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_L; break;
                case 'M': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_M; break;
                case 'N': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_N; break;
                case 'O': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_O; break;
                case 'P': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_P; break;
                case 'Q': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_Q; break;
                case 'R': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_R; break;
                case 'S': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_S; break;
                case 'T': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_T; break;
                case 'U': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_U; break;
                case 'V': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_V; break;
                case 'W': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_W; break;
                case 'X': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_X; break;
                case 'Y': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_Y; break;
                case 'Z': vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_Z; break;

                default: vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_SPACE; break;
            }


            Keyboard_Click(vk);
        }




        return this;
    }
}
