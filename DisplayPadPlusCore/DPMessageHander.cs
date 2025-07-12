// Decompiled with JetBrains decompiler
// Type: DisplayPad.SDK.Helper.DPMessageHander
// Assembly: DisplayPad.SDK, Version=1.0.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 7DCCFAF8-4DB1-4F98-9EC2-478463B0B913
// Assembly location: C:\Users\Unkreativ\.nuget\packages\displaypad.sdk\1.0.6\lib\net6.0-windows7.0\DisplayPad.SDK.dll
// XML documentation location: C:\Users\Unkreativ\.nuget\packages\displaypad.sdk\1.0.6\lib\net6.0-windows7.0\DisplayPad.SDK.xml

#nullable disable
namespace DisplayPadPlusCore
{
    internal class DPMessageHander : NativeWindow
    {
        public DPMessageHander() => this.CreateHandle(new CreateParams());

        protected override void WndProc(ref Message m)
        {
            Console.WriteLine("msg: " + m.Msg);
            if (m.Msg == 21505)
            {
                IntPtr num = m.LParam;
                int int32_1 = num.ToInt32();
                num = m.WParam;
                int int32_2 = num.ToInt32();
                DisplayPadHelper.PlugCallback(1, int32_1, int32_2);
            }
            else if (m.Msg == 21506)
            {
                IntPtr num = m.LParam;
                int int32_3 = num.ToInt32();
                num = m.WParam;
                int int32_4 = num.ToInt32();
                DisplayPadHelper.ProgressCallback(2, int32_3, int32_4);
            }
            else if (m.Msg == 16 || m.Msg != 25600)
                ;
            base.WndProc(ref m);
        }
    }
}
