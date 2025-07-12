using DisplayPadPlus.DeviceSystem.Visuals;
using DisplayPadPlusCore;
using System.Drawing;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DisplayPadPlus.DeviceSystem;
public class Device
{
    Layer HomeLayer;
    Layer CurrentLayer;

    internal DisplayPad AssignedDisplayPad;

    Background? defaultBackground;
    public Background DefaultBackground
    {
        get => defaultBackground ?? DisplayPadManager.GlobalDefaultBackground;
        set => defaultBackground = value;
    }

    ImageSettings? defaultImageSettings;
    public ImageSettings DefaultImageSettings
    {
        get => defaultImageSettings ?? DisplayPadManager.GlobalDefaultImageSettings;
        set => defaultImageSettings = value;
    }

    internal Device() { }

    internal void LoadHomeLayer()
        => LoadLayer(HomeLayer);

    internal void LoadLayer(Layer layerToLoad)
    {
        CurrentLayer = layerToLoad;
        if (AssignedDisplayPad is null)
            return;

        CLog($"load layer {layerToLoad.GetType().Name}");
        UpdatePannelImage(CurrentLayer.LayerBackground.Data.GetAdr());
    }



    internal void UpdateIcon(DisplayIcon icon, IconData data)
    {
        if (icon.Layer != CurrentLayer)
        {
            CLog("not selected Layer");
            return;
        }

        Rectangle rect = DisplayConsts.GetButtonPos(icon.GetPos());
        UpdatePannelImage(data.Data.GetAdr(), (uint)rect.Left, (uint)rect.Top, DisplayConsts.DisplaySizeX - (uint)rect.Right - 1, DisplayConsts.DisplaySizeY - (uint)rect.Bottom - 1);
    }


    void UpdatePannelImage(nint pdyData, uint left = 0, uint top = 0, uint right = DisplayConsts.DisplaySizeX - 1, uint bottom = DisplayConsts.DisplaySizeY - 1)
    {
        DisplayPadSDKPublic.SetPanelImage(AssignedDisplayPad.DeviceId, pdyData, left , top, right, bottom);
    }


    internal void ReseaveKeyPress(int key, bool down)
    {
        //CLog($"button pressed: {key}, down: {down}");
        CurrentLayer.ReseaveKeyPress(key, down);
    }


    internal void SetHomeLayer<THomeLayer>() where THomeLayer : Layer, new()
    {
        HomeLayer = Layer.CreateLayer<THomeLayer>(this);
    }


    internal void CLog(string message)
    {
        Console.WriteLine($"DeviceId: {AssignedDisplayPad.DeviceId}. {message}");
    }
}
