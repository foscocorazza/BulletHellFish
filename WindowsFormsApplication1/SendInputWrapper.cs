using System;
using System.Runtime.InteropServices;
using System.Threading;
using WindowsInput;
using InputTypes = BulletHellFish.TripletteList<WindowsInput.VirtualKeyCode, BulletHellFish.ScanCodeShort, string>;

namespace BulletHellFish
{
    class SendInputWrapper
    {

        [DllImport("user32.dll")]
        static extern int SendInput(int nInputs, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] INPUT[] pInputs, int cbSize);


        public static void Lift(VirtualKeyCode key, bool sendWithDirectX)
        {
            if (sendWithDirectX)
            {
                SendDirectXKey(GetScanCode(key), (int)KeyFlag.ScanCode | (int)KeyFlag.KeyUp);
            }
            else
            {
                InputSimulator.SimulateKeyUp(key);
            }
        }


        public static void PressCombo(VirtualKeyCode[] combo, int minHoldTime, int maxHoldTime, bool sendWithDirectX, Random r)
        {

            if (combo == null || combo.Length == 0)
            {
                Thread.Sleep(r.Next(minHoldTime, maxHoldTime));
            }
            else
            {
                if (sendWithDirectX)
                {

                    foreach (VirtualKeyCode key in combo) SendDirectXKey(GetScanCode(key), (int)KeyFlag.ScanCode | (int)KeyFlag.KeyDown);
                    Thread.Sleep(r.Next(minHoldTime, maxHoldTime));
                    foreach (VirtualKeyCode key in combo) SendDirectXKey(GetScanCode(key), (int)KeyFlag.ScanCode | (int)KeyFlag.KeyUp);
                }
                else
                {

                    foreach (VirtualKeyCode key in combo) InputSimulator.SimulateKeyDown(key);
                    Thread.Sleep(r.Next(minHoldTime, maxHoldTime));
                    foreach (VirtualKeyCode key in combo) InputSimulator.SimulateKeyUp(key);
                }
            }

        }

        private static void SendDirectXKey(ScanCodeShort Keycode, int flag)
        {
            INPUT[] InputData = new INPUT[1];

            InputData[0].type = 1;
            InputData[0].ki.wScan = (short)Keycode;
            InputData[0].ki.dwFlags = flag;
            InputData[0].ki.time = 0;
            InputData[0].ki.dwExtraInfo = IntPtr.Zero;

            SendInput(1, InputData, Marshal.SizeOf(typeof(INPUT)));
        }



        private static InputTypes _inputTypes;

        public static InputTypes InputTypes() {
            if (_inputTypes == null) {
                _inputTypes = new InputTypes();
                _inputTypes.Add(VirtualKeyCode.SPACE,   ScanCodeShort.SPACE, "SPACE");
                _inputTypes.Add(VirtualKeyCode.RETURN,  ScanCodeShort.RETURN,   "RETURN");

                _inputTypes.Add(VirtualKeyCode.VK_A,    ScanCodeShort.KEY_A,    "A");
                _inputTypes.Add(VirtualKeyCode.VK_B,    ScanCodeShort.KEY_B,    "B");
                _inputTypes.Add(VirtualKeyCode.VK_C,    ScanCodeShort.KEY_C,    "C");
                _inputTypes.Add(VirtualKeyCode.VK_D,    ScanCodeShort.KEY_D,    "D");
                _inputTypes.Add(VirtualKeyCode.VK_E,    ScanCodeShort.KEY_E,    "E");
                _inputTypes.Add(VirtualKeyCode.VK_F,    ScanCodeShort.KEY_F,    "F");
                _inputTypes.Add(VirtualKeyCode.VK_G,    ScanCodeShort.KEY_G,    "G");
                _inputTypes.Add(VirtualKeyCode.VK_H,    ScanCodeShort.KEY_H,    "H");
                _inputTypes.Add(VirtualKeyCode.VK_I,    ScanCodeShort.KEY_I,    "I");
                _inputTypes.Add(VirtualKeyCode.VK_J,    ScanCodeShort.KEY_J,    "J");
                _inputTypes.Add(VirtualKeyCode.VK_K,    ScanCodeShort.KEY_K,    "K");
                _inputTypes.Add(VirtualKeyCode.VK_L,    ScanCodeShort.KEY_L,    "L");
                _inputTypes.Add(VirtualKeyCode.VK_M,    ScanCodeShort.KEY_M,    "M");
                _inputTypes.Add(VirtualKeyCode.VK_N,    ScanCodeShort.KEY_N,    "N");
                _inputTypes.Add(VirtualKeyCode.VK_O,    ScanCodeShort.KEY_O,    "O");
                _inputTypes.Add(VirtualKeyCode.VK_P,    ScanCodeShort.KEY_P,    "P");
                _inputTypes.Add(VirtualKeyCode.VK_Q,    ScanCodeShort.KEY_Q,    "Q");
                _inputTypes.Add(VirtualKeyCode.VK_R,    ScanCodeShort.KEY_R,    "R");
                _inputTypes.Add(VirtualKeyCode.VK_S,    ScanCodeShort.KEY_S,    "S");
                _inputTypes.Add(VirtualKeyCode.VK_T,    ScanCodeShort.KEY_T,    "T");
                _inputTypes.Add(VirtualKeyCode.VK_U,    ScanCodeShort.KEY_U,    "U");
                _inputTypes.Add(VirtualKeyCode.VK_V,    ScanCodeShort.KEY_V,    "V");
                _inputTypes.Add(VirtualKeyCode.VK_W,    ScanCodeShort.KEY_W,    "W");
                _inputTypes.Add(VirtualKeyCode.VK_X,    ScanCodeShort.KEY_X,    "X");
                _inputTypes.Add(VirtualKeyCode.VK_Y,    ScanCodeShort.KEY_Y,    "Y");
                _inputTypes.Add(VirtualKeyCode.VK_Z,    ScanCodeShort.KEY_Z,    "Z");

                _inputTypes.Add(VirtualKeyCode.VK_0,    ScanCodeShort.KEY_0,    "0");
                _inputTypes.Add(VirtualKeyCode.VK_1,    ScanCodeShort.KEY_1,    "1");
                _inputTypes.Add(VirtualKeyCode.VK_2,    ScanCodeShort.KEY_2,    "2");
                _inputTypes.Add(VirtualKeyCode.VK_3,    ScanCodeShort.KEY_3,    "3");
                _inputTypes.Add(VirtualKeyCode.VK_4,    ScanCodeShort.KEY_4,    "4");
                _inputTypes.Add(VirtualKeyCode.VK_5,    ScanCodeShort.KEY_5,    "5");
                _inputTypes.Add(VirtualKeyCode.VK_6,    ScanCodeShort.KEY_6,    "6");
                _inputTypes.Add(VirtualKeyCode.VK_7,    ScanCodeShort.KEY_7,    "7");
                _inputTypes.Add(VirtualKeyCode.VK_8,    ScanCodeShort.KEY_8,     "8");
                _inputTypes.Add(VirtualKeyCode.VK_9,    ScanCodeShort.KEY_9,    "9");

                _inputTypes.Add(VirtualKeyCode.UP,      ScanCodeShort.UP,       "UP");
                _inputTypes.Add(VirtualKeyCode.DOWN,    ScanCodeShort.DOWN,     "DOWN");
                _inputTypes.Add(VirtualKeyCode.LEFT,    ScanCodeShort.LEFT,     "LEFT");
                _inputTypes.Add(VirtualKeyCode.RIGHT,   ScanCodeShort.RIGHT,    "RIGHT");


            }
            return _inputTypes;
        }



        public static ScanCodeShort GetScanCode(VirtualKeyCode key)
        {
            return InputTypes().Get(key).Second;
        }

        public static VirtualKeyCode GetVKeyCode(String key)
        {
            return InputTypes().Get(key).First;
        }

    }


    [StructLayout(LayoutKind.Explicit)]
    struct INPUT
    {
        [FieldOffset(0)]
        public int type;
        [FieldOffset(4)]
        public MOUSEINPUT mi;
        [FieldOffset(4)]
        public KEYBDINPUT ki;
        [FieldOffset(4)]
        public HARDWAREINPUT hi;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public int mouseData;
        public int dwFlags;
        public int time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct KEYBDINPUT
    {
        public short wVk;
        public short wScan;
        public int dwFlags;
        public int time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct HARDWAREINPUT
    {
        public int uMsg;
        public short wParamL;
        public short wParamH;
    }


    [Flags]
    public enum KeyFlag
    {
        KeyDown = 0x0000,
        ExtendedKey = 0x0001,
        KeyUp = 0x0002,
        UniCode = 0x0004,
        ScanCode = 0x0008
    }

    internal enum ScanCodeShort : short
    {
        LBUTTON = 0,
        RBUTTON = 0,
        CANCEL = 70,
        MBUTTON = 0,
        XBUTTON1 = 0,
        XBUTTON2 = 0,
        BACK = 14,
        TAB = 15,
        CLEAR = 76,
        RETURN = 28,
        SHIFT = 42,
        CONTROL = 29,
        MENU = 56,
        PAUSE = 0,
        CAPITAL = 58,
        KANA = 0,
        HANGUL = 0,
        JUNJA = 0,
        FINAL = 0,
        HANJA = 0,
        KANJI = 0,
        ESCAPE = 1,
        CONVERT = 0,
        NONCONVERT = 0,
        ACCEPT = 0,
        MODECHANGE = 0,
        SPACE = 57,
        PRIOR = 73,
        NEXT = 81,
        END = 79,
        HOME = 71,
        LEFT = 203,//75 + 128,//203,
        UP = 200,//72 + 128,//200,
        RIGHT = 205,// 77 + 128,//205,
        DOWN = 208,//80 + 128,//208,
        SELECT = 0,
        PRINT = 0,
        EXECUTE = 0,
        SNAPSHOT = 84,
        INSERT = 82,
        DELETE = 83,
        HELP = 99,
        KEY_0 = 11,
        KEY_1 = 2,
        KEY_2 = 3,
        KEY_3 = 4,
        KEY_4 = 5,
        KEY_5 = 6,
        KEY_6 = 7,
        KEY_7 = 8,
        KEY_8 = 9,
        KEY_9 = 10,
        KEY_A = 30,
        KEY_B = 48,
        KEY_C = 46,
        KEY_D = 32,
        KEY_E = 18,
        KEY_F = 33,
        KEY_G = 34,
        KEY_H = 35,
        KEY_I = 23,
        KEY_J = 36,
        KEY_K = 37,
        KEY_L = 38,
        KEY_M = 50,
        KEY_N = 49,
        KEY_O = 24,
        KEY_P = 25,
        KEY_Q = 16,
        KEY_R = 19,
        KEY_S = 31,
        KEY_T = 20,
        KEY_U = 22,
        KEY_V = 47,
        KEY_W = 17,
        KEY_X = 45,
        KEY_Y = 21,
        KEY_Z = 44,
        LWIN = 91,
        RWIN = 92,
        APPS = 93,
        SLEEP = 95,
        NUMPAD0 = 82,
        NUMPAD1 = 79,
        NUMPAD2 = 80,
        NUMPAD3 = 81,
        NUMPAD4 = 75,
        NUMPAD5 = 76,
        NUMPAD6 = 77,
        NUMPAD7 = 71,
        NUMPAD8 = 72,
        NUMPAD9 = 73,
        MULTIPLY = 55,
        ADD = 78,
        SEPARATOR = 0,
        SUBTRACT = 74,
        DECIMAL = 83,
        DIVIDE = 53,
        F1 = 59,
        F2 = 60,
        F3 = 61,
        F4 = 62,
        F5 = 63,
        F6 = 64,
        F7 = 65,
        F8 = 66,
        F9 = 67,
        F10 = 68,
        F11 = 87,
        F12 = 88,
        F13 = 100,
        F14 = 101,
        F15 = 102,
        F16 = 103,
        F17 = 104,
        F18 = 105,
        F19 = 106,
        F20 = 107,
        F21 = 108,
        F22 = 109,
        F23 = 110,
        F24 = 118,
        NUMLOCK = 69,
        SCROLL = 70,
        LSHIFT = 42,
        RSHIFT = 54,
        LCONTROL = 29,
        RCONTROL = 29,
        LMENU = 56,
        RMENU = 56,
        BROWSER_BACK = 106,
        BROWSER_FORWARD = 105,
        BROWSER_REFRESH = 103,
        BROWSER_STOP = 104,
        BROWSER_SEARCH = 101,
        BROWSER_FAVORITES = 102,
        BROWSER_HOME = 50,
        VOLUME_MUTE = 32,
        VOLUME_DOWN = 46,
        VOLUME_UP = 48,
        MEDIA_NEXT_TRACK = 25,
        MEDIA_PREV_TRACK = 16,
        MEDIA_STOP = 36,
        MEDIA_PLAY_PAUSE = 34,
        LAUNCH_MAIL = 108,
        LAUNCH_MEDIA_SELECT = 109,
        LAUNCH_APP1 = 107,
        LAUNCH_APP2 = 33,
        OEM_1 = 39,
        OEM_PLUS = 13,
        OEM_COMMA = 51,
        OEM_MINUS = 12,
        OEM_PERIOD = 52,
        OEM_2 = 53,
        OEM_3 = 41,
        OEM_4 = 26,
        OEM_5 = 43,
        OEM_6 = 27,
        OEM_7 = 40,
        OEM_8 = 0,
        OEM_102 = 86,
        PROCESSKEY = 0,
        PACKET = 0,
        ATTN = 0,
        CRSEL = 0,
        EXSEL = 0,
        EREOF = 93,
        PLAY = 0,
        ZOOM = 98,
        NONAME = 0,
        PA1 = 0,
        OEM_CLEAR = 0,
    }
}
