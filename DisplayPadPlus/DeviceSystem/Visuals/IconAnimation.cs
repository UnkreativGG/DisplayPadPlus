using System.Drawing;

namespace DisplayPadPlus.DeviceSystem.Visuals;
public class IconAnimation
{
    internal readonly IconData[] Frames;

    public readonly TimeSpan Duration;

    public IconAnimation(IconData[] frames, TimeSpan duration)
    {
        Frames = frames;
        Duration = duration;
    }
}
