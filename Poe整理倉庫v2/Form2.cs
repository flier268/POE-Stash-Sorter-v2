using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static Poe整理倉庫v2.JsonClass;

namespace Poe整理倉庫v2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        List<KeyValuePair<string, string>> SpeciesDic = new List<KeyValuePair<string, string>>();
        List<RootObject> ItemList = new List<RootObject>();
        public Form2(string clip,string Name)
        {
            InitializeComponent();
            ApplicationHelper.SetForegroundWindow(this.Handle);
            richTextBox1.Text = clip;
            if (PrivateFunction.IsChineseContain(Name))
                textBox1.Text = Name;
            else
                textBox2.Text = Name;
            SpeciesDic.Add(new KeyValuePair<string, string>("Amulet", "護身符"));
            SpeciesDic.Add(new KeyValuePair<string, string>("Ring", "戒指"));
            SpeciesDic.Add(new KeyValuePair<string, string>("Jewel", "珠寶"));
            SpeciesDic.Add(new KeyValuePair<string, string>("Currency", "通貨"));
            SpeciesDic.Add(new KeyValuePair<string, string>("Map", "地圖"));
            SpeciesDic.Add(new KeyValuePair<string, string>("DivinationCard", "命運卡"));
            SpeciesDic.Add(new KeyValuePair<string, string>("Leaguestone", "聯盟石"));
            SpeciesDic.Add(new KeyValuePair<string, string>("MiscMapItem", "其他"));
            SpeciesDic.Add(new KeyValuePair<string, string>("UniqueFragment", "碎片"));
            SpeciesDic.Add(new KeyValuePair<string, string>("Gem", "技能寶石"));
            SpeciesDic.Add(new KeyValuePair<string, string>("Other", "其他"));
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

        private void button1_Click(object sender, EventArgs e)
        {
            using (StreamReader r = new StreamReader(Path.Combine(Application.StartupPath, "ItemList_Adden.txt"), Encoding.UTF8))
            {
                ItemList = JsonConvert.DeserializeObject<List<JsonClass.RootObject>>(r.ReadToEnd());
                if (ItemList == null)
                    ItemList = new List<RootObject>();
                ItemList.Add(new RootObject()
                {
                    c = textBox1.Text,
                    e = textBox2.Text,
                    type = SpeciesDic.Where(x => x.Value == comboBox1.Text).FirstOrDefault().Key,
                    w = Int32.Parse(comboBox2.Text),
                    h = Int32.Parse(comboBox3.Text),
                    GC = char.Parse(textBox3.Text),
                    url = textBox4.Text
                });
                r.Close();
                using (StreamWriter w = new StreamWriter(Path.Combine(Application.StartupPath, "ItemList_Adden.txt"), false, Encoding.UTF8))
                {                    
                    w.Write(JsonConvert.SerializeObject(ItemList,Formatting.Indented));
                    w.Flush();
                }
            }
            this.Close();
        }
    }
}
