using DisplayPadPlus.DeviceSystem;

namespace DisplayPadPlus
{
    internal class DisplayPad
    {
        internal readonly uint DeviceId;

        private Device? AssignedDevice;

        internal Device GetAssignedDevice()
            => AssignedDevice ?? throw new Exception("Tryed to");

        internal void SetAssignedDevice(Device device)
        {
            AssignedDevice = device;
            AssignedDevice.AssignedDisplayPad = this;
            AssignedDevice.LoadHomeLayer();
        }


        public DisplayPad(uint deviceId)
        {
            DeviceId = deviceId;
        }
    }
}
