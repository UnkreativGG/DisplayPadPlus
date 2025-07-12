using DisplayPadPlus.DeviceSystem.Actions;
using DisplayPadPlus.DeviceSystem.Visuals;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DisplayPadPlus.DeviceSystem;
public abstract class Layer : ASimpleAction, IHasDefaultImage
{
    internal object Parent;

    Background? background;
    public Background Background
    {
        get => background ?? GetDevice().DefaultBackground;
        set => background = value;
    }

    ImageSettings? imageSettings;
    public ImageSettings ImageSettings
    {
        get => imageSettings ?? GetDevice().DefaultImageSettings;
        set => imageSettings = value;
    }

    internal protected readonly DisplayIcon?[] Icons = new DisplayIcon[DisplayConsts.IconsCount];

    internal readonly ScreenData LayerBackground = new();

    void CalcScreenData()
    {
        Background.backGroundData.Data.CopyTo(LayerBackground.Data, 0);
        for (int i = 0; i < Icons.Length; i++)
            Icons[i]?.UpdateLayerData();
    }

    void UpdateScreenDataAtIcon(int iconId)
    {
        DisplayIcon? icon = Icons[iconId];
        if (icon != null)
        {
            IconData iconData = icon.CurrentDisplayIconData;
            Rectangle pos = DisplayConsts.GetButtonPos(iconId);

            for (int x = 0; x < pos.Width; x++)
                for (int y = 0; y < pos.Height; y++)
                {
                    int screenPos = ((pos.Y + y) * DisplayConsts.DisplaySizeX + pos.X + x) * 3;
                    int iconPos = (y * DisplayConsts.IconSize + x) * 3;
                    LayerBackground.Data[screenPos + 0] = iconData.Data[iconPos + 0];
                    LayerBackground.Data[screenPos + 1] = iconData.Data[iconPos + 1];
                    LayerBackground.Data[screenPos + 2] = iconData.Data[iconPos + 2];
                }
        }
    }

    void AssignDefaultImages()
    {
        for (int i = 0; i < Icons.Length; i++)
        {
            DisplayIcon? icon = Icons[i];
            if (icon != null)
            {
                if (icon.ImageOverriden)
                {
                    icon.UpdateLayerData();
                    continue;
                }
                ASimpleAction? defaultAction = icon.Action;
                if (defaultAction != null && defaultAction is IHasDefaultImage hasDefaultImage)
                {
                    icon.CalculateWithBackGround(hasDefaultImage.GetImage(), icon.CurrentDisplayIconData);
                }
            }
        }
    }

    internal enum LayerState
    {
        HomeLayer,
        SubLayer,
    }


    internal LayerState GetLayerState()
    {
        if (Parent is DisplayIcon)
            return LayerState.SubLayer;
        else if (Parent is Device)
            return LayerState.HomeLayer;

        throw new Exception("Layer is somehow independent");
    }


    internal Layer GetParentLayer()
        => ((DisplayIcon)Parent).Layer;


    internal Device GetDevice()
    {
        return GetLayerState() switch
        {
            LayerState.HomeLayer => (Device)Parent,
            LayerState.SubLayer => GetParentLayer().GetDevice(),
            _ => throw new Exception("Layer is somehow independent"),
        };
    }

    public virtual void ReseaveKeyPress(int key, bool down)
        => Icons[key]?.KeyUpdate(down);



    public abstract void Config();


    public DisplayIcon CreateIcon(int position)
        => Icons[position] = new DisplayIcon(this);


    public override void InvokeAction()
        => GetDevice().LoadLayer(this);



    internal static Layer CreateLayer<TLayer>(object parent) where TLayer : Layer, new()
    {
        Layer layer = new TLayer()
        {
            Parent = parent,
        };

        layer.Config();
        layer.AssignDefaultImages();
        layer.CalcScreenData();

        return layer;
    }


    protected Layer() { }



    public virtual Bitmap GetImage()
        => AddText(DrawLayerIcon());



    #region VisualCreation
    private Bitmap AddText(Bitmap bmp)
    {
        Graphics graphics = Graphics.FromImage(bmp);
        GraphicsPath path = new();

        int height = bmp.Height;
        int width = bmp.Width;

        const float topDisance = 0.05f;
        const float bottomDisance = 0.7f;
        const float sideDisance = 0.08f;

        Color color = ImageSettings.SecundaryColor;


        RectangleF rec = new(
            sideDisance * width,
            topDisance * height,
            (1 - sideDisance * 2) * width,
            (1 - topDisance - bottomDisance) * height);

        FontFamily fontFamily = ImageSettings.DefaultFontFamily;

        StringFormat stringFormat = new()
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center,
        };

        path.AddString(GetType().Name, fontFamily, 0, 15, rec, stringFormat);

        graphics.FillPath(new SolidBrush(color), path);
        //graphics.DrawPath(new Pen(color, (height + width) / 2 * thicknes), path);

        return bmp;
    }

    private Bitmap DrawLayerIcon()
    {
        Bitmap imageToEddit = new(DisplayConsts.IconSize, DisplayConsts.IconSize);

        int height = imageToEddit.Height;
        int width = imageToEddit.Width;

        const float topDisance = 0.5f;
        const float bottomDisance = 0.15f;
        const float sideDisance = 0.21f;
        const float thicknes = 0.04f;
        const float arcSize = 0.11f;

        const float flapOfset = 0.0f;
        const float flapHeight = 0.07f;
        const float flapWidth = 0.08f;
        const float flapArcSize = 0.12f;

        Color backgroundColor = Color.Transparent;
        Color drawColor = ImageSettings.PrimaryColor;

        Graphics graphics = Graphics.FromImage(imageToEddit);
        graphics.Clear(backgroundColor);
        graphics.SmoothingMode = SmoothingMode.AntiAlias;


        GraphicsPath path = new();

        path.AddArc((int)(sideDisance * width), (int)(topDisance * height), (int)(arcSize * width), (int)(arcSize * height), 180f, 90f);
        path.AddArc(width - (int)((sideDisance + arcSize) * width), (int)(topDisance * height), (int)(arcSize * width), (int)(arcSize * height), 270f, 90f);
        path.AddArc(width - (int)((sideDisance + arcSize) * width), height - (int)((bottomDisance + arcSize) * height), (int)(arcSize * width), (int)(arcSize * height), 000f, 90f);
        path.AddArc((int)(sideDisance * width), height - (int)((bottomDisance + arcSize) * height), (int)(arcSize * width), (int)(arcSize * height), 090f, 90f);
        path.AddLines(new PointF[] { path.PathPoints[0] });

        graphics.DrawPath(new Pen(drawColor, (height + width) / 2 * thicknes), path);

        float baseSide = sideDisance + arcSize / 2 + flapOfset;

        path = new();

        path.AddLines(new PointF[] { new(baseSide * width, topDisance * height) });
        path.AddArc((int)(baseSide * width), (int)((topDisance - flapHeight) * height), (int)(flapArcSize * width), (int)(flapArcSize * height), 180f, 90f);
        path.AddArc((int)((baseSide + flapWidth) * width), (int)((topDisance - flapHeight) * height), (int)(flapArcSize * width), (int)(flapArcSize * height), 270f, 90f);
        path.AddLines(new PointF[] { new((baseSide + flapWidth + flapArcSize) * width, topDisance * height) });

        graphics.FillPath(new SolidBrush(drawColor), path);

        return imageToEddit;
    }
    #endregion
}
