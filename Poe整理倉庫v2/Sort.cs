using System.Collections.Generic;
using System.Linq;
using Poe整理倉庫v2.Enums;
using Poe整理倉庫v2.Helper;

namespace Poe整理倉庫v2
{
    public partial class Form1
    {
        public class SortRule : IComparer<Item>
        {
            private Setting s = new Setting();

            public SortRule(Setting _s)
            {
                s = _s;
            }

            public int Compare(Item s1, Item s2)
            {
                int value = 0;
                if (s.LowQ > 0)
                    if (s1.quality > s.LowQ || s2.quality > s.LowQ)
                        value = value == 0 ? s1.quality.CompareTo(s2.quality) : value;
                foreach (var t in s.Priority)
                {
                    switch (t)
                    {
                        case "MapLevel":
                            value = value == 0 ? s1.maplevel.CompareTo(s2.maplevel) : value;
                            break;

                        case "Q":
                            value = value == 0 ? s1.quality.CompareTo(s2.quality) : value;
                            break;

                        case "Name":
                            value = value == 0 ? s1.Name_eng.CompareTo(s2.Name_eng) : value;
                            break;

                        case "ItemLevel":
                            value = value == 0 ? s1.itemlevel.CompareTo(s2.itemlevel) : value;
                            break;

                        case "GemColor":
                            value = value == 0 ? s1.GC.CompareTo(s2.GC) : value;
                            break;

                        case "GemLevel":
                            value = value == 0 ? s1.level.CompareTo(s2.level) : value;
                            break;

                        case "Rarity":
                            value = value == 0 ? RarityHelper.StringToRarity(s1.Rarity).CompareTo(RarityHelper.StringToRarity(s2.Rarity)) : value;
                            break;

                        case "Type":
                            var a = s.Species.Where(x => s1.type.Contains(x)).FirstOrDefault();
                            var b = s.Species.Where(x => s2.type.Contains(x)).FirstOrDefault();
                            value = value == 0 ? s.Species.ToList().FindIndex(x => x.Equals(a)).CompareTo(s.Species.ToList().FindIndex(x => x.Equals(b))) : value;
                            break;
                    }
                }
                return value;
            }
        }

        public List<Item> Sort(List<Item> source, List<POINT> used, int length)
        {
            SortRule rule = new SortRule(Config);
            List<Item> clone = new List<Item>();

            source.ForEach(x => clone.Add((Item)x.Clone()));
            List<Item> item_1x1 = clone.Where(x => x.w * x.h == 1).ToList();
            item_1x1 = item_1x1.OrderBy(s => s, rule).ToList();

            List<POINT> free = new List<POINT>();
            var temp = item_1x1.Select(x => x.point).ToList();

            foreach (POINT _temp in temp)
                used.Remove(_temp);
            int index = 0;
            for (int x = 0; x < length; x++)
            {
                for (int y = 0; y < length; y++)
                {
                    if (Config.Direction == "Vertical")
                    {
                        if (used.Where(a => a.X == x && a.Y == y).Any())
                            continue;
                    }
                    else
                    {
                        if (used.Where(a => a.X == y && a.Y == x).Any())
                            continue;
                    }
                    if (index < item_1x1.Count)
                    {
                        if (Config.Direction == "Vertical")
                            item_1x1[index].point = new POINT(x, y);
                        else
                            item_1x1[index].point = new POINT(y, x);
                        index++;
                    }
                }
            }
            List<Item> resoult = new List<Item>();
            resoult.AddRange(item_1x1);
            resoult.AddRange(source.Where(x => x.w * x.h != 1).ToList());

            return resoult;
        }
    }
}