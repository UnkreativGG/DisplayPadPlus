namespace DisplayPadPlus.DeviceSystem.Visuals;
public class IconData
{
    internal byte[] Data;

    public IconData()
    {
        Data = new byte[DisplayConsts.IconDataBufferSize];
    }
}
