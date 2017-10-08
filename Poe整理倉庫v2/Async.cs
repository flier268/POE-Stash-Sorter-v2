using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Poe整理倉庫v2
{
    public partial class Form1
    {
        /// <summary>
        /// 移動滑鼠到x,y上(桌面座標)，並複製
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void POE_GetItemInfo(int x,int y)
        {
            MouseTools.SetCursorPosition(x, y);
            Thread.Sleep(Config.Delay_Scan);
            KeyBoardTool.KeyDown(Keys.LControlKey);
            KeyBoardTool.KeyDown(Keys.C);
            Thread.Sleep(Config.Delay_Scan);
            KeyBoardTool.KeyUp(Keys.C);            
            KeyBoardTool.KeyUp(Keys.LControlKey);
            Thread.Sleep(Config.Delay_Scan);
            Application.DoEvents();
        }
        private string reg = @":\s(.*?)\r\n(.*?)\r\n(.*?)--------\r\n(.*)";

        string GetStringAfterSomething(string s, string beforeWhat)
        {
            string temp = s.Substring(s.IndexOf(beforeWhat) + 1, s.Length - s.IndexOf(beforeWhat) - 1);
            return temp;
        }
        /// <summary>
        /// 取得並分析POE倉庫頁面中所有物品的資訊
        /// </summary>
        /// <param name="length">倉庫頁的大小，12或24，由於本人沒有24*24倉庫頁，因此24未完成</param>
        public async void GetWarehouse(int length = 12)
        {
            await Task.Delay(0);
            Items.Clear();
            used.Clear();
            resoult.Clear();
            
            //可能需要英文化
            string reg_itemlevel = @"物品等級:\s(\d+)";
            string reg_quality = @"品質:\s\+(\d+)";
            string reg_level = @"(?<!需求:)\r\n^等級:\s(\d+)";
            string reg_maplevel = @"地圖階級:\s(\d+)";


            Regex r_itemlevel = new Regex(reg_itemlevel, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex r_quality = new Regex(reg_quality, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex r_level = new Regex(reg_level, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex r_maplevel = new Regex(reg_maplevel, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex r = new Regex(reg, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match m, m_itemlevel, m_quality, m_level,m_maplevel;
            int id = 0;
            for (int x = 0; x < 12; x++)
            {
                for (int y = 0; y < 12; y++)
                {
                    if (Stop)
                    {
                        DrawBoxRegion(Items, 1);
                        return;
                    }
                    if (used.Any(u => u.X == x && u.Y == y))
                        continue;
                    Clipboard.Clear();
                    POE_GetItemInfo((int)(startPos.X + cellWidth * x), (int)(startPos.Y + cellHeight * y));
                    
                    
                    string clip = Clipboard.GetText(TextDataFormat.UnicodeText);
                    if (clip=="")
                        clip = Clipboard.GetText(TextDataFormat.UnicodeText);
                    if (clip == "")
                        continue;

                    m = r.Match(clip);

                    Item temp = new Item();
                    if (m.Groups[3].ToString() == "")
                    {

                        temp.Name = m.Groups[2].ToString().Trim();
                    }
                    else
                    {
                        if (m.Groups[1].ToString() == "傳奇" || m.Groups[1].ToString() == "Unique")
                            temp.Name = m.Groups[2].ToString().Trim()+" "+ m.Groups[3].ToString().Trim();
                        else
                            temp.Name = m.Groups[3].ToString().Trim();
                    }

                    temp.Rarity = m.Groups[1].ToString().Trim();


                    JsonClass.RootObject t;
                    if (m.Groups[1].ToString() == "傳奇" || m.Groups[1].ToString() == "Unique")
                        t = ItemList_Unique.Where(a => a.c.EndsWith(GetStringAfterSomething(temp.Name,"」")) || a.e.EndsWith(GetStringAfterSomething(temp.Name, "」"))).FirstOrDefault();
                    else
                        t = ItemList.Where(a => temp.Name.EndsWith(a.c) || temp.Name.EndsWith(a.e)).FirstOrDefault();
                    temp.w = t.w;
                    temp.h = t.h;
                    temp.point = new POINT(x, y);
                    temp.url = t.url;
                    temp.GC = t.GC;
                    temp.Name_eng = t.e;
                    temp.type = t.type;
                    m_itemlevel = r_itemlevel.Match(m.Groups[4].ToString());
                    m_level = r_level.Match(m.Groups[4].ToString());
                    m_maplevel=r_maplevel.Match(m.Groups[4].ToString());
                    m_quality = r_quality.Match(m.Groups[4].ToString());
                    temp.itemlevel = m_itemlevel.Groups.Count == 1 ? 0 : int.Parse(m_itemlevel.Groups[1].ToString());
                    temp.level = m_level.Groups.Count == 1 ? 0 : int.Parse(m_level.Groups[1].ToString());
                    temp.quality = m_quality.Groups.Count == 1 ? 0 : int.Parse(m_quality.Groups[1].ToString());

                    temp.maplevel = m_maplevel.Groups.Count == 1 ? 0 : int.Parse(m_maplevel.Groups[1].ToString());
                    temp.priority = Array.IndexOf(Config.Species, t.type);
                    
                    temp.id = ++id;

                    for (int i = x; i < x + t.w; i++)
                        for (int j = y; j < y + t.h; j++)
                            used.Add(new POINT(i, j));
                    Items.Add(temp);
                }
            }
            DrawBoxRegion(Items,1);
        }


        static void RunAsSTAThread(Action goForIt)
        {
            AutoResetEvent @event = new AutoResetEvent(false);
            Thread thread = new Thread(
                () =>
                {
                    goForIt();
                    @event.Set();
                });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            @event.WaitOne();
        }
        public async void StartSorting()
        {
            await Task.Delay(0);
            //由於ClipBoard的緣故，需要在STAThread執行
            RunAsSTAThread(
            () =>
            {
                if (resoult.Count > 0)
                {
                    List<Item> _Items = new List<Item>();
                    Items.ForEach(x => _Items.Add((Item)x.Clone()));
                    if (radioButton6.Checked)
                    {
                        //從結果找到一個跟目前同個ID但不同位置的物品
                        var diff = resoult.Where(x => !x.point.Equals(_Items.Where(y => y.id == x.id).FirstOrDefault().point)).Select(t => t).FirstOrDefault();
                        Item onHand = null;
                        while (diff != null)
                        {

                            if (Stop)
                                return;
                            Item p0 = _Items.Where(x => x.id == diff.id).Select(t => t).FirstOrDefault();
                            ClickItem(poeHwnd,
                                   (int)(((float)p0.point.X * cellWidth) + startPos.X),
                                   (int)(((float)p0.point.Y * cellHeight) + startPos.Y));
                            ClickItem(poeHwnd,
                                   (int)(((float)diff.point.X * cellWidth) + startPos.X),
                                   (int)(((float)diff.point.Y * cellHeight) + startPos.Y));

                            p0.point = new POINT(diff.point);


                            onHand = _Items.Where(x => x.id != diff.id && x.point.Equals(diff.point)).Select(t => t).FirstOrDefault();
                            while (onHand != null)
                            {

                                if (Stop)
                                    return;
                                Item p3 = resoult.Where(x => x.id == onHand.id).FirstOrDefault();

                                ClickItem(poeHwnd,
                                       (int)(((float)p3.point.X * cellWidth) + startPos.X),
                                       (int)(((float)p3.point.Y * cellHeight) + startPos.Y));

                                onHand.point = new POINT(p3.point);
                                onHand = _Items.Where(x => x.id != onHand.id && x.point.Equals(onHand.point)).Select(t => t).FirstOrDefault();

                            }

                            diff = resoult.Where(x => !x.point.Equals(_Items.Where(y => y.id == x.id).FirstOrDefault().point)).Select(t => t).FirstOrDefault();
                        }
                    }
                    else
                    {
                        //檢查背包第一二格是否清空
                        MouseTools.SetCursorPosition(startPos.X, startPos.Y - (int)cellHeight * 3);
                        MouseTools.MouseClickEvent(70);
                        MouseTools.MouseClickEvent(70);
                        Task.Delay(500);
                        Clipboard.Clear();
                        POE_GetItemInfo(bagstartPos.X, bagstartPos.Y);
                        string clip = Clipboard.GetText(TextDataFormat.UnicodeText);
                        if (clip == "")
                            clip = Clipboard.GetText(TextDataFormat.UnicodeText);
                        if (clip == "")
                        {
                            Clipboard.Clear();
                            POE_GetItemInfo(bagstartPos.X, bagstartPos.Y + (int)cellHeight);
                            clip = Clipboard.GetText(TextDataFormat.UnicodeText);
                            if (clip == "")
                                clip = Clipboard.GetText(TextDataFormat.UnicodeText);
                            if (clip != "")
                            {
                                MessageBox.Show(String.Format("請清空背包第二格\n物品資訊為：\n\n{0}", clip));
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show(String.Format("請清空背包第二格\n物品資訊為：\n\n{0}", clip));
                            return;
                        }


                        //依照結果清單和目前清單，建立可用暫存空間的清單
                        List<Item> swap = new List<Item>();
                        swap.Add(new Item() { point = new POINT(0, 0) });
                        swap.Add(new Item() { point = new POINT(0, 1) });



                        //從結果找到一個跟目前同個ID但不同位置的物品
                        var diff = resoult.Where(x => !x.point.Equals(_Items.Where(y => y.id == x.id && y.point.X >= 0).FirstOrDefault().point)).Select(t => t).FirstOrDefault();

                        while (diff != null)
                        {

                            if (Stop)
                                return;
                            Item k0 = _Items.Where(x => x.point.Equals(diff.point)).FirstOrDefault();
                            if (k0 != null)
                            {
                                //原本的位置有東西占用，先移到swap
                                ClickItem(poeHwnd,
                                    (int)(((float)diff.point.X * cellWidth) + startPos.X),
                                    (int)(((float)diff.point.Y * cellHeight) + startPos.Y));

                                var k1 = swap.Where(x => x.id == 0).FirstOrDefault();
                                if (k1 != null)
                                {
                                    ClickItem(poeHwnd,
                                           (int)(((float)k1.point.X * cellWidth) + bagstartPos.X),
                                           (int)(((float)k1.point.Y * cellHeight) + bagstartPos.Y));

                                    k1.id = k0.id;
                                    k0.point = new POINT(-1 - k1.point.X, -1 - k1.point.Y);
                                }
                                else
                                {

                                }

                            }
                            Item p0 = _Items.Where(x => x.id == diff.id).Select(t => t).FirstOrDefault();
                            ClickItem(poeHwnd,
                                   (int)(((float)p0.point.X * cellWidth) + startPos.X),
                                   (int)(((float)p0.point.Y * cellHeight) + startPos.Y));

                            ClickItem(poeHwnd,
                                   (int)(((float)diff.point.X * cellWidth) + startPos.X),
                                   (int)(((float)diff.point.Y * cellHeight) + startPos.Y));

                            p0.point = new POINT(diff.point);


                            while (swap.Any(x => x.id != 0))
                            {

                                if (Stop)
                                    return;
                                var FirstItemInSwap = swap.Where(x => x.id != 0).FirstOrDefault();
                                var FirstItemInResoult_IdIsFirstItemInSwap = resoult.Where(x => x.id.Equals(FirstItemInSwap.id)).FirstOrDefault();
                                var ItemNow = _Items.Where(x => x.point.Equals(FirstItemInResoult_IdIsFirstItemInSwap.point)).FirstOrDefault();
                                if (ItemNow != null)
                                {
                                    ClickItem(poeHwnd,
                                               (int)(((float)FirstItemInResoult_IdIsFirstItemInSwap.point.X * cellWidth) + startPos.X),
                                               (int)(((float)FirstItemInResoult_IdIsFirstItemInSwap.point.Y * cellHeight) + startPos.Y));

                                    var FirstFreeSapceInSwap = swap.Where(x => x.id == 0).FirstOrDefault();
                                    if (FirstFreeSapceInSwap != null)
                                    {
                                        ClickItem(poeHwnd,
                                               (int)(((float)FirstFreeSapceInSwap.point.X * cellWidth) + bagstartPos.X),
                                               (int)(((float)FirstFreeSapceInSwap.point.Y * cellHeight) + bagstartPos.Y));

                                        FirstFreeSapceInSwap.id = ItemNow.id;
                                        _Items.Where(x => x.id == FirstItemInResoult_IdIsFirstItemInSwap.id).FirstOrDefault().point = new POINT(-1 - FirstFreeSapceInSwap.point.X, -1 - FirstFreeSapceInSwap.point.Y * -1);
                                    }
                                    else
                                    {

                                    }
                                }
                                ClickItem(poeHwnd,
                                           (int)(((float)FirstItemInSwap.point.X * cellWidth) + bagstartPos.X),
                                           (int)(((float)FirstItemInSwap.point.Y * cellHeight) + bagstartPos.Y));


                                ClickItem(poeHwnd,
                                           (int)(((float)FirstItemInResoult_IdIsFirstItemInSwap.point.X * cellWidth) + startPos.X),
                                           (int)(((float)FirstItemInResoult_IdIsFirstItemInSwap.point.Y * cellHeight) + startPos.Y));

                                _Items.Where(x => x.id == FirstItemInSwap.id).FirstOrDefault().point = new POINT(FirstItemInResoult_IdIsFirstItemInSwap.point);
                                FirstItemInSwap.id = 0;
                            }
                            diff = resoult.Where(x => !x.point.Equals(_Items.Where(y => y.id == x.id && y.point.X >= 0).FirstOrDefault().point)).Select(t => t).FirstOrDefault();
                        }
                    }
                }
            });
            Stop = true;
        }
        
        private void DrawBoxRegion(List<Item> _items,int info)
        {
            Graphics g;
            MemoryStream stream;
            Image img;
            SolidBrush drawBrush = new SolidBrush(Color.Red);
            Font drawFont = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);
            System.Net.WebClient WC = new System.Net.WebClient();

            g = info == 1 ? Graphics.FromImage(RegionImage) : Graphics.FromImage(RegionImage2);

            g.Clear(Color.Black);

            stream = new MemoryStream(WC.DownloadData("https://web.poe.garena.tw/image/gen/inventory/StashPanelGrid.png"));
            img = Image.FromStream(stream);
            stream.Close();
            g.DrawImage(img, 0, 0, 480, 480);

            
            if (info == 1)
            {
                foreach (Item item in _items)
                {
                    if (File.Exists(Path.Combine(Application.StartupPath, "Image", Path.ChangeExtension(item.Name_eng, ".png"))))
                        img = Image.FromFile(Path.Combine(Application.StartupPath, "Image", Path.ChangeExtension(item.Name_eng, ".png")));
                    else
                    {
                        stream = new MemoryStream(WC.DownloadData(item.url));
                        img = Image.FromStream(stream);
                        stream.Close();
                    }                    
                    g.DrawImage(img, item.point.X * 40, item.point.Y * 40, item.w * 40, item.h * 40);
                    g.DrawString(item.id.ToString(), drawFont, drawBrush, item.point.X * 40, item.point.Y * 40);
                    pictureBox1.Image = RegionImage;

                }
                pictureBox1.Image = RegionImage;
            }
            else
            {
                foreach (Item item in _items)
                {
                    if (File.Exists(Path.Combine(Application.StartupPath, "Image", Path.ChangeExtension(item.Name_eng, ".png"))))
                        img = Image.FromFile(Path.Combine(Application.StartupPath, "Image", Path.ChangeExtension(item.Name_eng, ".png")));
                    else
                    {
                        stream = new MemoryStream(WC.DownloadData(item.url));
                        img = Image.FromStream(stream);
                        stream.Close();
                    }
                    g.DrawImage(img, item.point.X * 40, item.point.Y * 40, item.w * 40, item.h * 40);
                    g.DrawString(item.id.ToString(), drawFont, drawBrush, item.point.X * 40, item.point.Y * 40);
                    pictureBox2.Image = RegionImage2;

                }
                pictureBox2.Image = RegionImage2;
            }
        }
    }
}
