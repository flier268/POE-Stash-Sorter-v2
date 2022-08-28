using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using DataGetter.Models;
using LiteDB;
using Polly;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DataGetter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Client = new HttpClient(HttpClientHandler);
        }

        private const string wikiBase = "www.poewiki.net";

        private const string PoedbTradeUri = "https://poedb.tw/tw/api/Trade";
        private const string WikiItemsCountUri = "https://" + wikiBase + "/api.php?format=json&action=cargoquery&tables=items&fields=count(*)";
        private const string WikiItemsUri = "https://" + wikiBase + "/api.php?format=json&action=cargoquery&tables=items,skill_gems&join_on=items._pageID=skill_gems._pageID&fields=items.name,items.class_id,items.size_x,items.size_y,items.rarity_id,items.base_item,items.tags,items.inventory_icon,skill_gems.dexterity_percent,skill_gems.strength_percent,skill_gems.intelligence_percent,removal_version,items.release_version&limit=500";
        private const string WikiQueryUri = "https://" + wikiBase + "/api.php?action=query&titles={0}&prop=imageinfo&iiprop=url&format=json";
        private HttpClientHandler HttpClientHandler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
        private object lock_imagedownload = new object();
        private HttpClient Client { get; }
        private string DatabasePath = Path.Combine(Application.StartupPath, "Datas.db");

        private string ImageDirPath = Path.Combine(Application.StartupPath, "Image");

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Text = "Downloading...";
            button1.Enabled = false;
            MessageBox.Show("Please wait,it may cost several minute.");

            await DownloadAllData();

            button1.Text = "從poedb取得資料";
            button1.Enabled = true;
            //物件序列化
        }

        private async Task DownloadAllData()
        {
            string countString = await Client.GetStringAsync(WikiItemsCountUri);
            int count = Convert.ToInt32(JsonSerializer.Deserialize<WikiItemsCount>(countString).Cargoquerys.First().Title.Count);
            List<Task<string>> tasks = new List<Task<string>>();
            for (int i = 0; i < count; i = i + 500)
            {
                tasks.Add(Client.GetStringAsync(WikiItemsUri + $"&offset={i}"));
            }
            var wikiItemsDownloadTasks = await Task.WhenAll(tasks);
            var wikiItems = wikiItemsDownloadTasks.SelectMany(x => JsonSerializer.Deserialize<WikiItemsModel>(x).Cargoquerys).Select(x => x.Title).OrderByDescending(x => x.ReleaseVersion).ToList();
            var wikiItemsDictionary = wikiItems.ToLookup(y => y.Name, x => x).ToDictionary(x => x.Key, x => x.First());

            string poedbTradeString = await Client.GetStringAsync(PoedbTradeUri);

            var poedbTrade = JsonSerializer.Deserialize<PoedbTradeModel>(poedbTradeString).Data.Where(x => x.Type == "BaseType" || x.Type == "Unique").ToList();
            var images = await GetImagesUrl(wikiItemsDictionary.Values.AsEnumerable());

            DateTime now = DateTime.Now;
            List<Data> datas = new List<Data>();
            foreach (var x in poedbTrade)
            {
                try
                {
                    if (wikiItemsDictionary.TryGetValue(x.Us, out var wiki))
                    {
                        bool hasImageg = images.TryGetValue(wiki.InventoryIcon, out var image);
                        var d = new Data()
                        {
                            Name_English = x.Us,
                            Name_Chinese = x.Lang,
                            GemColor = wiki.GemColor,
                            Width = Convert.ToInt32(wiki.SizeX),
                            Height = Convert.ToInt32(wiki.SizeY),
                            Rarity = wiki.RarityId == "normal" ? 0 : 1,
                            Type = wiki.ClassId,
                            BaseItem = wiki.BaseItem,
                            StrengthPercent = wiki.StrengthPercent == null ? Convert.ToDecimal(wiki.StrengthPercent) : 0,
                            DexterityPercent = wiki.StrengthPercent == null ? Convert.ToDecimal(wiki.StrengthPercent) : 0,
                            IntelligencePercent = wiki.StrengthPercent == null ? Convert.ToDecimal(wiki.StrengthPercent) : 0,
                            ImageURL = hasImageg ? image.Url : null,
                            WikiUrl = hasImageg ? image.Descriptionshorturl : null,
                            Tags = wiki.Tags.Split(',').ToList(),
                            UpdateDate = now
                        };
                        datas.Add(d);
                    }
                    else
                    {
                        Debug.WriteLine(x);
                    }
                }
                catch (Exception)
                {
                }
            }

            WriteToSqlite(datas);
            await DownloadImages(datas.Select(x => ($"{x.Type}\\{x.Name_English}", x.Name_English + ".png", x.ImageURL)).ToList());
            MessageBox.Show("Success!");
        }

        private async Task<Dictionary<string, WikiQueryModel.Imageinfo>> GetImagesUrl(IEnumerable<WikiItemsModel.Title> titles)
        {
            List<Task<string>> tasks = new List<Task<string>>();

            var bulkhead = Policy.BulkheadAsync(16, int.MaxValue);

            var pages = titles.Page(20);
            tasks.AddRange(pages.Select(x => bulkhead.ExecuteAsync(() => Client.GetStringAsync(string.Format(WikiQueryUri, string.Join("|", x.Select(y => HttpUtility.HtmlDecode(y.InventoryIcon))))))));

            var result = await Task.WhenAll(tasks);
            return result.Select(x => JsonSerializer.Deserialize<WikiQueryModel>(x)).SelectMany(y => y.Query.Pages.Values).Where(y => y.Imageinfo != null).Distinct().ToDictionary(y => y.Title, y => y.Imageinfo[0]);
        }

        private void WriteToSqlite(IEnumerable<Data> datas)
        {
            var databasePath = Path.Combine(Application.StartupPath, "Datas.db");

            using (var db = new LiteDatabase(databasePath))
            {
                var col = db.GetCollection<Data>();
                col.EnsureIndex(x => x.Name_English);
                col.EnsureIndex(x => x.Name_Chinese);
                col.Upsert(datas);
            }
        }

        private async Task DownloadImages(List<(string id, string filename, string url)> list)
        {
            LiteDatabase liteDatabase = new LiteDatabase(DatabasePath);
            liteDatabase.FileStorage.Upload("QuadStashPanelGrid", "QuadStashPanelGrid.png");
            liteDatabase.FileStorage.Upload("StashPanelGrid", "StashPanelGrid.png");
            List<Task> tasks = new List<Task>();

            var bulkhead = Policy.BulkheadAsync(16, int.MaxValue);
            WebClient webClient = new WebClient();
            Directory.CreateDirectory(ImageDirPath);
            tasks.AddRange(list.Select(x => bulkhead.ExecuteAsync(async () =>
            {
                try
                {
                    Stream stream = await Client.GetStreamAsync(x.url);
                    liteDatabase.FileStorage.Upload(x.id, x.filename, stream);
                }
                catch
                {
                    Debug.WriteLine("Wrong URL: " + x.url);
                }
            })));
            //tasks.AddRange(list.Select(x => bulkhead.ExecuteAsync(async () => await webClient.DownloadFileTaskAsync(x.url, x.filename))));
            await Task.WhenAll(tasks);
            liteDatabase.Dispose();
        }

        //private async Task DownloadImage()
        //{
        //    try
        //    {
        //        System.Net.WebClient WC = new System.Net.WebClient();
        //        if (!Directory.Exists(ImageDirPath))
        //            Directory.CreateDirectory(ImageDirPath);
        //        bool Delay = false;

        //        while (true)
        //        {
        //            if (Delay)
        //            {
        //                Delay = false;
        //                SpinWait.SpinUntil(() => false, 500);
        //                continue;
        //            }
        //            Struct_Image image;
        //            lock (lock_imagedownload)
        //            {
        //                if (ImageDownloadList.Count == 0)
        //                {
        //                    Delay = true;
        //                    if (cancellationTokenSource.IsCancellationRequested)
        //                        return;
        //                    continue;
        //                }
        //                image = ImageDownloadList[0];
        //                ImageDownloadList.RemoveAt(0);
        //            }
        //            bool DownloadError = false;
        //            try
        //            {
        //                await WC.DownloadFileTaskAsync(image.URL, image.FileName);
        //            }
        //            catch (Exception e)
        //            { Debug.Print(e.Message); DownloadError = true; }
        //            if (image.GetSizeNeed || DownloadError)
        //            {
        //                Data data;
        //                switch (image.DataType)
        //                {
        //                    case 1:
        //                        data = Data.Where(x => x.Name_English == image.EngName).First();
        //                        break;

        //                    case 2:
        //                        data = Data_unique.Where(x => x.Name_English == image.EngName).First();
        //                        break;

        //                    case 3:
        //                        data = Data_Prophecy.Where(x => x.Name_English == image.EngName).First();
        //                        break;

        //                    default:
        //                        data = null;
        //                        break;
        //                }

        //                if (image.GetSizeNeed)
        //                {
        //                    Size size = new Size();
        //                    string url = null;
        //                    try
        //                    {
        //                        size = ImageUtilities.GetDimensions(image.FileName);
        //                    }
        //                    catch (Exception e)
        //                    {
        //                        size = new Size(47, 47); url = "question-mark.png";
        //                    }
        //                    if (data != null)
        //                    {
        //                        data.Width = size.Width / 47;
        //                        data.Height = size.Height / 47;
        //                        if (url != null)
        //                            data.ImageURL = url;
        //                    }
        //                }
        //                if (DownloadError)
        //                    data.ImageURL = "question-mark.png";
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //    }
        //}

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = String.Format("{0}   {1}: {2}", this.Text, "Ver", Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }
    }
}