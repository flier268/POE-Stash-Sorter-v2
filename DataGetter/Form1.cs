using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities;
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
            var List_cn = GetList_cn();
            
           /* foreach (cn a in List_cn)
                if (!File.Exists(Path.Combine(Application.StartupPath, a.name + ".txt")))
                    Debug.Print(a.name);*/
            Data.Clear();
            running = List_cn.Count - 3;
            foreach (var cn in List_cn)
            {
                DownloadData_Async(cn);
            }
            wait();
            //物件序列化


        }

        string reg_Name = @"<a\s.*?>(.*?)</a>.*<span.*?>(.*?)</span>";
        string reg_Name2 = @"(.*?)<br>.*>(.*?)</span>";
        string reg_imgURL = @"<img\s+src='(.*?)'";
        string reg_unique = @"<tr><td><img\s+src=\'(.*?)\'\/>.*?<a\s.*?href=.*?\'>(.*?\s(.*?))<\/a>.*?grey'>(.*?)</span>";
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
        private async Task wait()
        {
            while (running != 0)
                await Task.Delay(1);
            /*if (MessageBox.Show("Start downloading...", "Message", MessageBoxButtons.OKCancel) != DialogResult.OK)
                return;*/
            File.Delete(FileName);
            string strJson = JsonConvert.SerializeObject(Data, Formatting.Indented);
            //輸出結果
            //System.Diagnostics.Debug.Write(strJson);

            using (StreamWriter w = new StreamWriter(FileName))
            {
                w.Write(strJson);
                w.Flush();
            }
            /*
            using (StreamWriter w = new StreamWriter("TypeList.txt"))
            {
                var t = Data.Select(x => x.type).Distinct().ToList();
                foreach (string r in t)
                    w.WriteLine(r);
                w.Flush();
            }    */
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

            
            MessageBox.Show("Success!");
        }
        private async Task DownloadData_Async_unique()
        {
            string basename = "";
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
                        try
                        {
                            //地圖的大小沒有在圖片的網址寫出來，但都固定是1x1
                            basename = m.Groups[3].ToString();
                            //Debug用
                            //if (aaaaa.Contains("聖戰長靴"))
                            //   MessageBox.Show("");
                            Regex r_getbass = new Regex(".*\\s(.*?)$");
                            r_getbass.Match(m.Groups[2].ToString());
                            string name_base = r_getbass.Match(m.Groups[2].ToString().Trim()).Groups[1].ToString();
                            var BaseInfo = Data.Where(x => x.c == name_base).FirstOrDefault();
                            roots.Add(new RootObject
                            {
                                GC = 'n',
                                c = m.Groups[2].ToString().Trim(),
                                e = m.Groups[4].ToString().Trim(),
                                url = m.Groups[1].ToString(),
                                w = BaseInfo.w,
                                h = BaseInfo.h,
                                type = BaseInfo.type
                            });
                        }
                        catch (Exception e)
                        {
                            Debug.Print(basename);
                            Debug.Print(e.Message);
                        }
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
                Debug.Print(basename);
                Debug.Print(e.Message);
            }
            System.Net.WebClient WC = new System.Net.WebClient();
            if (!Directory.Exists(Path.Combine(Application.StartupPath, "Image")))
                Directory.CreateDirectory(Path.Combine(Application.StartupPath, "Image"));
            string basepath = Path.Combine(Application.StartupPath, "Image");
            foreach (RootObject r in Data_unique)
            {
                try
                {
                    if (!r.e.EndsWith("</del>") && !File.Exists(Path.Combine(basepath, r.e + ".png")))
                        WC.DownloadFile(r.url, Path.Combine(basepath, r.e + ".png"));
                }
                catch (Exception e)
                { Debug.Print(e.Message + "," + r.e); }
            }
        }
        private async Task DownloadData_Async(cn l)
        {
            switch (l.name_eng)
            {
                case "Microtransaction":
                    return;
                case "HideoutDoodad":
                    return;
                case "Chest":
                    return;
            }
            try
            {                
                bool 技能寶石 = (l.url.ToLower().Contains("gem"));
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(String.Format("http://poedb.tw/json.php/item_class?cn={0}", l.name_eng));
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var gg = poedbJson.FromJson(responseBody);
                Regex r_Name = new Regex(reg_Name, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                Regex r_Name2 = new Regex(reg_Name2, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                Regex r_imgURL = new Regex(reg_imgURL, RegexOptions.IgnoreCase);
                Regex r_GemColor = new Regex(@".Gems/(.*?)'.*?>(.*)", RegexOptions.IgnoreCase);


                //有時nameC會變成<img .....，因此需要再處理一次
                Regex r_realName = new Regex(@"<img\s.*/>(.*)", RegexOptions.IgnoreCase);

                List<RootObject> roots = new List<RootObject>();



                foreach (var tt in gg.Data)
                {
                    if (tt[1].Contains("巨靈脛甲"))
                        Debug.Print("");
                    GroupCollection NameGroup = tt[1].StartsWith("<a") ?  r_Name.Match(tt[1]).Groups: r_Name2.Match(tt[1]).Groups;
                    var url = r_imgURL.Match(tt[0]).Groups[1].ToString();
                    string nameC = NameGroup[1].ToString().Trim(), nameE = NameGroup[2].ToString().Trim();
                    
                    char colorGem = 'n';
                    if (技能寶石)
                    {
                        var temp = r_GemColor.Match(nameC).Groups;
                        nameC = temp[2].ToString().Trim();
                        switch (temp[1].ToString())
                        {
                            case "IntelligenceGem.png":
                                colorGem = 'b';
                                break;
                            case "DexterityGem.png":
                                colorGem = 'g';
                                break;
                            case "StrengthGem.png":
                                colorGem = 'r';
                                break;
                        }
                    }
                    if (nameC.StartsWith("<img"))
                        nameC = r_realName.Match(nameC).Groups[1].ToString().Trim();
                    System.Net.WebClient WC = new System.Net.WebClient();
                    if (!Directory.Exists(Path.Combine(Application.StartupPath, "Image")))
                        Directory.CreateDirectory(Path.Combine(Application.StartupPath, "Image"));
                    string filepath = Path.Combine(Path.Combine(Application.StartupPath, "Image"), nameE + ".png");
                    try
                    {
                        if (!nameE.EndsWith("</del>") && !File.Exists(filepath))
                            WC.DownloadFile(url, filepath);
                    }
                    catch (Exception e)
                    { Debug.Print(e.Message + "," + nameE); }

                    Size size = ImageUtilities.GetDimensions(filepath);
                    /*
                    Uri uri = new Uri(url);
                    var actual = ImageUtilities.GetWebDimensions(uri);*/

                    roots.Add(new RootObject
                    {
                        c = nameC,
                        e = NameGroup[2].ToString(),
                        url = url,
                        GC = colorGem,
                        w = size.Width / 47,
                        h = size.Height / 47,
                        type = l.name_eng
                    });
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
                running--;
                Debug.Print(l.name + "," + e.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = String.Format("{0}   {1}: {2}", this.Text, "Ver", Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }
    }
}