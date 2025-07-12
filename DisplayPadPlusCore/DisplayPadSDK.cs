// Decompiled with JetBrains decompiler
// Type: DisplayPad.SDK.DisplayPadSDK
// Assembly: DisplayPad.SDK, Version=1.0.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 7DCCFAF8-4DB1-4F98-9EC2-478463B0B913
// Assembly location: C:\Users\Unkreativ\.nuget\packages\displaypad.sdk\1.0.6\lib\net6.0-windows7.0\DisplayPad.SDK.dll
// XML documentation location: C:\Users\Unkreativ\.nuget\packages\displaypad.sdk\1.0.6\lib\net6.0-windows7.0\DisplayPad.SDK.xml

using System.Runtime.InteropServices;

#nullable disable
namespace DisplayPadPlusCore
{
    public class DisplayPadSDK
    {
        internal const int WM_USER = 1024;
        internal const int FW_SECTOR_NUM = 80;
        internal const int FW_SECTOR_MAX_LENGTH = 4096;
        internal const int MAX_DEV_COUNT = 10;
        internal const int FW_NUM_PROFILE = 5;
        internal const int FW_NUM_KEY = 12;
        internal const int FW_ALL_EVT_NOTIFY = 12;
        internal const int FW_MAX_BMP_NUM = 86;
        internal const int MAX_KEYBOARD_EVENTS = 1000;
        internal const int NUM_FW_ICON_PACKET = 31;
        internal const int WM_DEVICE_PLUG = 21505;
        internal const int WM_FW_PROGRESS = 21506;
        internal const uint WM_KEY_STATUS = 25600;
        internal const uint WM_CLOSE = 16;
        internal static int iDeviceID = 0;
        /// <summary>contains connected device ids</summary>
        internal static List<int> lstDeviceID = new List<int>();

        [DllImport("user32.dll")]
        internal static extern int PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        /// <summary>Get SDK Dll's Version</summary>
        /// <returns>DLL's Version</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern int GetDLLVersion();

        /// <summary>Initial the device and detect the plug/ unplug</summary>
        /// <param name="handle">the handle for notification</param>
        /// <returns>true plugged in，false not plugged in</returns>
        /// 
        ///             When the device is inserted, removed or suspended, the SDK will send WM_DEVICE_PLUG to the hNotifyhWnd.
        ///             Please process suspend job within 3 seconds, when you receive the suspended notification.
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool OpenUSBDriver(IntPtr handle);

        /// <summary>Close the device's control</summary>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern void CloseUSBDriver();

        /// <summary>Get the count of inserted DKDs.</summary>
        /// <param name="iDevCount">return the count.</param>
        /// <returns>true is success, false is fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool GetDevCount(ref int iDevCount);

        /// <summary>Confirm whether the firmware is being updated.</summary>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true is updated, false is not updated</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool IsUpdating(uint ID);

        /// <summary>Start FW update (open update thread)</summary>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <param name="fwInfo"></param>
        /// <returns></returns>
        /// 
        ///             Use hNotifyhWnd of “OpenUSBDriver” to PostMessage “WM_FW_PROGRESS
        ///             WParam value is the target's ID
        ///             LPARAM lParam : The percent of progress
        ///             If percent is -1 : Failed to update FW
        ///             IF percent value is 100 : Success to update FW
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool StartFWUpdate(uint ID, DisplayPadSDK.FWBinUpdateInfo fwInfo);

        /// <summary>Reboot the FW</summary>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true Success，false Fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool ResetDevice(uint ID);

        /// <summary>Get version-related information</summary>
        /// <param name="devInfo">return the DeviceInfo</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true Success，false Fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool GetDeviceInfo(ref DisplayPadSDK.DevInfo devInfo, uint ID);

        /// <summary>Get FW version number</summary>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>FW Version number</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern ushort GetDevAppVer(uint ID);

        /// <summary>Check if the device is plugged in</summary>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true plugged in，false not plugged in</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        [return: MarshalAs((UnmanagedType)3)]
        internal static extern bool IsDevicePlug(uint ID);

        /// <summary>Get the FWInfo data</summary>
        /// <param name="fwInfo">the struct is saved about the current FW Info</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true Success，false Fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool GetFWInfo(ref DisplayPadSDK.FWInfo fwInfo, uint ID);

        /// <summary>Enable/Disale the SW's control</summary>
        /// <param name="bEnable">If true, all are controlled by SW, otherwise some functions are con controlled by FW</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true Success，false Fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool APEnable(bool bEnable, uint ID);

        /// <summary>Get the number of BMP spaces remaining in the FW</summary>
        /// <param name="bySurplusNum">return the number of BMP that can be used.</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true Success，false Fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool CalculateBMP(ref byte bySurplusNum, uint ID);

        /// <summary>
        /// Only Used when apenable is true. Sets the button image to display.
        /// </summary>
        /// <param name="byIconNumber">the index of button.</param>
        /// <param name="pData">the data of picture (BGR888 format)</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true Success，false Fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool SetIconPacket(
          byte byIconNumber,
          ref DisplayPadSDK.PicPacketInfo pData,
          uint ID);

        /// <summary>Sets the button image to display.</summary>
        /// <param name="picInfo">Tell FW to refresh how to refresh the target image.</param>
        /// <param name="pbyData">the data of picture (BGR888 format)</param>
        /// <param name="iDataLen">the length of data</param>
        /// <param name="bCopyBuffer">If the value is true, the function will create a new buffer for the pbyData in the function.</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true Success，false Fail</returns>
        /// 
        ///             if the BMP amount is not enough in FW, it will return false.
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool SetIconPic(
          DisplayPadSDK.DisplayPicInfo picInfo,
          IntPtr pbyData,
          int iDataLen = 31212,
          bool bCopyBuffer = false,
          uint ID = 0);

        /// <summary>Set the time for TFT-LCD to go to sleep</summary>
        /// <param name="byStatus">Sleep function switch. 0 is Off. 1 is On.</param>
        /// <param name="byHH">hour, 0~24</param>
        /// <param name="byMM">min, 0~59</param>
        /// <param name="bySS">sec, 0~59</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true Success，false Fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool SetTFTSleepTime(
          byte byStatus,
          byte byHH,
          byte byMM,
          byte bySS,
          uint ID);

        /// <summary>Switch the Led's Brightness</summary>
        /// <param name="byBrightness">LCM Brightness 0 = 0%, 25 = 25%, 50 = 50%, 75 = 75%, 100 = 100%</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true Success，false Fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool SetMainBrightness(byte byBrightness, uint ID);

        /// <summary>Enable/Disable the custom keys function for keyboard</summary>
        /// <param name="bOn">if true, the function is ON, else the function is  Off.</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true Success，false Fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool EnableKeyFunc(bool bOn, uint ID);

        /// <summary>Set SyncAcross Profile's Status</summary>
        /// <param name="bEnable">0: Disable, 1:Eanble</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true Success，false Fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool SetSyncAcrossProfiles(bool bEnable, uint ID);

        /// <summary>Get the SyncAcross Profile's Status</summary>
        /// <param name="bEnable">return value, 0: Disable, 1:Eanble</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true Success，false Fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool GetSyncAcrossProfiles(ref bool bEnable, uint ID);

        /// <summary>Reset the profiles in Flash.</summary>
        /// <param name="bAll">if  0, reset the current profile, else reset the all profiles.</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true Success，false Fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool ResetFlash(bool bAll, uint ID);

        /// <summary>Reset the button of the current profile.</summary>
        /// <param name="byProfile">Target profile.</param>
        /// <param name="byBtnIndex">Target button.</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true Success，false Fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool ResetBtnPic(byte byProfile, byte byBtnIndex, uint ID);

        /// <summary>
        /// Reset the all buttons's pictures of the current profile.
        /// </summary>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true Success，false Fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool ResetPicture(uint ID);

        /// <summary>Restore the key Assignment to default values.</summary>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true Success，false Fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool ResetKeys(uint ID);

        /// <summary>Clear the specified sector's data( Clear to 0xFF)</summary>
        /// <param name="iStart">The Start Index</param>
        /// <param name="iEnd">The End Index</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true Success，false Fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool EraseSectorMem(int iStart, int iEnd, uint ID);

        /// <summary>
        /// Write data to the specified sector
        /// Note - In most cases, the same address needs to be cleared before repeated writing.
        /// </summary>
        /// <param name="iSectorIndex">The sector Index</param>
        /// <param name="iAddress">The address index in the sector</param>
        /// <param name="iLength">The data length</param>
        /// <param name="pData">The pointer of data contents</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true Success，false Fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool WriteSectorMem(
          int iSectorIndex,
          int iAddress,
          int iLength,
          IntPtr pData,
          uint ID);

        /// <summary>Read data to the specified sector</summary>
        /// <param name="iSectorIndex">The sector Index</param>
        /// <param name="iAddress">The address index in the sector</param>
        /// <param name="iLength">The data length</param>
        /// <param name="pData">pointer to the data contents of the returned data</param>
        /// <param name="iInterval">FW packet transmission speed</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true Success，false Fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool ReadSectorMemory(
          int iSectorIndex,
          int iAddress,
          int iLength,
          IntPtr pData,
          int iInterval = 2,
          uint ID = 0);

        /// <summary>Switch the current profile and current effect.</summary>
        /// <param name="iProfileNo">switch to the target profile(Profile Index 1~5)</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true Success，false Fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool SwitchProfile(int iProfileNo, uint ID);

        /// <summary>Change the key to other custom functions.</summary>
        /// <param name="iSource">source keys</param>
        /// <param name="iTarget">key function</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true Success，false Fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool ChangeKey(int iSource, int iTarget, uint ID);

        /// <summary>Change the key to other custom functions.</summary>
        /// <param name="iSource">source keys</param>
        /// <param name="iTarget">Standard key function (0x01~0xFE see.Key Table)</param>
        /// <param name="iControlKey"> Enable is 1
        /// bit [0] = Ctrl
        /// bit[1] = Shift
        /// bit[2] = Alt
        /// bit[3] = GUI(Win)</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true Success，false Fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool ChangeShortcutKey(
          int iSource,
          int iTarget,
          int iControlKey,
          uint ID);

        /// <summary>Change the key to the launch functions.</summary>
        /// <param name="iSource">source keys</param>
        /// <param name="iDataLen">data's length</param>
        /// <param name="pData">Save the SW information data to FW.</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true Success，false Fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool SetLaunchContent(int iSource, int iDataLen, [MarshalAs((UnmanagedType)42)] byte[] pData, uint ID);

        /// <summary>Change the key to the launch functions.</summary>
        /// <param name="iSrc">source keys</param>
        /// <param name="iType">data's length</param>
        /// <param name="iTimes">Execution times.</param>
        /// <param name="iEventLen">event counts</param>
        /// <param name="pData">the contents array of event.</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true Success，false Fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool SetFullMacroData(
          int iSrc,
          int iType,
          int iTimes,
          int iEventLen,
          [MarshalAs((UnmanagedType)42)] DisplayPadSDK.KEYMAP_EVENT[] pData,
          uint ID);

        /// <summary>Read the settings of the macro key.</summary>
        /// <param name="iFWProfile">source Profile</param>
        /// <param name="iMacroIndex">source keys</param>
        /// <param name="macroContent">MacroContent array, return the Macro's Contents</param>
        /// <param name="iInterval"></param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true Success，false Fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool GetMacroContent(
          int iFWProfile,
          int iMacroIndex,
          ref DisplayPadSDK.MacroContent macroContent,
          int iInterval = 2,
          uint ID = 0);

        /// <summary>When the key of the keyboard is pressed, notify SW.</summary>
        /// <param name="callback">callback Fucntion</param>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern void SetKeyCallBack(DisplayPadSDK.KEY_CALLBACK callback);

        /// <summary>
        /// Get the number of BMP spaces remaining in the FW.
        /// Note: When FW is saving bmp and the power is suddenly turned off, an error may occurs in the bmp storage.
        /// </summary>
        /// <param name="byRestoreCount">Returns the count of repaired BMPs</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true Success，false Fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool CheckBMPStorage(ref byte byRestoreCount, uint ID);

        /// <summary>
        /// Get the count of BMP used by target's button in the target's profile in the FW
        /// </summary>
        /// <param name="byProfile">The target's profile</param>
        /// <param name="byBtnIndex">The target's button</param>
        /// <param name="byLength">Return the count of  BMP used by button.</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true Success，false Fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool GetBtnBMPLength(
          byte byProfile,
          byte byBtnIndex,
          ref byte byLength,
          uint ID);

        /// <summary>Get the brightness of Led</summary>
        /// <param name="byBrightness">Profile 's Brightness 0=0%, 25=25%, 50=50%, 75=75%, 100=100%</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true Success，false Fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool GetMainBrightness(ref byte byBrightness, uint ID);

        /// <summary>Set the time for TFT-LCD to go to sleep</summary>
        /// <param name="byStatus">Sleep function switch. 0 is Off. 1 is On</param>
        /// <param name="byHH"> hour, 0~24</param>
        /// <param name="byMM">min, 0 ~ 59</param>
        /// <param name="bySS">sec, 0 ~ 59</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true Success，false Fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool GetTFTSleepTime(
          ref byte byStatus,
          ref byte byHH,
          ref byte byMM,
          ref byte bySS,
          uint ID);

        /// <summary>Show/Hide the logo</summary>
        /// <param name="bEnable">If true, show the logo. If false, hide the logo</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true Success，false Fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool DisplayLogo(bool bEnable, uint ID);

        /// <summary>
        /// Sets the image to be displayed in the full screen panel.
        /// </summary>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <param name="pbyData">the data of picture (BGR888 format)</param>
        /// <param name="iLeft">Left border Position</param>
        /// <param name="iTop">Top border Position</param>
        /// <param name="iRight">Right border Position</param>
        /// <param name="iBottom">Bottom border Position</param>
        /// <returns>true Success，false Fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool SetPanelImage(
          uint ID,
          IntPtr pbyData,
          uint iLeft = 0,
          uint iTop = 0,
          uint iRight = 799,
          uint iBottom = 239);

        /// <summary>
        /// Save the logo image to be displayed  in the full screen panel.
        /// </summary>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <param name="pbyData">the data of picture (BGR888 format), the size is (800 * 400 * 3)</param>
        /// <returns>true Success，false Fail</returns>
        [DllImport("DisplayPadSDK.dll", CallingConvention = (CallingConvention)2)]
        internal static extern bool SavePanelImage(uint ID, IntPtr pbyData);

        private enum DEV_STATUS
        {
            DEV_REMOVE,
            DEV_PLUG,
            DEV_SUSPEND,
        }

        private enum MACRO_REPEAT
        {
            MACRO_ONCE = 1,
            MACRO_LOOP = 2,
            MACRO_TOGGLE = 3,
            MACRO_PRESS = 4,
        }

        private enum MACRO_ACTION
        {
            ACTION_UP = 1,
            ACTION_DOWN = 2,
            ACTION_WHEEL = 3,
            ACTION_MOUSE = 4,
            ACTION_ONLY_DELAY = 12, // 0x0000000C
            ACTION_CAP_DOWN = 13, // 0x0000000D
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct PicPacket
        {
            internal byte byReportID;
            [MarshalAs((UnmanagedType)30, SizeConst = 1024, ArraySubType = (UnmanagedType)10)]
            internal byte[] byData;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct tagPicPacketInfo
        {
            private DisplayPadSDK.PicPacket[] picPacket;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct DevInfo
        {
            public ushort vid;
            public ushort pid;
            public ushort fwVer;
            public ushort bootloadVer;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct FWInfo
        {
            public ushort fwVer;
            public ushort wUndef;
            public byte sizeProfile;
            public byte byEffectModeIndex;
            public byte currentlyProfileIndex;
            public byte byEffectMenuIndex;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MacroContent
        {
            public ushort wMacroSize;
            public byte byKeyType;
            public byte byDataIndex;
            public byte byDataIndex2;
            [MarshalAs((UnmanagedType)30, SizeConst = 1000, ArraySubType = (UnmanagedType)27)]
            public DisplayPadSDK.KEYMAP_EVENT[] evt;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct KEYMAP_EVENT
        {
            public byte keyByte;
            public byte keyActionPage;
            public ushort timer;

            public byte keyAction
            {
                get => Convert.ToByte((int)this.keyActionPage & 15);
                set
                {
                    this.keyActionPage = Convert.ToByte(((int)this.keyActionPage & 240) + ((int)value & 15));
                }
            }

            public byte keyPage
            {
                get => Convert.ToByte((int)this.keyActionPage >> 4 & 15);
                set
                {
                    this.keyActionPage = Convert.ToByte(((int)this.keyActionPage & 15) + (((int)value & 15) << 4));
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct FWBinUpdateInfo
        {
            internal IntPtr pBinImage1;
            internal uint dwBinLength1;
            internal byte byTargetDev;
            internal uint uFWVersion;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct FWBinUpdateInfo_byte
        {
            public byte[] pBinImage1;
            public uint dwBinLength1;
            public byte byTargetDev;
            public uint uFWVersion;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct PicPacketInfo
        {
            [MarshalAs((UnmanagedType)30, SizeConst = 31, ArraySubType = (UnmanagedType)27)]
            internal DisplayPadSDK.PicPacket[] picPacket;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct PicPacketInfo_byte
        {
            internal byte byIconNumber;
            [MarshalAs((UnmanagedType)30, SizeConst = 31, ArraySubType = (UnmanagedType)27)]
            internal DisplayPadSDK.PicPacket[] picPacket;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct DisplayPicInfo
        {
            internal byte byProfile;
            internal byte byIcon;
            internal byte byLeft;
            internal byte byTop;
            internal byte byRight;
            internal byte byBottom;
            internal byte byPicNum;
            internal byte byPicIndex;
            internal ushort wPicInterval;
        }

        internal struct DisplayPicInfo_byte
        {
            internal byte byProfile;
            internal byte byIcon;
            internal byte byLeft;
            internal byte byTop;
            internal byte byRight;
            internal byte byBottom;
            internal byte byPicNum;
            internal byte byPicIndex;
            internal ushort wPicInterval;
            internal byte[] imgData;
            internal int iDataLen;
            internal bool bCopyBuffer;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct LaunchContent
        {
            public int iSource;
            public int iDataLen;
            [MarshalAs((UnmanagedType)30, SizeConst = 1000, ArraySubType = (UnmanagedType)27)]
            public byte[] pData;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MacroInputContent_DisplayPad
        {
            public int iSrc;
            public int iType;
            public int iTimes;
            public int iEventLen;
            [MarshalAs((UnmanagedType)30, SizeConst = 1000, ArraySubType = (UnmanagedType)27)]
            public DisplayPadSDK.KEYMAP_EVENT[] pData;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct PicInfo_gif
        {
            internal string ImagePath;
            internal int iTargetPic;
            internal bool IsAPEnable;
            internal int iProfileIndex;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct PCInfo
        {
            internal int iTargetPic;
            internal string PCInfoType;
            internal string imageType;
        }

        /// <summary>KEY_CALLBACK</summary>
        /// <param name="wMatrix">matrix value</param>
        /// <param name="bPressed">key is pressed or released.</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void KEY_CALLBACK(ushort wMatrix, bool bPressed, uint ID);
    }
}
