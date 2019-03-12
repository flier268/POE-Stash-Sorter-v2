using SQLite;
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

namespace DataGetter
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }
        string FileName = Path.Combine(Application.StartupPath, "ItemList.txt");
        string FileName_Unique = Path.Combine(Application.StartupPath, "ItemList_Unique.txt");
        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Text = "Downloading...";
            button1.Enabled = false;
            List<cn> List_cn = new List<cn>();
            List_cn = await GetList_cn();


            MessageBox.Show("Please wait,it may cost several minute.");
            Data.Clear();
            Data_unique.Clear();
            Data_Prophecy.Clear();
            await DownloadData_Async_Prophecy();
            foreach (var cn in List_cn)
            {
                await DownloadData_Async(cn);
                await Task.Delay(500);
            }
            await DownloadData_Async_unique();

            await WriteToSqlite();
            button1.Text = "從poedb取得資料";
            button1.Enabled = true;
            //物件序列化
        }

        string reg_Name = @"<a\s.*?>(.*?)</a>.*<span.*?>(.*?)</span>";
        string reg_Name2 = @"(.*?)<br>.*>(.*?)</span>";
        string reg_imgURL = @"<img\s+src=[""|'](.*?)[""|']/>";
        string reg_unique = "<img\\s+src=[\"'](.*?)[\"']/></a><td><a class=[\"']item_unique[\"'].*?>(.*?)</a>.*?<span class=[\"']item_description[\"']>(.*?)</span>";
        private async Task<List<cn>> GetList_cn()
        {
            List<cn> a = new List<cn>();
            string temp = await DownloadData("http://poedb.tw/item.php");
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
                        a.Add(new cn { name = t1.Groups[2].ToString(), url = t1.Groups[1].ToString().Replace("area.php?cn=", "item.php?cn="), name_eng = m_getcn.Groups[1].ToString() });
                    }
                    //特殊狀況
                    a.Add(new cn { name = "異界地圖", name_eng = "Map", url = "item.php?cn=Map" });
                    a.Add(new cn { name = "聯盟石", name_eng = "Leaguestone", url = "item.php?cn=Leaguestone" });
                    a.Add(new cn { name = "任務物品", name_eng = "QuestItem", url = "item.php?cn=QuestItem" });
                }
            }
            return a;
        }

        private async Task<string> DownloadData(string url)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    // by calling .Result you are performing a synchronous call
                    var responseContent = response.Content;

                    // by calling .Result you are synchronously reading the result
                    string responseString = await responseContent.ReadAsStringAsync();
                    return responseString;
                }
                return "";
            }
        }
        private Object thislock = new object();
        private List<Data> Data = new List<Data>();
        private List<Data> Data_unique = new List<Data>();
        private List<Data> Data_Prophecy = new List<Data>();

        private async Task WriteToSqlite()
        {
            var databasePath = Path.Combine(Application.StartupPath, "Datas.db");
            var db = new SQLiteAsyncConnection(databasePath);
            await db.CreateTableAsync<Data>();
            await db.CreateIndexAsync("Data", "Name_Chinese");
            await db.CreateIndexAsync("Data", "Name_English");
            await db.RunInTransactionAsync(conn => Data.ForEach(x => db.InsertOrReplaceAsync(x)));
            await db.RunInTransactionAsync(conn => Data_unique.ForEach(x => db.InsertOrReplaceAsync(x)));
            await db.RunInTransactionAsync(conn => Data_Prophecy.ForEach(x => db.InsertOrReplaceAsync(x)));
            await db.CloseAsync();
            MessageBox.Show("Success!");
        }
        HttpClient client = new HttpClient();
        private async Task DownloadData_Async_unique()
        {
            string basename = "";
            try
            {
                HttpResponseMessage response = await client.GetAsync("http://poedb.tw/unique.php?l=1");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                Regex r = new Regex(reg_unique, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                MatchCollection mm = r.Matches(responseBody);

                List<Data> roots = new List<Data>();
                DateTime Now = DateTime.Now;
                if (mm.Count > 0)
                {
                    foreach (Match m in mm)
                    {
                        try
                        {
                            //地圖的大小沒有在圖片的網址寫出來，但都固定是1x1
                            var match_GetBaseName = Regex.Match(m.Groups[2].Value.Trim(), "(.*) (.*)");
                            if (m.Groups[2].Value.StartsWith(m.Groups[3].Value))
                            {
                                basename = m.Groups[2].Value.Substring(m.Groups[3].Value.Length + 1);
                            }
                            else
                            {
                                basename = match_GetBaseName.Groups[2].Value;
                            }
                            var BaseInfo = Data.Where(x => x.Name_Chinese == basename || x.Name_English == basename).FirstOrDefault();
                            if (BaseInfo != null)
                                roots.Add(new Data
                                {
                                    GemColor = "n",
                                    Name_Chinese = match_GetBaseName.Groups[1].Value.Trim(),
                                    Name_English = m.Groups[3].Value.Trim(),
                                    Rarity = 1,
                                    ImageURL = m.Groups[1].Value,
                                    Width = BaseInfo.Width,
                                    Height = BaseInfo.Height,
                                    Type = BaseInfo.Type,
                                    UpdateDate = Now
                                });
                            else
                            {

                            }
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
                    Data_unique.AddRange(roots);
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
            foreach (var r in Data_unique)
            {
                try
                {
                    if (!r.Name_English.EndsWith("</del>") && !File.Exists(Path.Combine(basepath, r.Name_English + ".png")))
                        await WC.DownloadFileTaskAsync(r.ImageURL, Path.Combine(basepath, r.Name_English + ".png"));
                }
                catch (Exception e)
                { Debug.Print(e.Message + "," + r.Name_English); }
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
                DateTime Now = DateTime.Now;
                bool 技能寶石 = (l.url.ToLower().Contains("gem"));
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

                List<Data> roots = new List<Data>();



                foreach (var tt in gg.Data)
                {
                    GroupCollection NameGroup = tt[1].StartsWith("<a") ? r_Name.Match(tt[1]).Groups : r_Name2.Match(tt[1]).Groups;
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
                        if (!nameE.EndsWith("</del>"))
                            if (!File.Exists(filepath) || (File.Exists(filepath) && new FileInfo(filepath).Length == 0))
                                await WC.DownloadFileTaskAsync(new Uri(url), filepath);
                    }
                    catch (Exception e)
                    { Debug.Print(e.Message + "," + nameE); }
                    Size size = new Size();
                    try
                    {
                        size = ImageUtilities.GetDimensions(filepath);
                    }
                    catch (Exception e) { size = new Size(47, 47); url = "question-mark.png"; Debug.Print(e.Message + "," + filepath); }

                    roots.Add(new Data
                    {
                        Name_Chinese = nameC,
                        Name_English = NameGroup[2].ToString(),
                        Rarity = 0,
                        ImageURL = url,
                        GemColor = colorGem.ToString(),
                        Width = size.Width / 47,
                        Height = size.Height / 47,
                        Type = l.name_eng,
                        UpdateDate = Now
                    });
                }
                lock (thislock)
                {
                    Data.AddRange(roots);
                }
            }
            catch (Exception e)
            {
                Debug.Print(l.name + "," + e.Message);
            }
        }
        private async Task DownloadData_Async_Prophecy()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("http://poedb.tw/item.php?cn=Prophecy");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                Regex r = new Regex("<tr><td><a href=.*?>(.*?)</a><br>(.*?)<td>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                MatchCollection mm = r.Matches(responseBody);

                List<Data> roots = new List<Data>();
                DateTime Now = DateTime.Now;
                if (mm.Count > 0)
                {
                    foreach (Match m in mm)
                    {
                        try
                        {
                            roots.Add(new Data
                            {
                                GemColor = "n",
                                Name_Chinese = m.Groups[1].Value,
                                Name_English = m.Groups[2].Value,
                                Rarity = 0,
                                ImageURL = "https://web.poecdn.com/image/Art/2DItems/Currency/ProphecyOrbRed.png?scale=1&w=1&h=1",
                                Width = 1,
                                Height = 1,
                                Type = "Prophecy",
                                UpdateDate = Now
                            });
                        }
                        catch
                        {
                        }
                    }
                }

                lock (thislock)
                {
                    Data_Prophecy.AddRange(roots);
                }
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
            }
            System.Net.WebClient WC = new System.Net.WebClient();
            if (!Directory.Exists(Path.Combine(Application.StartupPath, "Image")))
                Directory.CreateDirectory(Path.Combine(Application.StartupPath, "Image"));
            string basepath = Path.Combine(Application.StartupPath, "Image");
            try
            {
                await WC.DownloadFileTaskAsync("https://web.poecdn.com/image/Art/2DItems/Currency/ProphecyOrbRed.png?scale=1&w=1&h=1", Path.Combine(basepath, "Prophecy.png"));
            }
            catch (Exception e)
            { Debug.Print(e.Message); }
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = String.Format("{0}   {1}: {2}", this.Text, "Ver", Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }
    }
}