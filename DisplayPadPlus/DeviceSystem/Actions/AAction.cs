using DisplayPadPlus.DeviceSystem.Visuals;
using System.Drawing;

namespace DisplayPadPlus.DeviceSystem.Actions;
public abstract class AAction
{
    public abstract void Down();
    public abstract void Up();


    internal DisplayIcon? Icon;

    public void PlayAnimation(IconAnimation animation)
    {
        Icon?.PlayAnimation(animation);
    }

    public IconData AddOntoBackGround(Bitmap bmp)
    {
        if (Icon == null)
            throw new Exception(nameof(Icon) + " is null");

        IconData data = new();
        Icon.CalculateWithBackGround(bmp, data);
        return data;
    }
}
