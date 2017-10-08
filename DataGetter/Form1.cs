using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DataGetter.JsonClass;

namespace DataGetter
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }
        string FileName = Path.Combine(Application.StartupPath , "ItemList.txt");
        string FileName_Unique = Path.Combine(Application.StartupPath , "ItemList_Unique.txt");
        private static int running = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            File.Delete(FileName);
            var List_cn = GetList_cn();
            
           /* foreach (cn a in List_cn)
                if (!File.Exists(Path.Combine(Application.StartupPath, a.name + ".txt")))
                    Debug.Print(a.name);*/
            Data.Clear();
            running = List_cn.Count - 2;
            foreach (var cn in List_cn)
            {
                DownloadData_Async(cn);
            }
            wait();
            //物件序列化


        }

        string reg1 = @"<tr><td><img\s+src=\'(.*?)\'\/>.*?<a\shref=.*?\'>(.*?)<\/a>.*?class=\'mod_grey\'>(.*?)<\/span>";
        string reg_gem = @"<tr.*?>.*?<td><a\s+.*?<img\s+.*?src=\'(.*?)'/>.*?<a(.*?)'><img\s.*?>(.*?)</a><br>(.*?)<td>";
        string reg_Currency = @"<tr.*?>.*?<td><a\s+.*?<img\s+.*?src=\'(.*?)\'\/>(.*?)<\/a>\((.*?)\)";
        string reg_Flask = @"Flask'>(.*?)<\/a>\((.*?)\).*?<img\s+src='(.*?)'\/>";
        string reg_Map = @"<tr><td><img\s+src=\'(.*?)\'\/><td><a\s+.*?'>(.*?)</a><br>.*?class='mod_grey'>(.*?)</span>";
        string reg_Microtransaction = @"<tr><td><img\s+src=\'(.*?)\'\/><td>(.*?)<br>.*?class='mod_grey'>(.*?)</span>";
        string reg_unique = @"<tr><td><img\s+src=\'(.*?)\'\/>.*?<a\s.*?href=.*?\'>(.*?\s(.*?))<\/a>.*?grey'>(.*?)</span>";
        string reg_Prophecy = @"<td><a\shref='item\.php\?n=.*?'>(.*?)<\/a><br>(.*?)<td>()";
        private List<cn> GetList_cn()
        {
            List<cn> a = new List<cn>();
            string temp = DownloadData("http://poedb.tw/item.php");
            string reg_pre = @"id=""navbar-collapse2"">\s*<ul class=""nav navbar-nav"">(.*?)</ul>\s*</div>";
            Regex r = new Regex(reg_pre, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            string reg_getcn = ".*?cn=(.*)";
            Regex r_getcn = new Regex(reg_getcn, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            

            Match m = r.Match(temp);
            if (m.Success)
            {
                string reg = "<li><a\\s+href=(?:\"|\')(.*?)(?:\"|\')>(.*?)</a>";
                r = new Regex(reg, RegexOptions.IgnoreCase | RegexOptions.Singleline);

                var mm = r.Matches(m.Groups[1].ToString());
                if (mm.Count > 0)
                {
                    foreach (Match t1 in mm)
                    {
                        Match m_getcn = r_getcn.Match(t1.Groups[1].ToString());
                        a.Add(new cn { name = t1.Groups[2].ToString(), url = t1.Groups[1].ToString().Replace("area.php?cn=", "item.php?cn="),name_eng= m_getcn.Groups[1].ToString() });
                    }
                    //特殊狀況
                    a.Add(new cn { name = "預言", name_eng = "Prophecy", url = "item.php?cn=Prophecy" });
                }
            }
            return a;
        }

        private string DownloadData(string url)
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    // by calling .Result you are performing a synchronous call
                    var responseContent = response.Content;

                    // by calling .Result you are synchronously reading the result
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    return responseString;
                }
                return "";
            }
        }
        private Object thislock = new object();
        private List<RootObject> Data = new List<RootObject>();
        private List<RootObject> Data_unique = new List<RootObject>();
        private async void wait()
        {
            while (running != 0)
                await Task.Delay(1);
            string strJson = JsonConvert.SerializeObject(Data, Formatting.Indented);
            //輸出結果
            //System.Diagnostics.Debug.Write(strJson);

            using (StreamWriter w = new StreamWriter(FileName))
            {
                w.Write(strJson);
                w.Flush();
            }
            using (StreamWriter w = new StreamWriter("111.txt"))
            {
                var t = Data.Where(s => s.h * s.w == 1).Select(x => x.type).Distinct().ToList();
                foreach(string r in t)
                w.WriteLine(r);
                w.Flush();
            }            
            //------------
            running++;
            DownloadData_Async_unique();
            while (running != 0)
                await Task.Delay(1);

            strJson = JsonConvert.SerializeObject(Data_unique, Formatting.Indented);
            //輸出結果
            //System.Diagnostics.Debug.Write(strJson);
            using (StreamWriter w = new StreamWriter(FileName_Unique))
            {
                w.Write(strJson);
                w.Flush();
            }

            if (MessageBox.Show("Download Image?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                System.Net.WebClient WC = new System.Net.WebClient();
                if (!Directory.Exists(Path.Combine(Application.StartupPath, "Image")))
                    Directory.CreateDirectory(Path.Combine(Application.StartupPath, "Image"));
                string basepath = Path.Combine(Application.StartupPath, "Image");
                foreach (RootObject r in Data)
                {
                    try
                    {
                        if (!r.e.EndsWith("</del>") && !File.Exists(Path.Combine(basepath, r.e + ".png")) )
                            WC.DownloadFile(r.url, Path.Combine(basepath, r.e + ".png"));
                    }
                    catch(Exception e)
                    { Debug.Print(e.Message + "," + r.e); }
                }
                foreach (RootObject r in Data_unique)
                {
                    try
                    {
                        if (!r.e.EndsWith("</del>") &&!File.Exists(Path.Combine(basepath, r.e + ".png")) )
                            WC.DownloadFile(r.url, Path.Combine(basepath, r.e + ".png"));
                    }
                    catch (Exception e)
                    { Debug.Print(e.Message + "," + r.e); }
                }
            }
            MessageBox.Show("Success!");
        }
        private async void DownloadData_Async_unique()
        {
            string aaaaa = "";
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("http://poedb.tw/unique.php?l=1");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Regex r = new Regex(reg_unique, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                MatchCollection mm = r.Matches(responseBody);

                List<RootObject> roots = new List<RootObject>();
                if (mm.Count > 0)
                {
                    foreach (Match m in mm)
                    {
                        //地圖的大小沒有在圖片的網址寫出來，但都固定是1x1
                        aaaaa = m.Groups[3].ToString();
                        var BaseInfo = Data.Where(x => x.c == m.Groups[3].ToString()).FirstOrDefault();
                        roots.Add(new RootObject
                        {
                            GC = 'n',
                            c = m.Groups[2].ToString(),
                            e = m.Groups[4].ToString(),
                            url = m.Groups[1].ToString(),
                            w = BaseInfo.w,
                            h = BaseInfo.h,
                            type=BaseInfo.type
                        });
                    }
                }

                lock (thislock)
                {
                    running--;
                    Data_unique.AddRange(roots);
                    /*
                    using (StreamWriter w = new StreamWriter(Application.StartupPath + "/" + "傳奇" + ".txt", false, Encoding.UTF8))
                    {
                        w.WriteAsync(JsonConvert.SerializeObject(roots, Formatting.Indented));
                        w.FlushAsync();
                    }*/
                }
            }
            catch (Exception e)
            {
                Debug.Print(aaaaa);
                Debug.Print(e.Message);
            }
        }
        private async void DownloadData_Async(cn l)
        {
            if (l.name != "藏身處裝飾" &&  l.name != "保險箱")
                try
                {
                    HttpClient client = new HttpClient();
                    HttpResponseMessage response = await client.GetAsync(String.Format("http://poedb.tw/{0}", l.url));
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Regex r;
                    if (l.url.ToLower().Contains("gem"))
                        r = new Regex(reg_gem, RegexOptions.IgnoreCase);
                    else if (l.url.ToLower().Contains("flask"))
                        r = new Regex(reg_Flask, RegexOptions.IgnoreCase);
                    else if (l.url.ToLower().Contains("currency"))
                        r = new Regex(reg_Currency, RegexOptions.IgnoreCase);
                    else if (l.url.ToLower().EndsWith("map"))
                        r = new Regex(reg_Map, RegexOptions.IgnoreCase);
                    else if (l.url.ToLower().EndsWith("microtransaction"))
                        r = new Regex(reg_Microtransaction, RegexOptions.IgnoreCase);
                    else if(l.url.ToLower().EndsWith("prophecy"))
                        r = new Regex(reg_Prophecy, RegexOptions.IgnoreCase);
                    else
                        r = new Regex(reg1, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    MatchCollection mm = r.Matches(responseBody);

                    //從網址解析出物品大小
                    Regex r_url = new Regex(@"&w=(\d)&h=(\d)", RegexOptions.IgnoreCase);
                    Regex r_GemColor = new Regex(@"class='gem_(.)", RegexOptions.IgnoreCase);
                    List<RootObject> roots = new List<RootObject>();
                    if (mm.Count > 0)
                    {
                        //一般的順序
                        int o1 = 1, o2 = 2, o3 = 3, o4 = 4;

                        //由於藥水、地圖、通貨的排版特別，因此要另外處理
                        bool 順序不同 = (l.url.ToLower().Contains("flask")) || l.url.ToLower().EndsWith("prophecy");
                        bool 沒有顯示大小 = (l.url.ToLower().Contains("currency"));
                        bool 技能寶石 = (l.url.ToLower().Contains("gem"));
                        if (順序不同)
                        {
                            o1 = 3; o2 = 1; o3 = 2;
                        }
                        if (技能寶石)
                        {
                            o1 = 1; o2 = 3; o3 = 4; o4 = 2;
                        }

                        foreach (Match m in mm)
                        {
                            string Icon_url = m.Groups[o1].ToString();
                            if (l.url.ToLower().EndsWith("prophecy"))
                                Icon_url = "http://web.poecdn.com/image/Art/2DItems/Currency/ProphecyOrbRed.png?scale=1&w=1&h=1";

                            //地圖的大小沒有在圖片的網址寫出來，但都固定是1x1
                            Match m_url = 沒有顯示大小 ? null : r_url.Match(Icon_url);
                            var GemColor = r_GemColor.Match(m.Groups[o4].ToString()).Groups;
                            roots.Add(new RootObject
                            {
                                c = m.Groups[o2].ToString(),
                                e = m.Groups[o3].ToString(),
                                url = Icon_url,
                                GC = 技能寶石 ? char.Parse(GemColor.Count == 1 ? "w" : GemColor[1].ToString()) : 'n',
                                w = 沒有顯示大小 ? 1 : int.Parse(m_url.Groups[1].ToString()),
                                h = 沒有顯示大小 ? 1 : int.Parse(m_url.Groups[2].ToString()),
                                type = l.name_eng
                            });
                        }
                    }

                    lock (thislock)
                    {
                        running--;

                        Data.AddRange(roots);
                        /*
                         * 依照類型寫入各個檔案
                         using (StreamWriter w = new StreamWriter(Application.StartupPath + "/" + l.name + ".txt", false, Encoding.UTF8))
                        {
                            w.WriteAsync(JsonConvert.SerializeObject(roots, Formatting.Indented));
                            w.FlushAsync();
                        }*/
                    }
                }
                catch (Exception e)
                {
                    Debug.Print(l.name + "," + e.Message);
                }

        }

    }
}