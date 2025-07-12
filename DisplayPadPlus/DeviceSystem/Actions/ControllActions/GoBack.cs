using DisplayPadPlus.DeviceSystem.Visuals;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DisplayPadPlus.DeviceSystem.Actions.ControllActions;
internal class GoBack(DisplayIcon displayIcon) : ASimpleAction, IHasDefaultImage
{
    readonly DisplayIcon DisplayIcon = displayIcon;

    public override void InvokeAction()
        => DisplayIcon.Layer.GetParentLayer().InvokeAction();


    public Bitmap GetImage()
    {
        Bitmap imageToEddit = new(DisplayConsts.IconSize, DisplayConsts.IconSize);

        int height = imageToEddit.Height;
        int width = imageToEddit.Width;

        const float thicknes = 0.04f;

        const float topDisance = 0.2f;
        const float leftDisance = 0.15f;
        const float rightDisance = 0.2f;
        const float bottomDisance = 0.2f;

        const float arrowThicknes = 0.15f;
        const float ArrowTipWidht = 0.1f;
        const float ArrowTipLength = 0.2f;

        float X1 = width * leftDisance;
        float X2 = width * (leftDisance + ArrowTipLength);
        float X3 = width - width * (rightDisance + arrowThicknes);
        float X4 = width - width * rightDisance;

        float Y1 = height * topDisance;
        float Y2 = height - height * (bottomDisance + ArrowTipWidht + arrowThicknes + ArrowTipWidht);
        float Y3 = height - height * (bottomDisance + ArrowTipWidht + arrowThicknes);
        float Y4 = height - height * (bottomDisance + ArrowTipWidht + arrowThicknes / 2);
        float Y5 = height - height * (bottomDisance + ArrowTipWidht);
        float Y6 = height - height * bottomDisance;

        PointF[] pathPoints =
        [
            new(X4, Y1),
            new(X3, Y1),
            new(X3, Y3),
            new(X2, Y3),
            new(X2, Y2),
            new(X1, Y4),
            new(X2, Y6),
            new(X2, Y5),
            new(X4, Y5),
            new(X4, Y1),
            new(X3, Y1),
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
