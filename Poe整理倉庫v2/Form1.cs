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
using DataGetter;
using LiteDB;

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
        private bool Loaded = false;
        private IntPtr poeHwnd = IntPtr.Zero;
        private List<Data> ItemList = new List<Data>();
        private List<Data> ItemList_Adden = new List<Data>();
        private List<Data> ItemList_Unique = new List<Data>();
        private List<Item> Items = new List<Item>();
        private List<POINT> used = new List<POINT>();
        private List<Item> resoult = new List<Item>();
        private Bitmap RegionImage = new Bitmap(480, 480);
        private Bitmap RegionImage2 = new Bitmap(480, 480);

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

        private void GetItem(IntPtr hwnd, int x, int y)
        {
            MouseTools.SetCursorPosition(x, y);
            Thread.Sleep(Config.Delay2);
            KeyBoardTool.KeyDown(Keys.ControlKey);
            Thread.Sleep(100);
            SimulateMouseLeft(poeHwnd, x, y);
            Thread.Sleep(100);
            KeyBoardTool.KeyUp(Keys.ControlKey);
            Thread.Sleep(Config.Delay1);
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
                MessageBox.Show("還未載入完全\r\nNot loaded");
                return;
            }
            poeHwnd = ApplicationHelper.OpenPathOfExile();
            if (poeHwnd == IntPtr.Zero)
            {
                MessageBox.Show("未偵測到Path Of Exile\r\nCould not find Path Of Exile");
                return;
            }
            GetStashDimentions();

            MouseTools.SetCursorPosition(startPos1.X, startPos1.Y - (int)cellHeight1 * 2);
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
                MessageBox.Show("還未載入完全\r\nNot loaded.");
                return;
            }
            poeHwnd = ApplicationHelper.OpenPathOfExile();
            if (poeHwnd == IntPtr.Zero)
            {
                MessageBox.Show("未偵測到Path Of Exile\r\nCould not find Path Of Exile");
                return;
            }
            GetStashDimentions();
            MouseTools.SetCursorPosition(startPos1.X, startPos1.Y - (int)cellHeight1 * 3);
            MouseTools.MouseClickEvent(70);
            MouseTools.MouseClickEvent(70);
            Task.Delay(500);
            if (radioButton1.Checked)
                StartSorting((radioButton4.Checked ? 12 : 24));
            else
            {
                Round40Q((radioButton4.Checked ? 12 : 24), radioButton2.Checked ? 0 : 1);
                resoult = Sort(Items, used, (radioButton4.Checked ? 12 : 24));
                DrawBoxRegion(Items, (radioButton4.Checked ? 12 : 24), 1);
                DrawBoxRegion(resoult, (radioButton4.Checked ? 12 : 24), 2);
            }
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
                        var response = client.GetAsync("https://raw.githubusercontent.com/flier268/POE-Stash-Sorter-v2/master/Poe%E6%95%B4%E7%90%86%E5%80%89%E5%BA%ABv2/Properties/AssemblyInfo.cs").Result;

                        if (response.IsSuccessStatusCode)
                        {
                            // by calling .Result you are performing a synchronous call
                            var responseContent = response.Content;

                            // by calling .Result you are synchronously reading the result
                            string responseString = responseContent.ReadAsStringAsync().Result;
                            Regex r = new Regex(@"\[assembly: AssemblyVersion.*?([\d|\.]+).*?\]", RegexOptions.IgnoreCase);
                            var ms = r.Matches(responseString);
                            var m = ms[ms.Count - 1];
                            Version ver = new Version(m.Groups[1].ToString());
                            Version verson = Assembly.GetEntryAssembly().GetName().Version;
                            int tm = verson.CompareTo(ver);

                            if (tm >= 0)
                            {
                                ChangeControlText("Up to Date.", linkLabel1);
                            }
                            else
                            {
                                ChangeControlText("Update Available", linkLabel1);
                                MessageBox.Show("發現新版本！請點擊右下角連結到Github更新\nNew version available. Click to download the update from GitHub.");
                            }
                        }
                    }
                }
                catch
                {
                    ChangeControlText("Error checking for update.", linkLabel1);
                }
            });
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (linkLabel1.Text == "Update available" || linkLabel1.Text == "Error checking for update.")
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
            radioButton_langTW.CheckedChanged -= radioButton8_CheckedChanged;
            radioButton_langEN.CheckedChanged -= radioButton9_CheckedChanged;
            if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name == "en")
                radioButton_langEN.Checked = true;
            else
                radioButton_langTW.Checked = true;

            radioButton_langTW.CheckedChanged += radioButton8_CheckedChanged;
            radioButton_langEN.CheckedChanged += radioButton9_CheckedChanged;
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
            radioButton_langTW.CheckedChanged -= radioButton8_CheckedChanged;
            radioButton_langEN.CheckedChanged -= radioButton9_CheckedChanged;
            if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name == "en")
                radioButton_langEN.Checked = true;
            else
                radioButton_langTW.Checked = true;

            radioButton_langTW.CheckedChanged += radioButton8_CheckedChanged;
            radioButton_langEN.CheckedChanged += radioButton9_CheckedChanged;
            CheckUpdate();
        }

        public struct table
        {
            public int[] list;
            public int total;
        }

        public class path
        {
            public bool end;
            public int[] total;
            public int totalHashCode;
            public int id, count, age, mom;
        }

        private class pathComparer_age_count : IComparer<path>
        {
            // 遞增排序
            public int Compare(path x, path y)
            {
                int temp = y.age.CompareTo(x.age);
                if (!temp.Equals(0))
                    temp = y.count.CompareTo(x.count);
                return temp;
            }
        }

        private class pathComparer_end : IComparer<path>
        {
            // 遞增排序
            public int Compare(path x, path y)
            {
                return y.end.CompareTo(x.end);
            }
        }

        private List<int[]> FindAnswer(ref List<int[]> Data, int[] Q)
        {
            var temp = Data.Where(x => x[1] <= Q[1]).Where(x => x[2] <= Q[2]).Where(x => x[3] <= Q[3]).Where(x => x[4] <= Q[4]).Where(x => x[5] <= Q[5])
                .Where(x => x[6] <= Q[6]).Where(x => x[7] <= Q[7]).Where(x => x[8] <= Q[8]).Where(x => x[9] <= Q[9]).Where(x => x[10] <= Q[10])
                .Where(x => x[11] <= Q[11]).Where(x => x[12] <= Q[12]).Where(x => x[13] <= Q[13]).Where(x => x[14] <= Q[14]).Where(x => x[15] <= Q[15])
                .Where(x => x[16] <= Q[16]).Where(x => x[17] <= Q[17]).Where(x => x[18] <= Q[18]).Where(x => x[19] <= Q[19]).Where(x => x[0] <= Q[0]);
            return temp.ToList();
        }

        public int CalcArrayTotal(int[] array)
        {
            int total = 0;
            foreach (var t in array)
                total += t;
            return total;
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void ShowItemInfo(object sender, MouseEventArgs e)
        {
            var showwhat = ((PictureBox)sender) == pictureBox1 ? pictureBox1 : pictureBox2;
            var temp = ((PictureBox)sender) == pictureBox1 ? Items : resoult;
            Point p = new Point(((int)Math.Floor((double)e.X / 40 * (radioButton4.Checked ? 1 : 2))), ((int)Math.Floor((double)e.Y / 40 * (radioButton4.Checked ? 1 : 2))));
            var item = temp.Where(x => PrivateFunction.IsWithin(p, x.point, Point.Add(x.point, new Size(x.w - 1, x.h - 1)))).FirstOrDefault();
            // toolTip1.SetToolTip(showwhat, String.Format("{0},{1}", e.X, e.Y));
            if (item != null)
            {
                if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name == "en")
                    toolTip1.SetToolTip(showwhat, String.Format("Name:{0}\nItem level:{1}\nQuality:{2}\nRarity:{3}\nLevel:{4}", item.Name_eng, item.itemlevel, item.quality, item.Rarity, item.level));
                else
                    toolTip1.SetToolTip(showwhat, String.Format("物品名稱:{0}\n物品等級:{1}\n品質:{2}\n稀有度:{3}\n等級:{4}", item.Name, item.itemlevel, item.quality, item.Rarity, item.level));
            }
            else
                toolTip1.SetToolTip(showwhat, "");
        }

        private async void ItemList_Load()
        {
            await Task.Delay(0);
            if (!File.Exists(Path.Combine(Application.StartupPath, "Datas.db")))
            {
                MessageBox.Show("找不到Datas.db，請確認是否解壓縮完整\r\nCan't find Datas.db, please check that the zip file uncompressed properly.");
                return;
            }

            ItemList = DataRepository.Find(x => x.Rarity == 0).ToList();
            ItemList_Unique = DataRepository.Find(x => x.Rarity == 1).ToList();

            string databasePath = Path.Combine(Application.StartupPath, "Datas_Adden.db");
            if (File.Exists(databasePath))
            {
                using (var db = new LiteDatabase(databasePath))
                {
                    var col = db.GetCollection<Data>();
                    ItemList_Adden = col.FindAll().ToList();
                }
            }
            Loaded = true;
        }

        private void GetStashDimentions()
        {
            RECT rect = ApplicationHelper.PathOfExileDimentions;
            int width = rect.Right - rect.Left;
            int height = rect.Bottom - rect.Top;
            // 第0-0格的座標
            (decimal X, decimal Y) firstSlotPoint = (52, 204);
            // 第1-1格的座標
            (decimal X, decimal Y) _1_1SlotPoint = (123, 279);
            (decimal X, decimal Y) ScreenSize = (2560, 1440);

            decimal startX = height * firstSlotPoint.X / ScreenSize.Y;
            decimal startY = height * firstSlotPoint.Y / ScreenSize.Y;
            cellHeight1 = (float)(height * (_1_1SlotPoint.X - firstSlotPoint.X) / ScreenSize.Y);
            cellWidth1 = cellHeight1;
            startPos1 = new Point(rect.Left + (int)startX, rect.Top + (int)startY);

            cellHeight4 = cellHeight1 / 2;
            cellWidth4 = cellWidth1 / 2;
            startPos4 = new Point((int)(startPos1.X - cellWidth4 / 4), (int)(startPos1.Y - cellHeight4 / 4));

            bagstartPos = new Point((int)(rect.Right - height * 0.04f - cellWidth1 * 11), (int)(rect.Bottom - height * 0.233333f - cellHeight1 * 4));
        }

        private void GlobalKeyDown(object sender, Flier.SuperTools.Hook.KeyBoard.Global_Hook.KeyEventArgsEx e)
        {
            if (Config.HotkeyScan != 0)
                if (e.KeyValue == Config.HotkeyScan)
                {
                    Stop = false;
                    button_ReLoadBox.PerformClick();
                }
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