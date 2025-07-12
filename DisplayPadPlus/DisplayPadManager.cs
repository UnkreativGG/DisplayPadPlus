using DisplayPadPlus.DeviceSystem;
using DisplayPadPlus.DeviceSystem.Visuals;
using DisplayPadPlusCore;

namespace DisplayPadPlus;
public class DisplayPadManager
{
    static readonly List<Device> ReserveDevices = [];
    static readonly Dictionary<int, DisplayPad> DisplayPads = [];


    public static Background GlobalDefaultBackground { get; set; } = new();
    public static ImageSettings GlobalDefaultImageSettings { get; set; } = new();


    public static void AddDevice<THomeLayer>() where THomeLayer : Layer, new()
    {
        Device device = new();
        device.SetHomeLayer<THomeLayer>();
        ReserveDevices.Add(device);
    }


    public static void Finish()
    {
        _ = Helper;
        SetUpEvents();
        Assign();
    }

    private static void Assign()
    {
        new Thread(GivASecAndAssign)
        {
            IsBackground = true
        }
        .Start();
    }

    private static void GivASecAndAssign()
    {
        Thread.Sleep(1000);
        AssignDevices();
    }


    private static void AssignDevices()
    {
        if (ReserveDevices.Count == 0 || DisplayPads.Count == 0)
            return;
        else if (ReserveDevices.Count == 1 && DisplayPads.Count == 1)
        {
            Device device = ReserveDevices[0];
            if (device.AssignedDisplayPad == null)
            {
                KeyValuePair<int, DisplayPad> idAndDisplayPad = DisplayPads.First();
                idAndDisplayPad.Value.SetAssignedDevice(device);
            }

        }
    }



    static DisplayPadHelper? helper;
    static DisplayPadHelper Helper => helper ??= new();


    private static void SetUpEvents()
    {
        DisplayPadHelper.DisplayPadPlugCallBack += DisplayPadHelper_DisplayPadPlugCallBack;
        DisplayPadHelper.DisplayPadKeyCallBack += DisplayPadHelper_DisplayPadKeyCallBack;
        DisplayPadHelper.DisplayPadProgressCallBack += DisplayPadHelper_DisplayPadProgressCallBack;
    }


    static void DisplayPadHelper_DisplayPadPlugCallBack(int Status, int DeviceId)
    {
        Console.WriteLine("Device status: " + Status + " for Device Id: " + DeviceId);
        if (Status == 0)
        {
            if (DisplayPads.TryGetValue(DeviceId, out DisplayPad? displayPad))
            {
                DisplayPads.Remove(DeviceId);
                Device device = displayPad.GetAssignedDevice();
                displayPad.SetAssignedDevice(null);
                ReserveDevices.Add(device);
            }
        }
        else if (Status == 1)
        {
            DisplayPads.Add(DeviceId, new DisplayPad((uint)DeviceId));
        }



        bool PlugSatus = Helper.DisplayPadIsDevicePlug(DeviceId);
    }

    static void DisplayPadHelper_DisplayPadKeyCallBack(int KeyMatrix, int iPressed, int DeviceID)
    {
        int btnId = ((KeyMatrix + 1) / 9) - 1;
        if (btnId == 13) btnId = 11;
        bool pressed = iPressed == 1;

        Console.WriteLine("Device Button clicked. DeviceId: " + DeviceID);
        Console.WriteLine(" - matrix " + KeyMatrix + " id: " + btnId);
        Console.WriteLine(" - iPressed " + iPressed + " pressed: " + pressed);

        //Console.WriteLine("Key status: " + pressed + " at Key: " + btnId + " for Device Id: " + DeviceID);

        if (DisplayPads.TryGetValue(DeviceID, out DisplayPad? displayPad))
            displayPad.GetAssignedDevice().ReseaveKeyPress(btnId, pressed);
    }

    static void DisplayPadHelper_DisplayPadProgressCallBack(int Percentage)
    {
        Console.WriteLine("Device firmware update Progress status: " + Percentage);
    }
}
