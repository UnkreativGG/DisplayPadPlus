using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayPadPlusCore;
public class DisplayPadSDKPublic
{
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
    public static bool SetPanelImage(
        uint ID,
        IntPtr pbyData,
        uint iLeft = 0,
        uint iTop = 0,
        uint iRight = 799,
        uint iBottom = 239)
    {
        return DisplayPadSDK.SetPanelImage(ID, pbyData, iLeft, iTop, iRight, iBottom);
    }
}
