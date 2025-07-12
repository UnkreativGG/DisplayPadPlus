// Decompiled with JetBrains decompiler
// Type: DisplayPad.SDK.Helper.DisplayPadMessagePumpManager
// Assembly: DisplayPad.SDK, Version=1.0.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 7DCCFAF8-4DB1-4F98-9EC2-478463B0B913
// Assembly location: C:\Users\Unkreativ\.nuget\packages\displaypad.sdk\1.0.6\lib\net6.0-windows7.0\DisplayPad.SDK.dll
// XML documentation location: C:\Users\Unkreativ\.nuget\packages\displaypad.sdk\1.0.6\lib\net6.0-windows7.0\DisplayPad.SDK.xml

using System.Runtime.InteropServices;

#nullable disable
namespace DisplayPadPlusCore
{
    public class DisplayPadMessagePumpManager : IDisposable
    {
        private readonly Thread messagePump;
        private AutoResetEvent messagePumpRunning = new AutoResetEvent(false);
        private DisplayPadSDK.KEY_CALLBACK eventCallBack = (DisplayPadSDK.KEY_CALLBACK)null;
        private GCHandle gch;
        private IntPtr myHandle;
        private static DisplayPadMessagePumpManager instance;

        internal static DisplayPadMessagePumpManager Instance
        {
            get
            {
                if (DisplayPadMessagePumpManager.instance == null)
                    DisplayPadMessagePumpManager.instance = new DisplayPadMessagePumpManager();
                return DisplayPadMessagePumpManager.instance;
            }
        }

        private DisplayPadMessagePumpManager()
        {
            try
            {
                this.messagePump = new Thread(new ThreadStart(this.DisplayPadRunMessagePump))
                {
                    Name = nameof(DisplayPadMessagePumpManager)
                };
                this.messagePump.Start();
                this.messagePumpRunning.WaitOne();
            }
            catch (Exception ex1)
            {
                try
                {
                    this.messagePump.Abort();
                }
                catch (Exception ex2)
                {
                    this.messagePump = (Thread)null;
                    GC.SuppressFinalize((object)this);
                    this.messagePump = new Thread(new ThreadStart(this.DisplayPadRunMessagePump))
                    {
                        Name = nameof(DisplayPadMessagePumpManager)
                    };
                    this.messagePump.Start();
                    this.messagePumpRunning.WaitOne();
                }
            }
        }

        private void DisplayPadRunMessagePump()
        {
            DPMessageHander dpMessageHander = new DPMessageHander();
            this.myHandle = dpMessageHander.Handle;
            this.eventCallBack = new DisplayPadSDK.KEY_CALLBACK(this.CallBack);
            this.gch = GCHandle.Alloc((object)this.eventCallBack);
            DisplayPadSDK.SetKeyCallBack(this.eventCallBack);
            Logger.LogMessage("Display pad Message Pump Thread Started: " + DisplayPadSDK.OpenUSBDriver(dpMessageHander.Handle).ToString());
            this.messagePumpRunning.Set();
            Application.Run();
        }

        internal void CallBack(ushort wMatrix, bool bPressed, uint ID)
        {
            try
            {
                int int32 = Convert.ToInt32(bPressed);
                DisplayPadHelper.KeyCallback((int)wMatrix, int32, (int)ID);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "KeyCallBack");
            }
        }

        public void Dispose()
        {
            try
            {
                this.messagePump.Abort();
            }
            catch
            {
            }
            GC.SuppressFinalize((object)this);
        }

        ~DisplayPadMessagePumpManager() => this.Dispose();
    }
}
