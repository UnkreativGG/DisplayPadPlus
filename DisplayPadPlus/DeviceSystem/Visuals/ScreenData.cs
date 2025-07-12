namespace DisplayPadPlus.DeviceSystem.Visuals;
internal class ScreenData
{
    internal byte[] Data;

    public ScreenData()
    {
        Data = new byte[DisplayConsts.ScreenDataBufferSize];
    }
}
