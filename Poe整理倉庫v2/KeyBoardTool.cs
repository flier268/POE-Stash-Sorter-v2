using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Poe整理倉庫v2
{
    class KeyBoardTool
    {
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, IntPtr dwExtraInfo);

        //定義數值        
        const byte KEYEVENTF_KEYUP = 0x02;

        public static void KeyDown(Keys key)
        {
            keybd_event((byte)key, 0, 0, (IntPtr)0);
        }
        public static void KeyUp(Keys key)
        {
            keybd_event((byte)key, 0, KEYEVENTF_KEYUP, (IntPtr)0);
        }
    }
}