using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Poe整理倉庫v2
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        Setting Config = new Setting();
        List<KeyValuePair<string, string>> SpeciesDic = new List<KeyValuePair<string, string>>();
        List<KeyValuePair<string, string>> PriorityDic = new List<KeyValuePair<string, string>>();
        private void button_TypeListUp_Click(object sender, EventArgs e)
        {
            int i = listBox_TypeList.SelectedIndex;
            if (i >= 0)
            {
                object objItem = listBox_TypeList.Items[i];
                listBox_TypeList.Items.RemoveAt(i);
                listBox_TypeList.Items.Insert(i - 1, objItem);
            }
        }

        private void button_TypeListDown_Click(object sender, EventArgs e)
        {
            int i = listBox_TypeList.SelectedIndex;
            if (i >= 0)
            {
                object objItem = listBox_TypeList.Items[i];
                listBox_TypeList.Items.RemoveAt(i);
                listBox_TypeList.Items.Insert(i + 1, objItem);
            }
        }
        private void button_PriorityUp_Click(object sender, EventArgs e)
        {
            int i = listBox_Priority.SelectedIndex;
            if (i >= 0)
            {
                object objItem = listBox_Priority.Items[i];
                listBox_Priority.Items.RemoveAt(i);
                listBox_Priority.Items.Insert(i - 1, objItem);
            }
        }
        private void button_PriorityDown_Click(object sender, EventArgs e)
        {
            int i = listBox_Priority.SelectedIndex;
            if (i >= 0)
            {
                object objItem = listBox_Priority.Items[i];
                listBox_Priority.Items.RemoveAt(i);
                listBox_Priority.Items.Insert(i + 1, objItem);
            }
        }

       

        private void button_Save_Click(object sender, EventArgs e)
        {
            List<string> temp = new List<string>();
            foreach(string t in listBox_TypeList.Items)
            {
                temp.Add(SpeciesDic.Where(x => x.Value == t).FirstOrDefault().Key);               
            }            
            Config.Species = temp.ToArray();

            temp = new List<string>();
            foreach (string t in listBox_Priority.Items)
            {
                temp.Add(PriorityDic.Where(x => x.Value == t).FirstOrDefault().Key);
            }
            Config.Priority = temp.ToArray();

            Config.HotkeyStart = ToASCII(comboBox_hotkey_Start.SelectedItem.ToString());
            Config.HotkeyStop = ToASCII(comboBox_hotkey_Stop.SelectedItem.ToString());
            Config.LowQ = Int32.Parse(textBox1.Text) * (checkBox1.Checked ? 1 : -1);
            Config.Delay1 = trackBar1.Value;
            Config.Delay2 = trackBar2.Value;
            Config.Delay_Scan = trackBar3.Value;
         
            //FileStream fs = new FileStream(Path.ChangeExtension(Application.ExecutablePath, ".cfg"),FileMode..Truncate);
            using (StreamWriter w = new StreamWriter(Path.ChangeExtension(Application.ExecutablePath, ".cfg"),false, Encoding.UTF8))
            {
                w.Write(Serialize.ToJson(Config));
                w.Flush();
                MessageBox.Show("Saved");
            }
               
        }
        private int ToASCII(string Key)
        {
            switch(Key)
            {
                case "F1":
                    return 112;                    
                case "F2":
                    return 113;
                case "F3":
                    return 114;
                case "F4":
                    return 115;
                case "F5":
                    return 116;
                case "F6":
                    return 117;
                case "F7":
                    return 118;
                case "F8":
                    return 119;
                case "F9":
                    return 120;
                case "F10":
                    return 121;
                case "F11":
                    return 122;
                case "F12":
                    return 123;
                case "F13":
                    return 124;
                case "F14":
                    return 125;
                case "F15":
                    return 126;
                default:
                    return 0;
            }
        }
        private void AddSpeciesDic()
        {
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
        }
        private void AddPriorityDic()
        {
            PriorityDic.Add(new KeyValuePair<string, string>("MapLevel", "地圖等級"));
            PriorityDic.Add(new KeyValuePair<string, string>("Q", "品質"));
            PriorityDic.Add(new KeyValuePair<string, string>("Name", "物品名稱"));
            PriorityDic.Add(new KeyValuePair<string, string>("ItemLevel", "物品等級"));
            PriorityDic.Add(new KeyValuePair<string, string>("GemColor", "寶石顏色"));
            PriorityDic.Add(new KeyValuePair<string, string>("Rarity", "稀有度"));
            PriorityDic.Add(new KeyValuePair<string, string>("Type", "類型"));
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            using (StreamReader r = new StreamReader(Path.ChangeExtension(Application.ExecutablePath, ".cfg"), Encoding.UTF8))
            {
                Config =Setting.FromJson(r.ReadToEnd());
            }

            AddSpeciesDic();
            listBox_TypeList.Items.Clear();
            foreach (string t in Config.Species)
            {
                listBox_TypeList.Items.Add(SpeciesDic.Where(x => x.Key == t).FirstOrDefault().Value);               
            }

            AddPriorityDic();
            listBox_Priority.Items.Clear();
            foreach (string t in Config.Priority)
            {
                listBox_Priority.Items.Add(PriorityDic.Where(x => x.Key == t).FirstOrDefault().Value);
            }


            comboBox_hotkey_Start.SelectedItem = ToKode(Config.HotkeyStart);
            comboBox_hotkey_Stop.SelectedItem = ToKode(Config.HotkeyStop);
            checkBox1.Checked = Config.LowQ > 0;
            textBox1.Text = Math.Abs(Config.LowQ).ToString();
            trackBar1.Value = Config.Delay1;
            trackBar2.Value = Config.Delay2;
            trackBar3.Value = Config.Delay_Scan;
        }
        private string ToKode(int ASCII)
        {
            switch (ASCII)
            {
                case 112:
                    return "F1";
                case 113:
                    return "F2";
                case 114:
                    return "F3";
                case 115:
                    return "F4";
                case 116:
                    return "F5";
                case 117:
                    return "F6";
                case 118:
                    return "F7";
                case 119:
                    return "F8";
                case 120:
                    return "F9";
                case 121:
                    return "F10";
                case 122:
                    return "F11";
                case 123:
                    return "F12";
                case 124:
                    return "F13";
                case 125:
                    return "F14";
                case 126:
                    return "F15";
                default:
                    return "None";
            }
        }
    }
}
