using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Poe整理倉庫v2.JsonClass;

namespace Poe整理倉庫v2
{
    public partial class Form1 : Form
    {

        #region 全域變數
        internal static Setting Config = new Setting();
        private float cellHeight1, cellHeight4;
        private float cellWidth1, cellWidth4;
        private Point startPos1, startPos4;
        private Point bagstartPos;
        bool Loaded = false;
        IntPtr poeHwnd = IntPtr.Zero;
        List<RootObject> ItemList = new List<RootObject>();
        List<RootObject> ItemList_Unique = new List<RootObject>();
        List<Item> Items = new List<Item>();
        List<POINT> used = new List<POINT>();
        List<Item> resoult = new List<Item>();
        Bitmap RegionImage = new Bitmap(480, 480);
        Bitmap RegionImage2 = new Bitmap(480, 480);
        #endregion 全域變數

        #region subfunction_MoveItem

        private const int WM_LBUTTONDOWN = 0x201; //Left mousebutton down
        private const int WM_LBUTTONUP = 0x202;   //Left mousebutton up
        private bool Stop = true;

        [DllImport("user32.dll", EntryPoint = "PostMessageA", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hwnd, uint Msg, IntPtr wParam, IntPtr lParam);

        public void SimulateMouseLeft(IntPtr hwnd, int x, int y)
        {
            int coordinates = x | (y << 16);
            PostMessage(hwnd, WM_LBUTTONDOWN, (IntPtr)0x1, (IntPtr)coordinates);
            PostMessage(hwnd, WM_LBUTTONUP, (IntPtr)0x1, (IntPtr)coordinates);
        }
        private void ClickItem(IntPtr hwnd, int x, int y)
        {
            MouseTools.SetCursorPosition(x, y);
            Thread.Sleep(Config.Delay2);
            SimulateMouseLeft(poeHwnd, x, y);
            Thread.Sleep(Config.Delay1);
        }
        #endregion subfunction_MoveItem

        public Form1()
        {
            InitializeComponent();
        }
        private void button_Setting_Click(object sender, EventArgs e)
        {
            Form3 f = new Form3();
            f.ShowDialog();
            using (StreamReader r = new StreamReader(Path.ChangeExtension(Application.ExecutablePath, ".cfg"), Encoding.UTF8))
            {
                Config = Setting.FromJson(r.ReadToEnd());
            }
            resoult = Sort(Items, used, (radioButton4.Checked ? 12 : 24));
            DrawBoxRegion(resoult, (radioButton4.Checked ? 12 : 24), 2);

        }
        private void button_ReLoadBox_Click(object sender, EventArgs e)
        {
            Stop = false;
            if (!Loaded)
            {
                MessageBox.Show("還未載入完全");
                return;
            }
            poeHwnd = ApplicationHelper.OpenPathOfExile();
            if (poeHwnd == IntPtr.Zero)
            {
                MessageBox.Show("未偵測到Path Of Exile");
                return;
            }
            GetStashDimentions();

            MouseTools.SetCursorPosition(startPos1.X, startPos1.Y - (int)cellHeight1 * 3);
            MouseTools.MouseClickEvent(70);
            MouseTools.MouseClickEvent(70);
            Task.Delay(500);


            GetWarehouse(radioButton4.Checked ? 12 : 24);

            resoult = Sort(Items, used, (radioButton4.Checked ? 12 : 24));
            DrawBoxRegion(resoult, (radioButton4.Checked ? 12 : 24), 2);
            DrawBoxRegion(Items, (radioButton4.Checked ? 12 : 24), 1);
        }

        private void button_StartSort_Click(object sender, EventArgs e)
        {
            Stop = false;
            if (!Loaded)
            {
                MessageBox.Show("還未載入完全");
                return;
            }
            poeHwnd = ApplicationHelper.OpenPathOfExile();
            if (poeHwnd == IntPtr.Zero)
            {
                MessageBox.Show("未偵測到Path Of Exile");
                return;
            }
            GetStashDimentions();
            MouseTools.SetCursorPosition(startPos1.X, startPos1.Y - (int)cellHeight1 * 3);
            MouseTools.MouseClickEvent(70);
            MouseTools.MouseClickEvent(70);
            Task.Delay(500);
            StartSorting((radioButton4.Checked ? 12 : 24));
        }



        private async void ItemList_Load()
        {
            await Task.Delay(0);
            if (!File.Exists(Path.Combine(Application.StartupPath, "ItemList.txt")) || !File.Exists(Path.Combine(Application.StartupPath, "ItemList_Unique.txt")))
            {
                MessageBox.Show("找不到ItemList.txt和ItemList_Unique.txt");
                return;
            }
            int stats = 0;
            using (StreamReader r = new StreamReader(Path.Combine(Application.StartupPath, "ItemList.txt"), Encoding.UTF8))
            {
                ItemList = JsonConvert.DeserializeObject<List<JsonClass.RootObject>>(r.ReadToEnd());
                if (!ItemList.Count.Equals(0))
                    stats = stats + 1;
            }
            using (StreamReader r = new StreamReader(Path.Combine(Application.StartupPath, "ItemList_Unique.txt"), Encoding.UTF8))
            {
                ItemList_Unique = JsonConvert.DeserializeObject<List<JsonClass.RootObject>>(r.ReadToEnd());
                if (!ItemList_Unique.Count.Equals(0))
                    stats = stats + 3;
            }
            switch (stats)
            {
                case 0:
                    MessageBox.Show("ItemList.txt與ItemList_Unique.txt讀取失敗，請確認後再重新啟動程式");
                    break;
                case 1:
                    MessageBox.Show("ItemList.txt讀取失敗，請確認後再重新啟動程式");
                    break;
                case 3:
                    MessageBox.Show("ItemList_Unique.txt讀取失敗，請確認後再重新啟動程式");
                    break;
                case 4:
                    Loaded = true;
                    break;
            }
        }
        private void GetStashDimentions()
        {
            RECT rect = ApplicationHelper.PathOfExileDimentions;
            int width = rect.Right - rect.Left;
            int height = rect.Bottom - rect.Top;

            float startX = height * 0.033f;
            float startY = height * 0.1783f;
            cellHeight1 = height * 0.0484f;
            cellWidth1 = cellHeight1;
            startPos1 = new Point(rect.Left + (int)startX, rect.Top + (int)startY);

            cellHeight4 = cellHeight1 / 2;
            cellWidth4 = cellWidth1 / 2;
            startPos4 = new Point((int)(startPos1.X - cellWidth4/4), (int)(startPos1.Y - cellHeight4/4));

            bagstartPos = new Point((int)(rect.Right - height * 0.04f - cellWidth1 * 11), (int)(rect.Bottom - height * 0.233333f - cellHeight1 * 4));
        }
        private void GlobalKeyDown(object sender, Flier.SuperTools.Hook.KeyBoard.Global_Hook.KeyEventArgsEx e)
        {
            if (Config.HotkeyStart != 0)
                if (e.KeyValue == Config.HotkeyStart)
                {
                    Stop = false;
                    button_StartSort.PerformClick();
                }

            if (Config.HotkeyStop != 0)
                if (e.KeyValue == Config.HotkeyStop)
                    Stop = true;
        }
    }
}
