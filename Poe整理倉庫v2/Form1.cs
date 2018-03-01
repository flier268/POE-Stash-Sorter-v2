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
                                MessageBox.Show("發現新版本！請點擊右下角連結到Github更新\nFound new version,click right and down to connect to Github to update,please.");
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
            for (int i = Controls.Count-1 ; i > 0; i--)
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
                if(!temp.Equals(0))
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
        List<int[]> FindAnswer(ref List<int[]> Data, int[] Q)
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
        //private int[] ConvertTo20Carry(int numver)

        private void button1_Click(object sender, EventArgs e)
        {
            var temps = PrivateFunction.CheckCarry(new List<int>() { 19, 101, 30, 22, 50, 5 }, 20);
            //return;
            int mode = 1;
            if (mode == 0)
            {
                #region MakeTable
                StreamWriter w = new StreamWriter("table.txt", false, Encoding.ASCII);
                List<table> Table = new List<table>();
                List<int> temp_0 = new List<int>(new int[40]);

                //讀取之前的
                using (StreamReader r = new StreamReader("table1.txt", Encoding.ASCII))
                    while (!r.EndOfStream)
                    {
                        string[] temp = r.ReadLine().Split(':');
                        Table.Add(new table() { total = int.Parse(temp[1]), list = Array.ConvertAll(temp[2].Split(','), int.Parse) });
                    }

                while (temp_0.Count < 41)
                {
                    temp_0[39]++;
                    temp_0 = PrivateFunction.CheckCarry(temp_0, 20);



                    //跳過多於檢查，例如：檢查3300就是一件多於動作，因為0033等於3300，一直到3333都是如此，因此可以直接跳到3333再作後續動作
                    int max = 0;
                    bool biggerthenmax = false;
                    for (int i = 0; i < 40; i++)
                    {
                        if (biggerthenmax)
                        {
                            temp_0[i] = max;
                        }
                        else
                        {
                            max = Math.Max(max, temp_0[i]);
                            if (max > temp_0[i])
                            {
                                biggerthenmax = true;
                                temp_0[i] = max;
                            }
                        }
                    }


                    //如果總和大於42，則後面都不用繼續算了，因為不論如何都只會更大，因此直接把最後那個造成sum大於42的那個數的前一位+1，後面都填上0就可以加速運算了
                    int _total = temp_0.Sum();
                    if (_total > 42)
                    {
                        int sum = 0;
                        for (int i = 0; i < 40; i++)
                        {
                            sum += temp_0[i];
                            if (sum > 42 && i < 39)
                            {
                                temp_0[i - 1] += 1;
                                for (int j = i; j < 40; j++)
                                    temp_0[j] = 0;
                                temp_0 = PrivateFunction.CheckCarry(temp_0, 20);
                                break;
                            }
                        }
                    }
                    //統計出每種數字的個數，並存入Table
                    _total = temp_0.Sum();
                    if (_total >= 40 && _total <= 42)
                    {
                        int[] _count = new int[20];
                        for (int k = 1; k < 20; k++)
                        {
                            _count[k - 1] = temp_0.Where(x => x == k).Count();
                        }
                        table t = new table() { list = _count, total = _total };

                        if (!(Table.Where(x => x.total.Equals(_total)).Where(x => x.list.SequenceEqual(_count)).Any()))
                            Table.Add(t);
                    }
                }
                //輸出成txt
                for (int k = 40; k <= 42; k++)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        w.WriteLine(String.Format("=========== {0},{1} ===========", k, i + 1));
                        var fff = Table.Where(x => x.total.Equals(k)).Where(x => !x.list[i].Equals(0)).ToList();
                        fff.ForEach(x => w.WriteLine(String.Format("{0}:{1}", x.total, String.Join(",", x.list))));
                    }
                    w.WriteLine(String.Format("=========== {0},{1} ===========", k, "All"));
                    var ggg = Table.Where(x => x.total.Equals(k)).ToList();
                    ggg.ForEach(z => w.WriteLine(String.Format("{0}:{1}", z.total, String.Join(",", z.list))));
                }
                w.Flush();
                w.Close();
                #endregion MakeTable
            }
            else if (mode == 1)
            {
                List<int[]> Table = new List<int[]>();
                //讀取之前的
                using (StreamReader r = new StreamReader("table.txt", Encoding.ASCII))
                    while (!r.EndOfStream)
                    {
                        string[] temp = r.ReadLine().Split(':');
                        Table.Add(Array.ConvertAll(temp[1].Split(','), int.Parse));
                    }
                List<int> list = new List<int>() { 13, 3, 4, 10, 9, 19, 10, 10, 13, 1, 4, 8, 7, 1, 16, 13, 10, 11, 12, 6, 13, 10, 9, 10, 14, 18, 7, 15, 6, 12, 4, 15, 5, 11, 17, 17, 13, 19, 4, 17, 11, 12, 5, 4, 16, 4, 15, 2, 9, 4 };
                // List<int> list = new List<int>() {  7, 15, 6, 12, 4, 15, 5, 11, 17, 17, 13, 19, 4, 17, 11, 12, 5, 4, 16, 4, 15, 2, 9, 4 };

                int[] QCount = new int[20];
                for (int k = 1; k < 20; k++)
                {
                    QCount[k - 1] = list.Where(x => x == k).Count();
                }
                var TableTemp = FindAnswer(ref Table, QCount);
                List<path> Path = new List<path>();
                int PathC = 0;
                int _id = 1;
                foreach (var t in TableTemp)
                {
                    List<path> _Path = new List<path>();
                    _Path.Add(new path() { end = false, mom = 0, count = PrivateFunction.CalcArrayTotal(t), age = 1, id = _id++, total = t, totalHashCode = PrivateFunction.GetHashCode(t) });
                    int ageMax = 0;
                    int notendCount = 1;
                    int r = 0;
                    while (!notendCount.Equals(0))
                    {
                        /*   if (r == 100)
                           {
                               r = 0;
                               _Path.Sort(new pathComparer_end());
                           }
                           else
                               r++;*/
                        var temp = _Path.FindLast(x => !x.end);//.Where(y => !y.end).FirstOrDefault();
                        var answerlist = FindAnswer(ref TableTemp, PrivateFunction.ArrayMinus(QCount, temp.total));

                        foreach (var x in answerlist)
                        {
                            
                            int hash = PrivateFunction.GetHashCode(x);
                            var total = PrivateFunction.ArrayPlus(temp.total, x);
                            /*
                            bool founded = false;
                            foreach (var tt in _Path)
                            {
                                if (hash == tt.totalHashCode)
                                {
                                    founded = true;
                                    break;
                                }
                            }
                            if (!founded)*/
                            if (_Path.Select(y => y.totalHashCode).LastOrDefault(y => y.Equals(PrivateFunction.GetHashCode(x))) == 0)
                            {
                                //if (!_Path.Any(y =>hash.Equals( y.totalHashCode)) )
                                {
                                    _Path.Add(new path() { id = _id++, end = false, mom = temp.id, total = total, totalHashCode = PrivateFunction.GetHashCode(total), age = temp.age + 1, count = temp.count + CalcArrayTotal(x) });
                                    _Path.Add(new path() { id = _id++, end = false, mom = temp.id, total = total, totalHashCode = PrivateFunction.GetHashCode(total), age = temp.age + 1, count = temp.count + CalcArrayTotal(x) });

                                    // if (_Path.Select(y => y.totalHashCode).LastOrDefault(y => y.Equals(PrivateFunction.GetHashCode(total))) == 0)
                                    {
                                        //    _Path.Add(new path() { id = _id++, end = false, mom = temp.id, total = total, count = temp.count + CalcArrayTotal(x) });
                                    }
                                }
                            }
                        }

                        //  answerlist.ForEach(x => { if (!_Path.Select(y => y.totalHashCode).Where(y => y.Equals(PrivateFunction.GetHashCode( x))).Any()) _Path.Add(new path() { id = _id++, end = false, mom = temp.id, total = PrivateFunction.ArrayPlus(temp.total, x), totalHashCode = PrivateFunction.GetHashCode(PrivateFunction.ArrayPlus(temp.total, x)), age = temp.age + 1, count = temp.count + CalcArrayTotal(x) }); });
                        temp.end = true;

                        //_Path[temp.id - 1].end = true;

                        //_Path.ToList().Where(x => x.id.Equals(temp.id)).Select(x => { x.end = true; return x; }).ToList();

                        notendCount += (answerlist.Count - 1);

                    }
                    _Path.Sort(new pathComparer_age_count());
                    path _temp = new path();
                    _temp.mom = -1;
                    //var _temp = _Path[_Path.Count - 1];
                    while (_temp.mom != 0)
                    {
                        PathC++;
                        if (_temp.age == 0)
                            _temp = _Path[_Path.Count - 1];
                        else
                            _temp = _Path.Where(x => x.id.Equals(_temp.mom)).FirstOrDefault();
                        //_Path.Where(x => x.id.Equals(_temp.mom)).FirstOrDefault()
                        Path.Add(new path() { id = PathC, end = true, total = _temp.total, totalHashCode = PrivateFunction.GetHashCode(_temp.total), count = _temp.count, age = _temp.age, mom = _temp.mom == 0 ? 0 : PathC + 1 });

                    }
                    Path.Sort(new pathComparer_age_count());

                }

                Path.Sort(new pathComparer_age_count());
                using (StreamWriter ww = new StreamWriter("debug.txt", false, Encoding.ASCII))
                {
                    Path.ForEach(x => ww.WriteLine(x.age + "#" + string.Join(",", x.total)));
                    ww.Flush();
                }
                var _ttemp = Path[PathC - 1];
                /*
                while (_temp.mom != 0)
                {
                    PathC++;
                    Path.Add(new path() { id = PathC, end = true, total = _temp.total, count = _temp.count, age = _temp.age, mom = _temp.mom == 0 ? 0 : PathC + 1 });
                    _temp = Path.Where(x => x.id.Equals(_temp.mom)).FirstOrDefault();
                }*/

            }
            else if (mode == 2)
            {
                #region 1
                int n = 0;
                List<int> list = new List<int>() { 13, 3, 4, 10, 9, 19, 10, 10, 13, 1, 4, 8, 7, 1, 16, 13, 10, 11, 12, 6, 13, 10, 9, 10, 14, 18, 7, 15, 6, 12, 4, 15, 5, 11, 17, 17, 13, 19, 4, 17, 11, 12, 5, 4, 16, 4, 15, 2, 9, 4 };
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
                    if (sum > 40 && sum <= 40 + n)
                    {
                        List<int> temp = new List<int>();
                        array.ToList().ForEach(x => temp.Add(list[x]));
                        MessageBox.Show(temp.ToArray().ToString());
                    }
                }
                #endregion 1
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
                    r.Close();
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
