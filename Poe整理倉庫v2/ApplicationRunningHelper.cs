using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Poe整理倉庫v2
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public POINT(POINT p)
        {
            this.X = p.X;
            this.Y = p.Y;
        }

        public POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public POINT(System.Drawing.Point pt) : this(pt.X, pt.Y)
        {
        }

        public static implicit operator System.Drawing.Point(POINT p)
        {
            return new System.Drawing.Point(p.X, p.Y);
        }

        public static implicit operator POINT(System.Drawing.Point p)
        {
            return new POINT(p.X, p.Y);
        }

        public override bool Equals(object obj)
        {
            if (((POINT)obj).X == X && ((POINT)obj).Y == Y)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class ApplicationHelper
    {
        public static Process currentProcess;

        [DllImport("user32.dll")]
        public static extern
            bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern
            bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern
            bool IsIconic(IntPtr hWnd);

        public static IntPtr OpenPathOfExile()
        {
            const int swRestore = 9;
            var arrProcesses = Process.GetProcessesByName("PathOfExile");
            if (arrProcesses.Length <= 0)
                arrProcesses = Process.GetProcessesByName("PathOfExile_x64");
            if (arrProcesses.Length <= 0)
                arrProcesses = Process.GetProcessesByName("PathOfExileSteam");
            if (arrProcesses.Length <= 0)
                arrProcesses = Process.GetProcessesByName("PathOfExile_x64Steam");
            if (arrProcesses.Length > 0)
            {
                currentProcess = arrProcesses[0];
                IntPtr hWnd = arrProcesses[0].MainWindowHandle;
                if (IsIconic(hWnd))
                    ShowWindowAsync(hWnd, swRestore);
                SetForegroundWindow(hWnd);
                return hWnd;
            }
            return IntPtr.Zero;
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetWindowRect(IntPtr hWnd, ref RECT Rect);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetClientRect(IntPtr hWnd, ref RECT Rect);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int Width, int Height, bool Repaint);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int ScreenToClient(IntPtr hWnd, out POINT pt);

        public static RECT PathOfExileDimentions
        {
            get
            {
                RECT clientRect = new RECT();
                GetClientRect(currentProcess.MainWindowHandle, ref clientRect);

                POINT point;
                ScreenToClient(currentProcess.MainWindowHandle, out point);

                RECT rect = new RECT();
                rect.Left = point.X * -1 + clientRect.Left;
                rect.Right = point.X * -1 + clientRect.Right;
                rect.Top = point.Y * -1 + clientRect.Top;
                rect.Bottom = point.Y * -1 + clientRect.Bottom;

                return rect;
            }
        }
    }
}