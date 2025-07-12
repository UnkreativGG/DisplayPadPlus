using System.Runtime.InteropServices;

namespace DisplayPadPlusCore;
public class Testing
{
    const int depth = 3;


    const int width = 800;

    const int buttonsHorizontal = 6;
    const int boundsLeft = 15;
    const int horizontalBuffer = 31;
    const int boundsRight = 16;


    const int height = 240;

    const int buttonsVertical = 2;
    const int boundsTop = 0;
    const int verticalBuffer = 36;
    const int boundsBottom = 0;

    static DisplayPadHelper helper = new();

    public static void Test()
    {
        DisplayPadHelper helper = new();

        //*
        DisplayPadSDK.KEYMAP_EVENT keyMap_Event1 = new()
        {
            keyByte = 0x02,
            timer = 1,
            keyPage = 69,
            keyAction = 69,
        };

        DisplayPadSDK.KEYMAP_EVENT keyMap_Event2 = new()
        {
            keyByte = 0xff,
            timer = 1,
            keyPage = 0x00,
            keyAction = 0x0f,
        };

        DisplayPadSDK.MacroInputContent_DisplayPad macroInputContent_DisplayPad = new()
        {
            iSrc = 69,
            iType = 0x01,
            iTimes = 1,
            iEventLen = 2,
            pData = []
        };

        DisplayPadHelper.DisplayPadPlugCallBack += DisplayPadPlugCallBack;
        DisplayPadHelper.DisplayPadKeyCallBack += DisplayPadKeyCallBack;
        DisplayPadHelper.DisplayPadProgressCallBack += DisplayPadProgressCallBack;



        //*/



    }

    private static void DisplayPadProgressCallBack(int Percentage)
    {
    }

    private static void DisplayPadKeyCallBack(int KeyMatrix, int iPressed, int DeviceID)
    {
        if (iPressed == 1)
            for (int i = -5; i < 12; i++)
                for (int ii = -5; ii < 50; ii++)
                    for (int iii = 0b0000; iii < 0b1_0000; iii++)
                        for (uint iv = 0; iv < 2; iv++)
                        {
                            bool diggs = DisplayPadSDK.ChangeShortcutKey(i, ii, iii, iv);
                            Console.CursorLeft = 0;
                            Console.Write("{0,-5} {1,-5} {2,-5} {3,-5}", i, ii, iii, iv);
                            if (diggs)
                            {
                                Console.WriteLine("\nFoundIt");
                            }
                        }
    }

    private static void DisplayPadPlugCallBack(int Status, int DeviceID)
    {
        if (Status == 0)
        {

        }
        else if (Status == 1)
        {

        }

        Console.WriteLine("Device status: " + Status + " for Device Id: " + DeviceID);
    }

    static void TestArchive()
    {
        DisplayPadHelper helper = new();

        DisplayPadSDK.SetPanelImage(1, MudMud().GetAdr());


        for (int x = 0; x < buttonsHorizontal; x++)
            for (int y = 0; y < buttonsVertical; y++)
            {
                const int size = 102;

                const int sideBuffer = 14;
                const int sideSpace = 32;

                int xFrom = sideBuffer + x * (sideSpace + size);
                int xTo = sideBuffer + x * (sideSpace + size) + size;

                const int standBuffer = 0;
                const int standSpace = 36;

                int yFrom = standBuffer + y * (standSpace + size);
                int yTo = standBuffer + y * (standSpace + size) + size;


                byte[] data = new byte[(xTo - xFrom) * (yTo - yFrom) * 3];
                for (int i = 0; i < data.Length; i += 3)
                {
                    data[i + 0] = 0;
                    data[i + 1] = 0;
                    data[i + 2] = 255;
                }
                DisplayPadSDK.SetPanelImage(1, data.GetAdr(), (uint)xFrom, (uint)yFrom, (uint)xTo, (uint)yTo);
            }

        /*
        for (int x = 0; x < buttonsHorizontal; x++)
            for (int y = 0; y < buttonsVertical; y++)
            {
                int spaceX = width - boundsLeft - boundsRight;
                double buttonWidth = (double)(spaceX - horizontalBuffer * (buttonsHorizontal - 1)) / buttonsHorizontal;

                int xFrom = (int)(boundsLeft + x * (buttonWidth + horizontalBuffer));
                int xTo = (int)(boundsLeft + x * (buttonWidth + horizontalBuffer) + buttonWidth);


                int spaceY = height - boundsTop - boundsBottom;
                double buttonHeight = (double)(spaceY - verticalBuffer * (buttonsVertical - 1)) / buttonsVertical;

                int yFrom = (int)(boundsTop + y * (buttonHeight + verticalBuffer));
                int yTo = (int)(boundsTop + y * (buttonHeight + verticalBuffer) + buttonHeight);

                byte[] data = new byte[(xTo - xFrom) * (yTo - yFrom) * 3];
                for (int i = 0; i < data.Length; i += 3)
                {
                    data[i + 0] = 0;
                    data[i + 1] = 0;
                    data[i + 2] = 255;
                }

                DisplayPadSDK.SetPanelImage(1, data.GetAdr(), (uint)xFrom, (uint)yFrom, (uint)xTo, (uint)yTo);
            }
        */

        Bitmap bitmap = new Bitmap(1, 1);
        bitmap.SetPixel(0, 0, Color.FromArgb(255, 255, 255));
        for (int i = 0; i < 12; i++)
        {
            helper.UploadImage(1, bitmap, i);
        }
    }

    static byte[] MudMud()
    {
        byte[] data = new byte[width * height * depth];

        using Bitmap bmp = new(new Bitmap(@"---"), width, height);

        for (int i = 0; i < data.Length; i += depth)
        {
            int x = (i / depth) % width;
            int y = (i / depth) / width;

            Color c = bmp.GetPixel(x, y);

            data[i + 0] = c.B;
            data[i + 1] = c.G;
            data[i + 2] = c.R;
        }

        return data;
    }
}



public static class Ext
{
    public static IntPtr GetAdr(this object obj)
        => GCHandle.Alloc(obj, GCHandleType.Pinned).AddrOfPinnedObject();
}
