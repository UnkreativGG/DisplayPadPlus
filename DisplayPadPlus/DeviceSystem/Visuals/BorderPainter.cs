using DisplayPadPlus.DeviceSystem;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DisplayPadPlus.DeviceSystem.Visuals;
public class BorderPainter
{
    private static readonly Bitmap Border = DrawBorder();
    public static Bitmap GetNewBorder()
        => new(Border);


    public static Bitmap DrawBorder()
    {
        Bitmap imageToEddit = new(DisplayConsts.IconSize, DisplayConsts.IconSize);

        int height = imageToEddit.Height;
        int width = imageToEddit.Width;

        const float distance = 0.08f;
        const float thicknes = 0.03f;
        const float arcSize = 0.2f;

        Color backgroundColor = Color.Black;
        Color borderColor = Color.Blue;

        Graphics graphics = Graphics.FromImage(imageToEddit);
        graphics.Clear(backgroundColor);
        graphics.SmoothingMode = SmoothingMode.AntiAlias;


        GraphicsPath path = new();

        path.AddArc(/*   */ (int)(distance /*       */ * width), /*    */ (int)(distance /*       */ * height), (int)(arcSize * width), (int)(arcSize * height), 180f, 90f);
        path.AddArc(width - (int)((distance + arcSize) * width), /*    */ (int)(distance /*       */ * height), (int)(arcSize * width), (int)(arcSize * height), 270f, 90f);
        path.AddArc(width - (int)((distance + arcSize) * width), height - (int)((distance + arcSize) * height), (int)(arcSize * width), (int)(arcSize * height), 000f, 90f);
        path.AddArc(/*   */ (int)(distance /*       */ * width), height - (int)((distance + arcSize) * height), (int)(arcSize * width), (int)(arcSize * height), 090f, 90f);
        path.AddLines(new PointF[] { path.PathPoints[0] });

        graphics.DrawPath(new Pen(borderColor, (height + width) / 2 * thicknes), path);

        return imageToEddit;
    }
}
