using System.Drawing;

namespace DisplayPadPlus.DeviceSystem.Visuals;
public class Background
{
    internal readonly bool UseShadowCaster = false;
    internal ScreenData backGroundData = new();

    public Background(bool useShadowCaster = false)
    {
        UseShadowCaster = useShadowCaster;
    }



    public Background(Color color, bool useShadowCaster = true)
    {
        UseShadowCaster = useShadowCaster;

        for (int i = 0; i < backGroundData.Data.Length; i += 3)
        {
            backGroundData.Data[i + 0] = color.B;
            backGroundData.Data[i + 1] = color.G;
            backGroundData.Data[i + 2] = color.R;
        }
    }



    public Background(string path, bool useShadowCaster = true)
    {
        UseShadowCaster = useShadowCaster;

        using Bitmap resizedBmp = new(new Bitmap(path), DisplayConsts.DisplaySizeX, DisplayConsts.DisplaySizeY);

        for (int x = 0; x < resizedBmp.Width; x++)
            for (int y = 0; y < resizedBmp.Height; y++)
            {
                int i = (y * DisplayConsts.DisplaySizeX + x) * 3;
                Color pixel = resizedBmp.GetPixel(x, y);
                backGroundData.Data[i + 0] = pixel.B;
                backGroundData.Data[i + 1] = pixel.G;
                backGroundData.Data[i + 2] = pixel.R;
            }
    }



    public Background(Bitmap bmp, bool useShadowCaster = true)
    {
        UseShadowCaster = useShadowCaster;

        using Bitmap resizedBmp = new(bmp, DisplayConsts.DisplaySizeX, DisplayConsts.DisplaySizeY);

        for (int x = 0; x < resizedBmp.Width; x++)
            for (int y = 0; y < resizedBmp.Height; y++)
            {
                int i = (y * DisplayConsts.DisplaySizeX + x) * 3;
                Color pixel = resizedBmp.GetPixel(x, y);
                backGroundData.Data[i + 0] = pixel.B;
                backGroundData.Data[i + 1] = pixel.G;
                backGroundData.Data[i + 2] = pixel.R;
            }
    }
}
