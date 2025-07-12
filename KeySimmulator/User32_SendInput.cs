using System.Runtime.InteropServices;

namespace KeySimmulator;
public class User32_SendInput
{
    public static uint SendInput(Input[] pInputs)
    {
        return SendInput((uint)pInputs.Length, pInputs, Marshal.SizeOf<Input>());
    }


    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(uint nInputs, Input[] pInputs, int cbSize);



    public class Mouse
    {
        public static Input Inputbuilder(MouseEventFlags vk)
        {
            MouseInput mouseInput = default;
            mouseInput.dwFlags = vk;

            Input input = default;
            input.type = SendInputEventType.InputMouse;
            input.mkhi.mi = mouseInput;

            return input;
        }


        public static Input Inputbuilder(int x, int y)
        {
            MouseInput mouseInput = default;
            mouseInput.dwFlags = MouseEventFlags.MOUSEEVENT_ABSOLUTE;
            mouseInput.dx = x;
            mouseInput.dy = y;

            Input input = default;
            input.type = SendInputEventType.InputMouse;
            input.mkhi.mi = mouseInput;

            return input;
        }




        [StructLayout(LayoutKind.Sequential)]
        public struct MouseInput
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public MouseEventFlags dwFlags;
            public uint time;
            public nint dwExtraInfo;
        }

        [Flags]
        public enum MouseEventFlags : uint
        {
            MOUSEEVENT_MOVE = 0x0001,
            MOUSEEVENT_LEFTDOWN = 0x0002,
            MOUSEEVENT_LEFTUP = 0x0004,
            MOUSEEVENT_RIGHTDOWN = 0x0008,
            MOUSEEVENT_RIGHTUP = 0x0010,
            MOUSEEVENT_MIDDLEDOWN = 0x0020,
            MOUSEEVENT_MIDDLEUP = 0x0040,
            MOUSEEVENT_XDOWN = 0x0080,
            MOUSEEVENT_XUP = 0x0100,
            MOUSEEVENT_WHEEL = 0x0800,
            MOUSEEVENT_VIRTUALDESK = 0x4000,
            MOUSEEVENT_ABSOLUTE = 0x8000
        }
    }



    public class Keyboard
    {
        public static Input Inputbuilder_Down(KeyboardEventVK vk)
            => Inputbuilder(vk, true);

        public static Input Inputbuilder_Up(KeyboardEventVK vk)
            => Inputbuilder(vk, false);


        public static Input Inputbuilder(KeyboardEventVK vk, bool down)
        {
            KeyboardInput keyboardInput = default;
            keyboardInput.wVk = vk;

            if (down)
                keyboardInput.dwFlags = IsExtendKey(vk) ? 1u : 0u;
            else
                keyboardInput.dwFlags = IsExtendKey(vk) ? 3u : 2u;

            Input input = default;
            input.type = SendInputEventType.InputKeyboard;
            input.mkhi.ki = keyboardInput;

            return input;
        }


        public static bool IsExtendKey(KeyboardEventVK vk)
            =>
            vk == KeyboardEventVK.VK_MENU ||
            vk == KeyboardEventVK.VK_LMENU ||
            vk == KeyboardEventVK.VK_RMENU ||
            vk == KeyboardEventVK.VK_CONTROL ||
            vk == KeyboardEventVK.VK_RCONTROL ||
            vk == KeyboardEventVK.VK_INSERT ||
            vk == KeyboardEventVK.VK_DELETE ||
            vk == KeyboardEventVK.VK_HOME ||
            vk == KeyboardEventVK.VK_END ||
            vk == KeyboardEventVK.VK_PRIOR ||
            vk == KeyboardEventVK.VK_NEXT ||
            vk == KeyboardEventVK.VK_RIGHT ||
            vk == KeyboardEventVK.VK_UP ||
            vk == KeyboardEventVK.VK_LEFT ||
            vk == KeyboardEventVK.VK_DOWN ||
            vk == KeyboardEventVK.VK_NUMLOCK ||
            vk == KeyboardEventVK.VK_CANCEL ||
            vk == KeyboardEventVK.VK_SNAPSHOT ||
            vk == KeyboardEventVK.VK_DIVIDE;




        [StructLayout(LayoutKind.Sequential)]
        public struct KeyboardInput
        {
            public KeyboardEventVK wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public nint dwExtraInfo;
        }

        [Flags]
        public enum KeyboardEventVK : ushort
        {
            VK_LBUTTON = 0x01,              // Left mouse button
            VK_RBUTTON = 0x02,              // Right mouse button
            VK_CANCEL = 0x03,               // Control-break processing
            VK_MBUTTON = 0x04,              // Middle mouse button
            VK_XBUTTON1 = 0x05,             // X1 mouse button
            VK_XBUTTON2 = 0x06,             // X2 mouse button
            Reserved_0x07 = 0x07,           // Reserved
            VK_BACK = 0x08,                 // BACKSPACE key
            VK_TAB = 0x09,                  // TAB key
            Reserved_0x0A = 0x0A,           // Reserved
            Reserved_0x0B = 0x0B,           // Reserved
            VK_CLEAR = 0x0C,                // CLEAR key
            VK_RETURN = 0x0D,               // ENTER key
            Unassigned_0x0E = 0x0E,         // Unassigned
            Unassigned_0x0F = 0x0F,         // Unassigned
            VK_SHIFT = 0x10,                // SHIFT key
            VK_CONTROL = 0x11,              // CTRL key
            VK_MENU = 0x12,                 // ALT key
            VK_PAUSE = 0x13,                // PAUSE key
            VK_CAPITAL = 0x14,              // CAPS LOCK key
            VK_KANA = 0x15,                 // IME Kana mode
            VK_HANGUL = 0x15,               // IME Hangul mode
            VK_IME_ON = 0x16,               // IME On
            VK_JUNJA = 0x17,                // IME Junja mode
            VK_FINAL = 0x18,                // IME final mode
            VK_HANJA = 0x19,                // IME Hanja mode
            VK_KANJI = 0x19,                // IME Kanji mode
            VK_IME_OFF = 0x1A,              // IME Off
            VK_ESCAPE = 0x1B,               // ESC key
            VK_CONVERT = 0x1C,              // IME convert
            VK_NONCONVERT = 0x1D,           // IME nonconvert
            VK_ACCEPT = 0x1E,               // IME accept
            VK_MODECHANGE = 0x1F,           // IME mode change request
            VK_SPACE = 0x20,                // SPACEBAR
            VK_PRIOR = 0x21,                // PAGE UP key
            VK_NEXT = 0x22,                 // PAGE DOWN key
            VK_END = 0x23,                  // END key
            VK_HOME = 0x24,                 // HOME key
            VK_LEFT = 0x25,                 // LEFT ARROW key
            VK_UP = 0x26,                   // UP ARROW key
            VK_RIGHT = 0x27,                // RIGHT ARROW key
            VK_DOWN = 0x28,                 // DOWN ARROW key
            VK_SELECT = 0x29,               // SELECT key
            VK_PRINT = 0x2A,                // PRINT key
            VK_EXECUTE = 0x2B,              // EXECUTE key
            VK_SNAPSHOT = 0x2C,             // PRINT SCREEN key
            VK_INSERT = 0x2D,               // INS key
            VK_DELETE = 0x2E,               // DEL key
            VK_HELP = 0x2F,                 // HELP key
            VK_0 = 0x30,                    // 0 key
            VK_1 = 0x31,                    // 1 key
            VK_2 = 0x32,                    // 2 key
            VK_3 = 0x33,                    // 3 key
            VK_4 = 0x34,                    // 4 key
            VK_5 = 0x35,                    // 5 key
            VK_6 = 0x36,                    // 6 key
            VK_7 = 0x37,                    // 7 key
            VK_8 = 0x38,                    // 8 key
            VK_9 = 0x39,                    // 9 key
            Undefined_0x3A = 0x3A,          // Undefined
            Undefined_0x3B = 0x3B,          // Undefined
            Undefined_0x3C = 0x3C,          // Undefined
            Undefined_0x3D = 0x3D,          // Undefined
            Undefined_0x3E = 0x3E,          // Undefined
            Undefined_0x3F = 0x3F,          // Undefined
            Undefined_0x40 = 0x40,          // Undefined
            VK_A = 0x41,                    // A key
            VK_B = 0x42,                    // B key
            VK_C = 0x43,                    // C key
            VK_D = 0x44,                    // D key
            VK_E = 0x45,                    // E key
            VK_F = 0x46,                    // F key
            VK_G = 0x47,                    // G key
            VK_H = 0x48,                    // H key
            VK_I = 0x49,                    // I key
            VK_J = 0x4A,                    // J key
            VK_K = 0x4B,                    // K key
            VK_L = 0x4C,                    // L key
            VK_M = 0x4D,                    // M key
            VK_N = 0x4E,                    // N key
            VK_O = 0x4F,                    // O key
            VK_P = 0x50,                    // P key
            VK_Q = 0x51,                    // Q key
            VK_R = 0x52,                    // R key
            VK_S = 0x53,                    // S key
            VK_T = 0x54,                    // T key
            VK_U = 0x55,                    // U key
            VK_V = 0x56,                    // V key
            VK_W = 0x57,                    // W key
            VK_X = 0x58,                    // X key
            VK_Y = 0x59,                    // Y key
            VK_Z = 0x5A,                    // Z key
            VK_LWIN = 0x5B,                 // Left Windows key
            VK_RWIN = 0x5C,                 // Right Windows key
            VK_APPS = 0x5D,                 // Applications key
            Reserved_0x5E = 0x5E,           // Reserved
            VK_SLEEP = 0x5F,                // Computer Sleep key
            VK_NUMPAD0 = 0x60,              // Numeric keypad 0 key
            VK_NUMPAD1 = 0x61,              // Numeric keypad 1 key
            VK_NUMPAD2 = 0x62,              // Numeric keypad 2 key
            VK_NUMPAD3 = 0x63,              // Numeric keypad 3 key
            VK_NUMPAD4 = 0x64,              // Numeric keypad 4 key
            VK_NUMPAD5 = 0x65,              // Numeric keypad 5 key
            VK_NUMPAD6 = 0x66,              // Numeric keypad 6 key
            VK_NUMPAD7 = 0x67,              // Numeric keypad 7 key
            VK_NUMPAD8 = 0x68,              // Numeric keypad 8 key
            VK_NUMPAD9 = 0x69,              // Numeric keypad 9 key
            VK_MULTIPLY = 0x6A,             // Multiply key
            VK_ADD = 0x6B,                  // Add key
            VK_SEPARATOR = 0x6C,            // Separator key
            VK_SUBTRACT = 0x6D,             // Subtract key
            VK_DECIMAL = 0x6E,              // Decimal key
            VK_DIVIDE = 0x6F,               // Divide key
            VK_F1 = 0x70,                   // F1 key
            VK_F2 = 0x71,                   // F2 key
            VK_F3 = 0x72,                   // F3 key
            VK_F4 = 0x73,                   // F4 key
            VK_F5 = 0x74,                   // F5 key
            VK_F6 = 0x75,                   // F6 key
            VK_F7 = 0x76,                   // F7 key
            VK_F8 = 0x77,                   // F8 key
            VK_F9 = 0x78,                   // F9 key
            VK_F10 = 0x79,                  // F10 key
            VK_F11 = 0x7A,                  // F11 key
            VK_F12 = 0x7B,                  // F12 key
            VK_F13 = 0x7C,                  // F13 key
            VK_F14 = 0x7D,                  // F14 key
            VK_F15 = 0x7E,                  // F15 key
            VK_F16 = 0x7F,                  // F16 key
            VK_F17 = 0x80,                  // F17 key
            VK_F18 = 0x81,                  // F18 key
            VK_F19 = 0x82,                  // F19 key
            VK_F20 = 0x83,                  // F20 key
            VK_F21 = 0x84,                  // F21 key
            VK_F22 = 0x85,                  // F22 key
            VK_F23 = 0x86,                  // F23 key
            VK_F24 = 0x87,                  // F24 key
            Reserved_0x88 = 0x88,           // Reserved
            Reserved_0x89 = 0x89,           // Reserved
            Reserved_0x8A = 0x8A,           // Reserved
            Reserved_0x8B = 0x8B,           // Reserved
            Reserved_0x8C = 0x8C,           // Reserved
            Reserved_0x8D = 0x8D,           // Reserved
            Reserved_0x8E = 0x8E,           // Reserved
            Reserved_0x8F = 0x8F,           // Reserved
            VK_NUMLOCK = 0x90,              // NUM LOCK key
            VK_SCROLL = 0x91,               // SCROLL LOCK key
            OEM_specific_0x92 = 0x92,       // OEM specific
            OEM_specific_0x93 = 0x93,       // OEM specific
            OEM_specific_0x94 = 0x94,       // OEM specific
            OEM_specific_0x95 = 0x95,       // OEM specific
            OEM_specific_0x96 = 0x96,       // OEM specific
            Unassigned_0x97 = 0x97,         // Unassigned
            Unassigned_0x98 = 0x98,         // Unassigned
            Unassigned_0x99 = 0x99,         // Unassigned
            Unassigned_0x9A = 0x9A,         // Unassigned
            Unassigned_0x9B = 0x9B,         // Unassigned
            Unassigned_0x9C = 0x9C,         // Unassigned
            Unassigned_0x9D = 0x9D,         // Unassigned
            Unassigned_0x9E = 0x9E,         // Unassigned
            Unassigned_0x9F = 0x9F,         // Unassigned
            VK_LSHIFT = 0xA0,               // Left SHIFT key
            VK_RSHIFT = 0xA1,               // Right SHIFT key
            VK_LCONTROL = 0xA2,             // Left CONTROL key
            VK_RCONTROL = 0xA3,             // Right CONTROL key
            VK_LMENU = 0xA4,                // Left ALT key
            VK_RMENU = 0xA5,                // Right ALT key
            VK_BROWSER_BACK = 0xA6,         // Browser Back key
            VK_BROWSER_FORWARD = 0xA7,      // Browser Forward key
            VK_BROWSER_REFRESH = 0xA8,      // Browser Refresh key
            VK_BROWSER_STOP = 0xA9,         // Browser Stop key
            VK_BROWSER_SEARCH = 0xAA,       // Browser Search key
            VK_BROWSER_FAVORITES = 0xAB,    // Browser Favorites key
            VK_BROWSER_HOME = 0xAC,         // Browser Start and Home key
            VK_VOLUME_MUTE = 0xAD,          // Volume Mute key
            VK_VOLUME_DOWN = 0xAE,          // Volume Down key
            VK_VOLUME_UP = 0xAF,            // Volume Up key
            VK_MEDIA_NEXT_TRACK = 0xB0,     // Next Track key
            VK_MEDIA_PREV_TRACK = 0xB1,     // Previous Track key
            VK_MEDIA_STOP = 0xB2,           // Stop Media key
            VK_MEDIA_PLAY_PAUSE = 0xB3,     // Play/Pause Media key
            VK_LAUNCH_MAIL = 0xB4,          // Start Mail key
            VK_LAUNCH_MEDIA_SELECT = 0xB5,  // Select Media key
            VK_LAUNCH_APP1 = 0xB6,          // Start Application 1 key
            VK_LAUNCH_APP2 = 0xB7,          // Start Application 2 key
            Reserved_0xB8 = 0xB8,           // Reserved
            Reserved_0xB9 = 0xB9,           // Reserved
            VK_OEM_1 = 0xBA,                // Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the;: key
            VK_OEM_PLUS = 0xBB,             // For any country/region, the + key
            VK_OEM_COMMA = 0xBC,            // For any country/region, the, key
            VK_OEM_MINUS = 0xBD,            // For any country/region, the - key
            VK_OEM_PERIOD = 0xBE,           // For any country/region, the . key
            VK_OEM_2 = 0xBF,                // Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the /? key
            VK_OEM_3 = 0xC0,                // Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the `~ key
            Reserved_0xC1 = 0xC1,           // Reserved
            Reserved_0xC2 = 0xC2,           // Reserved
            Reserved_0xC3 = 0xC3,           // Reserved
            Reserved_0xC4 = 0xC4,           // Reserved
            Reserved_0xC5 = 0xC5,           // Reserved
            Reserved_0xC6 = 0xC6,           // Reserved
            Reserved_0xC7 = 0xC7,           // Reserved
            Reserved_0xC8 = 0xC8,           // Reserved
            Reserved_0xC9 = 0xC9,           // Reserved
            Reserved_0xCA = 0xCA,           // Reserved
            Reserved_0xCB = 0xCB,           // Reserved
            Reserved_0xCC = 0xCC,           // Reserved
            Reserved_0xCD = 0xCD,           // Reserved
            Reserved_0xCE = 0xCE,           // Reserved
            Reserved_0xCF = 0xCF,           // Reserved
            Reserved_0xD0 = 0xD0,           // Reserved
            Reserved_0xD1 = 0xD1,           // Reserved
            Reserved_0xD2 = 0xD2,           // Reserved
            Reserved_0xD3 = 0xD3,           // Reserved
            Reserved_0xD4 = 0xD4,           // Reserved
            Reserved_0xD5 = 0xD5,           // Reserved
            Reserved_0xD6 = 0xD6,           // Reserved
            Reserved_0xD7 = 0xD7,           // Reserved
            Reserved_0xD8 = 0xD8,           // Reserved
            Reserved_0xD9 = 0xD9,           // Reserved
            Reserved_0xDA = 0xDA,           // Reserved
            VK_OEM_4 = 0xDB,                // Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the [{ key
            VK_OEM_5 = 0xDC,                // Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the \\| key
            VK_OEM_6 = 0xDD,                // Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the]} key
            VK_OEM_7 = 0xDE,                // Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the '" key
            VK_OEM_8 = 0xDF,                // Used for miscellaneous characters; it can vary by keyboard.
            Reserved_0xE0 = 0xE0,           // Reserved
            OEM_specific_0xE1 = 0xE1,       // OEM specific
            VK_OEM_102 = 0xE2,              // The<> keys on the US standard keyboard, or the \\| key on the non-US 102-key keyboard
            OEM_specific_0xE3 = 0xE3,       // OEM specific
            OEM_specific_0xE4 = 0xE4,       // OEM specific
            VK_PROCESSKEY = 0xE5,           // IME PROCESS key
            OEM_specific_0xE6 = 0xE6,       // OEM specific
            VK_PACKET = 0xE7,               // Used to pass Unicode characters as if they were keystrokes.The VK_PACKET key is the low word of a 32-bit Virtual Key value used for non-keyboard input methods.For more information, see Remark in KEYBDINPUT, SendInput, WM_KEYDOWN, and WM_KEYUP
            Unassigned_0xE8 = 0xE8,         // Unassigned
            OEM_specific_0xE9 = 0xE9,       // OEM specific
            OEM_specific_0xEA = 0xEA,       // OEM specific
            OEM_specific_0xEB = 0xEB,       // OEM specific
            OEM_specific_0xEC = 0xEC,       // OEM specific
            OEM_specific_0xED = 0xED,       // OEM specific
            OEM_specific_0xEF = 0xEF,       // OEM specific
            OEM_specific_0xF0 = 0xF0,       // OEM specific
            OEM_specific_0xF1 = 0xF1,       // OEM specific
            OEM_specific_0xF2 = 0xF2,       // OEM specific
            OEM_specific_0xF3 = 0xF3,       // OEM specific
            OEM_specific_0xF4 = 0xF4,       // OEM specific
            OEM_specific_0xF5 = 0xF5,       // OEM specific
            VK_ATTN = 0xF6,                 // Attn key
            VK_CRSEL = 0xF7,                // CrSel key
            VK_EXSEL = 0xF8,                // ExSel key
            VK_EREOF = 0xF9,                // Erase EOF key
            VK_PLAY = 0xFA,                 // Play key
            VK_ZOOM = 0xFB,                 // Zoom key
            VK_NONAME = 0xFC,               // Reserved
            VK_PA1 = 0xFD,                  // PA1 key
            VK_OEM_CLEAR = 0xFE,            // Clear key
        }
    }




    [StructLayout(LayoutKind.Sequential)]
    public struct Input
    {
        public SendInputEventType type;
        public MouseAndKeyBoardInput mkhi;
    }

    [Flags]
    public enum SendInputEventType : uint
    {
        InputMouse,
        InputKeyboard,
        InputHardware
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct MouseAndKeyBoardInput
    {
        [FieldOffset(0)]
        public Mouse.MouseInput mi;

        [FieldOffset(0)]
        public Keyboard.KeyboardInput ki;

        [FieldOffset(0)]
        public HardwareInput hi;
    }








    [StructLayout(LayoutKind.Sequential)]
    public struct HardwareInput
    {
        public int uMsg;
        public short wParamL;
        public short wParamH;
    }



}