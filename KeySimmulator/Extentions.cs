using DisplayPadPlus.DeviceSystem;
using KeySimmulator.Macros;

namespace KeySimmulator;
public static class Extentions
{
    public static DisplayIcon KeyboardShortCut(this DisplayIcon displayIcon, User32_SendInput.Keyboard.KeyboardEventVK vK)
        => displayIcon.SetAction(new KeyboardShortCut(vK));


    public static DisplayIcon Macro(this DisplayIcon displayIcon, MacroBuilder macroBuilder)
        => displayIcon.SetAction(new Macro(macroBuilder));
}
