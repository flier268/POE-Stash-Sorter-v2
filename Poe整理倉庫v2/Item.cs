using System;
using System.Collections.Generic;

namespace Poe整理倉庫v2
{
    public class Item : ICloneable
    {
        /// <summary>
        /// 編號
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 稀有度
        /// </summary>
        public string Rarity { get; set; }
        /// <summary>
        /// 物品名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 物品名稱
        /// </summary>
        public string Name_eng { get; set; }
        /// <summary>
        /// 類型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 優先度(排序用)
        /// </summary>
        public int priority { get; set; }
        /// <summary>
        /// 顏色(寶石有rgb，其他都是n)
        /// </summary>
        public char GC { get; set; }
        /// <summary>
        /// 圖片url
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 品質
        /// </summary>
        public int quality { get; set; }
        /// <summary>
        /// 等級(寶石)
        /// </summary>
        public int level { get; set; }
        /// <summary>
        /// 物品等級
        /// </summary>
        public int itemlevel { get; set; }
        /// <summary>
        /// 地圖階級
        /// </summary>
        public int maplevel { get; set; }

        public POINT point{get;set;}               
        public int w { get; set; }
        public int h { get; set; }
    

        public object Clone()
        {
            object i =this.MemberwiseClone();
            POINT p = new POINT(this.point);
            ((Item)i).point = p;
            return i;
        }

        public class IdAndPointComparer : IEqualityComparer<Item>
        {
            public bool Equals(Item x, Item y)
            {
                if (x == null && y == null)
                    return true;
                else if (x == null | y == null)
                    return false;
                else if (x.id == y.id && x.point.Equals(y.point))
                    return true;
                return false;
            }

            public int GetHashCode(Item obj)
            {
                int hCode = obj.id ^ obj.point.X ^ obj.point.Y;
                return hCode.GetHashCode();
            }
        }
    }
}
