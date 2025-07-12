using System.Drawing;

namespace DisplayPadPlus.DeviceSystem;
public class DisplayConsts
{
    public const int DisplaySizeX = 800;
    public const int DisplaySizeY = 240;

    public const int HorizontalIcons = 6;
    public const int VerticalIcons = 2;
    public const int IconsCount = HorizontalIcons * VerticalIcons;

    public const int IconSize = 102;

    public const int ScreenDataBufferSize = DisplaySizeX * DisplaySizeY * 3;
    public const int IconDataBufferSize = IconSize * IconSize * 3;

    public const int LeftSpace = 14;
    public const int HorizontalPuffer = 32;
    public const int VerticalPuffer = 36;



    public static Rectangle GetButtonPos(int buttonId)
    {
        int buttonPosX = buttonId % 6;
        int buttonPosY = buttonId / 6;

        int x = LeftSpace + buttonPosX * (IconSize + HorizontalPuffer);
        int y = buttonPosY * (IconSize + VerticalPuffer);

        return new Rectangle(x, y, IconSize, IconSize);
    }
}
