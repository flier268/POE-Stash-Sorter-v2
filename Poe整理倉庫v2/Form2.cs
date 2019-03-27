using DataGetter;
using SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Poe整理倉庫v2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        List<KeyValuePair<string, string>> SpeciesDic = new List<KeyValuePair<string, string>>();
        List<Data> ItemList = new List<Data>();
        public Form2(string clip, string Name)
        {
            InitializeComponent();
            ApplicationHelper.SetForegroundWindow(this.Handle);
            richTextBox1.Text = clip;
            if (PrivateFunction.IsChineseContain(Name))
                textBox1.Text = Name;
            else
                textBox2.Text = Name;
            if (System.Threading.Thread.CurrentThread.CurrentUICulture == new CultureInfo("en"))
            {
                SpeciesDic.Add(new KeyValuePair<string, string>("Amulet", "Amulet"));
                SpeciesDic.Add(new KeyValuePair<string, string>("Ring", "Ring"));
                SpeciesDic.Add(new KeyValuePair<string, string>("Jewel", "Jewel"));
                SpeciesDic.Add(new KeyValuePair<string, string>("Currency", "Currency"));
                SpeciesDic.Add(new KeyValuePair<string, string>("Map", "Map"));
                SpeciesDic.Add(new KeyValuePair<string, string>("DivinationCard", "Divination Card"));
                SpeciesDic.Add(new KeyValuePair<string, string>("Leaguestone", "Leaguestone"));
                SpeciesDic.Add(new KeyValuePair<string, string>("MiscMapItem", "OtherMapItem"));
                SpeciesDic.Add(new KeyValuePair<string, string>("UniqueFragment", "Unique Fragment"));
                SpeciesDic.Add(new KeyValuePair<string, string>("Gem", "Gem"));
                SpeciesDic.Add(new KeyValuePair<string, string>("Other", "Other"));
                SpeciesDic.Add(new KeyValuePair<string, string>("Prophec", "Prophecy"));
            }
            else
            {
                SpeciesDic.Add(new KeyValuePair<string, string>("Amulet", "護身符"));
                SpeciesDic.Add(new KeyValuePair<string, string>("Ring", "戒指"));
                SpeciesDic.Add(new KeyValuePair<string, string>("Jewel", "珠寶"));
                SpeciesDic.Add(new KeyValuePair<string, string>("Currency", "通貨"));
                SpeciesDic.Add(new KeyValuePair<string, string>("Map", "地圖"));
                SpeciesDic.Add(new KeyValuePair<string, string>("DivinationCard", "命運卡"));
                SpeciesDic.Add(new KeyValuePair<string, string>("Leaguestone", "聯盟石"));
                SpeciesDic.Add(new KeyValuePair<string, string>("MiscMapItem", "其他地圖道具"));
                SpeciesDic.Add(new KeyValuePair<string, string>("UniqueFragment", "碎片"));
                SpeciesDic.Add(new KeyValuePair<string, string>("Gem", "技能寶石"));
                SpeciesDic.Add(new KeyValuePair<string, string>("Other", "其他"));
                SpeciesDic.Add(new KeyValuePair<string, string>("Prophec", "預言"));
            }
            comboBox1.Items.AddRange(SpeciesDic.Select(x => x.Value).ToArray());
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            textBox3.Text = "n";
            textBox4.Text = "question-mark.png";

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var databasePath = Path.Combine(Application.StartupPath, "Datas_Adden.db");
            var db = new SQLiteAsyncConnection(databasePath);
            await db.CreateTableAsync<Data>();
            await db.CreateIndexAsync("Data", "Name_Chinese");
            await db.CreateIndexAsync("Data", "Name_English");
            var data = new Data() {
                Name_Chinese = textBox1.Text,
                Name_English = textBox2.Text,
                Type = SpeciesDic.Where(x => x.Value == comboBox1.Text).FirstOrDefault().Key,
                Width = Int32.Parse(comboBox2.Text),
                Height = Int32.Parse(comboBox3.Text),
                GemColor = textBox3.Text,
                ImageURL = textBox4.Text,
                Rarity=2,
                UpdateDate=DateTime.Now
            };
            await db.InsertOrReplaceAsync(data);
            await db.CloseAsync();
            this.Close();
        }
    }
}
