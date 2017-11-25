using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
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
        List<RootObject> ItemList_Adden = new List<RootObject>();
        List<RootObject> ItemList_Unique = new List<RootObject>();
        List<Item> Items = new List<Item>();
        List<POINT> used = new List<POINT>();
        List<Item> resoult = new List<Item>();
        Bitmap RegionImage = new Bitmap(480, 480);
        Bitmap RegionImage2 = new Bitmap(480, 480);

        private delegate void myUICallBack_ControlText(string myStr, Control ctl);
        private void ChangeControlText(string myStr, Control ctl)
        {
            if (this.InvokeRequired)
            {
                myUICallBack_ControlText myUpdate = new myUICallBack_ControlText(ChangeControlText);
                this.Invoke(myUpdate, myStr, ctl);
            }
            else
            {
                ctl.Text = myStr;
            }
        }
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
                MessageBox.Show("還未載入完全\r\nDoes not loaded.");
                return;
            }
            poeHwnd = ApplicationHelper.OpenPathOfExile();
            if (poeHwnd == IntPtr.Zero)
            {
                MessageBox.Show("未偵測到Path Of Exile\r\nDid't find Path Of Exile");
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
                MessageBox.Show("還未載入完全\r\nDoes not loaded.");
                return;
            }
            poeHwnd = ApplicationHelper.OpenPathOfExile();
            if (poeHwnd == IntPtr.Zero)
            {
                MessageBox.Show("未偵測到Path Of Exile\r\nDid't find Path Of Exile");
                return;
            }
            GetStashDimentions();
            MouseTools.SetCursorPosition(startPos1.X, startPos1.Y - (int)cellHeight1 * 3);
            MouseTools.MouseClickEvent(70);
            MouseTools.MouseClickEvent(70);
            Task.Delay(500);
            StartSorting((radioButton4.Checked ? 12 : 24));
        }

        private void button_CheckUpdate_Click(object sender, EventArgs e)
        {
            CheckUpdate();
        }
        private void CheckUpdate()
        {
            var task = Task.Run(() =>
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        var response = client.GetAsync("https://github.com/flier268/POE-Stash-Sorter-v2/blob/master/Poe%E6%95%B4%E7%90%86%E5%80%89%E5%BA%ABv2/Properties/AssemblyInfo.cs").Result;

                        if (response.IsSuccessStatusCode)
                        {
                            // by calling .Result you are performing a synchronous call
                            var responseContent = response.Content;

                            // by calling .Result you are synchronously reading the result
                            string responseString = responseContent.ReadAsStringAsync().Result;
                            Regex r = new Regex(@"[^/\s]\[assembly: AssemblyVersion.*?([\d|\.]+).*?\]", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                            var m = r.Match(responseString);

                            Version ver = new Version(m.Groups[1].ToString());
                            Version verson = Assembly.GetEntryAssembly().GetName().Version;
                            int tm = verson.CompareTo(ver);

                            if (tm >= 0)
                            {
                                ChangeControlText("It is newst.", linkLabel1);
                            }
                            else
                            {
                                ChangeControlText("Have update", linkLabel1);
                            }
                        }
                    }
                }
                catch
                {
                    ChangeControlText("Check error.", linkLabel1);
                }
            });
            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (linkLabel1.Text == "Have update" || linkLabel1.Text == "Check error.")
                System.Diagnostics.Process.Start("https://github.com/flier268/POE-Stash-Sorter-v2/releases/latest");
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {            
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("zh-TW");
            for (int i = Controls.Count - 1; i > 0; i--)
            {
                Controls[i].Dispose();
            }
            InitializeComponent();
            radioButton8.CheckedChanged -= radioButton8_CheckedChanged;
            radioButton9.CheckedChanged -= radioButton9_CheckedChanged;
            if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name == "en")
                radioButton9.Checked = true;
            else
                radioButton8.Checked = true;

            radioButton8.CheckedChanged += radioButton8_CheckedChanged;
            radioButton9.CheckedChanged += radioButton9_CheckedChanged;
            CheckUpdate();
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            for (int i = Controls.Count - 1; i > 0; i--)
            {
                Controls[i].Dispose();
            }
            InitializeComponent();
            radioButton8.CheckedChanged -= radioButton8_CheckedChanged;
            radioButton9.CheckedChanged -= radioButton9_CheckedChanged;
            if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name == "en")
                radioButton9.Checked = true;
            else
                radioButton8.Checked = true;

            radioButton8.CheckedChanged += radioButton8_CheckedChanged;
            radioButton9.CheckedChanged += radioButton9_CheckedChanged;
            CheckUpdate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int n = 0;

            List<int> list = new List<int>(){ 13, 3, 4, 10, 9, 19, 10, 10, 13, 1, 4, 8, 7, 1, 16, 13, 10, 11, 12, 6, 13, 10, 9, 10, 14, 18, 7, 15, 6, 12, 4, 15, 5, 11, 17, 17, 13, 19, 4, 17, 11, 12, 5, 4, 16, 4, 15, 2, 9, 4 };
            int r = 0;
            int c = list.Count;
            int[] array = new int[40];

            while (array[39] != list[c - 1] && r != 39)
            {
                a:;
                array[r] = array[r] + 1;
                while (array[r] >= c)
                {
                    array[r] = 0;
                    r++;
                    array[r] = array[r] + 1;
                }
                r = 0;
                if (array.Distinct().ToList().Count + array.Where(x => x == 0).ToList().Count != array.Count())
                    goto a;
                int sum = 0;
                array.ToList().ForEach(x => sum += list[x]);
                if(sum>40 && sum<=40+n)
                {
                    List<int> temp = new List<int>();
                    array.ToList().ForEach(x => temp.Add(list[x]));
                    MessageBox.Show(temp.ToArray().ToString());
                }
            }
        }

        private async void ItemList_Load()
        {
            await Task.Delay(0);
            if (!File.Exists(Path.Combine(Application.StartupPath, "ItemList.txt")) || !File.Exists(Path.Combine(Application.StartupPath, "ItemList_Unique.txt")))
            {
                MessageBox.Show("找不到ItemList.txt或ItemList_Unique.txt，請確認是否解壓縮完整\r\nCan't find ItemList.txt or ItemList_Unique.txt,please check full unzip or not.");
                return;
            }
            int stats = 0;
            using (StreamReader r = new StreamReader(Path.Combine(Application.StartupPath, "ItemList.txt"), Encoding.UTF8))
            {
                ItemList = JsonConvert.DeserializeObject<List<JsonClass.RootObject>>(r.ReadToEnd());
                if (!ItemList.Count.Equals(0))
                    stats = stats + 1;
            }
            if (File.Exists(Path.Combine(Application.StartupPath, "ItemList_Adden.txt")))
                using (StreamReader r = new StreamReader(Path.Combine(Application.StartupPath, "ItemList_Adden.txt"), Encoding.UTF8))
                {
                    ItemList_Adden = JsonConvert.DeserializeObject<List<JsonClass.RootObject>>(r.ReadToEnd());
                }
            if (ItemList_Adden == null)
                ItemList_Adden = new List<RootObject>();
            using (StreamReader r = new StreamReader(Path.Combine(Application.StartupPath, "ItemList_Unique.txt"), Encoding.UTF8))
            {
                ItemList_Unique = JsonConvert.DeserializeObject<List<JsonClass.RootObject>>(r.ReadToEnd());
                if (!ItemList_Unique.Count.Equals(0))
                    stats = stats + 3;
            }
            switch (stats)
            {
                case 0:
                    MessageBox.Show("ItemList.txt與ItemList_Unique.txt讀取失敗，請確認後再重新啟動程式\r\nCan not load ItemList.txt and ItemList_Unique.txt,check and restart,please.");
                    break;
                case 1:
                    MessageBox.Show("ItemList.txt讀取失敗，請確認後再重新啟動程式\r\nCan not load ItemList.txt,check and restart,please.");
                    break;
                case 3:
                    MessageBox.Show("ItemList_Unique.txt讀取失敗，請確認後再重新啟動程式\r\nCan not load ItemList_Unique.txt,check and restart,please.");
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
