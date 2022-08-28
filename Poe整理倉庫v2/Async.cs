using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataGetter;
using LiteDB;

namespace Poe整理倉庫v2
{
    public partial class Form1
    {
        /// <summary>
        /// 移動滑鼠到x,y上(桌面座標)，並複製
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void POE_GetItemInfo(int x, int y)
        {
            MouseTools.SetCursorPosition(x, y);
            Thread.Sleep(Config.Delay_Scan);
            KeyBoardTool.KeyDown(Keys.ControlKey);
            KeyBoardTool.KeyDown(Keys.C);
            Thread.Sleep(Config.Delay_Scan);
            KeyBoardTool.KeyUp(Keys.C);
            KeyBoardTool.KeyUp(Keys.ControlKey);
            Thread.Sleep(Config.Delay_Scan);
            Application.DoEvents();
        }

        /*
            物品種類: 手套
            稀有度: 稀有
            精良的 粗革手套
            --------
            品質: +6% (augmented)
            閃避值: 188 (augmented)
            --------
            需求:
            敏捷: 78
            --------
            插槽: R-G-G-G
            --------
            物品等級: 77
            --------
            未鑑定
        */
        private string reg = @"(?:稀有度|Rarity):\s(.*?)\r\n(.*?)\r\n(.*?)--------\r\n(.*)";

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

            string reg_itemlevel = @"物品等級:\s(\d+)", reg_itemlevel_eng = @"Item Level:\s(\d+)";
            string reg_quality = @"品質:\s\+(\d+)", reg_quality_eng = @"Quality:\s\+(\d+)";
            string reg_level = @"(?<!需求:)\r\n^等級:\s(\d+)", reg_level_eng = @"(?<!Requirements:)\r\n^Level:\s(\d+)";
            string reg_maplevel = @"地圖階級:\s(\d+)", reg_maplevel_eng = @"Map Tier:\s(\d+)";

            Regex r_itemlevel = new Regex(reg_itemlevel, RegexOptions.IgnoreCase | RegexOptions.Multiline),
                r_itemlevel_eng = new Regex(reg_itemlevel_eng, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex r_quality = new Regex(reg_quality, RegexOptions.IgnoreCase | RegexOptions.Multiline),
                r_quality_eng = new Regex(reg_quality_eng, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex r_level = new Regex(reg_level, RegexOptions.IgnoreCase | RegexOptions.Multiline),
                r_level_eng = new Regex(reg_level_eng, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex r_maplevel = new Regex(reg_maplevel, RegexOptions.IgnoreCase | RegexOptions.Multiline),
                r_maplevel_eng = new Regex(reg_maplevel_eng, RegexOptions.IgnoreCase | RegexOptions.Multiline);

            Regex r = new Regex(reg, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match m, m_itemlevel, m_quality, m_level, m_maplevel;
            int id = 0;
            for (int positionX = 0; positionX < length; positionX++)
            {
                for (int positionY = 0; positionY < length; positionY++)
                {
                    if (Stop)
                    {
                        DrawBoxRegion(Items, length, 1);
                        return;
                    }
                    if (used.Any(u => u.X == positionX && u.Y == positionY))
                        continue;
                    Clipboard.Clear();
                    if (length == 12)
                        POE_GetItemInfo((int)(startPos1.X + cellWidth1 * positionX), (int)(startPos1.Y + cellHeight1 * positionY));
                    else
                        POE_GetItemInfo((int)(startPos4.X + cellWidth4 * positionX), (int)(startPos4.Y + cellHeight4 * positionY));

                    string clip = Clipboard.GetText(TextDataFormat.UnicodeText);
                    if (clip == "")
                        clip = Clipboard.GetText(TextDataFormat.UnicodeText);
                    if (clip == "")
                        continue;

                    m = r.Match(clip);
                    if (clip.Contains("赤紅珠寶"))
                    {
                    }
                    Item temp = new Item();
                    if (m.Groups[3].Value == "")
                    {
                        temp.Name = m.Groups[2].Value.Trim().Replace("精良的 ", "").Replace("Superior ", "");
                        temp.Base = temp.Name;
                    }
                    else
                    {
                        if (m.Groups[1].Value == "傳奇" || m.Groups[1].Value == "Unique")
                        {
                            temp.Name = m.Groups[2].Value.Trim();
                            temp.Base = m.Groups[3].Value.Trim();
                        }
                        else
                        {
                            // 非傳奇有詞墜，捨棄了詞墜，用Name當Name
                            temp.Name = m.Groups[3].Value.Trim();
                            temp.Base = m.Groups[3].Value.Trim();
                        }
                    }

                    temp.Rarity = m.Groups[1].Value.Trim();
                    m_itemlevel = r_itemlevel.Match(m.Groups[4].Value);
                    m_itemlevel = m_itemlevel.Groups.Count == 1 ? r_itemlevel_eng.Match(m.Groups[4].Value) : m_itemlevel;
                    m_level = r_level.Match(m.Groups[4].Value);
                    m_level = m_level.Groups.Count == 1 ? r_level_eng.Match(m.Groups[4].Value) : m_level;
                    m_maplevel = r_maplevel.Match(m.Groups[4].Value);
                    m_maplevel = m_maplevel.Groups.Count == 1 ? r_maplevel_eng.Match(m.Groups[4].Value) : m_maplevel;
                    m_quality = r_quality.Match(m.Groups[4].Value);
                    m_quality = m_quality.Groups.Count == 1 ? r_quality_eng.Match(m.Groups[4].Value) : m_quality;
                    temp.itemlevel = m_itemlevel.Groups.Count == 1 ? 0 : int.Parse(m_itemlevel.Groups[1].Value);
                    temp.level = m_level.Groups.Count == 1 ? 0 : int.Parse(m_level.Groups[1].Value);
                    temp.quality = m_quality.Groups.Count == 1 ? 0 : int.Parse(m_quality.Groups[1].Value);
                    temp.maplevel = m_maplevel.Groups.Count == 1 ? 0 : int.Parse(m_maplevel.Groups[1].Value);

                    Data data;
                    if (PrivateFunction.IsChineseContain(temp.Name))
                    {
                        data = DataRepository.FindOne(x => x.Name_Chinese == temp.Name);
                        if (data == null)
                        {
                            data = TakeMostSimilarOne(x => temp.Name.EndsWith(x.Name_Chinese));
                        }
                        if (data == null)
                        {
                            data = TakeMostSimilarOne(x => temp.Name.Contains(x.Name_Chinese));
                        }
                    }
                    else
                    {
                        data = DataRepository.FindOne(x => x.Name_English == temp.Name);
                        if (data == null)
                        {
                            data = TakeMostSimilarOne(x => temp.Name.EndsWith(x.Name_English));
                        }
                        if (data == null)
                        {
                            data = TakeMostSimilarOne(x => temp.Name.Contains(x.Name_English));
                        }
                    }
                    Data TakeMostSimilarOne(Expression<Func<Data, bool>> predicate)
                    {
                        var list = DataRepository.Find(predicate);
                        if (list.Count() > 1)
                            return list.Select(x => (Data: x, Similarity: x.Name_Chinese.GetSimilarityWith(temp.Name))).OrderByDescending(x => x.Similarity).First().Data;
                        else if (list.Count() == 1)
                            return list.First();
                        return null;
                    }

                    while (data == null)
                    {
                        Form2 f = new Form2(clip, temp.Name);
                        f.ShowDialog();
                        var databasePath = Path.Combine(Application.StartupPath, "Datas_Adden.db");

                        using (var db = new LiteDatabase(databasePath))
                        {
                            var col = db.GetCollection<Data>();
                            col.EnsureIndex(a => a.Name_English);
                            col.EnsureIndex(a => a.Name_Chinese);
                            ItemList_Adden = col.FindAll().ToList();
                        }
                        data = ItemList_Adden.Where(a => temp.Name.Equals(a.Name_Chinese) || temp.Name.Equals(a.Name_English)).FirstOrDefault();
                    }
                    temp.w = data.Width;
                    temp.h = data.Height;
                    temp.point = new POINT(positionX, positionY);
                    temp.url = data.ImageURL;
                    temp.GC = data.GemColor[0];

                    temp.Name = data.Name_Chinese;
                    temp.Name_eng = data.Name_English;

                    temp.type = data.Type;

                    temp.priority = Array.IndexOf(Config.Species, data.Type);

                    temp.Id = ++id;

                    for (int i = positionX; i < positionX + data.Width; i++)
                        for (int j = positionY; j < positionY + data.Height; j++)
                            used.Add(new POINT(i, j));
                    Items.Add(temp);
                }
            }
            DrawBoxRegion(Items, length, 1);
        }

        private static void RunAsSTAThread(Action goForIt)
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

        public async void StartSorting(int length)
        {
            await Task.Delay(0);
            if (resoult.Count > 0)
            {
                //由於ClipBoard的緣故，需要在STAThread執行
                RunAsSTAThread(
                   () =>
                   {
                       List<Item> _Items = new List<Item>();
                       Items.ForEach(x => _Items.Add((Item)x.Clone()));
                       if (radioButton6.Checked)
                       {
                           //從結果找到一個跟目前同個ID但不同位置的物品
                           var diff = resoult.Where(x => !x.point.Equals(_Items.Where(y => y.Id == x.Id).FirstOrDefault().point)).Select(t => t).FirstOrDefault();
                           Item onHand = null;
                           while (diff != null)
                           {
                               if (Stop)
                                   return;
                               Item p0 = _Items.Where(x => x.Id == diff.Id).Select(t => t).FirstOrDefault();
                               ClickItem(poeHwnd,
                                      (int)(((float)p0.point.X * (length == 12 ? cellWidth1 : cellWidth4)) + (length == 12 ? startPos1.X : startPos4.X)),
                                      (int)(((float)p0.point.Y * (length == 12 ? cellHeight1 : cellHeight4)) + (length == 12 ? startPos1.Y : startPos4.Y)));
                               ClickItem(poeHwnd,
                                      (int)(((float)diff.point.X * (length == 12 ? cellWidth1 : cellWidth4)) + (length == 12 ? startPos1.X : startPos4.X)),
                                      (int)(((float)diff.point.Y * (length == 12 ? cellHeight1 : cellHeight4)) + (length == 12 ? startPos1.Y : startPos4.Y)));

                               p0.point = new POINT(diff.point);

                               onHand = _Items.Where(x => x.Id != diff.Id && x.point.Equals(diff.point)).Select(t => t).FirstOrDefault();
                               while (onHand != null)
                               {
                                   if (Stop)
                                       return;
                                   Item p3 = resoult.Where(x => x.Id == onHand.Id).FirstOrDefault();
                                   ClickItem(poeHwnd,
                                        (int)(((float)p3.point.X * (length == 12 ? cellWidth1 : cellWidth4)) + (length == 12 ? startPos1.X : startPos4.X)),
                                        (int)(((float)p3.point.Y * (length == 12 ? cellHeight1 : cellHeight4)) + (length == 12 ? startPos1.Y : startPos4.Y)));
                                   onHand.point = new POINT(p3.point);
                                   onHand = _Items.Where(x => x.Id != onHand.Id && x.point.Equals(onHand.point)).Select(t => t).FirstOrDefault();
                               }

                               diff = resoult.Where(x => !x.point.Equals(_Items.Where(y => y.Id == x.Id).FirstOrDefault().point)).Select(t => t).FirstOrDefault();
                           }
                       }
                       else
                       {
                           //檢查背包第一二格是否清空
                           MouseTools.SetCursorPosition((startPos1.X), startPos1.Y - (int)cellHeight1 * 3);
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
                               POE_GetItemInfo(bagstartPos.X, bagstartPos.Y + (int)cellHeight1);
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
                           var diff = resoult.Where(x => !x.point.Equals(_Items.Where(y => y.Id == x.Id && y.point.X >= 0).FirstOrDefault().point)).Select(t => t).FirstOrDefault();
                           while (diff != null)
                           {
                               if (Stop)
                                   return;
                               Item k0 = _Items.Where(x => x.point.Equals(diff.point)).FirstOrDefault();
                               if (k0 != null)
                               {
                                   //原本的位置有東西占用，先移到swap
                                   ClickItem(poeHwnd,
                                        (int)(((float)diff.point.X * (length == 12 ? cellWidth1 : cellWidth4)) + (length == 12 ? startPos1.X : startPos4.X)),
                                        (int)(((float)diff.point.Y * (length == 12 ? cellHeight1 : cellHeight4)) + (length == 12 ? startPos1.Y : startPos4.Y)));

                                   var k1 = swap.Where(x => x.Id == 0).FirstOrDefault();
                                   if (k1 != null)
                                   {
                                       ClickItem(poeHwnd,
                                            (int)(((float)k1.point.X * cellWidth1) + bagstartPos.X),
                                            (int)(((float)k1.point.Y * cellHeight1) + bagstartPos.Y));
                                       k1.Id = k0.Id;
                                       k0.point = new POINT(-1 - k1.point.X, -1 - k1.point.Y);
                                   }
                                   else
                                   {
                                   }
                               }
                               Item p0 = _Items.Where(x => x.Id == diff.Id).Select(t => t).FirstOrDefault();
                               ClickItem(poeHwnd,
                                       (int)(((float)p0.point.X * (length == 12 ? cellWidth1 : cellWidth4)) + (length == 12 ? startPos1.X : startPos4.X)),
                                       (int)(((float)p0.point.Y * (length == 12 ? cellHeight1 : cellHeight4)) + (length == 12 ? startPos1.Y : startPos4.Y)));

                               ClickItem(poeHwnd,
                                       (int)(((float)diff.point.X * (length == 12 ? cellWidth1 : cellWidth4)) + (length == 12 ? startPos1.X : startPos4.X)),
                                       (int)(((float)diff.point.Y * (length == 12 ? cellHeight1 : cellHeight4)) + (length == 12 ? startPos1.Y : startPos4.Y)));

                               p0.point = new POINT(diff.point);
                               while (swap.Any(x => x.Id != 0))
                               {
                                   if (Stop)
                                       return;
                                   var FirstItemInSwap = swap.Where(x => x.Id != 0).FirstOrDefault();
                                   var FirstItemInResoult_IdIsFirstItemInSwap = resoult.Where(x => x.Id.Equals(FirstItemInSwap.Id)).FirstOrDefault();
                                   var ItemNow = _Items.Where(x => x.point.Equals(FirstItemInResoult_IdIsFirstItemInSwap.point)).FirstOrDefault();
                                   if (ItemNow != null)
                                   {
                                       ClickItem(poeHwnd,
                                            (int)(((float)FirstItemInResoult_IdIsFirstItemInSwap.point.X * (length == 12 ? cellWidth1 : cellWidth4)) + (length == 12 ? startPos1.X : startPos4.X)),
                                            (int)(((float)FirstItemInResoult_IdIsFirstItemInSwap.point.Y * (length == 12 ? cellHeight1 : cellHeight4)) + (length == 12 ? startPos1.Y : startPos4.Y)));

                                       var FirstFreeSapceInSwap = swap.Where(x => x.Id == 0).FirstOrDefault();
                                       if (FirstFreeSapceInSwap != null)
                                       {
                                           ClickItem(poeHwnd,
                                                  (int)(((float)FirstFreeSapceInSwap.point.X * cellWidth1) + bagstartPos.X),
                                                  (int)(((float)FirstFreeSapceInSwap.point.Y * cellHeight1) + bagstartPos.Y));

                                           FirstFreeSapceInSwap.Id = ItemNow.Id;
                                           _Items.Where(x => x.Id == FirstItemInResoult_IdIsFirstItemInSwap.Id).FirstOrDefault().point = new POINT(-1 - FirstFreeSapceInSwap.point.X, -1 - FirstFreeSapceInSwap.point.Y * -1);
                                       }
                                       else
                                       {
                                       }
                                   }
                                   ClickItem(poeHwnd,
                                              (int)(((float)FirstItemInSwap.point.X * cellWidth1) + bagstartPos.X),
                                              (int)(((float)FirstItemInSwap.point.Y * cellHeight1) + bagstartPos.Y));

                                   ClickItem(poeHwnd,
                                           (int)(((float)FirstItemInResoult_IdIsFirstItemInSwap.point.X * (length == 12 ? cellWidth1 : cellWidth4)) + (length == 12 ? startPos1.X : startPos4.X)),
                                           (int)(((float)FirstItemInResoult_IdIsFirstItemInSwap.point.Y * (length == 12 ? cellHeight1 : cellHeight4)) + (length == 12 ? startPos1.Y : startPos4.Y)));

                                   _Items.Where(x => x.Id == FirstItemInSwap.Id).FirstOrDefault().point = new POINT(FirstItemInResoult_IdIsFirstItemInSwap.point);
                                   FirstItemInSwap.Id = 0;
                               }
                               diff = resoult.Where(x => !x.point.Equals(_Items.Where(y => y.Id == x.Id && y.point.X >= 0).FirstOrDefault().point)).Select(t => t).FirstOrDefault();
                           }
                       }
                   });
            }
            Stop = true;
        }

        private static Item[] ZeroArray = new Item[] { };

        private static Item[] Find40Q(List<Item> list, int Level, int level_Target, int index, Item[] temp)
        {
            for (int i = index; i < list.Count; i++)
            {
                if (Level == 1 && list[0].quality < 40 / level_Target)
                    break;

                temp[Level - 1] = list[i];
                if (list[i].quality == 20)
                    return new Item[] { list[i] };
                if (Level == 1 && list.Sum(x => x.quality) < 40)
                    return ZeroArray;
                else if (Level == level_Target)
                {
                    if ((temp.Take(temp.Length - 1).Select(x => x.quality).Sum() + list.Skip(i).Select(x => x.quality).Max()) < 40)
                        return ZeroArray;
                    if (temp.Sum(x => x.quality) == 40)
                        return temp;
                    else
                        continue;
                }
                else
                {
                    var t = Find40Q(list, Level + 1, level_Target, i + 1, ((Item[])temp.Clone()));
                    if (t == ZeroArray && Level == 1)
                        return null;
                    else if (t == ZeroArray)
                        return ZeroArray;
                    else if (t != null)
                        return t;
                }
            }
            return null;
        }

        /// <summary>
        /// 湊出品質和為40的技能寶石或藥劑
        /// </summary>
        /// <param name="StashWidth">倉庫頁寬度</param>
        /// <param name="mode">0: 藥劑 ; 1: 技能寶石</param>
        public async void Round40Q(int StashWidth, int mode)
        {
            await Task.Delay(0);
            List<Item> _Items = new List<Item>();
            if (mode == 0)
                _Items = Items.Where(x => x.type.EndsWith("Flask") && x.quality > 1).ToList();
            else if (mode == 1)
                _Items = Items.Where(x => x.type.EndsWith("Skill Gem") && x.quality > 1).ToList();

            _Items = _Items.OrderByDescending(x => x.quality).ToList();
            var list_Priview = new List<Item>(_Items);
            List<Item> list_Move = new List<Item>();
            int Move_Count = 0;
            int MaxRound = Math.Min((int)Math.Ceiling(40.0 / list_Priview.Min(x => x.quality)), mode == 0 ? 24 : 60);
            for (int i = 3; i <= MaxRound; i++)
            {
                var ans = Find40Q(list_Priview, 1, i, 0, new Item[i]);
                while (ans != null && ans != ZeroArray)
                {
                    if (Move_Count + ans.Count() <= (mode == 0 ? 24 : 60))
                    {
                        list_Move.AddRange(ans);
                        foreach (var v in ans)
                            list_Priview.Remove(v);
                        Move_Count += ans.Count();
                    }
                    else
                    {
                        i = 25;
                        break;
                    }
                    ans = Find40Q(list_Priview, 1, i, 0, new Item[i]);
                }
            }
            int BagsHave2xItem = 0;
            list_Move.ForEach(x =>
            {
                GetItem(poeHwnd,
                      (int)(((float)x.point.X * (StashWidth == 12 ? cellWidth1 : cellWidth4)) + (StashWidth == 12 ? startPos1.X : startPos4.X)),
                      (int)(((float)x.point.Y * (StashWidth == 12 ? cellHeight1 : cellHeight4)) + (StashWidth == 12 ? startPos1.Y : startPos4.Y)));
                BagsHave2xItem++;
                Items.Remove(x);
            });
        }

        private void DrawBoxRegion(List<Item> _items, int length, int info)
        {
            Graphics g;
            Image img;
            SolidBrush drawBrush = new SolidBrush(Color.Red);
            Font drawFont = new Font("Arial", 60 / length, FontStyle.Bold, GraphicsUnit.Millimeter);
            System.Net.WebClient WC = new System.Net.WebClient();

            g = info == 1 ? Graphics.FromImage(RegionImage) : Graphics.FromImage(RegionImage2);

            g.Clear(Color.Black);
            if (length == 12)
            {
                img = Image.FromStream(DataRepository.GetFile("StashPanelGrid").OpenRead());
            }
            else
            {
                img = Image.FromStream(DataRepository.GetFile("QuadStashPanelGrid").OpenRead());
            }

            g.DrawImage(img, 0, 0, 480, 480);

            if (info == 1)
            {
                foreach (Item item in _items)
                {
                    if (item.url != "question-mark.png")
                    {
                        var file = DataRepository.GetFile($"{item.type}\\{item.Name_eng}");
                        if (file != null)
                        {
                            img = Image.FromStream(file.OpenRead());
                        }
                        else
                        {
                            img = Properties.Resources.question_mark;
                        }
                    }
                    else
                    {
                        img = Properties.Resources.question_mark;
                    }
                    g.DrawImage(img, item.point.X * 480 / length, item.point.Y * 480 / length, item.w * 480 / length, item.h * 480 / length);
                    g.DrawString(item.Id.ToString(), drawFont, drawBrush, item.point.X * 480 / length, item.point.Y * 480 / length);
                    pictureBox1.Image = RegionImage;
                }
                pictureBox1.Image = RegionImage;
            }
            else
            {
                foreach (Item item in _items)
                {
                    if (item.url != "question-mark.png")
                    {
                        var file = DataRepository.GetFile($"{item.type}\\{item.Name_eng}");
                        if (file != null)
                        {
                            img = Image.FromStream(file.OpenRead());
                        }
                        else
                        {
                            img = Properties.Resources.question_mark;
                        }
                    }
                    else
                    {
                        img = Properties.Resources.question_mark;
                    }
                    g.DrawImage(img, item.point.X * 480 / length, item.point.Y * 480 / length, item.w * 480 / length, item.h * 480 / length);
                    g.DrawString(item.Id.ToString(), drawFont, drawBrush, item.point.X * 480 / length, item.point.Y * 480 / length);
                    pictureBox2.Image = RegionImage2;
                }
                pictureBox2.Image = RegionImage2;
            }
        }
    }
}