using DisplayPadPlus.DeviceSystem.Actions;
using DisplayPadPlus.DeviceSystem.Visuals;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DisplayPadPlus.DeviceSystem;
public class DisplayIcon
{
    internal readonly Layer Layer;


    internal ASimpleAction? action;
    internal ASimpleAction? Action
    {
        get { return action; }
        set
        {
            if (action != null)
                action.Icon = null;

            action = value;

            if (action != null)
                action.Icon = this;
        }
    }



    public DisplayIcon SetAction(ASimpleAction action)
    {
        if (action is Layer)
            throw new Exception("Use SetAsLayer<TLayer>() to register a Layer");

        Action = action;
        return this;
    }

    public DisplayIcon SetAsLayer<TLayer>() where TLayer : Layer, new()
    {
        Action = Layer.CreateLayer<TLayer>(this);
        return this;
    }


    internal DisplayIcon(Layer layer)
    {
        Layer = layer;
    }




    internal void KeyUpdate(bool down)
    {
        if (down)
            Action?.Down();
        else
            Action?.Up();
    }



    internal int GetPos()
    {
        for (int i = 0; i < Layer.Icons.Length; i++)
            if (Layer.Icons[i] == this)
                return i;

        throw new Exception("Icon not Linked to the parent layer anymore.");
    }


    #region Visual


    internal bool ImageOverriden = false;
    internal readonly IconData CurrentDisplayIconData = new();



    internal void PlayAnimation(IconAnimation animation, bool updateLayerDataWithLastFrame = false)
    {
        if (ImageOverriden)
            return;

        if (animation.Frames.Length == 0) 
            return;

        DateTime start = DateTime.Now;
        TimeSpan timeBetweenFrames = animation.Duration / animation.Frames.Length;

        for (int i = 0; i < animation.Frames.Length - 1; i++)
        {
            UpdateImage(animation.Frames[i]);
            TimeSpan toSleep = (DateTime.Now - start) + ((i + 1) * timeBetweenFrames);
            Thread.Sleep(toSleep);
        }

        UpdateImage(animation.Frames[^1], updateLayerDataWithLastFrame);
    }


    public void UpdateImage(IconData data, bool updateLayerData = false)
    {/*
        if (ImageOverriden)
            return;*/
        
        Layer.GetDevice().UpdateIcon(this, data);

        if (updateLayerData)
            UpdateLayerData(data);
    }


    internal void UpdateLayerData()
        => UpdateLayerData(CurrentDisplayIconData);

    internal void UpdateLayerData(IconData data)
    {
        ScreenData LayerBackground = Layer.LayerBackground;
        Rectangle pos = DisplayConsts.GetButtonPos(GetPos());

        for (int x = 0; x < pos.Width; x++)
            for (int y = 0; y < pos.Height; y++)
            {
                int screenPos = ((pos.Y + y) * DisplayConsts.DisplaySizeX + pos.X + x) * 3;
                int iconPos = (y * DisplayConsts.IconSize + x) * 3;
                LayerBackground.Data[screenPos + 0] = data.Data[iconPos + 0];
                LayerBackground.Data[screenPos + 1] = data.Data[iconPos + 1];
                LayerBackground.Data[screenPos + 2] = data.Data[iconPos + 2];
            }
    }


    public DisplayIcon OverwriteImageWithText(string text)
    {
        Bitmap bmp = new(DisplayConsts.IconSize, DisplayConsts.IconSize);

        using Graphics graphics = Graphics.FromImage(bmp);
        graphics.Clear(Color.Transparent);

        GraphicsPath path = new();

        int height = bmp.Height;
        int width = bmp.Width;

        const float topDisance = 0.1f;
        const float bottomDisance = 0.1f;
        const float sideDisance = 0.1f;

        Color color = Layer.ImageSettings.SecundaryColor;

        RectangleF rec = new(
            sideDisance * width,
            topDisance * height,
            (1 - sideDisance * 2) * width,
            (1 - topDisance - bottomDisance) * height);

        FontFamily fontFamily = Layer.ImageSettings.DefaultFontFamily;

        StringFormat stringFormat = new()
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center,
        };

        path.AddString(text, fontFamily, 0, 15, rec, stringFormat);

        graphics.FillPath(new SolidBrush(color), path);

        OverwriteImageWithImage(bmp);

        return this;
    }
    public DisplayIcon OverwriteImageWithPath(string path)
    {
        OverwriteImageWithImage(new Bitmap(path));
        return this;
    }
    public DisplayIcon OverwriteImageWithImage(Bitmap image)
    {
        ImageOverriden = true;

        if (image.Height != DisplayConsts.IconSize || image.Width != DisplayConsts.IconSize)
            image = new(image, DisplayConsts.IconSize, DisplayConsts.IconSize);

        CalculateWithBackGround(image, CurrentDisplayIconData);

        return this;
    }



    internal void CalculateWithBackGround(Bitmap img, IconData iconData)
    {
        Rectangle area = DisplayConsts.GetButtonPos(GetPos());
        Background background = Layer.Background;
        ScreenData backgroundData = background.backGroundData;

        CopyBackGround(iconData, area, backgroundData);
        if (background.UseShadowCaster)
            CastShadow(iconData, img);
        CopyImage(iconData, img);
    }




    #region image proccessing



    static void CopyBackGround(IconData iconDataToWorkWith, Rectangle area, ScreenData bgData)
    {
        for (int x = 0; x < DisplayConsts.IconSize; x++)
            for (int y = 0; y < DisplayConsts.IconSize; y++)
            {
                int pos = ((DisplayConsts.IconSize * y) + x) * 3;
                int bgPos = ((area.Y + y) * DisplayConsts.DisplaySizeX + area.X + x) * 3;

                iconDataToWorkWith.Data[pos + 0] = bgData.Data[bgPos + 0];
                iconDataToWorkWith.Data[pos + 1] = bgData.Data[bgPos + 1];
                iconDataToWorkWith.Data[pos + 2] = bgData.Data[bgPos + 2];
            }
    }



    static void CastShadow(IconData iconDataToWorkWith, Bitmap image)
    {
        const int start = 4;
        const int end = 10;

        byte[] coppy = new byte[iconDataToWorkWith.Data.Length];
        iconDataToWorkWith.Data.CopyTo(coppy, 0);

        float[][] distanceMatrix = new float[end * 2 + 1][];
        for (int dx = -end; dx <= end; dx++)
        {
            float[] distanceMatrixStribe = new float[end * 2 + 1];

            for (int dy = -end; dy <= end; dy++)
                distanceMatrixStribe[dy + end] = MathF.Abs(MathF.Sqrt(dx * dx + dy * dy));

            distanceMatrix[dx + end] = distanceMatrixStribe;
        }

        // mit wenn factor größer einschrenken
        float[][] transparencyFactorMatrix = new float[DisplayConsts.IconSize][];
        for (int x = 0; x < transparencyFactorMatrix.Length; x++)
        {
            float[] stribe = new float[DisplayConsts.IconSize];

            for (int y = 0; y < stribe.Length; y++)
                stribe[y] = 2;

            transparencyFactorMatrix[x] = stribe;
        }


        for (int x = 0; x < DisplayConsts.IconSize; x++)
            for (int y = 0; y < DisplayConsts.IconSize; y++)
            {
                Color c = image.GetPixel(x, y);

                if (c.A != 0)
                {
                    // that way, all pixels at side get calc? 
                    // check diagonal pixels?
                    bool ignoreLeft_ = x == 0 || image.GetPixel(x - 1, y).A != 0;
                    bool ignoreRight = x == image.Width - 1 || image.GetPixel(x + 1, y).A != 0;
                    bool ignoreUp___ = y == 0 || image.GetPixel(x, y - 1).A != 0;
                    bool ignoreDown_ = y == image.Height - 1 || image.GetPixel(x, y + 1).A != 0;

                    // if flat line, optimiceable
                    int xStart = ignoreLeft_ ? 0 : -end;
                    int xEnd = ignoreRight ? 0 : end;
                    int yStart = ignoreUp___ ? 0 : -end;
                    int yEnd = ignoreDown_ ? 0 : end;

                    int dxS = x + xStart;
                    if (dxS < 0)
                        xStart -= dxS;

                    int dxE = x + xEnd - DisplayConsts.IconSize + 1;
                    if (dxE > 0)
                        xEnd -= dxE;

                    int dyS = y + yStart;
                    if (dyS < 0)
                        yStart -= dyS;

                    int dyE = y + yEnd - DisplayConsts.IconSize + 1;
                    if (dyE > 0)
                        yEnd -= dyE;

                    for (int dx = xStart; dx <= xEnd; dx++)
                        for (int dy = yStart; dy <= yEnd; dy++)
                        {
                            float dist = distanceMatrix[dx + end][dy + end];
                            int pos = ((DisplayConsts.IconSize * (y + dy)) + (x + dx)) * 3;

                            if (dist <= start)
                            {
                                //imgWS.SetPixel(x + dx + end, y + dy + end, Color.Black);
                                iconDataToWorkWith.Data[pos + 0] = 0;
                                iconDataToWorkWith.Data[pos + 1] = 0;
                                iconDataToWorkWith.Data[pos + 2] = 0;
                                transparencyFactorMatrix[x + dx][y + dy] = 0;
                            }
                            else if (dist <= end)
                            {
                                float fact = (dist - start) / (end - start);
                                //Color nc = Color.FromArgb((int)(255 - fact * 255), 0, 0, 0);
                                //if (imgWS.GetPixel(x + dx + end, y + dy + end).A < nc.A)
                                //    imgWS.SetPixel(x + dx + end, y + dy + end, nc);
                                if (fact < transparencyFactorMatrix[x + dx][y + dy])
                                {
                                    iconDataToWorkWith.Data[pos + 0] = (byte)(coppy[pos + 0] * fact + Color.Black.B * (1 - fact));
                                    iconDataToWorkWith.Data[pos + 1] = (byte)(coppy[pos + 1] * fact + Color.Black.G * (1 - fact));
                                    iconDataToWorkWith.Data[pos + 2] = (byte)(coppy[pos + 2] * fact + Color.Black.R * (1 - fact));
                                    transparencyFactorMatrix[x + dx][y + dy] = fact;
                                }
                            }
                        }
                }
            }
    }



    static void CopyImage(IconData iconDataToWorkWith, Bitmap image)
    {
        for (int x = 0; x < image.Width; x++)
            for (int y = 0; y < image.Height; y++)
            {
                Color c = image.GetPixel(x, y);
                if (c.A != 0)
                {
                    int pos = ((DisplayConsts.IconSize * y) + x) * 3;
                    iconDataToWorkWith.Data[pos + 0] = c.B;
                    iconDataToWorkWith.Data[pos + 1] = c.G;
                    iconDataToWorkWith.Data[pos + 2] = c.R;
                }
            }
    }



    #endregion
    #endregion
}
