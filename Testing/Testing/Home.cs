using DisplayPadPlus.DeviceSystem;
using KeySimmulator;
using KeySimmulator.Macros;

namespace Testing.Layers;
internal class Home : Layer
{
    public override void Config()
    {        
        CreateIcon(5)
            .SetAsLayer<FirstSub>();


        MacroBuilder macroBuilder = new MacroBuilder()
            .Keyboard_PrintString("hello world")
            .Delay(50)
            .Keyboard_Click(User32_SendInput.Keyboard.KeyboardEventVK.VK_RETURN);




        CreateIcon(0)
            .OverwriteImageWithPath(@"---")
            .Macro(macroBuilder);

        CreateIcon(1)
            .KeyboardShortCut(User32_SendInput.Keyboard.KeyboardEventVK.VK_K)
            .OverwriteImageWithText("Example Text");


        CreateIcon(3)
            .SetAsLayer<SecundSub>();


        MacroBuilder macroBuilder1 = new();

        for (int i = 0; i < 1000; i++)
        {
            macroBuilder1.Keyboard_PrintString(i);
            macroBuilder1.Keyboard_Click(User32_SendInput.Keyboard.KeyboardEventVK.VK_RETURN);
        }

        CreateIcon(6)
            .OverwriteImageWithText("1, 2, 3, 4, ...")
            .Macro(macroBuilder1);
    }
}
