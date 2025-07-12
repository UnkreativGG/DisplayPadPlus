using DisplayPadPlus.DeviceSystem.Visuals;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace DisplayPadPlus.DeviceSystem.Actions.ControllActions;
internal class GoHome(DisplayIcon displayIcon) : ASimpleAction, IHasDefaultImage
{
    readonly DisplayIcon DisplayIcon = displayIcon;

    public override void InvokeAction()
        => DisplayIcon.Layer.GetDevice().LoadHomeLayer();


    public Bitmap GetImage()
    {
        Bitmap imageToEddit = new(DisplayConsts.IconSize, DisplayConsts.IconSize);

        int height = imageToEddit.Height;
        int width = imageToEddit.Width;

        const float thicknes = 0.04f;

        const float topDisance = 0.2f;
        const float sideDisance = 0.25f;
        const float bottomDisance = 0.2f;

        const float roofFactor = 0.4f;

        float left = width * sideDisance;
        float right = width - width * sideDisance;

        float top = height * topDisance;
        float middle = height * topDisance + height * (1 - (topDisance + bottomDisance)) * roofFactor;
        float bottom = height - height * bottomDisance;

        PointF[] pathPoints =
        [
            new(left, middle),
            new(width / 2, top),
            new(right, middle),
            new(right, bottom),
            new(left, bottom),
            new(left, middle),
            new(right, middle),
        ];

        GraphicsPath path = new();
        path.AddLines(pathPoints);

        Color backgroundColor = Color.Transparent;
        Color drawColor = DisplayIcon.Layer.ImageSettings.PrimaryColor;

        Graphics graphics = Graphics.FromImage(imageToEddit);
        graphics.Clear(backgroundColor);
        graphics.SmoothingMode = SmoothingMode.AntiAlias;

        graphics.DrawPath(new Pen(drawColor, (height + width) / 2 * thicknes), path);

        return imageToEddit;
    }
}
