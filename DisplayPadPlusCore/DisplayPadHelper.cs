// Decompiled with JetBrains decompiler
// Type: DisplayPad.SDK.DisplayPadHelper
// Assembly: DisplayPad.SDK, Version=1.0.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 7DCCFAF8-4DB1-4F98-9EC2-478463B0B913
// Assembly location: C:\Users\Unkreativ\.nuget\packages\displaypad.sdk\1.0.6\lib\net6.0-windows7.0\DisplayPad.SDK.dll
// XML documentation location: C:\Users\Unkreativ\.nuget\packages\displaypad.sdk\1.0.6\lib\net6.0-windows7.0\DisplayPad.SDK.xml

using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

#nullable disable
namespace DisplayPadPlusCore
{
    public class DisplayPadHelper
    {
        private static DisplayPadMessagePumpManager DisplayPad_instance;

        public static event DisplayPadHelper.DisplayPadStatus DisplayPadPlugCallBack;

        public static event DisplayPadHelper.DisplayPadProgressStatus DisplayPadProgressCallBack;

        public static event DisplayPadHelper.DisplayPadKeyStatus DisplayPadKeyCallBack;

        /// <summary>constructor of DisplayPadHelper class.</summary>
        public DisplayPadHelper()
        {
            DisplayPadHelper.DisplayPad_instance = DisplayPadMessagePumpManager.Instance;
        }

        /// <summary>Raised when device status is changed like plug/unplug</summary>
        /// <param name="Message">Notification signal when device is inserted / removed</param>
        /// <param name="LParam">0 for REMOVE, i for PLUG, 2 for SUSPEND</param>
        /// <param name="WParam">the device ID</param>
        internal static void PlugCallback(int Message, int LParam, int WParam)
        {
            DisplayPadHelper.DisplayPadPlugCallBack(LParam, WParam);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message">Notification signal when device's FW is  updating</param>
        /// <param name="LParam">target device's ID</param>
        /// <param name="WParam">if WParam == -1, Failed to update FW, if WParam is 0~ 100,  it is progress.</param>
        internal static void ProgressCallback(int Message, int LParam, int WParam)
        {
            DisplayPadHelper.DisplayPadProgressCallBack(LParam);
        }

        /// <summary>Returns key press status</summary>
        /// <param name="KeyMatrix">Key matrix id. See the matrix table</param>
        /// <param name="iPressed">key is pressed or released. 1 for pressed and 0 for released.</param>
        /// <param name="DeviceID">control the target, the vaule is not zero</param>
        internal static void KeyCallback(int KeyMatrix, int iPressed, int DeviceID)
        {
            DisplayPadHelper.DisplayPadKeyCallBack(KeyMatrix, iPressed, DeviceID);
        }

        /// <summary>Returns the dll version</summary>
        /// <returns>int : DLL's Version</returns>
        public int DisplayPadDllVersion()
        {
            try
            {
                int dllVersion = DisplayPadSDK.GetDLLVersion();
                Logger.LogMessage(dllVersion.ToString(), nameof(DisplayPadDllVersion));
                return dllVersion;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, nameof(DisplayPadDllVersion));
                return 0;
            }
        }

        /// <summary>Get "DevInfo"，Get version-related information</summary>
        /// <returns>bool: true Success，false Fail</returns>
        public DisplayPadSDK.DevInfo DisplayPadGetDeviceInfo(int ID)
        {
            DisplayPadSDK.DevInfo devInfo = new DisplayPadSDK.DevInfo();
            try
            {
                Logger.LogMessage(DisplayPadSDK.GetDeviceInfo(ref devInfo, Convert.ToUInt32(ID)).ToString(), nameof(DisplayPadGetDeviceInfo));
                return devInfo;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, nameof(DisplayPadGetDeviceInfo));
                return devInfo;
            }
        }

        /// <summary>Device plug status</summary>
        /// <returns></returns>
        public bool DisplayPadIsDevicePlug(int ID)
        {
            try
            {
                bool flag = DisplayPadSDK.IsDevicePlug(Convert.ToUInt32(ID));
                if (flag)
                    DisplayPadSDK.APEnable(true, Convert.ToUInt32(ID));
                Logger.LogMessage(flag.ToString(), nameof(DisplayPadIsDevicePlug));
                return flag;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, nameof(DisplayPadIsDevicePlug));
                return false;
            }
        }

        /// <summary>Initial the device and detect the plug/ unplug</summary>
        /// <param name="hNotifyhWnd">the handle for notification</param>
        /// <returns>bool: true plugged in，false not plugged in</returns>
        public bool DisplayPadOpenUSBDriver(string hNotifyhWnd)
        {
            try
            {
                bool flag = DisplayPadSDK.OpenUSBDriver(new IntPtr(Convert.ToInt32(hNotifyhWnd, 16)));
                Logger.LogMessage(flag.ToString(), nameof(DisplayPadOpenUSBDriver));
                return flag;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, nameof(DisplayPadOpenUSBDriver));
                return false;
            }
        }

        /// <summary>Close the device's control</summary>
        public bool DisplayPadCloseUSBDriver()
        {
            try
            {
                DisplayPadSDK.CloseUSBDriver();
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, nameof(DisplayPadCloseUSBDriver));
                return false;
            }
        }

        /// <summary>Confirm whether the firmware is being updated.</summary>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true is updated, false is not updated</returns>
        public bool DisplayPadIsUpdating(int ID)
        {
            try
            {
                return DisplayPadSDK.IsUpdating(Convert.ToUInt32(ID));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, nameof(DisplayPadIsUpdating));
                return false;
            }
        }

        /// <summary>Start FW update (open update thread)</summary>
        /// <param name="_FWBinUpdateInfo"></param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>true is Success，false is Fail or wrong BIN File.</returns>
        public bool DisplayPadStartFWUpdate(
          DisplayPadSDK.FWBinUpdateInfo_byte _FWBinUpdateInfo,
          int ID)
        {
            try
            {
                DisplayPadSDK.FWBinUpdateInfo fwBinUpdateInfo = new DisplayPadSDK.FWBinUpdateInfo();
                fwBinUpdateInfo.dwBinLength1 = _FWBinUpdateInfo.dwBinLength1;
                fwBinUpdateInfo.byTargetDev = Convert.ToByte(_FWBinUpdateInfo.byTargetDev);
                fwBinUpdateInfo.uFWVersion = (uint)Convert.ToUInt16(_FWBinUpdateInfo.uFWVersion);
                IntPtr destination = Marshal.AllocHGlobal((int)fwBinUpdateInfo.dwBinLength1 * Marshal.SizeOf(typeof(byte)));
                if ((long)_FWBinUpdateInfo.pBinImage1.GetLength(0) < (long)_FWBinUpdateInfo.dwBinLength1)
                    fwBinUpdateInfo.dwBinLength1 = (uint)_FWBinUpdateInfo.pBinImage1.GetLength(0);
                Marshal.Copy(_FWBinUpdateInfo.pBinImage1, 0, destination, (int)fwBinUpdateInfo.dwBinLength1);
                fwBinUpdateInfo.pBinImage1 = destination;
                Marshal.SizeOf<DisplayPadSDK.FWBinUpdateInfo>(fwBinUpdateInfo);
                bool flag = DisplayPadSDK.StartFWUpdate(Convert.ToUInt32(ID), fwBinUpdateInfo);
                Logger.LogMessage(flag.ToString(), nameof(DisplayPadStartFWUpdate));
                return flag;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, nameof(DisplayPadStartFWUpdate));
                return false;
            }
        }

        /// <summary>
        /// Get the number of BMP spaces remaining in the FW
        /// Note : call SetIconPic(UploadImage) to save image. If it is used, the fucntion will return the remaining valule.
        /// </summary>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>bool: true Success，false Fail</returns>
        public bool DisplayPadCalculateBMP(int ID)
        {
            try
            {
                byte bySurplusNum = Convert.ToByte(0);
                bool bmp = DisplayPadSDK.CalculateBMP(ref bySurplusNum, Convert.ToUInt32(ID));
                Logger.LogMessage(bmp.ToString(), nameof(DisplayPadCalculateBMP));
                return bmp;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, nameof(DisplayPadCalculateBMP));
                return false;
            }
        }

        /// <summary>
        /// Reset the all buttons's pictures of the current profile.
        /// </summary>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>bool: true Success，false Fail</returns>
        public bool DisplayPadResetPicture(int ID)
        {
            try
            {
                bool flag = DisplayPadSDK.ResetPicture(Convert.ToUInt32(ID));
                Logger.LogMessage(flag.ToString(), nameof(DisplayPadResetPicture));
                return flag;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, nameof(DisplayPadResetPicture));
                return false;
            }
        }

        /// <summary>Clear the specified sector's data( Clear to 0xFF)</summary>
        /// <param name="strStart">The Start Index</param>
        /// <param name="strEnd">The End Index</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>bool: true Success，false Fail</returns>
        public bool DisplayPadEraseSectorMem(string strStart, string strEnd, int ID)
        {
            try
            {
                bool flag = DisplayPadSDK.EraseSectorMem(Convert.ToInt32(strStart), Convert.ToInt32(strEnd), Convert.ToUInt32(ID));
                Logger.LogMessage(flag.ToString(), nameof(DisplayPadEraseSectorMem));
                return flag;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, nameof(DisplayPadEraseSectorMem));
                return false;
            }
        }

        /// <summary>Switch the current profile and current effect.</summary>
        /// <param name="stringProfil0eNo">switch to the target profile</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>bool: true Success，false Fail</returns>
        public bool DisplayPadSwitchProfile(string stringProfil0eNo, int ID)
        {
            try
            {
                bool flag = DisplayPadSDK.SwitchProfile(Convert.ToInt32(stringProfil0eNo), Convert.ToUInt32(ID));
                Logger.LogMessage(flag.ToString(), nameof(DisplayPadSwitchProfile));
                return flag;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, nameof(DisplayPadSwitchProfile));
                return false;
            }
        }

        /// <summary>Get FW version number</summary>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>FW Version number</returns>
        public string DisplayPadGetDevAppVer(int ID)
        {
            try
            {
                ushort devAppVer = DisplayPadSDK.GetDevAppVer(Convert.ToUInt32(ID));
                Logger.LogMessage(devAppVer.ToString(), nameof(DisplayPadGetDevAppVer));
                return devAppVer.ToString();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, nameof(DisplayPadGetDevAppVer));
                return "0";
            }
        }

        /// <summary>Get FWInfo Struct data</summary>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>FWInfo Struct data</returns>
        public DisplayPadSDK.FWInfo DisplayPadGetFWInfo(int ID)
        {
            DisplayPadSDK.FWInfo fwInfo = new DisplayPadSDK.FWInfo();
            try
            {
                Logger.LogMessage(DisplayPadSDK.GetFWInfo(ref fwInfo, Convert.ToUInt32(ID)).ToString(), nameof(DisplayPadGetFWInfo));
                return fwInfo;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, nameof(DisplayPadGetFWInfo));
                string str = "{'isSuccess': '" + false.ToString() + "', 'fwInfo': ''}";
                return fwInfo;
            }
        }

        /// <summary>Enable/Disale the SW's control.</summary>
        /// <param name="byEnable">If true, all are controlled by SW, otherwise some functions are con controlled by FW</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>bool: true Success，false Fail</returns>
        public bool DisplayPadAPEnable(string byEnable, int ID)
        {
            try
            {
                bool flag = DisplayPadSDK.APEnable(Convert.ToBoolean(byEnable), Convert.ToUInt32(ID));
                Logger.LogMessage(flag.ToString(), nameof(DisplayPadAPEnable));
                return flag;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, nameof(DisplayPadAPEnable));
                return false;
            }
        }

        /// <summary>Set the time for TFT-LCD to go to sleep</summary>
        /// <param name="strbyStatus">Sleep function switch. 0 is Off. 1 is On.</param>
        /// <param name="strbyHH">hour: 0~24</param>
        /// <param name="strbyMM">min: 0~59</param>
        /// <param name="strbySS">sec: 0~59</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>bool: true Success，false Fail</returns>
        public bool DisplayPadSetTFTSleepTime(
          string strbyStatus,
          string strbyHH,
          string strbyMM,
          string strbySS,
          int ID)
        {
            try
            {
                bool flag = DisplayPadSDK.SetTFTSleepTime(Convert.ToByte(strbyStatus), Convert.ToByte(strbyHH), Convert.ToByte(strbyMM), Convert.ToByte(strbySS), Convert.ToUInt32(ID));
                Logger.LogMessage(flag.ToString(), nameof(DisplayPadSetTFTSleepTime));
                return flag;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, nameof(DisplayPadSetTFTSleepTime));
                return false;
            }
        }

        /// <summary>Switch the Led's Brightness</summary>
        /// <param name="Brightness"> LCM Brightness 0 = 0%, 25 = 25%, 50 = 50%, 75 = 75%, 100 = 100%</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>bool: true Success，false Fail</returns>
        public bool DisplayPadSetMainBrightness(int iBrightness, int ID)
        {
            try
            {
                return DisplayPadSDK.SetMainBrightness(Convert.ToByte(iBrightness), Convert.ToUInt32(ID));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, nameof(DisplayPadSetMainBrightness));
                return false;
            }
        }

        /// <summary>Enable/Disable the custom keys function for keyboard</summary>
        /// <param name="byOn">if true, the function is ON, else the function is  Off.</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>bool: true Success，false Fail</returns>
        public bool DisplayPadEnableKeyFunc(string byOn, int ID)
        {
            try
            {
                return DisplayPadSDK.EnableKeyFunc(Convert.ToBoolean(byOn), Convert.ToUInt32(ID));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, nameof(DisplayPadEnableKeyFunc));
                return false;
            }
        }

        /// <summary>Reset the profiles in Flash.</summary>
        /// <param name="byAll">if  0, reset the current profile, else reset the all profiles.</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>bool: true Success，false Fail</returns>
        public bool DisplayPadResetFlash(string byAll, int ID)
        {
            try
            {
                bool flag = DisplayPadSDK.ResetFlash(Convert.ToBoolean(byAll), Convert.ToUInt32(ID));
                Logger.LogMessage(flag.ToString(), nameof(DisplayPadResetFlash));
                return flag;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, nameof(DisplayPadResetFlash));
                return false;
            }
        }

        /// <summary>Restore the key Assignment to default values.</summary>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>bool: true Success，false Fail</returns>
        public bool DisplayPadResetKeys(int ID)
        {
            try
            {
                bool flag = DisplayPadSDK.ResetKeys(Convert.ToUInt32(ID));
                Logger.LogMessage(flag.ToString(), nameof(DisplayPadResetKeys));
                return flag;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, nameof(DisplayPadResetKeys));
                return false;
            }
        }

        /// <summary>Change the key to other custom functions.</summary>
        /// <param name="iSource">source keys</param>
        /// <param name="iTarget">key function</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>bool: true Success，false Fail</returns>
        public bool DisplayPadChangeKey(string iSource, string iTarget, int ID)
        {
            try
            {
                bool flag = DisplayPadSDK.ChangeKey(Convert.ToInt32(iSource), Convert.ToInt32(iTarget), Convert.ToUInt32(ID));
                Logger.LogMessage(flag.ToString(), nameof(DisplayPadChangeKey));
                return flag;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, nameof(DisplayPadChangeKey));
                return false;
            }
        }

        /// <summary>Change the key to other custom functions.</summary>
        /// <param name="iSource">source keys</param>
        /// <param name="iTarget">Standard key function (0x01~0xFE see.Key Table)</param>
        /// <param name="iControlKey">Enable is 1    bit[0] = Ctrl     bit[1] = Shift   bit[2] = Alt   bit[3] = GUI(Win) </param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>bool: true Success，false Fail</returns>
        public bool DisplayPadChangeShortcutKey(
          string iSource,
          string iTarget,
          string iControlKey,
          int ID)
        {
            try
            {
                bool flag = DisplayPadSDK.ChangeShortcutKey(Convert.ToInt32(iSource), Convert.ToInt32(iTarget), Convert.ToInt32(iControlKey), Convert.ToUInt32(ID));
                Logger.LogMessage(flag.ToString(), nameof(DisplayPadChangeShortcutKey));
                return flag;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, nameof(DisplayPadChangeShortcutKey));
                return false;
            }
        }

        /// <summary>Change the key to the launch functions.</summary>
        /// <param name="structLacunch">Struct of LaunchContent</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>bool: true Success，false Fail</returns>
        public bool DisplayPadSetLaunchContent(DisplayPadSDK.LaunchContent structLacunch, int ID)
        {
            try
            {
                bool flag = DisplayPadSDK.SetLaunchContent(structLacunch.iSource, structLacunch.iDataLen, structLacunch.pData, Convert.ToUInt32(ID));
                Logger.LogMessage(flag.ToString(), nameof(DisplayPadSetLaunchContent));
                return flag;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, nameof(DisplayPadSetLaunchContent));
                return false;
            }
        }

        /// <summary>Change the key to the launch functions.</summary>
        /// <param name="structMacro">&gt;Struct of MacroInputContent </param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>bool: true Success，false Fail</returns>
        public bool DisplayPadSetFullMacroData(
          DisplayPadSDK.MacroInputContent_DisplayPad structMacro,
          int ID)
        {
            try
            {
                bool flag = DisplayPadSDK.SetFullMacroData(structMacro.iSrc, structMacro.iType, structMacro.iTimes, structMacro.iEventLen, structMacro.pData, Convert.ToUInt32(ID));
                Logger.LogMessage(flag.ToString(), nameof(DisplayPadSetFullMacroData));
                return flag;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, nameof(DisplayPadSetFullMacroData));
                return false;
            }
        }

        /// <summary>Read the settings of the macro key.</summary>
        /// <param name="ProfileIndex">source Profile</param>
        /// <param name="SourceKey">source keys</param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>bool: true Success，false Fail</returns>
        public DisplayPadSDK.MacroContent DisplayPadGetMacroContent(
          string ProfileIndex,
          string SourceKey,
          int ID)
        {
            DisplayPadSDK.MacroContent macroContent = new DisplayPadSDK.MacroContent();
            try
            {
                Logger.LogMessage(DisplayPadSDK.GetMacroContent(Convert.ToInt32(ProfileIndex), Convert.ToInt32(SourceKey), ref macroContent, ID: Convert.ToUInt32(ID)).ToString(), nameof(DisplayPadGetMacroContent));
                return macroContent;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, nameof(DisplayPadGetMacroContent));
                return macroContent;
            }
        }

        /// <summary>Set SyncAcross Profile's Status</summary>
        /// <param name="bEnable">bool bEnable :  0: Disable, 1:Eanble</param>
        /// <returns>bool: true Success，false Fail</returns>
        public bool DisplayPadSetSyncAcrossProfiles(string bEnable, int ID)
        {
            try
            {
                bool flag = DisplayPadSDK.SetSyncAcrossProfiles(Convert.ToBoolean(bEnable), Convert.ToUInt32(ID));
                Logger.LogMessage(flag.ToString(), nameof(DisplayPadSetSyncAcrossProfiles));
                return flag;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, nameof(DisplayPadSetSyncAcrossProfiles));
                return false;
            }
        }

        public bool DisplayPadGetSyncAcrossProfiles(string bEnable, int ID)
        {
            try
            {
                bool boolean = Convert.ToBoolean(bEnable);
                bool syncAcrossProfiles = DisplayPadSDK.GetSyncAcrossProfiles(ref boolean, Convert.ToUInt32(ID));
                Logger.LogMessage(syncAcrossProfiles.ToString(), nameof(DisplayPadGetSyncAcrossProfiles));
                return syncAcrossProfiles;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, nameof(DisplayPadGetSyncAcrossProfiles));
                return false;
            }
        }

        /// <summary>Get the number of BMP spaces remaining in the FW</summary>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>Returns the count of repaired BMPs</returns>
        public byte DisplayPadCheckBMPStorage(int ID)
        {
            byte byRestoreCount = Convert.ToByte(0);
            try
            {
                DisplayPadSDK.CheckBMPStorage(ref byRestoreCount, Convert.ToUInt32(ID));
                return byRestoreCount;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "DisplayPadheckBMPStorage");
                return byRestoreCount;
            }
        }

        /// <summary>
        /// Get the count of BMP used by target's button in the target's profile in the FW
        /// </summary>
        /// <param name="strProfile">The target's profile</param>
        /// <param name="strBtnIndex">The target's button</param>
        /// <param name="strLength"></param>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>Return the count of  BMP used by button..</returns>
        public byte DisplayPadGetBtnBMPLength(
          string strProfile,
          string strBtnIndex,
          string strLength,
          int ID)
        {
            byte byLength = Convert.ToByte(Convert.ToInt32(strLength));
            try
            {
                DisplayPadSDK.GetBtnBMPLength(Convert.ToByte(Convert.ToInt32(strProfile)), Convert.ToByte(Convert.ToInt32(strBtnIndex)), ref byLength, Convert.ToUInt32(ID));
                return byLength;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, nameof(DisplayPadGetBtnBMPLength));
                return byLength;
            }
        }

        /// <summary>Get the Led's Brightness</summary>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>LCM Brightness 0 = 0%, 25 = 25%, 50 = 50%, 75 = 75%, 100 = 100%</returns>
        public int DisplayPadGetMainBrightness(int ID)
        {
            byte byBrightness = Convert.ToByte(0);
            try
            {
                DisplayPadSDK.GetMainBrightness(ref byBrightness, Convert.ToUInt32(ID));
                return Convert.ToInt32(byBrightness);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, nameof(DisplayPadGetMainBrightness));
                return Convert.ToInt32(byBrightness);
            }
        }

        /// <summary>Get the time for TFT-LCD to go to sleep</summary>
        /// <param name="ID">control the target, the vaule is not zero</param>
        /// <returns>TFT Sleep Time</returns>
        public DisplayPadHelper.SleepTime DisplayPadGetTFTSleepTime(int ID)
        {
            DisplayPadHelper.SleepTime tftSleepTime = new DisplayPadHelper.SleepTime();
            try
            {
                byte byStatus = Convert.ToByte(0);
                byte byHH = Convert.ToByte(0);
                byte byMM = Convert.ToByte(0);
                byte bySS = Convert.ToByte(0);
                DisplayPadSDK.GetTFTSleepTime(ref byStatus, ref byHH, ref byMM, ref bySS, Convert.ToUInt32(ID));
                tftSleepTime.byStatus = byStatus;
                tftSleepTime.byHH = byHH;
                tftSleepTime.byMM = byMM;
                tftSleepTime.bySS = bySS;
                return tftSleepTime;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "DisplayPadGetBtnBMPLength");
                return tftSleepTime;
            }
        }

        /// <summary>This method use to upload non animated image</summary>
        /// <param name="ID">Device ID</param>
        /// <param name="strImagePath">image path</param>
        /// <param name="ibtnIndex">button index</param>
        /// <returns></returns>
        public bool UploadImage(int ID, string strImagePath, int ibtnIndex)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream(File.ReadAllBytes(strImagePath));
                memoryStream.Position = 0L;
                Bitmap bitmap1 = (Bitmap)Image.FromStream((Stream)memoryStream);
                memoryStream.Close();
                return UploadImage(ID, bitmap1, ibtnIndex);

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, nameof(UploadImage));
                return false;
            }
        }

        /// <summary>This method use to upload non animated image</summary>
        /// <param name="ID">Device ID</param>
        /// <param name="image">image</param>
        /// <param name="ibtnIndex">button index</param>
        /// <returns></returns>
        public bool UploadImage(int ID, Image image, int ibtnIndex)
        {
            try
            {
                int num1 = 102;
                Bitmap bitmap1 = this.ResizeImage(image, num1, num1);
                Bitmap bitmap2 = new Bitmap(num1, num1, PixelFormat.Format32bppRgb);
                int num2 = 40;
                using (Graphics graphics = Graphics.FromImage((Image)bitmap2))
                {
                    graphics.Clear(Color.Black);
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    Brush brush = (Brush)new TextureBrush((Image)bitmap1);
                    GraphicsPath path = new GraphicsPath();
                    path.AddArc(0, 0, num2, num2, 180f, 90f);
                    path.AddArc(bitmap2.Width - num2, 0, num2, num2, 270f, 90f);
                    path.AddArc(bitmap2.Width - num2, bitmap2.Height - num2, num2, num2, 0.0f, 90f);
                    path.AddArc(0, bitmap2.Height - num2, num2, num2, 90f, 90f);
                    graphics.FillPath(brush, path);
                }
                byte[] bitmapBytes = this.GetBitmapBytes(bitmap2);
                Rectangle rectangle = new Rectangle(0, 0, 102, 102);
                byte byIconNumber = Convert.ToByte(ibtnIndex);
                DisplayPadSDK.PicPacketInfo pData = new DisplayPadSDK.PicPacketInfo();
                int num3 = bitmap2.Width * bitmap2.Height * 3;
                pData.picPacket = new DisplayPadSDK.PicPacket[31];
                int num4 = 0;
                for (int index1 = 0; index1 + 3 < bitmapBytes.Length; index1 += 4)
                {
                    int num5 = index1 / 4;
                    int y = num5 / bitmap2.Width;
                    int x = num5 % bitmap2.Width;
                    if (rectangle.Contains(new Point(x, y)))
                    {
                        byte num6 = bitmapBytes[index1];
                        byte num7 = bitmapBytes[index1 + 1];
                        byte num8 = bitmapBytes[index1 + 2];
                        byte num9 = bitmapBytes[index1 + 3];
                        int index2 = num4 * 3 / 1024;
                        int index3 = num4 * 3 % 1024;
                        if (pData.picPacket[index2].byData == null)
                            pData.picPacket[index2].byData = new byte[1024];
                        pData.picPacket[index2].byData[index3] = num6;
                        int index4 = (num4 * 3 + 1) / 1024;
                        int index5 = (num4 * 3 + 1) % 1024;
                        if (pData.picPacket[index4].byData == null)
                            pData.picPacket[index4].byData = new byte[1024];
                        pData.picPacket[index4].byData[index5] = num7;
                        int index6 = (num4 * 3 + 2) / 1024;
                        int index7 = (num4 * 3 + 2) % 1024;
                        if (pData.picPacket[index6].byData == null)
                            pData.picPacket[index6].byData = new byte[1024];
                        pData.picPacket[index6].byData[index7] = num8;
                        pData.picPacket[index6].byReportID = (byte)0;
                        ++num4;
                    }
                }
                return DisplayPadSDK.SetIconPacket(byIconNumber, ref pData, (uint)ID);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, nameof(UploadImage));
                return false;
            }
        }

        private Bitmap ResizeImage(Image image, int width, int height)
        {
            Rectangle destRect = new Rectangle(0, 0, width, height);
            Bitmap bitmap = new Bitmap(width, height);
            bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            using (Graphics graphics = Graphics.FromImage((Image)bitmap))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                using (ImageAttributes imageAttr = new ImageAttributes())
                {
                    imageAttr.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
                }
            }
            return bitmap;
        }

        /// <summary>
        /// this method use to reduced the button image and added border on it.
        /// </summary>
        /// <param name="bmp">image</param>
        /// <returns></returns>
        private Bitmap DrawBitmapWithBorder(Bitmap bmp)
        {
            int num = 11;
            int width = bmp.Width + num * 2;
            int height = bmp.Height + num * 2;
            Image image = (Image)new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                using (Brush brush = (Brush)new SolidBrush(Color.Black))
                    graphics.FillRectangle(brush, 0, 0, width, height);
                graphics.DrawImage((Image)bmp, new Rectangle(num, num, bmp.Width, bmp.Height));
            }
            return (Bitmap)image;
        }

        private byte[] GetBitmapBytes(Bitmap image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));
            BitmapData bitmapdata = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
            try
            {
                IntPtr scan0 = bitmapdata.Scan0;
                int length = bitmapdata.Stride * image.Height;
                byte[] destination = new byte[length];
                Marshal.Copy(scan0, destination, 0, length);
                return destination;
            }
            finally
            {
                image.UnlockBits(bitmapdata);
            }
        }

        private bool DisplayPadBySetIconPic(DisplayPadSDK.DisplayPicInfo_byte objpicInfo, int ID)
        {
            try
            {
                DisplayPadSDK.DisplayPicInfo picInfo = new DisplayPadSDK.DisplayPicInfo();
                picInfo.byProfile = objpicInfo.byProfile;
                picInfo.byIcon = objpicInfo.byIcon;
                picInfo.byLeft = objpicInfo.byLeft;
                picInfo.byTop = objpicInfo.byTop;
                picInfo.byRight = objpicInfo.byRight;
                picInfo.byBottom = objpicInfo.byBottom;
                picInfo.byPicNum = objpicInfo.byPicNum;
                picInfo.byPicIndex = objpicInfo.byPicIndex;
                picInfo.wPicInterval = objpicInfo.wPicInterval;
                IntPtr num = Marshal.AllocHGlobal(objpicInfo.iDataLen * Marshal.SizeOf(typeof(byte)));
                if (objpicInfo.imgData.GetLength(0) < objpicInfo.iDataLen)
                    objpicInfo.iDataLen = objpicInfo.imgData.GetLength(0);
                Marshal.Copy(objpicInfo.imgData, 0, num, objpicInfo.iDataLen);
                Logger.LogMessage(DisplayPadSDK.SetIconPic(picInfo, num, ID: Convert.ToUInt32(ID)).ToString(), "DisplayPadSetIconPic");
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "DisplayPadSetIconPic");
                return false;
            }
        }

        /// <summary>
        /// This method use to upload non animated image by SetIconPic
        /// </summary>
        /// <param name="ID">Device ID</param>
        /// <param name="strImagePath">image path</param>
        /// <param name="ibtnIndex">button index</param>
        /// <param name="IsAPEnable">APEnable state</param>
        /// <param name="iProfileIndex">Profile Index</param>
        /// <returns></returns>
        public bool UploadImageBySetIconPic(
          int ID,
          string strImagePath,
          int ibtnIndex,
          bool IsAPEnable,
          int iProfileIndex)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream(File.ReadAllBytes(strImagePath));
                memoryStream.Position = 0L;
                Bitmap bitmap1 = (Bitmap)Image.FromStream((Stream)memoryStream);
                memoryStream.Close();
                int num1 = 102;
                Bitmap bitmap2 = this.ResizeImage((Image)bitmap1, num1, num1);
                Bitmap image = new Bitmap(num1, num1, PixelFormat.Format32bppRgb);
                using (Graphics graphics = Graphics.FromImage((Image)image))
                    graphics.DrawImage((Image)bitmap2, new Rectangle(0, 0, num1, num1));
                byte[] bitmapBytes = this.GetBitmapBytes(image);
                DisplayPadSDK.DisplayPicInfo_byte objpicInfo = new DisplayPadSDK.DisplayPicInfo_byte();
                if (IsAPEnable)
                {
                    objpicInfo.byProfile = Convert.ToByte(0);
                    objpicInfo.byPicNum = (byte)0;
                }
                else
                {
                    objpicInfo.byProfile = (byte)iProfileIndex;
                    objpicInfo.byPicNum = (byte)1;
                }
                objpicInfo.byIcon = Convert.ToByte(ibtnIndex);
                objpicInfo.byLeft = (byte)0;
                objpicInfo.byTop = (byte)0;
                objpicInfo.byRight = (byte)101;
                objpicInfo.byBottom = (byte)101;
                objpicInfo.byPicIndex = (byte)0;
                objpicInfo.wPicInterval = (ushort)0;
                int num2 = 0;
                int num3 = 3;
                int length = image.Width * image.Height * 3;
                byte[] numArray = new byte[length];
                for (int index = 0; index + 3 < bitmapBytes.Length; index += 4)
                {
                    byte num4 = bitmapBytes[index];
                    byte num5 = bitmapBytes[index + 1];
                    byte num6 = bitmapBytes[index + 2];
                    byte num7 = bitmapBytes[index + 3];
                    numArray[num2 * num3] = num4;
                    numArray[num2 * num3 + 1] = num5;
                    numArray[num2 * num3 + 2] = num6;
                    ++num2;
                }
                objpicInfo.imgData = numArray;
                objpicInfo.iDataLen = length;
                objpicInfo.iDataLen = length;
                objpicInfo.bCopyBuffer = false;
                bool flag = this.DisplayPadBySetIconPic(objpicInfo, ID);
                image.Dispose();
                bitmap2.Dispose();
                bitmap1.Dispose();
                return flag;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "DisplayPadSetIconPic");
                return false;
            }
        }

        public delegate void DisplayPadStatus(int Status, int DeviceID);

        public delegate void DisplayPadProgressStatus(int Percentage);

        public delegate void DisplayPadKeyStatus(int KeyMatrix, int iPressed, int DeviceID);

        public struct SleepTime
        {
            public byte byStatus;
            public byte byHH;
            public byte byMM;
            public byte bySS;
        }
    }
}
